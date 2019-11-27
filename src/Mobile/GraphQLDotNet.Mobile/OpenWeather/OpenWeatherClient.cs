using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GraphQL.Client.Http;
using GraphQL.Common.Exceptions;
using GraphQL.Common.Request;
using GraphQL.Common.Response;
using GraphQLDotNet.Contracts;
using Microsoft.AppCenter.Crashes;
using Polly;

namespace GraphQLDotNet.Mobile.OpenWeather
{
    internal sealed class OpenWeatherClient : IOpenWeatherClient
    {
        private readonly SemaphoreSlim weatherLocationsSemaphore;
        private readonly GraphQLHttpClient graphQLHttpClient;

        private CancellationTokenSource cancellationTokenSource;

        public OpenWeatherClient(OpenWeatherConfiguration openWeatherConfiguration)
        {
            weatherLocationsSemaphore = new SemaphoreSlim(1);
            graphQLHttpClient = new GraphQLHttpClient(
                new GraphQLHttpClientOptions { EndPoint = new Uri(openWeatherConfiguration.GraphQLApiUrl) });
            cancellationTokenSource = new CancellationTokenSource();
        }
            
        public async Task<WeatherForecast> GetWeatherForecast(long locationId)
        {
            try
            {
                var graphQLRequest = new GraphQLRequest
                {
                    Query = "query Forecast($id: Long!) { forecast(city_id: $id) { id location date temperature openWeatherIcon summary description tempMin tempMax pressure humidity sunrise sunset windSpeed windDegrees visibility timezone } }",
                    OperationName = "Forecast",
                    Variables = new { id = locationId }
                };

                var response = await AttemptAndRetry(() => graphQLHttpClient.SendQueryAsync(graphQLRequest)).ConfigureAwait(false);
                return response.GetDataFieldAs<WeatherForecast>("forecast");
            }
            catch (Exception exception)
            {
                Crashes.TrackError(exception);
                throw new Exception();
            }
        }
               
        public async Task<(bool completed, IEnumerable<WeatherLocation> locations)> GetLocations(string searchTerm = "", int maxNumberOfResults = 8)
        {
            if (weatherLocationsSemaphore.CurrentCount == 0)
            {
                cancellationTokenSource.Cancel();
            }

            await weatherLocationsSemaphore.WaitAsync();
            cancellationTokenSource = new CancellationTokenSource();
            try
            {
                var graphQLRequest = new GraphQLRequest
                {
                    Query = "query GetLocations($beginsWith: String, $maxResults: Int) { locations(beginsWith: $beginsWith, maxResults: $maxResults)  { name country id latitude longitude } }",
                    OperationName = "GetLocations",
                    Variables = new { beginsWith = searchTerm, maxResults = maxNumberOfResults }
                };

                var response = await graphQLHttpClient.SendQueryAsync(graphQLRequest, cancellationTokenSource.Token).ConfigureAwait(false);
                return (true, response.GetDataFieldAs<IEnumerable<WeatherLocation>>("locations"));
            }
            catch (TaskCanceledException)
            {
                return (false, new WeatherLocation[0]);
            }
            catch (Exception exception)
            {
                Crashes.TrackError(exception);
                return (false, new WeatherLocation[0]);
            }
            finally
            {
                cancellationTokenSource.Dispose();
                weatherLocationsSemaphore.Release();
            }
        }

        public async Task<IEnumerable<WeatherSummary>> GetWeatherSummariesFor(params long[] locationIds)
        {
            try
            {
                var graphQLRequest = new GraphQLRequest
                {
                    Query = "query WeatherSummaries($location_ids: [Long]!) { summaries(location_ids: $location_ids)  { location temperature openWeatherIcon id date timezone } }",
                    OperationName = "WeatherSummaries",
                    Variables = new { location_ids = locationIds.ToArray() }
                };

                var response = await AttemptAndRetry(() => graphQLHttpClient.SendQueryAsync(graphQLRequest)).ConfigureAwait(false);
                var forecasts = response.GetDataFieldAs<IEnumerable<WeatherSummary>>("summaries");
                return forecasts;
            }
            catch (Exception exception)
            {
                Crashes.TrackError(exception);
                return new WeatherSummary[0];
            }
        }

        private static async Task<GraphQLResponse> AttemptAndRetry(Func<Task<GraphQLResponse>> action, int numRetries = 2)
        {            
            var response = await Policy.Handle<Exception>(e => e is TaskCanceledException).WaitAndRetryAsync(numRetries, pollyRetryAttempt).ExecuteAsync(action).ConfigureAwait(false);
            if (response.Errors != null && response.Errors.Count() > 1)
            {
                throw new AggregateException(response.Errors.Select(x => new GraphQLException(x)));
            }
                
            if (response.Errors != null && response.Errors.Count() is 1)
            {
                throw new GraphQLException(response.Errors.First());
            }

            return response;

            static TimeSpan pollyRetryAttempt(int attemptNumber) => TimeSpan.FromSeconds(Math.Pow(2, attemptNumber));
        }
    }
}

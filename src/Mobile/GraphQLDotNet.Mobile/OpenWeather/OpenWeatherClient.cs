using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Client.Http;
using GraphQL.Common.Exceptions;
using GraphQL.Common.Request;
using GraphQL.Common.Response;
using GraphQLDotNet.Contracts;
using GraphQLDotNet.Mobile.Models;
using Microsoft.AppCenter.Crashes;
using Polly;

namespace GraphQLDotNet.Mobile.OpenWeather
{
    internal sealed class OpenWeatherClient : IOpenWeatherClient
    {
        private readonly GraphQLHttpClient graphQLHttpClient;

        public OpenWeatherClient(OpenWeatherConfiguration openWeatherConfiguration) =>
            graphQLHttpClient = new GraphQLHttpClient(
                new GraphQLHttpClientOptions { EndPoint = new Uri(openWeatherConfiguration.GraphQLApiUrl) });

        public async Task<WeatherForecast> GetWeatherForecast(long locationId)
        {
            try
            {
                var graphQLRequest = new GraphQLRequest
                {
                    Query = "query Forecast($id: Long!) { forecast(city_id: $id) { id location date temperature openWeatherIcon summary description tempMin tempMax pressure humidity sunrise sunset windSpeed windDegrees visibility } }",
                    OperationName = "Forecast",
                    Variables = new { id = locationId }
                };

                var response = await AttemptAndRetry(() => graphQLHttpClient.SendQueryAsync(graphQLRequest)).ConfigureAwait(false);

                // TODO: fetch only whats needed and use a custom type
                return response.GetDataFieldAs<WeatherForecast>("forecast");
            }
            catch (Exception exception)
            {
                Crashes.TrackError(exception);
                throw new Exception();
            }
        }

        public async Task<IEnumerable<WeatherLocation>> GetLocations(string searchTerm = "", int maxNumberOfResults = 8)
        {
            try
            {
                var graphQLRequest = new GraphQLRequest
                {
                    Query = "query GetLocations($beginsWith: String, $maxResults: Int) { locations(beginsWith: $beginsWith, maxResults: $maxResults)  { name country id latitude longitude } }",
                    OperationName = "GetLocations",
                    Variables = new { beginsWith = searchTerm, maxResults = maxNumberOfResults }
                };

                var response = await AttemptAndRetry(() => graphQLHttpClient.SendQueryAsync(graphQLRequest)).ConfigureAwait(false);

                return response.GetDataFieldAs<IEnumerable<WeatherLocation>>("locations");
            }
            catch (Exception exception)
            {
                Crashes.TrackError(exception);
                return new WeatherLocation[0];
            }
        }

        public async Task<WeatherSummary> GetWeatherSummaryFor(long locationId)
        {
            try
            {
                var graphQLRequest = new GraphQLRequest
                {
                    Query = "query WeatherSummary($id: Long!) { forecast(city_id: $id)  { location temperature openWeatherIcon id } }",
                    OperationName = "WeatherSummary",
                    Variables = new { id = locationId }
                };

                var response = await AttemptAndRetry(() => graphQLHttpClient.SendQueryAsync(graphQLRequest)).ConfigureAwait(false);

                return response.GetDataFieldAs<WeatherSummary>("forecast");
            }
            catch (Exception exception)
            {
                Crashes.TrackError(exception);
                return WeatherSummary.Default;
            }
        }

        public async Task<IEnumerable<WeatherSummary>> GetWeatherSummaryFor(IEnumerable<long> locationIds)
        {
            try
            {
                var graphQLRequest = new GraphQLRequest
                {
                    Query = "query WeatherSummaries($location_ids: [Long]!) { forecasts(location_ids: $location_ids)  { location temperature openWeatherIcon id } }",
                    OperationName = "WeatherSummaries",
                    Variables = new { location_ids = locationIds.ToArray() }
                };

                var response = await AttemptAndRetry(() => graphQLHttpClient.SendQueryAsync(graphQLRequest)).ConfigureAwait(false);
                var forecasts = response.GetDataFieldAs<IEnumerable<WeatherSummary>>("forecasts");
                return forecasts;
            }
            catch (Exception exception)
            {
                Crashes.TrackError(exception);
                return new WeatherSummary[0];
            }
        }

        public async Task<IEnumerable<WeatherSummary>> GetWeatherUpdatesFor(IEnumerable<long> locationIds)
        {
            try
            {
                var graphQLRequest = new GraphQLRequest
                {
                    Query = "query WeatherSummaries($location_ids: [Long]!) { forecasts(location_ids: $location_ids)  { temperature openWeatherIcon id } }",
                    OperationName = "WeatherSummaries",
                    Variables = new { location_ids = locationIds.ToArray() }
                };

                var response = await AttemptAndRetry(() => graphQLHttpClient.SendQueryAsync(graphQLRequest)).ConfigureAwait(false);
                var forecasts = response.GetDataFieldAs<IEnumerable<WeatherSummary>>("forecasts");
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
            var response = await Policy.Handle<Exception>().WaitAndRetryAsync(numRetries, pollyRetryAttempt).ExecuteAsync(action).ConfigureAwait(false);
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

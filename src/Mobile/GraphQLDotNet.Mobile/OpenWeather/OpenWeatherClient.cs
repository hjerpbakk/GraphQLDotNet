using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Client.Http;
using GraphQL.Common.Exceptions;
using GraphQL.Common.Request;
using GraphQL.Common.Response;
using GraphQLDotNet.Contracts;
using GraphQLDotNet.Mobile.Models;
using Polly;
using Xamarin.Forms;

namespace GraphQLDotNet.Mobile.OpenWeather
{
    public static class OpenWeatherClient
    {
        static readonly Lazy<GraphQLHttpClient> _clientHolder = new Lazy<GraphQLHttpClient>(CreateGraphQLClient);

        static GraphQLHttpClient Client => _clientHolder.Value;

        public static async Task<IEnumerable<WeatherLocation>> GetLocations(string searchTerm = "", int maxNumberOfResults = 8)
        {
            var graphQLRequest = new GraphQLRequest
            {
                Query = "query GetLocations($beginsWith: String, $maxResults: Int) { locations(beginsWith: $beginsWith, maxResults: $maxResults)  { name country id latitude longitude } }",
                OperationName = "GetLocations",
                Variables = new { beginsWith = searchTerm, maxResults = maxNumberOfResults }
            };

            var response = await AttemptAndRetry(() => Client.SendQueryAsync(graphQLRequest)).ConfigureAwait(false);

            return response.GetDataFieldAs<IEnumerable<WeatherLocation>>("locations");
        }

        public static async Task<WeatherSummary> GetWeatherSummaryFor(long locationId)
        {
            var graphQLRequest = new GraphQLRequest
            {
                Query = "query WeatherSummary($id: Long!) { forecast(city_id: $id)  { location temperature openWeatherIcon id } }",
                OperationName = "WeatherSummary",
                Variables = new { id = locationId }
            };

            var response = await AttemptAndRetry(() => Client.SendQueryAsync(graphQLRequest)).ConfigureAwait(false);

            return response.GetDataFieldAs<WeatherSummary>("forecast");
        }

        public static async Task<IEnumerable<WeatherSummary>> GetWeatherSummaryFor(IEnumerable<long> locationIds)
        {
            try
            {
                var graphQLRequest = new GraphQLRequest
                {
                    Query = "query WeatherSummaries($location_ids: [Long]!) { forecasts(location_ids: $location_ids)  { location temperature openWeatherIcon id } }",
                    OperationName = "WeatherSummaries",
                    Variables = new { location_ids = locationIds.ToArray() }
                };

                var response = await AttemptAndRetry(() => Client.SendQueryAsync(graphQLRequest)).ConfigureAwait(false);
                var forecasts = response.GetDataFieldAs<IEnumerable<WeatherSummary>>("forecasts");
                return forecasts;
            }
            catch (Exception ex)
            {
                // TODO: Exception handling, here and elsewhere. async void begone
                Debug.WriteLine(ex);
                return new WeatherSummary[0];
            }
        }

        static GraphQLHttpClient CreateGraphQLClient() => new GraphQLHttpClient(new GraphQLHttpClientOptions
        {
            EndPoint = new Uri(BackendConstants.GraphQLApiUrl),
#if !DEBUG
            HttpMessageHandler = new ModernHttpClient.NativeMessageHandler()
#endif
        });

        static async Task<GraphQLResponse> AttemptAndRetry(Func<Task<GraphQLResponse>> action, int numRetries = 2)
        {
            var response = await Policy.Handle<Exception>().WaitAndRetryAsync(numRetries, pollyRetryAttempt).ExecuteAsync(action).ConfigureAwait(false);

            if (response.Errors != null && response.Errors.Count() > 1)
                throw new AggregateException(response.Errors.Select(x => new GraphQLException(x)));

            if (response.Errors != null && response.Errors.Count() is 1)
                throw new GraphQLException(response.Errors.First());

            return response;

            TimeSpan pollyRetryAttempt(int attemptNumber) => TimeSpan.FromSeconds(Math.Pow(2, attemptNumber));
        }

        private static class BackendConstants
        {
#if DEBUG
            public static string GraphQLApiUrl { get; } = Device.RuntimePlatform is Device.Android ? "http://10.0.2.2:5000/graphql" : "http://127.0.0.1:5000/graphql";
#else
#error Missing GraphQL Api Url
        public static string GraphQLApiUrl { get; } = "";
#endif
        }
    }
}

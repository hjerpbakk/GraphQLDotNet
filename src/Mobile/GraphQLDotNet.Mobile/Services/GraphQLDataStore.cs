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
using Polly;

namespace GraphQLDotNet.Mobile.Services
{
    public class GraphQLDataStore : IDataStore<WeatherForecastModel>
    {
        static readonly Lazy<GraphQLHttpClient> _clientHolder = new Lazy<GraphQLHttpClient>(CreateGraphQLClient);

        static GraphQLHttpClient Client => _clientHolder.Value;

        public GraphQLDataStore()
        {
        }

        public Task<bool> AddItemAsync(WeatherForecastModel item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteItemAsync(DateTime date)
        {
            throw new NotImplementedException();
        }

        public Task<WeatherForecastModel> GetItemAsync(DateTime date)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<WeatherForecastModel>> GetItemsAsync(bool forceRefresh = false)
        {
            var request = new GraphQLRequest { Query = "query { forecasts { date summary kind } } " };

            var response = await AttemptAndRetry(() => Client.SendQueryAsync(request)).ConfigureAwait(false);

            return response.GetDataFieldAs<IEnumerable<WeatherForecastModel>>("forecasts");
        }

        public Task<bool> UpdateItemAsync(WeatherForecastModel item)
        {
            throw new NotImplementedException();
        }

        private static GraphQLHttpClient CreateGraphQLClient() => new GraphQLHttpClient(new GraphQLHttpClientOptions
        {
            EndPoint = new Uri(BackendConstants.GraphQLApiUrl),
#if !DEBUG
            HttpMessageHandler = new ModernHttpClient.NativeMessageHandler()
#endif
        });

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

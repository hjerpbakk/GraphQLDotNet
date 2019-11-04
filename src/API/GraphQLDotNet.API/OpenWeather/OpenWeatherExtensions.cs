using System;
using System.Net.Http;
using GraphQLDotNet.API.GraphTypes;
using GraphQLDotNet.API.Schemas;
using GraphQLDotNet.Services.OpenWeather;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;

namespace GraphQLDotNet.API.OpenWeather
{
    public static class OpenWeatherExtensions
    {
        static readonly Random jitterer;

        static OpenWeatherExtensions()
        {
            jitterer = new Random();
        }

        public static void AddOpenWeather(this IServiceCollection services, IConfiguration configuration)
        {
            var openWeatherConfiguration = configuration.GetSection(nameof(OpenWeatherConfiguration)).Get<OpenWeatherConfiguration>();
            if (openWeatherConfiguration == null)
            {
                throw new Exception("Could not create an OpenWeatherConfiguration from the given configuration.");
            }

            openWeatherConfiguration.Verify();
            services.AddSingleton(openWeatherConfiguration);
            services.AddSingleton<WeatherForecastQuery>();
            services.AddSingleton<WeatherForecastSchema>();

            services.AddHttpClient<IOpenWeatherClient, OpenWeatherClient>(client =>
            {
                client.BaseAddress = new Uri(openWeatherConfiguration.BaseUrl);
            })
                .SetHandlerLifetime(TimeSpan.FromMinutes(2)) 
                .AddPolicyHandler(GetRetryPolicy()); 
        }

        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() =>
            HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(6, retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) +
                    TimeSpan.FromMilliseconds(jitterer.Next(0, 100))
                );
    }
}

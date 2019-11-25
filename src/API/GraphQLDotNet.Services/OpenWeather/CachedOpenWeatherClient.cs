using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Dasync.Collections;
using GraphQLDotNet.Contracts;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace GraphQLDotNet.Services.OpenWeather
{
    public class CachedOpenWeatherClient : IOpenWeatherClient
    {
        private readonly OpenWeatherConfiguration openWeatherConfiguration;
        private readonly HttpClient httpClient;
        private readonly IMemoryCache memoryCache;
        private readonly ILogger<CachedOpenWeatherClient> logger;
        private readonly Lazy<WeatherLocation[]> availableLocations;

        public CachedOpenWeatherClient(OpenWeatherConfiguration openWeatherConfiguration,
            HttpClient httpClient,
            IMemoryCache memoryCache,
            ILogger<CachedOpenWeatherClient> logger)
        {
            this.openWeatherConfiguration = openWeatherConfiguration;
            this.httpClient = httpClient;
            this.memoryCache = memoryCache;
            this.logger = logger;
            availableLocations = new Lazy<WeatherLocation[]>(ReadAllLocationsFromDisc);
        }

        public async Task<WeatherForecast> GetWeatherFor(long id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Specify a valid location id.", nameof(id));
            }

            return await GetOrSet(id,
                GetWeather,
                new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) });

            async Task<WeatherForecast> GetWeather()
            {
                var url = string.Format(openWeatherConfiguration.OpenWeatherURL, id);
                var openWeatherForecast = await httpClient.GetAsync<Forecast>(url);
                var weather = openWeatherForecast.Weather.First();
                var weatherForecast = new WeatherForecast(
                    openWeatherForecast.Id,
                    openWeatherForecast.Name,
                    DateTimeOffset.FromUnixTimeSeconds(openWeatherForecast.Dt).UtcDateTime,
                    Math.Round(openWeatherForecast.Main.Temp, 1),
                    weather.Icon,
                    weather.Main,
                    weather.Description,
                    Math.Round(openWeatherForecast.Main.TempMin, 1),
                    Math.Round(openWeatherForecast.Main.TempMax, 1),
                    openWeatherForecast.Main.Pressure,
                    openWeatherForecast.Main.Humidity,
                    DateTimeOffset.FromUnixTimeSeconds(openWeatherForecast.Sys.Sunrise).UtcDateTime,
                    DateTimeOffset.FromUnixTimeSeconds(openWeatherForecast.Sys.Sunset).UtcDateTime,
                    openWeatherForecast.Wind.Speed,
                    openWeatherForecast.Wind.Deg,
                    openWeatherForecast.Visibility,
                    openWeatherForecast.Timezone);
                return weatherForecast;
            }
        }

        public async Task<IEnumerable<WeatherForecast>> GetWeatherFor(long[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                throw new ArgumentException("Specify at least one location id", nameof(ids));
            }

            var results = new ConcurrentBag<WeatherForecast>();
            await ids.ParallelForEachAsync(async id =>
            {
                var weatherForecast = await GetWeatherFor(id);
                results.Add(weatherForecast);
            }, 0);

            return results;
        }

        public IEnumerable<WeatherLocation> GetLocations(string beginsWith, int maxResults)
        {
            return GetOrSet(beginsWith + maxResults,
                GetLocations,
                new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1) });

            IEnumerable<WeatherLocation> GetLocations()
            {
                if (maxResults <= 0)
                {
                    maxResults = IOpenWeatherClient.MaxNumberOfResults;
                }

                if (string.IsNullOrEmpty(beginsWith))
                {
                    // TODO: Hent de mest populære byer fra ID etc
                    return availableLocations.Value.Take(maxResults);
                }

                var nameAndCountry = beginsWith.Split(',');
                if (nameAndCountry.Length > 1)
                {
                    return availableLocations.Value
                        .Where(l => l.Name.StartsWith(nameAndCountry[0].Trim(), StringComparison.InvariantCultureIgnoreCase)
                            && l.Country.StartsWith(nameAndCountry[1].Trim(), StringComparison.InvariantCultureIgnoreCase))
                        .Take(maxResults);
                }

                return availableLocations.Value
                        .Where(l => l.Name.StartsWith(beginsWith.Trim(), StringComparison.InvariantCultureIgnoreCase))
                        .Take(maxResults);
            }
        }

        private WeatherLocation[] ReadAllLocationsFromDisc()
        {
            using var cities = File.OpenRead(openWeatherConfiguration.CitiesFile);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var locations = JsonSerializer.DeserializeAsync<Location[]>(cities, options).GetAwaiter().GetResult();
            return locations
                .OrderBy(location => location.Name)
                .Select(location => new WeatherLocation(location.Id, location.Name, location.Country, location.Coord.Lat, location.Coord.Lon)).ToArray();
        }

        private async Task<T> GetOrSet<T>(object key, Func<Task<T>> create, MemoryCacheEntryOptions options)
        {
            if (!memoryCache.TryGetValue(key, out T result))
            {
                logger.LogInformation($"Cache miss for {key}");
                result = await create();
                memoryCache.Set(key, result, options);
            }

            return result;
        }

        private T GetOrSet<T>(object key, Func<T> create, MemoryCacheEntryOptions options)
        {
            if (!memoryCache.TryGetValue(key, out T result))
            {
                logger.LogInformation($"Cache miss for {key}");
                result = create();
                memoryCache.Set(key, result, options);
            }

            return result;
        }
    }
}

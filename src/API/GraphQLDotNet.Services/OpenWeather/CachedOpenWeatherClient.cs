using System;
using System.Threading.Tasks;
using GraphQLDotNet.Contracts;
using Microsoft.Extensions.Caching.Memory;

namespace GraphQLDotNet.Services.OpenWeather
{
    public class CachedOpenWeatherClient : IOpenWeatherClient
    {
        private readonly IOpenWeatherClient openWeatherClient;
        private readonly IMemoryCache memoryCache;

        public CachedOpenWeatherClient(IOpenWeatherClient openWeatherClient, IMemoryCache memoryCache)
        {
            this.openWeatherClient = openWeatherClient;
            this.memoryCache = memoryCache;
        }

        public async Task<WeatherLocation> GetLoactionFor(string name) =>
            await GetOrSet(name,
                () => openWeatherClient.GetLoactionFor(name),
                new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1) });

        public async Task<WeatherForecast> GetWeatherFor(long id) =>
            await GetOrSet(id,
                () => openWeatherClient.GetWeatherFor(id),
                new MemoryCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) });

        private async Task<T> GetOrSet<T>(object key, Func<Task<T>> create, MemoryCacheEntryOptions options)
        {
            if (!memoryCache.TryGetValue(key, out T result))
            {
                result = await create();
                memoryCache.Set(key, result, options);
            }

            return result;
        }
    }
}

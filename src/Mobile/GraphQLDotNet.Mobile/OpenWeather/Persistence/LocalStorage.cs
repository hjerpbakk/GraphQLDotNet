using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GraphQLDotNet.Mobile.Models;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace GraphQLDotNet.Mobile.OpenWeather.Persistence
{
    public sealed class LocalStorage : ILocalStorage
    {
        private static readonly SemaphoreSlim weatherLocationsSemaphore;
        private static readonly string weatherLocationsPath;

        static LocalStorage()
        {
            weatherLocationsSemaphore = new SemaphoreSlim(1);
            weatherLocationsPath = Path.Combine(FileSystem.AppDataDirectory, "weather-locations.dat");
        }

        public async Task Save(IEnumerable<OrderedWeatherSummary> weatherLocationIds)
        {
            await weatherLocationsSemaphore.WaitAsync();
            try
            {
                var data = JsonConvert.SerializeObject(new WeatherLocations { WeatherSummaries = weatherLocationIds.ToArray() });
                await File.WriteAllTextAsync(weatherLocationsPath, data);
            }
            finally
            {
                weatherLocationsSemaphore.Release();
            }
        }

        public OrderedWeatherSummary[] Load()
        {
            if (!File.Exists(weatherLocationsPath))
            {
                return new OrderedWeatherSummary[0];
            }

            var data = File.ReadAllText(weatherLocationsPath);
            var loaded = JsonConvert.DeserializeObject<WeatherLocations>(data);
            return loaded.WeatherSummaries;
        }
    }
}

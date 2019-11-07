using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using Xamarin.Essentials;
using System.Collections.Generic;

namespace GraphQLDotNet.Mobile.Services
{
    public class LocalStorage
    {
        private static readonly SemaphoreSlim weatherLocationsSemaphore;
        private static readonly string weatherLocationsPath;

        static LocalStorage()
        {
            weatherLocationsSemaphore = new SemaphoreSlim(1);
            weatherLocationsPath = Path.Combine(FileSystem.AppDataDirectory, "weather-locations.dat");
        }

        public async Task Save(IEnumerable<long> weatherLocationIds)
        {
            await weatherLocationsSemaphore.WaitAsync();
            try
            {
                var data = JsonSerializer.Serialize(new WeatherLocations { Ids = weatherLocationIds.ToArray() });
                await File.WriteAllTextAsync(weatherLocationsPath, data);
            }
            finally
            {
                weatherLocationsSemaphore.Release();
            }
        }

        public async Task<long[]> Load()
        {
            if (!File.Exists(weatherLocationsPath))
            {
                return new long[0];
            }

            var data = await File.ReadAllTextAsync(weatherLocationsPath);
            var loaded = JsonSerializer.Deserialize<WeatherLocations>(data);
            return loaded.Ids;
        }
    }

    public class WeatherLocations
    {
        public long[] Ids { get; set; } = new long[0];
    }
}

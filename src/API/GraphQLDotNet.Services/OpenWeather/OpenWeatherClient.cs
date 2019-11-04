using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using GraphQLDotNet.Contracts;

namespace GraphQLDotNet.Services.OpenWeather
{
    public sealed class OpenWeatherClient : IOpenWeatherClient
    {
        private readonly OpenWeatherConfiguration openWeatherConfiguration;
        private readonly HttpClient httpClient;

        public OpenWeatherClient(OpenWeatherConfiguration openWeatherConfiguration, HttpClient httpClient)
        {
            this.openWeatherConfiguration = openWeatherConfiguration;
            this.httpClient = httpClient;
        }

        public async Task<WeatherForecast> GetWeatherFor(long id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Specify a valid location id.", nameof(id));
            }

            var url = string.Format(openWeatherConfiguration.OpenWeatherURL, id);
            var openWeatherForecast = await httpClient.GetAsync<Forecast>(url);
            var weather = openWeatherForecast.Weather.First();
            return new WeatherForecast(
                openWeatherForecast.Id,
                openWeatherForecast.Name,
                DateTimeOffset.FromUnixTimeSeconds(openWeatherForecast.Dt).UtcDateTime,
                openWeatherForecast.Main.Temp,
                weather.Icon,
                weather.Main,
                weather.Description);
        }

        public async Task<WeatherLocation> GetLoactionFor(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Specify a valid name", nameof(name));
            }

            using var cities = File.OpenRead(openWeatherConfiguration.CitiesFile);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var locations = await JsonSerializer.DeserializeAsync<Location[]>(cities, options);
            var location = locations.First(l => string.Compare(l.Name, name, true) == 0);
            return new WeatherLocation(location.Id, location.Name, location.Country);
        }
    }
}

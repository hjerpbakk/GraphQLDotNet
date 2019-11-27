using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQLDotNet.Contracts;

namespace GraphQLDotNet.Mobile.OpenWeather
{
    public interface IOpenWeatherClient
    {
        Task<WeatherForecast> GetWeatherForecast(long locationId);
        Task<(bool completed, IEnumerable<WeatherLocation> locations)> GetLocations(string searchTerm = "", int maxNumberOfResults = 8);
        Task<IEnumerable<WeatherSummary>> GetWeatherSummariesFor(params long[] locationIds);
    }
}

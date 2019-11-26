using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQLDotNet.Contracts;

namespace GraphQLDotNet.Mobile.OpenWeather
{
    public interface IOpenWeatherClient
    {
        Task<WeatherForecast> GetWeatherForecast(long locationId);
        Task<IEnumerable<WeatherLocation>> GetLocations(string searchTerm = "", int maxNumberOfResults = 8);
        Task<WeatherSummary> GetWeatherSummaryFor(long locationId);
        Task<IEnumerable<WeatherSummary>> GetWeatherSummaryFor(IEnumerable<long> locationIds);
        Task<IEnumerable<WeatherSummary>> GetWeatherUpdatesFor(IEnumerable<long> locationIds);
    }
}

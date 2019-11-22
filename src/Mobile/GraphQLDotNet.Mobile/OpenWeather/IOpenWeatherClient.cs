using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQLDotNet.Contracts;
using GraphQLDotNet.Mobile.Models;

namespace GraphQLDotNet.Mobile.OpenWeather
{
    public interface IOpenWeatherClient
    {
        // TODO: Contracts vs. model objects
        Task<WeatherForecast> GetWeatherForecast(long locationId);
        Task<IEnumerable<WeatherLocation>> GetLocations(string searchTerm = "", int maxNumberOfResults = 8);
        Task<WeatherSummary> GetWeatherSummaryFor(long locationId);
        Task<IEnumerable<WeatherSummary>> GetWeatherSummaryFor(IEnumerable<long> locationIds);
        Task<IEnumerable<WeatherSummary>> GetWeatherUpdatesFor(IEnumerable<long> locationIds);
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQLDotNet.Contracts;

namespace GraphQLDotNet.Services.OpenWeather
{
    public interface IOpenWeatherClient
    {
        static int MaxNumberOfResults { get { return 8; } }

        Task<WeatherForecast> GetWeatherFor(long id);
        Task<IEnumerable<WeatherForecast>> GetWeatherFor(long[] id);
        // TODO: beginswith er feil ord
        IEnumerable<WeatherLocation> GetLocations(string beginsWith, int maxResults);
    }
}

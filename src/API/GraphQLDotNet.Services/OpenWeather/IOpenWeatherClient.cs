using System.Threading.Tasks;
using GraphQLDotNet.Contracts;

namespace GraphQLDotNet.Services.OpenWeather
{
    public interface IOpenWeatherClient
    {
        Task<WeatherForecast> GetWeatherFor(long id);
        Task<WeatherLocation> GetLoactionFor(string name);
    }
}

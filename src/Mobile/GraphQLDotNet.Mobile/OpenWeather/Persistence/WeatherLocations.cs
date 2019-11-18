using GraphQLDotNet.Mobile.Models;

namespace GraphQLDotNet.Mobile.OpenWeather.Persistence
{
    internal sealed class WeatherLocations
    {
        public OrderedWeatherSummary[] WeatherSummaries { get; set; } = new OrderedWeatherSummary[0];
    }
}

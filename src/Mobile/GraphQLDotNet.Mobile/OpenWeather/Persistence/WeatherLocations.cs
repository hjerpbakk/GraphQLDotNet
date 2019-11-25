using GraphQLDotNet.Mobile.ViewModels;

namespace GraphQLDotNet.Mobile.OpenWeather.Persistence
{
    internal sealed class WeatherLocations
    {
        public WeatherSummaryViewModel[] WeatherSummaries { get; set; } = new WeatherSummaryViewModel[0];
    }
}

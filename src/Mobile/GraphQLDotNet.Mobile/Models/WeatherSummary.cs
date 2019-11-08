using GraphQLDotNet.Mobile.OpenWeather;

namespace GraphQLDotNet.Mobile.Models
{
    public sealed class WeatherSummary
    {
        public WeatherSummary(string location, string temperature, string openWeatherIcon, long id)
        {
            Name = location;
            Temperature = temperature;
            OpenWeatherIcon = OpenWeatherConfiguration.GetIconURL(openWeatherIcon);
            Id = id;
        }

        public string Name { get; }
        public string Temperature { get; }
        public string OpenWeatherIcon { get; }
        public long Id { get; }
    }
}

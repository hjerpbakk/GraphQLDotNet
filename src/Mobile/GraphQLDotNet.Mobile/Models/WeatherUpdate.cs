using GraphQLDotNet.Mobile.OpenWeather;

namespace GraphQLDotNet.Mobile.Models
{
    public sealed class WeatherUpdate
    {
        public WeatherUpdate(long id, string openWeatherIcon, string temperature)
        {
            Id = id;
            OpenWeatherIcon = WeatherIcons.GetFullWeatherIconUrl(openWeatherIcon);
            Temperature = temperature;
        }

        public long Id { get; }
        public string OpenWeatherIcon { get; }
        public string Temperature { get; }
    }
}

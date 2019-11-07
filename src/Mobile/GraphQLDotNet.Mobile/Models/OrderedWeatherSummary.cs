using Newtonsoft.Json;

namespace GraphQLDotNet.Mobile.Models
{
    public sealed class OrderedWeatherSummary
    {
        public OrderedWeatherSummary(WeatherSummary weatherSummary, int ordering)
        {
            Name = weatherSummary.Name;
            Temperature = weatherSummary.Temperature;
            OpenWeatherIcon = weatherSummary.OpenWeatherIcon;
            Id = weatherSummary.Id;
            Ordering = ordering;
        }

        [JsonConstructor]
        public OrderedWeatherSummary()
        {
        }

        public string Name { get; set; } = "";
        public string Temperature { get; set; } = "";
        public string OpenWeatherIcon { get; set; } = "";
        public long Id { get; set; } = -1;
        public int Ordering { get; set; } = -1;

        public OrderedWeatherSummary UpdateWeather(WeatherSummary newWeather)
        {
            OpenWeatherIcon = newWeather.OpenWeatherIcon;
            Temperature = newWeather.Temperature;
            return this;
        }
    }
}

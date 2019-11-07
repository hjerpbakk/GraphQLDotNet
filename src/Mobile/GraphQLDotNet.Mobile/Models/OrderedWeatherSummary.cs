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
        public OrderedWeatherSummary(string name, long id, int ordering)
        {
            Name = name;
            Id = id;
            Ordering = ordering;
        }

        public string Name { get; }
        public string Temperature { get; set; } = "";
        public string OpenWeatherIcon { get; set; } = "";
        public long Id { get; }
        public int Ordering { get; }

        public OrderedWeatherSummary UpdateWeather(WeatherSummary weatherSummary)
        {
            OpenWeatherIcon = weatherSummary.OpenWeatherIcon;
            Temperature = weatherSummary.Temperature;
            return this;
        }
    }
}

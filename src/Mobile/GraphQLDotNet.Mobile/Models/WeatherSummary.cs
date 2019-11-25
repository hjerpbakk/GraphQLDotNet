using System;
using GraphQLDotNet.Mobile.OpenWeather;

namespace GraphQLDotNet.Mobile.Models
{
    public sealed class WeatherSummary : IEquatable<WeatherSummary>
    {
        public WeatherSummary(string location, double temperature, string openWeatherIcon, long id, DateTime date, long timezone)
        {
            // TODO: Inherit?!?
            Name = location;
            Temperature = temperature;
            OpenWeatherIcon = OpenWeatherConfiguration.GetIconURL(openWeatherIcon);
            Id = id;
            Date = date;
            Timezone = timezone;
        }

        public static WeatherSummary Default => new WeatherSummary("", 0D, "", 0L, DateTime.MinValue, 0L);

        public DateTime Date { get; }
        public string Name { get; }
        public double Temperature { get; }
        public string OpenWeatherIcon { get; }
        public long Id { get; }
        public long Timezone { get; }

        public bool Equals(WeatherSummary other) => Id == other.Id;

        public static bool operator ==(WeatherSummary x, WeatherSummary y) => x.Equals(y);

        public static bool operator !=(WeatherSummary x, WeatherSummary y) => !(x == y);

        public override bool Equals(object obj) => obj is WeatherSummary weatherSummary && Equals(weatherSummary);

        public override int GetHashCode() => Id.GetHashCode();
    }
}

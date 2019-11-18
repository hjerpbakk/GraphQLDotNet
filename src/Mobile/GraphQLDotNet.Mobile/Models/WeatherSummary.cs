using System;
using GraphQLDotNet.Mobile.OpenWeather;

namespace GraphQLDotNet.Mobile.Models
{
    public sealed class WeatherSummary : IEquatable<WeatherSummary>
    {
        public WeatherSummary(string location, string temperature, string openWeatherIcon, long id)
        {
            Name = location;
            Temperature = temperature;
            OpenWeatherIcon = OpenWeatherConfiguration.GetIconURL(openWeatherIcon);
            Id = id;
        }

        public static WeatherSummary Default => new WeatherSummary("", "", "", 0L);

        public string Name { get; }
        public string Temperature { get; }
        public string OpenWeatherIcon { get; }
        public long Id { get; }

        public bool Equals(WeatherSummary other) => Id == other.Id;

        public static bool operator ==(WeatherSummary x, WeatherSummary y) => x.Equals(y);

        public static bool operator !=(WeatherSummary x, WeatherSummary y) => !(x == y);

        public override bool Equals(object obj) => obj is WeatherSummary && Equals((WeatherSummary)obj);

        public override int GetHashCode() => Id.GetHashCode();
    }
}

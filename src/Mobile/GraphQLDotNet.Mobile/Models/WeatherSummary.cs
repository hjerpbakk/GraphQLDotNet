using System;
using GraphQLDotNet.Mobile.OpenWeather;

namespace GraphQLDotNet.Mobile.Models
{
    public sealed class WeatherSummary : IEquatable<WeatherSummary>
    {
        public WeatherSummary(string location, double temperature, string openWeatherIcon, long id)
        {
            // TODO: Inherit?!?
            Name = location;
            Temperature = temperature + "° C";
            OpenWeatherIcon = OpenWeatherConfiguration.GetIconURL(openWeatherIcon);
            Id = id;
        }

        public static WeatherSummary Default => new WeatherSummary("", 0D, "", 0L);

        public string Name { get; }
        public string Temperature { get; }
        public string OpenWeatherIcon { get; }
        public long Id { get; }

        public bool Equals(WeatherSummary other) => Id == other.Id;

        public static bool operator ==(WeatherSummary x, WeatherSummary y) => x.Equals(y);

        public static bool operator !=(WeatherSummary x, WeatherSummary y) => !(x == y);

        public override bool Equals(object obj) => obj is WeatherSummary weatherSummary && Equals(weatherSummary);

        public override int GetHashCode() => Id.GetHashCode();
    }
}

using System;

namespace GraphQLDotNet.Contracts
{
    public sealed class WeatherSummary : IEquatable<WeatherSummary>
    {
        public WeatherSummary(string location, double temperature, string openWeatherIcon, long id, DateTime date, long timezone, long clouds)
        {
            Location = location;
            Temperature = temperature;
            OpenWeatherIcon = openWeatherIcon;
            Id = id;
            Date = date;
            Timezone = timezone;
            Clouds = clouds;
        }

        public DateTime Date { get; }
        public string Location { get; }
        public double Temperature { get; }
        public string OpenWeatherIcon { get; }
        public long Id { get; }
        public long Timezone { get; }
        public long Clouds { get; }

        public bool Equals(WeatherSummary other) => Id == other.Id;

        public static bool operator ==(WeatherSummary x, WeatherSummary y) => x.Equals(y);

        public static bool operator !=(WeatherSummary x, WeatherSummary y) => !(x == y);

        public override bool Equals(object obj) => obj is WeatherSummary weatherSummary && Equals(weatherSummary);

        public override int GetHashCode() => Id.GetHashCode();
    }
}

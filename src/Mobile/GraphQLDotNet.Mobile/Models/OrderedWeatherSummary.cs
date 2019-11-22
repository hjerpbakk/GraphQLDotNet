using System;
using Newtonsoft.Json;

namespace GraphQLDotNet.Mobile.Models
{
    public sealed class OrderedWeatherSummary : IEquatable<OrderedWeatherSummary>
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
        public OrderedWeatherSummary(long id, string name = "", int ordering = -1)
        {
            Name = name;
            Id = id;
            Ordering = ordering;
        }

        public static OrderedWeatherSummary Default => new OrderedWeatherSummary(0L);

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

        public bool Equals(OrderedWeatherSummary other) => Id == other.Id;

        public static bool operator ==(OrderedWeatherSummary x, OrderedWeatherSummary y) => x.Equals(y);

        public static bool operator !=(OrderedWeatherSummary x, OrderedWeatherSummary y) => !(x == y);

        public override bool Equals(object obj) => obj is OrderedWeatherSummary orderedWeatherSummary && Equals(orderedWeatherSummary);

        public override int GetHashCode() => Id.GetHashCode();
    }
}

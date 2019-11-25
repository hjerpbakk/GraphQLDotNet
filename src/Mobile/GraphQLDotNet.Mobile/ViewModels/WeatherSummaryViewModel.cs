using System;
using GraphQLDotNet.Mobile.Models;
using GraphQLDotNet.Mobile.ViewModels.Common;
using Newtonsoft.Json;

namespace GraphQLDotNet.Mobile.ViewModels
{
    public sealed class WeatherSummaryViewModel : ViewModelBase, IEquatable<WeatherSummaryViewModel>
    {
        public WeatherSummaryViewModel(WeatherSummary weatherSummary, int ordering)
        {
            Name = weatherSummary.Name;
            Temperature = weatherSummary.Temperature;
            OpenWeatherIcon = weatherSummary.OpenWeatherIcon;
            Id = weatherSummary.Id;
            Ordering = ordering;
        }

        [JsonConstructor]
        public WeatherSummaryViewModel(long id, string name = "", int ordering = -1)
        {
            Name = name;
            Id = id;
            Ordering = ordering;
        }

        public static WeatherSummaryViewModel Default => new WeatherSummaryViewModel(0L);

        public string Name { get; }
        public long Id { get; }
        public int Ordering { get; }

        string temperature = "";
        public string Temperature
        {
            get { return temperature; }
            set { SetProperty(ref temperature, value); }
        }

        string openWeatherIcon = "";
        public string OpenWeatherIcon
        {
            get { return openWeatherIcon; }
            set { SetProperty(ref openWeatherIcon, value); }
        }

        public WeatherSummaryViewModel UpdateWeather(WeatherSummary weatherSummary)
        {
            OpenWeatherIcon = weatherSummary.OpenWeatherIcon;
            Temperature = weatherSummary.Temperature;
            return this;
        }

        public bool Equals(WeatherSummaryViewModel other) => Id == other.Id;

        public static bool operator ==(WeatherSummaryViewModel x, WeatherSummaryViewModel y) => x.Equals(y);

        public static bool operator !=(WeatherSummaryViewModel x, WeatherSummaryViewModel y) => !(x == y);

        public override bool Equals(object obj) => obj is WeatherSummaryViewModel orderedWeatherSummary && Equals(orderedWeatherSummary);

        public override int GetHashCode() => Id.GetHashCode();
    }
}

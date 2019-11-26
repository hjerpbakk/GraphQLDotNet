using System;
using GraphQLDotNet.Contracts;
using GraphQLDotNet.Mobile.OpenWeather;
using GraphQLDotNet.Mobile.ViewModels.Common;
using Newtonsoft.Json;

namespace GraphQLDotNet.Mobile.ViewModels
{
    public sealed class WeatherSummaryViewModel : ViewModelBase, IEquatable<WeatherSummaryViewModel>
    {
        public WeatherSummaryViewModel(WeatherSummary weatherSummary, int ordering)
        {
            Name = weatherSummary.Name;
            Temperature = GetShortTemperature(weatherSummary.Temperature);
            OpenWeatherIcon = OpenWeatherConfiguration.GetIconURL(weatherSummary.OpenWeatherIcon);
            Id = weatherSummary.Id;
            Ordering = ordering;
            Time = ToTimeAtLocation(weatherSummary);
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

        // Added space to preserve layout
        string time = " ";
        public string Time
        {
            get { return time; }
            set { SetProperty(ref time, value); }
        }

        public WeatherSummaryViewModel UpdateWeather(WeatherSummary weatherSummary)
        {
            OpenWeatherIcon = OpenWeatherConfiguration.GetIconURL(weatherSummary.OpenWeatherIcon);
            Temperature = GetShortTemperature(weatherSummary.Temperature);
            Time = ToTimeAtLocation(weatherSummary);
            return this;
        }

        public bool Equals(WeatherSummaryViewModel other) => Id == other.Id;

        public static bool operator ==(WeatherSummaryViewModel x, WeatherSummaryViewModel y) => x.Equals(y);

        public static bool operator !=(WeatherSummaryViewModel x, WeatherSummaryViewModel y) => !(x == y);

        public override bool Equals(object obj) => obj is WeatherSummaryViewModel orderedWeatherSummary && Equals(orderedWeatherSummary);

        public override int GetHashCode() => Id.GetHashCode();

        private string GetShortTemperature(double temp) => Math.Round(temp, 0) + "° C";

        private string ToTimeAtLocation(WeatherSummary weatherSummary) =>
            (weatherSummary.Date + TimeSpan.FromSeconds(weatherSummary.Timezone)).ToString("HH:mm");  
    }
}

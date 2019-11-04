using System;

namespace GraphQLDotNet.Contracts
{
    public sealed class WeatherForecast
    {
        public WeatherForecast(long id, string location, DateTime dateTime, double temperature, string icon, string summary, string description)
        {
            Id = id;
            Location = location;
            Date = dateTime;
            Temperature = temperature;
            OpenWeatherIcon = icon;
            Summary = summary;
            Description = description;
        }

        public long Id { get; }
        public string Location { get; }
        public DateTime Date { get; }
        public double Temperature { get; }
        public double TemperatureF => 32 + (int)(Temperature / 0.5556);
        public string OpenWeatherIcon { get; }
        public string Summary { get; }
        public string Description { get; }
    }
}

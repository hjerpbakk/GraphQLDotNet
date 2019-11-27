using System;

namespace GraphQLDotNet.Contracts
{
    public sealed class WeatherForecast
    {
        public WeatherForecast(long id,
            string location,
            DateTime date,
            double temperature,
            string openWeatherIcon,
            string summary,
            string description,
            double tempMin,
            double tempMax,
            long pressure,
            long humidity,
            DateTime sunrise,
            DateTime sunset,
            double windSpeed,
            long windDegrees,
            long visibility,
            long timezone,
            long clouds)
        {
            Id = id;
            Location = location;
            Date = date;
            Temperature = temperature;
            OpenWeatherIcon = openWeatherIcon;
            Summary = summary;
            Description = description;
            TempMin = tempMin;
            TempMax = tempMax;
            Pressure = pressure;
            Humidity = humidity;
            Sunrise = sunrise;
            Sunset = sunset;
            WindSpeed = windSpeed;
            WindDegrees = windDegrees;
            Visibility = visibility;
            Timezone = timezone;
            Clouds = clouds;
        }

        public long Id { get; }
        public string Location { get; }
        public DateTime Date { get; }
        public double Temperature { get; }
        public string OpenWeatherIcon { get; }
        public string Summary { get; }
        public string Description { get; }
        public double TempMin { get; }
        public double TempMax { get; }
        public long Pressure { get; }
        public long Humidity { get; }
        public DateTime Sunrise { get; }
        public DateTime Sunset { get; }
        public double WindSpeed { get; }
        public long WindDegrees { get; }
        public long Visibility { get; }
        public long Timezone { get; }
        public long Clouds { get; }
    }
}

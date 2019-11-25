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

        // TODO: Remove defaults if default constructor is not needed
        public long Id { get; } = -1L;
        public string Location { get; } = "";
        public DateTime Date { get; } = DateTime.MinValue;
        public double Temperature { get; } = 0D;
        public double TemperatureF => 32 + (int)(Temperature / 0.5556);
        public string OpenWeatherIcon { get; } = "";
        public string Summary { get; } = "";
        public string Description { get; } = "";
        public double TempMin { get; } = 0D;
        public double TempMax { get; } = 0D;
        public long Pressure { get; } = 0L;
        public long Humidity { get; } = 0L;
        public DateTime Sunrise { get; } = DateTime.MinValue;
        public DateTime Sunset { get; } = DateTime.MinValue;
        public double WindSpeed { get; } = 0D;
        public long WindDegrees { get; } = 0L;
        public long Visibility { get; } = 0L;
        public long Timezone { get; } = 0L;
        public long Clouds { get; } = 0L;
    }
}

using System;

namespace GraphQLDotNet.Contracts
{
    public class WeatherForecast
    {
        public WeatherForecast(WeatherKind kind)
        {
            Kind = kind;
        }

        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; } = "";

        public WeatherKind Kind { get; }
    }
}

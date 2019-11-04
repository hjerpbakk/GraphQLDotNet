using System;
using GraphQLDotNet.Contracts;
using GraphQLDotNet.Mobile.ViewModels;

namespace GraphQLDotNet.Mobile.Models
{
    public class WeatherForecastModel
    {
        readonly WeatherForecast weatherForecast;

        public WeatherForecastModel(WeatherForecast weatherForecast)
        {
            this.weatherForecast = weatherForecast;
            WeatherIcon = (string)typeof(IconFont).GetField(WeatherKind.Sunny.ToString()).GetRawConstantValue();
        }

        public string WeatherIcon { get; }
        public string Date => weatherForecast.Date.ToLongDateString();
        public string Summary => weatherForecast.Summary;
    }
}
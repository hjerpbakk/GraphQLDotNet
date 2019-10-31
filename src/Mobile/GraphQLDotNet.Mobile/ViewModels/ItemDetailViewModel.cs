using System;
using GraphQLDotNet.Contracts;
using GraphQLDotNet.Mobile.Models;

namespace GraphQLDotNet.Mobile.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public ItemDetailViewModel(WeatherForecastModel weatherForecast)
        {
            Title = weatherForecast.Date;
            Item = weatherForecast;
        }

        public WeatherForecastModel Item { get; }
    }
}

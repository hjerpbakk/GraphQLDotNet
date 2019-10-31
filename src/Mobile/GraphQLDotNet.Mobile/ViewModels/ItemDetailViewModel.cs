using System;
using GraphQLDotNet.Contracts;
using GraphQLDotNet.Mobile.Models;

namespace GraphQLDotNet.Mobile.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        public ItemDetailViewModel(WeatherForecast weatherForecast)
        {
            Title = weatherForecast.Date.ToLongDateString();
            Item = weatherForecast;
        }

        public WeatherForecast Item { get; }
    }
}

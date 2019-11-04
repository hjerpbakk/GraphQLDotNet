using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using GraphQLDotNet.Mobile.Models;
using GraphQLDotNet.Mobile.ViewModels;
using GraphQLDotNet.Contracts;

namespace GraphQLDotNet.Mobile.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemDetailPage : ContentPage
    {
        ItemDetailViewModel? viewModel;

        public ItemDetailPage(ItemDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public ItemDetailPage()
        {
            InitializeComponent();

            /*var item = new WeatherForecast(WeatherKind.Sunny)
            {
                Date = DateTime.Now,
                Summary = "Always sunny in Philadelphia"
            };

            viewModel = new ItemDetailViewModel(new WeatherForecastModel(item));
            BindingContext = viewModel;*/
        }
    }
}
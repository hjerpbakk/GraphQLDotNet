using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using GraphQLDotNet.Mobile.Models;
using GraphQLDotNet.Contracts;

namespace GraphQLDotNet.Mobile.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class NewItemPage : ContentPage
    {
        public WeatherForecast? Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();

            /*Item = new WeatherForecastModel(WeatherKind.Sunny)
            {
                Date = DateTime.Now.Date,
                Summary = "This is an item description."
            };*/

            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AddItem", Item);
            await Navigation.PopModalAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
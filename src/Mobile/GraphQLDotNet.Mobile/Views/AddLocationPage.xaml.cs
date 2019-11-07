using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace GraphQLDotNet.Mobile.Views
{
    public partial class AddLocationPage : ContentPage
    {
        public AddLocationPage()
        {
            InitializeComponent();
        }

        // TODO: Fjern denne når jeg får page sheet til å funke
        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}

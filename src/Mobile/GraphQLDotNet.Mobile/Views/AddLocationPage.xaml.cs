using Xamarin.Forms;

namespace GraphQLDotNet.Mobile.Views
{
    public partial class AddLocationPage : ContentPage
    {
        public AddLocationPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            searchField.Focus();
        }
    }
}

using System;
using System.Threading.Tasks;
using System.Windows.Input;
using GraphQLDotNet.Mobile.ViewModels.Common;
using GraphQLDotNet.Mobile.ViewModels.Navigation;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GraphQLDotNet.Mobile.ViewModels
{
    public class AboutViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;

        public AboutViewModel(INavigationService navigationService)
        {
            Title = "About2";

#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
            OpenWebCommand = new Command(async () =>
#pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void
            {
                // TODO: async void
                await navigationService.NavigateToAsync<AboutViewModel>();
                //Launcher.OpenAsync(new Uri("https://xamarin.com/platform"));
            });
            this.navigationService = navigationService;
        }

        public ICommand OpenWebCommand { get; }

        public override async Task Initialize()
        {
            await Task.Delay(500);
            Title = "Finished";
        }
    }
}
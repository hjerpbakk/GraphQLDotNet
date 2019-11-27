using System;
using System.Threading.Tasks;
using GraphQLDotNet.Mobile.ViewModels.Commands;
using GraphQLDotNet.Mobile.ViewModels.Common;
using GraphQLDotNet.Mobile.ViewModels.Navigation;
using Xamarin.Essentials;

namespace GraphQLDotNet.Mobile.ViewModels
{
    public class SettingsViewModel : PageViewModelBase
    {
        private readonly INavigationService navigationService;

        public SettingsViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        // TODO: Use radio-button to create something
        public IAsyncCommand OpenWebCommand => new AsyncCommand(async () =>
        {
            await Launcher.OpenAsync(new Uri("https://github.com/Sankra/GraphQLDotNet"));
        });

        public override async Task Initialize()
        {
            await Task.Delay(1);
        }
    }
}
using System.Threading.Tasks;
using GraphQLDotNet.Mobile.ViewModels.Commands;
using GraphQLDotNet.Mobile.ViewModels.Common;
using GraphQLDotNet.Mobile.ViewModels.Navigation;

namespace GraphQLDotNet.Mobile.ViewModels
{
    public class AboutViewModel : PageViewModelBase
    {
        private readonly INavigationService navigationService;

        public AboutViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        // TODO: Use radio-button to create something
        public IAsyncCommand OpenWebCommand => new AsyncCommand(async () =>
        {
            await navigationService.NavigateTo<AboutViewModel>();
            //Launcher.OpenAsync(new Uri("https://xamarin.com/platform"));
        });

        public override async Task Initialize()
        {
            await Task.Delay(1);
        }
    }
}
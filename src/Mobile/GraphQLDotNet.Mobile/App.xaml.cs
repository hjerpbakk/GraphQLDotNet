using GraphQLDotNet.Mobile.Helpers;
using GraphQLDotNet.Mobile.ViewModels;
using GraphQLDotNet.Mobile.ViewModels.Common;
using GraphQLDotNet.Mobile.ViewModels.Navigation;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;

namespace GraphQLDotNet.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            // TODO: Dark mode
            //DependencyService.Register<MockDataStore>();
            //MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            AppCenter.Start($"{Secrets.AppCenteriOsSecret}",
                  typeof(Analytics), typeof(Crashes));
            var navigationService = ViewModelLocator.Resolve<INavigationService>();
            navigationService.NavigateTo<MainViewModel>().FireAndForgetSafeAsync();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

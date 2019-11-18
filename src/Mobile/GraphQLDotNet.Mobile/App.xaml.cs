using GraphQLDotNet.Mobile.Helpers;
using GraphQLDotNet.Mobile.ViewModels;
using GraphQLDotNet.Mobile.ViewModels.Common;
using GraphQLDotNet.Mobile.ViewModels.Navigation;
using GraphQLDotNet.Mobile.Views;
using LightInject;
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
            //DependencyService.Register<MockDataStore>();
            //MainPage = new MainPage();
        }

#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
        protected override async void OnStart()
#pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void
        {
            // TODO: async void
            AppCenter.Start($"{Secrets.AppCenteriOsSecret}",
                  typeof(Analytics), typeof(Crashes));
            var navigationService = ViewModelLocator.Resolve<INavigationService>();
            await navigationService.NavigateTo<MainViewModel>();
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

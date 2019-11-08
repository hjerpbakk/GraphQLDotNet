using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GraphQLDotNet.Mobile.Views;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using GraphQLDotNet.Mobile.Helpers;

namespace GraphQLDotNet.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //DependencyService.Register<MockDataStore>();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            AppCenter.Start($"ios={Secrets.AppCenteriOsSecret};" +
                  //"uwp={Your UWP App secret here};" +
                  //"android={Your Android App secret here}",
                  typeof(Analytics), typeof(Crashes));
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

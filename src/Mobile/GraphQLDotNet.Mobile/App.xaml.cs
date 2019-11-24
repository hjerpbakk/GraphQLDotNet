using System;
using System.Threading.Tasks;
using GraphQLDotNet.Mobile.Helpers;
using GraphQLDotNet.Mobile.Services;
using GraphQLDotNet.Mobile.ViewModels;
using GraphQLDotNet.Mobile.ViewModels.Common;
using GraphQLDotNet.Mobile.ViewModels.Navigation;
using GraphQLDotNet.Mobile.Views.Styles;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;

namespace GraphQLDotNet.Mobile
{
    public partial class App : Application
    {
        private readonly Lazy<LightTheme> lightTheme;
        private readonly Lazy<DarkTheme> darkTheme;
        private readonly Action setPlatformSpecificStyles;

        public App(Action setPlatformSpecificStyles) : this()
        {
            this.setPlatformSpecificStyles = setPlatformSpecificStyles;
            SetTheme();
        }

        public App()
        {
            setPlatformSpecificStyles = () => { };
            lightTheme = new Lazy<LightTheme>(new LightTheme());
            darkTheme = new Lazy<DarkTheme>(new DarkTheme());
            InitializeComponent();
        }

        protected override void OnStart()
        {
            AppCenter.Start($"{Secrets.AppCenteriOsSecret}",
                  typeof(Analytics), typeof(Crashes));
            InitApp().FireAndForgetSafeAsync();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }
        
        protected override void OnResume() => SetTheme();

        private async Task InitApp()
        {
            SetTheme();
            var navigationService = ViewModelLocator.Resolve<INavigationService>();
            await navigationService.NavigateTo<MainViewModel>();
        }

        private void SetTheme()
        {
            var theme = DependencyService.Get<IEnvironment>().GetTheme();
            if (theme == Theme.Dark && Current.Resources != darkTheme.Value)
            {
                SetTheme(Theme.Dark);
            }
            else if (theme == Theme.Light && Current.Resources != lightTheme.Value)
            {
                SetTheme(Theme.Light);
            }

            setPlatformSpecificStyles();

            void SetTheme(Theme theme)
            {
                Current.Resources = theme switch
                {
                    Theme.Light => lightTheme.Value,
                    Theme.Dark => darkTheme.Value,
                    _ => throw new NotSupportedException($"Unsupported theme {theme}."),
                };
            }
        }      
    }
}

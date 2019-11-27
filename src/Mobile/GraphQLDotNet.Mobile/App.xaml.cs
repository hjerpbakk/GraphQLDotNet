using System;
using System.Threading.Tasks;
using GraphQLDotNet.Mobile.Helpers;
using GraphQLDotNet.Mobile.OpenWeather;
using GraphQLDotNet.Mobile.OpenWeather.Persistence;
using GraphQLDotNet.Mobile.Services;
using GraphQLDotNet.Mobile.ViewModels;
using GraphQLDotNet.Mobile.ViewModels.Common;
using GraphQLDotNet.Mobile.ViewModels.Navigation;
using GraphQLDotNet.Mobile.Views.Styles;
using LightInject;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Wibci.CountryReverseGeocode;
using Xamarin.Forms;

namespace GraphQLDotNet.Mobile
{
    public partial class App : Application
    {
        private readonly Lazy<LightTheme> lightTheme;
        private readonly Lazy<DarkTheme> darkTheme;
        private readonly Action setPlatformSpecificStyles;

        private readonly IServiceContainer serviceContainer;

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
            serviceContainer = new ServiceContainer(new ContainerOptions { EnablePropertyInjection = false });
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
            await Compose().NavigateTo<MainViewModel>();
        }

        private INavigationService Compose()
        {
            serviceContainer.Register(f => serviceContainer, new PerContainerLifetime());
            serviceContainer.Register<ViewModelLocator>(new PerContainerLifetime());
            serviceContainer.Register<OpenWeatherConfiguration>(new PerContainerLifetime());
            serviceContainer.Register<INavigationService, NavigationService>(new PerContainerLifetime());
            serviceContainer.Register<ILocalStorage, LocalStorage>(new PerContainerLifetime());
            serviceContainer.Register<ICountryLocator, CountryLocator>(new PerContainerLifetime());
            serviceContainer.Register<IOpenWeatherClient, OpenWeatherClient>(new PerContainerLifetime());
            serviceContainer.Register<CountryReverseGeocodeService>(new PerContainerLifetime());

            return serviceContainer.GetInstance<INavigationService>();
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

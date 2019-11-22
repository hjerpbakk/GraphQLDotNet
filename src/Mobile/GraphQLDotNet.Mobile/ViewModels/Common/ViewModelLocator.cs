using System;
using System.Globalization;
using System.Reflection;
using GraphQLDotNet.Mobile.OpenWeather;
using GraphQLDotNet.Mobile.OpenWeather.Persistence;
using GraphQLDotNet.Mobile.ViewModels.Navigation;
using LightInject;
using Wibci.CountryReverseGeocode;
using Xamarin.Forms;

namespace GraphQLDotNet.Mobile.ViewModels.Common
{
    public static class ViewModelLocator
    {
        private static readonly ServiceContainer serviceContainer;

        static ViewModelLocator()
        {
            serviceContainer = new ServiceContainer(new ContainerOptions { EnablePropertyInjection = false });

            serviceContainer.Register<MainViewModel>();
            serviceContainer.Register<LocationsViewModel>();
            serviceContainer.Register<AboutViewModel>();
            serviceContainer.Register<AddLocationViewModel>();
            serviceContainer.Register<WeatherViewModel>();

            serviceContainer.Register<OpenWeatherConfiguration>(new PerContainerLifetime());
            serviceContainer.Register<INavigationService, NavigationService>(new PerContainerLifetime());
            serviceContainer.Register<ILocalStorage, LocalStorage>(new PerContainerLifetime());
            serviceContainer.Register<ICountryLocator, CountryLocator>(new PerContainerLifetime());
            serviceContainer.Register<IOpenWeatherClient, OpenWeatherClient>(new PerContainerLifetime());
            serviceContainer.Register<CountryReverseGeocodeService>(new PerContainerLifetime());
        }

        public static readonly BindableProperty AutoWireViewModelProperty =
            BindableProperty.CreateAttached("AutoWireViewModel", typeof(bool), typeof(ViewModelLocator), default(bool), propertyChanged: OnAutoWireViewModelChanged);

        public static bool GetAutoWireViewModel(BindableObject bindable)
        {
            return (bool)bindable.GetValue(AutoWireViewModelProperty);
        }

        public static void SetAutoWireViewModel(BindableObject bindable, bool value)
        {
            bindable.SetValue(AutoWireViewModelProperty, value);
        }

        public static T Resolve<T>() where T : class => serviceContainer.GetInstance<T>();
        
        private static void OnAutoWireViewModelChanged(BindableObject bindable, object oldValue, object newValue)
        {
            // TODO: Perhaps use a better and more performant way
            if (!(bindable is Element view))
            {
                return;
            }

            var viewType = view.GetType();
            var viewName = viewType.FullName.Replace(".Views.", ".ViewModels.");
            var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
            var viewModelName = string.Format(CultureInfo.InvariantCulture, "{0}ViewModel, {1}", viewName.Remove(viewName.Length - 4), viewAssemblyName);
            var viewModelType = Type.GetType(viewModelName);
            if (viewModelType == null)
            {
                return;
            }

            var viewModel = serviceContainer.GetInstance(viewModelType);
            view.BindingContext = viewModel;
        }
    }
}

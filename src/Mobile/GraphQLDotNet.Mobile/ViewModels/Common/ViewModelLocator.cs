using System;
using GraphQLDotNet.Mobile.Views;
using LightInject;
using Xamarin.Forms;

namespace GraphQLDotNet.Mobile.ViewModels.Common
{
    public sealed class ViewModelLocator
    {
        private readonly static PageViewModelMapping pageViewModelMapping;
        private static IServiceContainer? serviceContainer;

        static ViewModelLocator()
            => pageViewModelMapping = new PageViewModelMapping();

        public ViewModelLocator(IServiceContainer serviceContainer)
        {
            ViewModelLocator.serviceContainer = serviceContainer;
            AddMapping<MainViewModel, MainPage>();
            AddMapping<LocationsViewModel, LocationsPage>();
            AddMapping<SettingsViewModel, SettingsPage>();
            AddMapping<AddLocationViewModel, AddLocationPage>();
            AddMapping<WeatherViewModel, WeatherPage>();
        }

        public static readonly BindableProperty AutoWireViewModelProperty =
            BindableProperty.CreateAttached("AutoWireViewModel", typeof(bool), typeof(ViewModelLocator), default(bool), propertyChanged: OnAutoWireViewModelChanged);

        public static bool GetAutoWireViewModel(BindableObject bindable)
            => (bool)bindable.GetValue(AutoWireViewModelProperty);

        public static void SetAutoWireViewModel(BindableObject bindable, bool value)
            => bindable.SetValue(AutoWireViewModelProperty, value);

        public Page CreatePage(Type viewModelType)
        {
            var pageType = pageViewModelMapping.GetPageType(viewModelType);
            var page = (Page)serviceContainer!.GetInstance(pageType);
            return page;
        }

        private static void OnAutoWireViewModelChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is Page page))
            {
                return;
            }

            var viewModelType = pageViewModelMapping.GetViewModelType(page.GetType());
            var viewModel = serviceContainer!.GetInstance(viewModelType);
            page.BindingContext = viewModel;
        }

        private void AddMapping<TViewModel, TPage>()
            where TViewModel : ViewModelBase
            where TPage : Page
        {
            serviceContainer!.Register<TViewModel>();
            serviceContainer!.Register<TPage>();
            pageViewModelMapping.AddMapping<TViewModel, TPage>();
        }
    }
}

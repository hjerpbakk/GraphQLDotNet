using System;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using GraphQLDotNet.Mobile.ViewModels.Common;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;

namespace GraphQLDotNet.Mobile.ViewModels.Navigation
{
    public sealed class NavigationService : INavigationService
    {
        public async Task NavigateTo<TViewModel>() where TViewModel : PageViewModelBase
        {
            try
            {
                var page = CreatePage(typeof(TViewModel));
                var init = ((PageViewModelBase)page.BindingContext).Initialize();
                if (((TabbedPage)Application.Current.MainPage)?.CurrentPage is NavigationPage navigationPage)
                {
                    await navigationPage.PushAsync(page, true);
                }
                else
                {
                    Application.Current.MainPage = page;
                }

                await init;
            }
            catch (Exception exception)
            {
                Crashes.TrackError(exception);
            }
        }

        // TODO: De-duplicate
        public async Task NavigateTo<TViewModel, TPageArgument>(TPageArgument argument) where TViewModel : PageViewModelBase<TPageArgument>
        {
            try
            {
                var page = CreatePage(typeof(TViewModel));
                var init = ((PageViewModelBase<TPageArgument>)page.BindingContext).Initialize(argument);
                if (((TabbedPage)Application.Current.MainPage)?.CurrentPage is NavigationPage navigationPage)
                {
                    await navigationPage.PushAsync(page, true);
                }
                else
                {
                    Application.Current.MainPage = page;
                }

                await init;
            }
            catch (Exception exception)
            {
                Crashes.TrackError(exception);
            }
        }

        public async Task NavigateModallyTo<TViewModel>() where TViewModel : PageViewModelBase
        {
            try
            {
                var page = CreatePage(typeof(TViewModel));
                // TODO: Consider the use of Application here...
                var init = ((PageViewModelBase)page.BindingContext).Initialize();
                await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(page), true);
                await init;
            }
            catch (Exception exception)
            {
                Crashes.TrackError(exception);
            }
        }

        public async Task PopModal() =>
            await Application.Current.MainPage.Navigation.PopModalAsync(true);

        public async Task Pop()
        {
            if (((TabbedPage)Application.Current.MainPage)?.CurrentPage is NavigationPage navigationPage)
            {
                await navigationPage.PopAsync(true);
            }
        }

        private Page CreatePage(Type viewModelType)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType);
            if (pageType == null)
            {
                throw new Exception($"Cannot locate page type for {viewModelType}");
            }

            Page page = (Page)Activator.CreateInstance(pageType);
            return page;
        }

        private Type GetPageTypeForViewModel(Type viewModelType)
        {
            // TODO: Make it easier and more performant
            var shitty = viewModelType.FullName.Replace("Model", "");
            var shitty2 = shitty.Remove(shitty.Length - 4);
            var viewName = shitty2 + "Page";
            var viewModelAssemblyName = viewModelType.GetTypeInfo().Assembly.FullName;
            var viewAssemblyName = string.Format(CultureInfo.InvariantCulture, "{0}, {1}", viewName, viewModelAssemblyName);
            var viewType = Type.GetType(viewAssemblyName);
            return viewType;
        }
    }
}

using System;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using GraphQLDotNet.Mobile.ViewModels.Common;
using Xamarin.Forms;

namespace GraphQLDotNet.Mobile.ViewModels.Navigation
{
    public sealed class NavigationService : INavigationService
    {
        public async Task NavigateTo<TViewModel>() where TViewModel : ViewModelBase
        {
            var page = CreatePage(typeof(TViewModel));
            if (((TabbedPage)Application.Current.MainPage)?.CurrentPage is NavigationPage navigationPage)
            {
                await navigationPage.PushAsync(page);
            }
            else
            {
                Application.Current.MainPage = page;
            }

            await ((ViewModelBase)page.BindingContext).Initialize();
        }

        public async Task NavigateModallyTo<TViewModel>() where TViewModel : ViewModelBase
        {
            var page = CreatePage(typeof(TViewModel));
            // TODO: Consider the use of Application here...
            await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(page));
            await ((ViewModelBase)page.BindingContext).Initialize();
        }

        public async Task PopModal() =>
            await Application.Current.MainPage.Navigation.PopModalAsync();

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

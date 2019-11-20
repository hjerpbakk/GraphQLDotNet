using System;
using System.Threading.Tasks;
using GraphQLDotNet.Mobile.ViewModels.Common;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;

namespace GraphQLDotNet.Mobile.ViewModels
{
    public sealed class MainViewModel : ViewModelBase
    {
        public async Task InitializeTab(Page currentPage)
        {
            if (currentPage is NavigationPage navigationPage &&
                navigationPage.CurrentPage?.BindingContext is ViewModelBase viewModel)
            {
                try
                {
                    await viewModel.Initialize();
                }
                catch (Exception exception)
                {
                    Crashes.TrackError(exception);
                }   
            }
        }
    }
}

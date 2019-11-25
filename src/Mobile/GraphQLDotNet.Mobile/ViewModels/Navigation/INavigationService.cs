using System.Threading.Tasks;
using GraphQLDotNet.Mobile.ViewModels.Common;

namespace GraphQLDotNet.Mobile.ViewModels.Navigation
{
    public interface INavigationService
    {
        Task NavigateTo<TViewModel>() where TViewModel : PageViewModelBase;
        Task NavigateTo<TViewModel, TPageArgument>(TPageArgument argument) where TViewModel : PageViewModelBase;
        Task NavigateModallyTo<TViewModel>() where TViewModel : PageViewModelBase;
        Task Pop();
        Task PopModal();
    }
}

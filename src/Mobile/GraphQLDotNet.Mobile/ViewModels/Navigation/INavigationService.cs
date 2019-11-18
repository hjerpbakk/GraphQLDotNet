using System.Threading.Tasks;
using GraphQLDotNet.Mobile.ViewModels.Common;

namespace GraphQLDotNet.Mobile.ViewModels.Navigation
{
    public interface INavigationService
    {
        Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase;
    }
}

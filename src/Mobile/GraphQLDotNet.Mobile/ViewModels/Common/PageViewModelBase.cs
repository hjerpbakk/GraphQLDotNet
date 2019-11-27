using System.Threading.Tasks;

namespace GraphQLDotNet.Mobile.ViewModels.Common
{
    public abstract class PageViewModelBase : ViewModelBase
    {
        public virtual Task Initialize() => Task.FromResult(false);
    }

    public abstract class PageViewModelBase<TArgument> : ViewModelBase
    {
        public virtual Task Initialize(TArgument argument) => Task.FromResult(false);
    }
}

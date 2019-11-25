using System.Threading.Tasks;

namespace GraphQLDotNet.Mobile.ViewModels.Common
{
    public abstract class PageViewModelBase : ViewModelBase
    {
        // TODO: Needed?
        bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = "";
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public virtual Task Initialize() => Task.FromResult(false);
    }

    public abstract class PageViewModelBase<TArgument> : PageViewModelBase
    {
        public virtual Task Initialize(TArgument argument) => Task.FromResult(false);
    }
}

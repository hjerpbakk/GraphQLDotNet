using GraphQLDotNet.Mobile.ViewModels;
using GraphQLDotNet.Mobile.ViewModels.Common;
using Xamarin.Forms;

namespace GraphQLDotNet.Mobile.Views
{
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            // Not elegant, but w00rks 🙈
            ((MainViewModel)BindingContext)
                .InitializeTab(CurrentPage)
                .FireAndForgetSafeAsync();
        }
    }
}
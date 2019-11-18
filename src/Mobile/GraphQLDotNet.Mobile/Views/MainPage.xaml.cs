using System.ComponentModel;
using GraphQLDotNet.Mobile.ViewModels;
using GraphQLDotNet.Mobile.ViewModels.Common;
using GraphQLDotNet.Mobile.ViewModels.Messages;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace GraphQLDotNet.Mobile.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    [Preserve(AllMembers = true)]
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
        protected override async void OnCurrentPageChanged()
#pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void
        {
            // TODO: async void
            base.OnCurrentPageChanged();
            // TODO: Force init children need to behave in regards to Init
            if (CurrentPage is NavigationPage navigationPage &&
                navigationPage.CurrentPage?.BindingContext is ViewModelBase viewModel)
            {
                await viewModel.Initialize();               
            }
        }
    }
}
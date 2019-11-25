using Foundation;
using GraphQLDotNet.Mobile.iOS.Views;
using GraphQLDotNet.Mobile.Views.Controls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(AccessorizedListView), typeof(AccessorizedListViewRenderer))]
namespace GraphQLDotNet.Mobile.iOS.Views
{
    public class AccessorizedListViewRenderer : ListViewRenderer, IUITableViewDelegate
    {
        private AccessorizedListView? formsControl;

        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.WeakDelegate = this;
            }

            formsControl = (AccessorizedListView)e.NewElement;
        }

#pragma warning disable IDE0060 // Remove unused parameter
        [Export("tableView:accessoryButtonTappedForRowWithIndexPath:")]
        public void AccessoryButtonTapped(UITableView tableView, NSIndexPath indexPath) =>
            formsControl?.AccessoryTappedCommand?.Execute(indexPath.Row);

        [Export("tableView:didSelectRowAtIndexPath:")]
        public void RowSelected(UITableView tableView, NSIndexPath indexPath) =>
            formsControl?.RowTappedCommand?.Execute(indexPath.Row);
#pragma warning restore IDE0060 // Remove unused parameter
    }
}

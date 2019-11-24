using Foundation;
using GraphQLDotNet.Mobile.iOS.Views;
using GraphQLDotNet.Mobile.Views.Controls;
using GraphQLDotNet.Mobile.Views.Styles;
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
            //var color = Xamarin.Forms.Application.Current.Resources["ActionColor"];
            //TintColor = new UIColor(((Color)color).ToCGColor());
        }

        public override void TraitCollectionDidChange(UITraitCollection previousTraitCollection)
        {
            // TODO: Simplify
            base.TraitCollectionDidChange(previousTraitCollection);
            if (TraitCollection.UserInterfaceStyle == UIUserInterfaceStyle.Dark)
            {
                var a = new DarkTheme();
                //TintColor = new UIColor(((Color)a["ActionColor"]).ToCGColor());
            }
            else
            {
                var a = new LightTheme();
                //TintColor = new UIColor(((Color)a["ActionColor"]).ToCGColor());
            }
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

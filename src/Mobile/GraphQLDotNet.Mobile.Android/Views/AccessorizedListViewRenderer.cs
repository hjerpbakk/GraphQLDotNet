using Android.Content;
using GraphQLDotNet.Mobile.Droid.Views;
using GraphQLDotNet.Mobile.Views.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(AccessorizedListView), typeof(AccessorizedListViewRenderer))]
namespace GraphQLDotNet.Mobile.Droid.Views
{
    public class AccessorizedListViewRenderer : ListViewRenderer
    {
        private AccessorizedListView? formsControl;

        public AccessorizedListViewRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                //Control.WeakDelegate = this;
            }

            formsControl = (AccessorizedListView)e.NewElement;
            if (formsControl == null)
            {
                if (e.OldElement != null)
                {
                    e.OldElement.ItemTapped -= FormsControl_ItemTapped;
                }
            }
            else
            {
                formsControl.ItemTapped += FormsControl_ItemTapped;
            }
        }

        private void FormsControl_ItemTapped(object sender, ItemTappedEventArgs e) =>
            formsControl?.RowTappedCommand?.Execute(e.ItemIndex);
    }
}

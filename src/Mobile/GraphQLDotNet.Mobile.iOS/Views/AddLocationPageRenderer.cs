using GraphQLDotNet.Mobile.iOS.Views;
using GraphQLDotNet.Mobile.Views;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

//[assembly: ExportRenderer(typeof(AddLocationPage), typeof(AddLocationPageRenderer))]
namespace GraphQLDotNet.Mobile.iOS.Views
{
    public class AddLocationPageRenderer : PageRenderer
    {
        public override void WillMoveToParentViewController(UIViewController parent)
        {               
            parent.ModalPresentationStyle = UIModalPresentationStyle.PageSheet;
            base.WillMoveToParentViewController(parent);
        }
    }
}

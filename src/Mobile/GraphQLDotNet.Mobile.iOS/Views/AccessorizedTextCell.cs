using GraphQLDotNet.Mobile.iOS.Views;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(TextCell), typeof(AccessorizedTextCell))]
namespace GraphQLDotNet.Mobile.iOS.Views
{
    public class AccessorizedTextCell : TextCellRenderer
    {
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var cell = base.GetCell(item, reusableCell, tv);
            cell.Accessory = item.StyleId switch
            {
                "none" => UITableViewCellAccessory.None,
                "checkmark" => UITableViewCellAccessory.Checkmark,
                "detail-button" => UITableViewCellAccessory.DetailButton,
                "detail-disclosure-button" => UITableViewCellAccessory.DetailDisclosureButton,
                "disclosure" => UITableViewCellAccessory.DisclosureIndicator,
                _ => UITableViewCellAccessory.None,
            };          

            return cell;
        }
    }
}

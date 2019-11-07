using System.Windows.Input;
using Xamarin.Forms;

namespace GraphQLDotNet.Mobile.Views.Controls
{
    public class AccessorizedListView : ListView
    {
        public static readonly BindableProperty AccessoryTappedCommandProperty =
            BindableProperty.Create("AccessoryTappedCommand", typeof(ICommand), typeof(AccessorizedListView));

        public ICommand AccessoryTappedCommand
        {
            get { return (ICommand)GetValue(AccessoryTappedCommandProperty); }
            set { SetValue(AccessoryTappedCommandProperty, value); }
        }

        public static readonly BindableProperty RowTappedCommandProperty =
            BindableProperty.Create("AccessoryTappedCommand", typeof(ICommand), typeof(AccessorizedListView));

        public ICommand RowTappedCommand
        {
            get { return (ICommand)GetValue(RowTappedCommandProperty); }
            set { SetValue(RowTappedCommandProperty, value); }
        }
    }
}

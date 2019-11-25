using System;
using System.Globalization;
using Xamarin.Forms;

namespace GraphQLDotNet.Mobile.Views.Converters
{
    public class CloudsToBackgroundConverter : IValueConverter
    {
        // We are Grey. We stand between the darkness and the light.
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => ((Color)Application.Current.Resources["CloudColor"]).MultiplyAlpha((long)value / 100D);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException("No reason to convert the other way");
    }
}

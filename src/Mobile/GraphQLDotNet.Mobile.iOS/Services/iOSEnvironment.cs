using System;
using GraphQLDotNet.Mobile.iOS.Services;
using GraphQLDotNet.Mobile.Services;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(iOSEnvironment))]
namespace GraphQLDotNet.Mobile.iOS.Services
{
    public sealed class iOSEnvironment : IEnvironment
    {
        public Theme GetTheme()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(12, 0))
            {
                var userInterfaceStyle = UIScreen.MainScreen.TraitCollection.UserInterfaceStyle;
                return userInterfaceStyle switch
                {
                    UIUserInterfaceStyle.Light => Theme.Light,
                    UIUserInterfaceStyle.Dark => Theme.Dark,
                    _ => throw new NotSupportedException($"UIUserInterfaceStyle {userInterfaceStyle} not supported")
                };
            }

            return Theme.Light;
        }
    }
}

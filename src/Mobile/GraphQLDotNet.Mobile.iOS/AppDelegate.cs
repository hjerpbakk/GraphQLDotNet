using System;
using Foundation;
using GraphQLDotNet.Mobile.Services;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace GraphQLDotNet.Mobile.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public sealed class AppDelegate : FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
            Forms.Init();
            LoadApplication(new App(ChangeTintColor));
            return base.FinishedLaunching(uiApplication, launchOptions);
        }

        private void ChangeTintColor()
        {
            var color = Xamarin.Forms.Application.Current.Resources["ActionColor"];
            var tintColor = new UIColor(((Color)color).ToCGColor());

            // TODO: How to change tintcolor of already visible views
            UIView.Appearance.TintColor = tintColor;
            UIBarButtonItem.Appearance.TintColor = tintColor;
        }
    }
}

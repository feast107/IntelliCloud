using Foundation;
using UIKit;

namespace IntelliMobile.IOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register ("AppDelegate")]
    public class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {

        public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
        {
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());
            // Override point for customization after application launch.
            // If not required for your application you can safely delete this method
            return base.FinishedLaunching(application,launchOptions);
        }
     
    }
}


using CoreFoundation;
using Foundation;
using System;
using System.Drawing;
using UIKit;

namespace IntelliMobile.IOS
{
    [Register("UniversalView")]
    public class CameraView : UIView
    {
        public CameraView()
        {
            Initialize();
        }

        public CameraView(RectangleF bounds) : base(bounds)
        {
            Initialize();
        }

        void Initialize()
        {
            BackgroundColor = UIColor.FromRGBA(168,168,168,64);
        }
    }

    [Register("UIViewController1")]
    public class CameraViewController : UIViewController
    {
        public CameraViewController()
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            View = new CameraView();

            base.ViewDidLoad();

            // Perform any additional setup after loading the view
        }
    }
}
using Foundation;
using System;
using System.Linq;
using UIKit;
using System.Threading.Tasks;


namespace IntelliMobile.IOS
{
    public partial class ViewController : UIViewController
    {
        private UIView v;
        public ViewController (IntPtr handle) : base (handle)
        {
            
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            // Perform any additional setup after loading the view, typically from a nib.
            v = new UIView
            {
                BackgroundColor = UIColor.Blue,
                Frame = new CoreGraphics.CGRect(0, 0, 300, 300)
            };
            View.AddSubview (v);
            v.RemoveFromSuperview();

            UIButton b = new UIButton
            {
                BackgroundColor = UIColor.Black,
                Frame = new CoreGraphics.CGRect(v.Frame.Location,new CoreGraphics.CGSize(20,20)),
                
            };
            v.Subviews.ToList().Contains(b);

            UIProgressView progress = new UIProgressView() {
                Frame = new CoreGraphics.CGRect(v.Frame.Location, new CoreGraphics.CGSize(20, 40)),
                Progress = 0,
            };
            BeginInvokeOnMainThread(() => { });

            Invoke(v,() => { });
        }



        public void Invoke(UIView view, Action action) 
        {
            view.InvokeOnMainThread(() => { action(); });
        }

        public virtual void Try(ref int dele) {
            picker.SourceType = UIImagePickerControllerSourceType.SavedPhotosAlbum;
            picker.MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.SavedPhotosAlbum);
            picker.FinishedPickingImage += Picker_FinishedPickingImage;
            picker.Canceled += Picker_Canceled;
        }

        private void Picker_Canceled(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Picker_FinishedPickingImage(object sender, UIImagePickerImagePickedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public UIImagePickerController picker;

        public override void DidReceiveMemoryWarning ()
        {
            base.DidReceiveMemoryWarning ();
            // Release any cached data, images, etc that aren't in use.
             
        }
    }
}
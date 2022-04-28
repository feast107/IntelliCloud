using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

namespace IntelliMobile.IOS
{
    internal class CameraController : UIViewController 
    {
		nfloat ScreenWidth { get { return View.Bounds.Size.Width; } }

		nfloat ScreenHeight { get { return View.Bounds.Size.Height; } }

		UIImageView imageView;

		UIImagePickerController imagePicker;
		public CameraController()
        {
			UIButton selectBtn = new UIButton(UIButtonType.System);
			selectBtn.Layer.MasksToBounds = true;
			selectBtn.Layer.BorderWidth = 0.5f;
			selectBtn.Layer.BorderColor = UIColor.LightGray.CGColor;
			selectBtn.SetTitle("选择照片", UIControlState.Normal);
			selectBtn.Frame = new CoreGraphics.CGRect((ScreenWidth - 150) / 2, ScreenHeight - 80, 150, 30);
			selectBtn.TouchUpInside += but_Click;
			this.View.AddSubview(selectBtn);

			//视图容器  存放取的的照片
			imageView = new UIImageView();
			imageView.ClipsToBounds = true;
			imageView.BackgroundColor = UIColor.LightGray;
			imageView.Frame = new CoreGraphics.CGRect((ScreenWidth - 250) / 2, 100, 250, 250);
			imageView.ContentMode = UIViewContentMode.ScaleAspectFit;
			this.View.AddSubview(imageView);

			//保存相片到相册
			UIButton surviveImgBtn = new UIButton(UIButtonType.System);
			surviveImgBtn.Layer.BorderWidth = 0.5f;
			surviveImgBtn.Layer.BorderColor = UIColor.LightGray.CGColor;
			surviveImgBtn.Frame = new CoreGraphics.CGRect((ScreenWidth - 150) / 2, 400, 150, 30);
			surviveImgBtn.SetTitle("保存照片", UIControlState.Normal);
			this.View.AddSubview(surviveImgBtn);

			//创建图像选择器控制
			imagePicker = new UIImagePickerController();
			imagePicker.FinishedPickingMedia += Handle_FinishedPickingMedia;
			imagePicker.Canceled += Handle_Canceled;

		}
		protected void Handle_FinishedPickingMedia(object sender, UIImagePickerMediaPickedEventArgs e)
		{
			bool isImage = false;
			switch (e.Info[UIImagePickerController.MediaType].ToString())
			{
				case "public.image":
					Console.WriteLine("Image selected");
					isImage = true;
					break;
				case "public.video":
					Console.WriteLine("Video selected");
					break;
			}

			// get common info (shared between images and video)
			NSUrl referenceURL = e.Info[new NSString("UIImagePickerControllerReferenceUrl")] as NSUrl;
			if (referenceURL != null)
			{
				Console.WriteLine("Url:" + referenceURL.ToString());
			}

			if (isImage)
			{
				// get the original image
				UIImage originalImage = e.Info[UIImagePickerController.OriginalImage] as UIImage;
				if (originalImage != null)
				{
					// do something with the image
					Console.WriteLine("got the original image");
					imageView.Image = originalImage; // display
				}
			}
			else
			{ // if it's a video
			  // get video url
				NSUrl mediaURL = e.Info[UIImagePickerController.MediaURL] as NSUrl;
				if (mediaURL != null)
				{
					Console.WriteLine(mediaURL.ToString());
				}
			}

			this.imagePicker.DismissViewControllerAsync(true);
		}

		void Handle_Canceled(object sender, EventArgs e)
		{
			UIAlertView alertView = new UIAlertView();
			alertView.Title = "您已取消";
			alertView.AddButton("Cancel");
			alertView.Show();
			this.imagePicker.DismissViewControllerAsync(true);
		}
			private void but_Click(object sender, EventArgs e)
		{
			UIActionSheet actionSheet = new UIActionSheet();
			actionSheet.Title = "请选择";
			actionSheet.AddButton("相机");
			actionSheet.AddButton("相册");
			actionSheet.AddButton("取消");
			//actionSheet.DestructiveButtonIndex = 0;
			actionSheet.CancelButtonIndex = 2;
			actionSheet.Clicked += delegate (object a, UIButtonEventArgs b)
			{
				string indexStr = b.ButtonIndex.ToString();
				if (int.Parse(indexStr) == 0)
				{
					//相机
					if (UIImagePickerController.IsSourceTypeAvailable(UIImagePickerControllerSourceType.Camera))
					{
						imagePicker.SourceType = UIImagePickerControllerSourceType.Camera;
						//设置摄像头
						imagePicker.CameraDevice = UIImagePickerControllerCameraDevice.Front;//前置摄像头
																							 //设置闪光灯
						imagePicker.CameraFlashMode = UIImagePickerControllerCameraFlashMode.On;//打开摄像头
																								//设置相机模式 这里面有两个模式 一个是拍摄静止的照片 一个是拍摄动的照片 也是通过CameraDevice来设置的

						imagePicker.Canceled += Handle_Canceled;
						this.PresentModalViewController(imagePicker, true);
					}
					else
					{
						UIAlertView alertView = new UIAlertView();
						alertView.Title = "相机不可用";
						alertView.AddButton("Cancel");
						alertView.Show();
					}
				}
				else if (int.Parse(indexStr) == 1)
				{
					//相册
					//设置源和媒体类型
					imagePicker.SourceType = UIImagePickerControllerSourceType.SavedPhotosAlbum;
					imagePicker.MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.SavedPhotosAlbum);
					this.PresentModalViewController(imagePicker, true);
				}
				else
				{
					//取消
				}
			};
			actionSheet.ShowInView(this.View);
		}
    }
}
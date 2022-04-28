using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;

namespace TestAndroid
{
    public class MainPageViewModel :INotifyPropertyChanged
    {

        private static string url="127.0.0.1:5000/Home/GetPage";

        public MainPageViewModel()
        {
            _Result = "一开始的内容";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string _Result
        {
            get { return _Result; }
            set {
                if(_Result != value) {
                    _Result = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(_Result)));
                }
            }
        }

        
        async Task TakePhotoFromCamera()
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await App.Current.MainPage.DisplayAlert("无法访问摄像头", "设备不支持", "OK");
                return;
            }
            //检查照相机和存储权限，没有的话进行一次请求
            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<CameraPermission>();
            var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>();

            if (cameraStatus != PermissionStatus.Granted || storageStatus != PermissionStatus.Granted)
            {
                var camresults = await CrossPermissions.Current.RequestPermissionAsync<CameraPermission>();
                var storresult = await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>();
                cameraStatus = camresults;
                storageStatus = storresult;
            }

            if (cameraStatus == PermissionStatus.Granted && storageStatus == PermissionStatus.Granted)
            {
                var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    SaveToAlbum = false //保存到照片库
                });

                if (file == null)
                    return;

                //Debug.WriteLine(file.AlbumPath); //公共照片库路径
                //Debug.WriteLine(file.Path); // 私有路径

                MessagingCenter.Send(this, "AddImage", file.GetStream());
                //AddPlantPhotoObject(file.AlbumPath, file.GetStream());
                byte[] bytes = System.IO.File.ReadAllBytes(file.AlbumPath);
                string base64 = Convert.ToBase64String(bytes);
                file.Dispose();
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("没有权限", "没有权限访问摄像头或进行文件存储", "OK");

                //iOS客户端时打开设置界面
                if (Device.RuntimePlatform == Device.iOS)
                {
                    CrossPermissions.Current.OpenAppSettings();
                }
            }
        }

    }
}

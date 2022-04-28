using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xamarin.Forms;

namespace IntelliMobile.API
{
    public class CameraBase : ICamera
    {
        /// <summary>
        /// 检查相机权限
        /// </summary>
        /// <param name="Ask">请求</param>
        /// <returns></returns>
        public async Task<bool> CheckCameraAuthority(bool Ask) 
        {
            if(PermissionStatus.Granted != await CrossPermissions.Current.CheckPermissionStatusAsync<CameraPermission>())
            {
                if (Ask)
                {
                    if (await CrossPermissions.Current.RequestPermissionAsync<CameraPermission>() == PermissionStatus.Granted)
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 检查相册权限
        /// </summary>
        /// <param name="Ask">请求</param>
        /// <returns></returns>
        public async Task<bool> CheckAlbumAuthority(bool Ask) 
        {
            if (PermissionStatus.Granted !=await CrossPermissions.Current.CheckPermissionStatusAsync<StoragePermission>())
            {
                if (Ask)
                {
                    if(await CrossPermissions.Current.RequestPermissionAsync<StoragePermission>() == PermissionStatus.Granted)
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 相机是否可用
        /// </summary>
        public bool CameraAvailable 
        {
            get
            {
                return CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported;
            }
        }
  
        public bool PickPhotoSupported
        {
            get
            {
                return CrossMedia.Current.IsPickPhotoSupported;
            }
        }

        public bool VideoSupported
        {
            get
            {
                return CrossMedia.Current.IsTakeVideoSupported;
            }
        }

        private async Task<bool> Initalize() 
        {
            return await CrossMedia.Current.Initialize();        
        }

        /// <summary>
        /// 获取照片
        /// </summary>
        /// <returns></returns>
        public async Task<MediaFile> TakePhotoAsync() 
        {
            return await TakePhotoAsync(new StoreCameraMediaOptions() { SaveToAlbum = false }); 
        }
    
        /// <summary>
        /// 获取照片
        /// </summary>
        /// <param name="SaveOptions"></param>
        /// <returns></returns>
        public async Task<MediaFile> TakePhotoAsync(StoreCameraMediaOptions SaveOptions)
        {
            await Initalize();
            if (!CameraAvailable)
            {
                await App.Current.MainPage.DisplayAlert("无法访问摄像头", "设备不支持", "OK");
                return null;
            }
            if (SaveOptions.SaveToAlbum)
            {
                if (! await CheckAlbumAuthority(SaveOptions.SaveToAlbum))
                {
                    await App.Current.MainPage.DisplayAlert("没有权限", "没有权限访问文件存储", "OK");
                    return null;
                }
            }
            if (await CheckCameraAuthority(true))
            {
                return await CrossMedia.Current.TakePhotoAsync(SaveOptions);
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("没有权限", "没有权限访问摄像头", "OK");

                //iOS客户端时打开设置界面
                if (Device.RuntimePlatform == Device.iOS)
                {
                    CrossPermissions.Current.OpenAppSettings();
                }
                return null;
            }
        }

        public async Task<MediaFile> PickPhotoAsync()
        {
            if (await CheckAlbumAuthority(true))
            {
                return await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions());
            }
            else
            {
                return null;
            }
        }

        public async Task<List<MediaFile>> PickPhotosAsync()
        {
            if (await CheckAlbumAuthority(true))
            {
                return await CrossMedia.Current.PickPhotosAsync(new PickMediaOptions());
            }
            else
            {
                return null;
            }
        }
    }
}

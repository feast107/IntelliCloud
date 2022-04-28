using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Plugin.Media.Abstractions;

namespace IntelliMobile.API
{
    public interface ICamera
    {
        Task<MediaFile> TakePhotoAsync();
        Task<MediaFile> TakePhotoAsync(StoreCameraMediaOptions SaveOptions);
        Task<MediaFile> PickPhotoAsync();
        Task<List<MediaFile>> PickPhotosAsync();
    }
}

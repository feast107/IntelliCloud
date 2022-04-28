using IntelliMobile.API;
using Plugin.Media.Abstractions;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using IntelliHttpClient;
using System.Net.Http;
using System.Drawing;
using MyOpenCv.API;
using System.IO;

namespace IntelliMobile.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly ICamera _camera = DependencyService.Get<ICamera>();
        private readonly IHttp _client = DependencyService.Get<IHttp>();
        private readonly I2ImageFilter filter1 = DependencyService.Get<I2ImageFilter>();
        private MediaFile _pic { get; set; }
        public MainViewModel()
        {
            Title = "表单识别";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://106.14.32.120:5000"));
            OpenCameraCommand = new Command(async () =>
            {
                _pic = await _camera.TakePhotoAsync();
                Source= ImageSource.FromStream(()=> _pic.GetStream());
            });
            OpenAlbumCommand = new Command(async() => 
            {
                _pic = await _camera.PickPhotoAsync();
                Source = ImageSource.FromStream(() => _pic.GetStream());
            });
            UploadCommand = new Command(() =>
            {
                if (_pic != null)
                {
                    var s = _pic.GetStream();
                    byte[] buffer = new byte[s.Length];
                    s.Read(buffer, 0, buffer.Length);


                    var r = _client.GetAsync(Route.GetAddress() + "/api/Access/Regist?UserName=321&Password=312").Result;
                    var r2 = _client.PostAsync(
                        Route.GetAddress() + "/" + nameof(Routes.api) + "/" + nameof(Routes.Picture) + "/" + nameof(Picture.Upload),
                        new MultipartFormDataContent()
                    {
                        { new ByteArrayContent(buffer), "picture", "picture.jpg" }
                    }).Result;
                }
            });
        }

        ImageSource source;
        public ImageSource Source { get 
            {
                return source;
            } 
            set 
            {
                Status = true;
                SetProperty(ref source, value);
               
            } 
        }
        bool status;

        public bool Status
        {
            get { return _pic != null; }
            set { SetProperty(ref status, value); }

        }

        public ICommand OpenWebCommand { get; }

        /// <summary>
        /// 相册
        /// </summary>
        public ICommand OpenAlbumCommand { get; }

        /// <summary>
        /// 相机
        /// </summary>
        public ICommand OpenCameraCommand { get; }

        /// <summary>
        /// 上传
        /// </summary>
        public ICommand UploadCommand { get; }
    }
}
using IntelliMobile.Services;
using IntelliMobile.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using IntelliMobile.Models;
using IntelliMobile.API;
using MyOpenCv.API;
using IntelliHttpClient;
namespace IntelliMobile
{

    public partial class App : Application
    {
        public App()
        {
            
            InitializeComponent();
            //注册图像处理实例
            DependencyService.Register<IImageFilter, CvImageFilter>();
            DependencyService.Register<I2ImageFilter, Cv2ImageFilter>();
            DependencyService.Register<ICamera, CameraBase>();
            DependencyService.Register<IHttp, Http>();
            DependencyService.RegisterSingleton(new MockDataStore());
            IDataStore<Item> q2 = DependencyService.Get<MockDataStore>();
            IDataStore<Item> d2 = DependencyService.Resolve<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

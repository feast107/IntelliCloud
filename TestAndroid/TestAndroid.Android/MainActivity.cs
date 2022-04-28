using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;


namespace TestAndroid.Droid
{
    [Activity(Label = "TestAndroid", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static Java.IO.File _file;
        public static Java.IO.File _dir;

        protected override void OnCreate(Bundle savedInstanceState)
        {

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(savedInstanceState);
    


            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());


        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }




        private void initPhotoError()
        {
            // android 7.0系统解决拍照的问题
            StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
            StrictMode.SetVmPolicy(builder.Build());
            builder.DetectFileUriExposure();
        }



        public static bool isExternalStorageDocument(Android.Net.Uri uri)
        {
            return "com.android.externalstorage.documents".Equals(uri.Authority);
        }
        public static bool isDownloadsDocument(Android.Net.Uri uri)
        {
            return "com.android.providers.downloads.documents".Equals(uri.Authority);
        }
        public static bool isMediaDocument(Android.Net.Uri uri)
        {
            return "com.android.providers.media.documents".Equals(uri.Authority);
        }
        public static bool isGooglePhotosUri(Android.Net.Uri uri)
        {
            return "com.google.android.apps.photos.content".Equals(uri.Authority);
        }

    }
}
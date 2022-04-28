using System;
using System.ComponentModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using IntelliMobile.API;
namespace IntelliMobile.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Browser.OpenAsync("https://106.14.32.120:5000");
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            
        }
    }
}
using IntelliMobile.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace IntelliMobile.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}
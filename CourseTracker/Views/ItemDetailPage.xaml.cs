using CourseTracker.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace CourseTracker.Views
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
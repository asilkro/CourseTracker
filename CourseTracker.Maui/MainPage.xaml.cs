using System.Diagnostics;
using CourseTracker.Maui.ViewModels;
using CourseTracker.Maui.Views;
using CourseTracker.Maui.Factories;
using CourseTracker.Maui.Services;

namespace CourseTracker.Maui
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageVM(); // I don't think this is used but it could be later.
        }

    }
}

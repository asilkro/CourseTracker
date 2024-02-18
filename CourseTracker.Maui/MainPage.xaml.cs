using CourseTracker.Maui.ViewModels;

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

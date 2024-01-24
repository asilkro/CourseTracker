using System.Diagnostics;
using CourseTracker.Maui.ViewModels;
using CourseTracker.Maui.Views;

namespace CourseTracker.Maui
{
    public partial class MainPage : FlyoutPage
    {
        MainPageVM viewModel;

        public MainPage()
        {
            InitializeComponent();
            viewModel = new MainPageVM();
            BindingContext = viewModel;
        }

        private void FlyoutButton_OnClicked(object? sender, EventArgs e)
        {
            var button = (Button) sender!;
            switch (button.Text)
            {
                case "Home":
                    Navigation.PushAsync(new Homepage());
                    break;
                case "Courses":
                    Navigation.PushAsync(new ListCourses());
                    break;
                case "Terms":
                    Navigation.PushAsync(new ListTerms());
                    break;
                case "Add Course":
                    Navigation.PushAsync(new AddCourses());
                    break;
                case "Add Term":
                    Navigation.PushAsync(new AddTerms());
                    break;
                case "Edit Course":
                    Navigation.PushAsync(new EditCourses());
                    break;
                case "Edit Term":
                    Navigation.PushAsync(new EditTerms());
                    break;
                default:
                    Debug.WriteLine($"Unknown button clicked: {button.Text}");
                    break;
            }
        }
    }

}

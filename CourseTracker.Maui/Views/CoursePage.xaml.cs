using CourseTracker.Maui.ViewModels;

namespace CourseTracker.Maui.Views;

public partial class CoursePage : ContentPage
{
    CourseVM viewModel;

    public CoursePage()
    {
        InitializeComponent();
        BindingContext = viewModel = new CourseVM();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel.OnAppearing();
    }
}
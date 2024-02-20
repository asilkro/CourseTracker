using CourseTracker.Maui.ViewModels;

namespace CourseTracker.Maui.Views;

public partial class UserGuide : ContentPage
{
    private readonly UserGuideVM viewModel;
    public UserGuide()
    {
        InitializeComponent();
        BindingContext = viewModel = new UserGuideVM();
    }
}
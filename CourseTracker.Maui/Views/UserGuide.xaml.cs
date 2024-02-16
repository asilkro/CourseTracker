using CourseTracker.Maui.ViewModels;

namespace CourseTracker.Maui.Views;

public partial class UserGuide : ContentPage
{
    private UserGuideVM viewModel;
    public UserGuide()
	{
		InitializeComponent();
		BindingContext = viewModel = new UserGuideVM();
	}
}
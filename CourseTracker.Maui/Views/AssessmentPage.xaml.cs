using CourseTracker.Maui.ViewModels;

namespace CourseTracker.Maui.Views;

public partial class AssessmentPage : ContentPage
{
	readonly AssessmentVM viewModel;

	public AssessmentPage()
	{
		InitializeComponent();
        BindingContext = viewModel = new AssessmentVM();
	}

	protected override void OnAppearing()
	{
        base.OnAppearing();
        viewModel.OnAppearing();
    }

}
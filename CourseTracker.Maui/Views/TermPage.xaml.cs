using CourseTracker.Maui.ViewModels;

namespace CourseTracker.Maui.Views;

public partial class TermPage : ContentPage
{
	TermVM viewModel;
	
    public TermPage()
	{
		InitializeComponent();
		BindingContext = viewModel = new TermVM();
        termCourseCountEntry.IsReadOnly = true; // Function updates this
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel.OnAppearing();
    }
}
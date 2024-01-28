using System.Diagnostics;
using CourseTracker.Maui.ViewModels;
using CourseTracker.Maui.Services;

namespace CourseTracker.Maui.Views;

public partial class EditCourses : ContentPage
{
	EditCoursesVM viewModel;
    readonly Connection database = new Connection();
    public EditCourses()
	{
		InitializeComponent();
		viewModel = new EditCoursesVM();
		BindingContext = viewModel;
	}

    private void SubmitButton_Clicked(object sender, EventArgs e)
    {
        Debug.WriteLine(sender + " triggered this.");
    }

    private async void CancelButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
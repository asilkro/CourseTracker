using System.Diagnostics;
using CourseTracker.Maui.ViewModels;

namespace CourseTracker.Maui.Views;

public partial class AddCourses : ContentPage
{
	AddCoursesVM viewModel;

	public AddCourses()
	{
		InitializeComponent();
		viewModel = new AddCoursesVM();
		BindingContext = viewModel;
	}

    private void SubmitButton_Clicked(object sender, EventArgs e)
    {
        Console.WriteLine(sender + " triggered this.");
    }

    private void CancelButton_Clicked(object sender, EventArgs e)
    {
        Console.WriteLine(sender + " triggered this.");
    }
}
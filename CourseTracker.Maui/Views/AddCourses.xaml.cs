using System.Diagnostics;
using CourseTracker.Maui.Factories;
using CourseTracker.Maui.Models;
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

        var factory = new CourseFactory();
        factory.CreateCourse(viewModel, out var errorMessage);
    }



    private async void CancelButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
using System.Diagnostics;
using CourseTracker.Maui.Factories;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Services;
using CourseTracker.Maui.ViewModels;

namespace CourseTracker.Maui.Views;

public partial class AddCourses : ContentPage
{
	AddCoursesVM viewModel;
    readonly Connection database = new Connection();

    public AddCourses()
	{
		InitializeComponent();
		viewModel = new AddCoursesVM();
		BindingContext = viewModel;
	}

    private async void SubmitButton_Clicked(object sender, EventArgs e)
    {
        Debug.WriteLine(sender + " triggered this.");

        var factory = new CourseFactory(database);
        var course = (factory.CreateCourse(viewModel, out var errorMessage));
        await database.InsertAsync<Course>(course);
    }


    private async void CancelButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
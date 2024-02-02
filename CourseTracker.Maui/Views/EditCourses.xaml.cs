using System.Diagnostics;
using CourseTracker.Maui.ViewModels;
using CourseTracker.Maui.Services;
using CourseTracker.Maui.Models;

namespace CourseTracker.Maui.Views;

public partial class EditCourses : ContentPage
{
	EditCoursesVM viewModel;
    readonly Connection database = new();
    public EditCourses()
	{
		InitializeComponent();
        viewModel = new EditCoursesVM();
        this.BindingContext = viewModel;
	}

    public EditCourses(Course? course)
    {
        InitializeComponent();
        viewModel = new EditCoursesVM();
        this.BindingContext = viewModel;
        if (course != null)
        {
            setCourse(course);
        }
    }

    private void setCourse(Course course)
    {
        viewModel.Course = course;
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
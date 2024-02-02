using System.Diagnostics;
using CourseTracker.Maui.ViewModels;
using CourseTracker.Maui.Services;
using CourseTracker.Maui.Models;

namespace CourseTracker.Maui.Views;

public partial class CoursePage : ContentPage
{
    CourseVM viewModel;
    readonly Connection database = new();
    public CoursePage()
    {
        InitializeComponent();
        viewModel = new CourseVM();
        this.BindingContext = viewModel;
    }

    public CoursePage(int courseId = 0)
    {
        InitializeComponent();
        viewModel = new CourseVM(courseId);
        BindingContext = viewModel;
        viewModel.InitializeAsync();
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
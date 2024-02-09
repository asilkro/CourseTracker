using System.Diagnostics;
using CourseTracker.Maui.ViewModels;
using CourseTracker.Maui.Services;
using CourseTracker.Maui.Models;

namespace CourseTracker.Maui.Views;

public partial class CoursePage : ContentPage
{
    CourseVM viewModel;
    readonly Connection database = new();
    int nextCourseId = TrackerDb.GetNextAutoIncrementID("Course");
    public CoursePage()
    {
        InitializeComponent();
        viewModel = new CourseVM();
        this.BindingContext = viewModel;
        courseIdEntry.Text = nextCourseId.ToString();
        courseIdEntry.IsReadOnly = true;
    }

    public CoursePage(Course course)
    {
        InitializeComponent();
        viewModel = new CourseVM(course);
        BindingContext = viewModel;
        courseIdEntry.Text = course.CourseId.ToString();
        courseIdEntry.IsReadOnly = true;
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
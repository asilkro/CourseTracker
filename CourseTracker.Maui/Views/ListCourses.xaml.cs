using CourseTracker.Maui.Models;
using CourseTracker.Maui.ViewModels;
using CourseTracker.Maui.Services;
using CourseTracker.Maui.Data;

namespace CourseTracker.Maui.Views;

public partial class ListCourses : ContentPage
{
    private ListCoursesVM viewModel;
    SQLiteAsyncConnection _database;

    public ListCourses()
    {
        InitializeComponent();
        BindingContext = viewModel = new ListCoursesVM();
    }

    private void CourseListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is Course selectedCourse)
        {
            ShowActionSheet(selectedCourse);
        }

        ((ListView)sender).SelectedItem = null;
    }

    private async void ShowActionSheet(Course course)
    {
        string action = await DisplayActionSheet("Course Actions", "Cancel", null, "Edit Course", "Delete Course");
        switch (action)
        {
            case "Edit Course":
                await Shell.Current.GoToAsync($"{nameof(CoursePage)}?{nameof(CourseVM.EditCourseId)}={course.CourseId}");
                break;
            case "Delete Course":
                await CourseDB.RemoveCourseAsync(course);
                break;
            default:
                break;
        }
    }
}
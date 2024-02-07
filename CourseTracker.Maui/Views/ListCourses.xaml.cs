using CourseTracker.Maui.Models;
using CourseTracker.Maui.ViewModels;
using CourseTracker.Maui.Services;
using System.Collections.ObjectModel;
using CourseTracker.Maui.Factories;

namespace CourseTracker.Maui.Views;

public partial class ListCourses : ContentPage
{
    private ListCoursesVM viewModel;
    private Connection _database;

    public ListCourses()
    {
        InitializeComponent();
        viewModel = new ListCoursesVM();
        BindingContext = viewModel;
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
                NavigateToEditCourseASync(course);
                break;
            case "Delete Course":
                RemoveCourseAsync(course);
                break;
            default:
                break;
        }
    }

    private async void NavigateToEditCourseASync(Course course)
    {
        await Navigation.PushAsync(new CoursePage(course));
    }

    private async void RemoveCourseAsync(Course course)
    {
        var result = await DisplayAlert("Delete Course", $"Are you sure you want to delete {course.CourseName}?", "Yes", "No");
        if (result)
        {
            if (_database == null)
            {
                _database = new Connection();
                _database.GetAsyncConnection();
            }
            await _database.DeleteAsync(course);
            await viewModel.LoadCourses();
        }
    }

    private async Task InitializeDataAsync()
    {
        if (_database == null)
        {
            _database = new Connection();
            _database.GetAsyncConnection();
        }

        var list = await _database.Table<Course>();
        courseListView.ItemsSource = list;
    }

}
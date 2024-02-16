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
            viewModel.ShowActionSheet(selectedCourse);
        }

        ((ListView)sender).SelectedItem = null;
    }


}
using CourseTracker.Maui.Models;
using CourseTracker.Maui.ViewModels;

namespace CourseTracker.Maui.Views;

public partial class ListCourses : ContentPage
{
    private readonly ListCoursesVM viewModel;

    public ListCourses()
    {
        InitializeComponent();
        BindingContext = viewModel = new ListCoursesVM();
    }

    public void CourseListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is Course selectedCourse)
        {
            viewModel.ShowActionSheet(selectedCourse);
        }
        ((ListView)sender).SelectedItem = null;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        viewModel.OnAppearing();
    }
}
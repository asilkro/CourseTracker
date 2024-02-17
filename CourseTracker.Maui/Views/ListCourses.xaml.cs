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

}
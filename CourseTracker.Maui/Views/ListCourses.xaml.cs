using CourseTracker.Maui.Models;
using CourseTracker.Maui.ViewModels;

namespace CourseTracker.Maui.Views;

public partial class ListCourses : ContentPage
{
	ListCoursesVM viewModel;

	public ListCourses()
	{
		InitializeComponent();
		viewModel = new ListCoursesVM();
		BindingContext = viewModel;

		// This is how you'll add dummmy data to the list
		//viewModel.Courses.Add(new bullshit);
	}
}
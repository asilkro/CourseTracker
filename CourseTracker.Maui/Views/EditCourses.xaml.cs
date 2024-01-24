using CourseTracker.Maui.ViewModels;

namespace CourseTracker.Maui.Views;

public partial class EditCourses : ContentPage
{
	EditCoursesVM viewModel;

	public EditCourses()
	{
		InitializeComponent();
		viewModel = new EditCoursesVM();
		BindingContext = viewModel;
	}

    private void SubmitButton_Clicked(object sender, EventArgs e)
    {
        Console.WriteLine(sender + " triggered this.");
    }

    private void CancelButton_Clicked(object sender, EventArgs e)
    {
        Console.WriteLine(sender + " triggered this.");
    }
}
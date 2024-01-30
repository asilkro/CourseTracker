using CourseTracker.Maui.Models;
using CourseTracker.Maui.ViewModels;
using CourseTracker.Maui.Services;

namespace CourseTracker.Maui.Views;

public partial class ListCourses : ContentPage
{
	ListCoursesVM viewModel;
    readonly Connection _database;
    public ListCourses()
	{
		InitializeComponent();
		viewModel = new ListCoursesVM();
		BindingContext = viewModel;
		if (_database == null)
		{
            _database = new Connection();
			_database.GetAsyncConnection();
        }
		var list = _database.Table<Course>();
		courseListView.ItemsSource = (System.Collections.IEnumerable)list;
	}

	public async void OnAddCourseClicked(object sender, EventArgs e)
	{
        await Navigation.PushAsync(new AddCourses());
    }

	private async void OnEditCourseClicked(object sender, EventArgs e)
	{
        var selected = (Course)courseListView.SelectedItem;
        if (selected == null) { return; }
		await Navigation.PushAsync(new EditCourses(selected));
    }

	private async void OnDeleteCourseClicked(object sender, EventArgs e)
	{
		var selected = (Course)courseListView.SelectedItem;
		if (selected == null) { return; }
		var result = await DisplayAlert("Delete Course", "Are you sure you want to delete this course?", "Yes", "No");
		if (result)
		{
            await _database.DeleteAsync<Course>(selected);
            await Navigation.PopAsync();
        }
	}
}
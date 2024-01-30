using CourseTracker.Maui.Models;
using CourseTracker.Maui.ViewModels;
using CourseTracker.Maui.Services;
using System.Collections.ObjectModel;
using CourseTracker.Maui.Factories;

namespace CourseTracker.Maui.Views;

public partial class ListCourses : ContentPage
{
	ListCoursesVM viewModel;
	private readonly CourseFactory _courseFactory;
	private readonly Connection _connection;
	public ObservableCollection<Course> Courses { get; private set; }
    public ListCourses(CourseFactory courseFactory)
	{
		InitializeComponent();
        _courseFactory = courseFactory;
        viewModel = new ListCoursesVM();
		BindingContext = viewModel;
	}

	protected override async void OnAppearing()
	{
        base.OnAppearing();
        await LoadCoursesFromDatabase();
    }

	private async Task LoadCoursesFromDatabase()
	{
		try
		{
			var courses = await _courseFactory.GetAllObjects();
			Courses = new ObservableCollection<Course>(await _courseFactory.GetAllObjects());
			OnPropertyChanged(nameof(Courses));
		}
		catch (Exception ex)
		{
            await DisplayAlert("Error", $"Error while loading courses: {ex.Message}", "OK");
        }
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
            //TODO add await Delete method
            await Navigation.PopAsync();
        }
	}

}
using CourseTracker.Maui.Models;
using CourseTracker.Maui.ViewModels;
using CourseTracker.Maui.Services;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace CourseTracker.Maui.Views;

public partial class ListInstructors : ContentPage
{
	private ListInstructorsVM viewModel;
	private Connection _database;
    private ObservableCollection<Instructor> _instructors = new();

	public ListInstructors()
	{
		InitializeComponent();
		BindingContext = new ListInstructorsVM();
	}

    private async Task InitializeDataAsync()
    {
        if (_database == null)
        {
            _database = new Connection();
            _database.GetAsyncConnection();
        }

        var list = await _database.Table<Instructor>();
        instructorListView.ItemsSource = list;
    }


    private void InstructorListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is Instructor selectedInstructor)
        {
            ShowActionSheet(selectedInstructor);
        }

        ((ListView)sender).SelectedItem = null;
    }
    private async void ShowActionSheet(Instructor instructor)
    {
        string action = await DisplayActionSheet("Instructor Actions", "Cancel", null, "Edit Instructor", "Delete Instructor");
        switch (action)
        {
            case "Edit Instructor":
                NavigateToEditInstructorASync(instructor);
                break;
            case "Delete Instructor":
                RemoveInstructorAsync(instructor);
                break;
            default:
                break;
        }
    }

    private async Task RemoveInstructorAsync(Instructor instructor)
    {
        var result = await DisplayAlert("Delete Instructor", $"Are you sure you want to delete {instructor.InstructorName}?", "Yes", "No");
        if (result)
        {
            await _database.DeleteAsync(instructor);
            await LoadInstructors();
        }
    }

    private async Task NavigateToEditInstructorASync(Instructor instructor) // workaround for not being able to use await Nav
    {
        await Navigation.PushAsync(new InstructorPage(instructor));
    }

    private async Task LoadInstructors()
    {
        try
        {
            _database = _database ?? new Connection();
            var updatedInstructorsList = await _database.Table<Instructor>();
            _instructors.Clear();
            foreach (var instructor in updatedInstructorsList)
            {
                _instructors.Add(instructor);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Issue loading terms: " + ex.Message);
        }
    }
}
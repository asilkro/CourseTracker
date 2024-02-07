using CourseTracker.Maui.Models;
using CourseTracker.Maui.ViewModels;
using CourseTracker.Maui.Services;
using System.Collections.ObjectModel;
using CourseTracker.Maui.Factories;

namespace CourseTracker.Maui.Views;

public partial class ListAssessments : ContentPage
{
	private ListAssessmentsVM viewModel;
	private Connection _database;

	public ListAssessments()
	{
		InitializeComponent();
		BindingContext = new ListAssessmentsVM();
	}

    private void AssessmentListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is Assessment selectedAssessment)
        {
            ShowActionSheet(selectedAssessment);
        }

    ((ListView)sender).SelectedItem = null;
    }

    private async void ShowActionSheet(Assessment assessment)
    {
        string action = await DisplayActionSheet("Assessment Actions", "Cancel", null, "Edit Assessment", "Delete Assessment");
        switch (action)
        {
            case "Edit Assessment":
                NavigateToEditAssessmentASync(assessment);
                break;
            case "Delete Assessment":
                RemoveAssessmentAsync(assessment);
                break;
            default:
                break;
        }
    }

    private async void NavigateToEditAssessmentASync(Assessment assessment)
    {
        await Navigation.PushAsync(new AssessmentPage(assessment));
    }

private async void RemoveAssessmentAsync(Assessment assessment)
    {
        var result = await DisplayAlert("Delete Assessment", $"Are you sure you want to delete {assessment.AssessmentName}?", "Yes", "No");
        if (result)
        {
            if (_database == null)
            {
                _database = new Connection();
                _database.GetAsyncConnection();
            }
            await _database.DeleteAsync(assessment);
            await viewModel.LoadAssessments();
        }
    }


}
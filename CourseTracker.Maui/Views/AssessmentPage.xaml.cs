using System.Diagnostics;
using CourseTracker.Maui.ViewModels;
using CourseTracker.Maui.Services;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Factories;
using System.ComponentModel.DataAnnotations.Schema;
using static CourseTracker.Maui.Factories.AssessmentFactory;

namespace CourseTracker.Maui.Views;

public partial class AssessmentPage : ContentPage
{
	AssessmentVM viewModel;
	readonly Connection database = new();
    readonly AssessmentFactory _assessmentFactory;
    int nextAssessmentId = TrackerDb.GetNextAutoIncrementID("Assessment");
    readonly int previousId;

	public AssessmentPage()
	{
		InitializeComponent();
		viewModel = new AssessmentVM();
		this.BindingContext = viewModel;
		assessmentIdEntry.Text = nextAssessmentId.ToString();
        assessmentIdEntry.IsReadOnly = true;
	}

	public AssessmentPage(Assessment assessment)
	{
        InitializeComponent();
        viewModel = new AssessmentVM(assessment);
        BindingContext = viewModel;
        previousId = assessment.RelatedCourseId;
        
        #region Populate fields with existing data
        assessmentIdEntry.Text = assessment.AssessmentId.ToString();
        assessmentNameEntry.Text = assessment.AssessmentName;
        assessmentTypePicker.SelectedItem = assessment.AssessmentType;
        assessmentStartDatePicker.Date = assessment.AssessmentStartDate;
        assessmentEndDatePicker.Date = assessment.AssessmentEndDate;
        relatedCourseIdEntry.Text = assessment.RelatedCourseId.ToString();
        notificationsEnabledSwitch.IsToggled = assessment.NotificationsEnabled;
        #endregion
        
        assessmentIdEntry.IsReadOnly = true;
    }

    private async void submitButton_Clicked(object sender, EventArgs e)
    {
        var assessmentResult = await _assessmentFactory.CreateAssessmentAsync(viewModel);

        if (assessmentResult?.Assessment == null)
        {
            Debug.WriteLine("Error creating assessment: " + assessmentResult?.ErrorMessage);
            return;
        }

        var assessment = assessmentResult.Assessment;
        var searchId = assessment.AssessmentId;
        
        try
        {
            var exists = await database.FindAsync<Assessment>(searchId);

            if (exists == null)
            {
                await _assessmentFactory.InsertAssessmentAndUpdateCourseCount(assessment);
            }
            else
            {
                await _assessmentFactory.LowerCourseAssessmentCount(previousId);
                await _assessmentFactory.UpdateAssessmentAndUpdateCourseCount(assessment);
            }
            bool anotherAssessmentWanted = await DisplayAlert("Assessment Saved", "Would you like to add another assessment?", "Yes", "No");
            if (anotherAssessmentWanted) 
            {
                await Shell.Current.GoToAsync(nameof(AssessmentPage));
            }
            else
            {
                await Shell.Current.GoToAsync("//homepage");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error submitting data: " + ex.Message);
        }
    }

    private async void cancelButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//homepage");
    }

}
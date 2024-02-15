using System.Diagnostics;
using CourseTracker.Maui.ViewModels;
using CourseTracker.Maui.Services;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Data;
using CourseTracker.Maui.Factories;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseTracker.Maui.Views;

public partial class AssessmentPage : ContentPage
{
	AssessmentVM viewModel;

	public AssessmentPage()
	{
		InitializeComponent();
        BindingContext = viewModel = new AssessmentVM();
        
        assessmentIdEntry.IsReadOnly = true;
	}

    private async void submitButton_Clicked(object sender, EventArgs e)
    {
/*        var assessmentResult = await _assessmentFactory.CreateAssessment(viewModel, out string errorMessage);

        if (assessmentResult?.Assessment == null)
        {
            Debug.WriteLine("Error creating assessment: " + assessmentResult?.ErrorMessage);
            return;
        }

        var assessment = assessmentResult.Assessment;
        var searchId = assessment.AssessmentId;*/

        try
        {
/*            var exists = await database.FindAsync<Assessment>(searchId);

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
            }*/
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
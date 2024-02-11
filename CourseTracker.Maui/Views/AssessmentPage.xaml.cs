using System.Diagnostics;
using CourseTracker.Maui.ViewModels;
using CourseTracker.Maui.Services;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Factories;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseTracker.Maui.Views;

public partial class AssessmentPage : ContentPage
{
	AssessmentVM viewModel;
	readonly Connection database = new();
    readonly AssessmentFactory _assessmentFactory;
    int nextAssessmentId = TrackerDb.GetNextAutoIncrementID("Assessment");

	public AssessmentPage()
	{
		InitializeComponent();
		viewModel = new AssessmentVM();
		this.BindingContext = viewModel;
		assessmentIdEntry.Text = nextAssessmentId.ToString();
	}

	public AssessmentPage(Assessment assessment)
	{
        InitializeComponent();
        viewModel = new AssessmentVM(assessment);
        BindingContext = viewModel;
		assessmentIdEntry.Text = assessment.AssessmentId.ToString();
    }

    private async void submitButton_Clicked(object sender, EventArgs e)
    {
        var assessment = _assessmentFactory.CreateAssessmentAsync(viewModel);
        if (assessment == null)
        {
            Debug.WriteLine("Error creating assessment");
            return;
        }
        try
		{

            int exists = await database.FindAsync<Assessment>(assessment);

            switch (exists)
            {
                case null:
                    await database.InsertAsync<Assessment>(assessment);
                    break;
                default:
                    await database.UpdateAsync<Assessment>(assessment);
                    break;
            }
        }
        catch (Exception ex)
        {
               Debug.WriteLine("Error submitting data: " + ex.Message);
        }

    }
}
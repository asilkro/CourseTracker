using System.Diagnostics;
using CourseTracker.Maui.ViewModels;
using CourseTracker.Maui.Services;
using CourseTracker.Maui.Models;

namespace CourseTracker.Maui.Views;

public partial class AssessmentPage : ContentPage
{
	AssessmentVM viewModel;
	readonly Connection database = new();
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

    private void submitButton_Clicked(object sender, EventArgs e)
    {
		//TODO: finish save logic
    }

    private void cancelButton_Clicked(object sender, EventArgs e)
    {
		Navigation.PopToRootAsync();
    }
}
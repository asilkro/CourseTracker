using System.Diagnostics;
using CourseTracker.Maui.Factories;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Services;

namespace CourseTracker.Maui.Views;

public partial class Homepage : ContentPage
{
    TrackerDb _trackerDb = new();
	Connection connection = new();

    public Homepage()
	{
		InitializeComponent();
        StartDB();
	}

	private async Task StartDB()
	{
        _trackerDb ??= new TrackerDb();
        await TrackerDb.Initialize();
    }

    private async void LoadSampleData(Term demoTerm,
        Course demoCourse, Assessment demoAssessment1, Assessment demoAssessment2)
    {
        if (connection == null)
        {
            connection = new Connection();
            connection.GetAsyncConnection(); // Ensure this method sets up the connection properly
        }

        var termId = await connection.InsertAndGetIdAsync<Term>(demoTerm);

        demoCourse.TermId = termId;

        var courseId = await connection.InsertAndGetIdAsync<Course>(demoCourse);

        demoAssessment1.RelatedCourseId = courseId;
        demoAssessment2.RelatedCourseId = courseId;

        await connection.InsertAsync<Assessment>(demoAssessment1);
        await connection.InsertAsync<Assessment>(demoAssessment2);

        await DisplayAlert(
            "Sample Data Loaded",
            "Sample data has been loaded",
            "OK");
    }


    private async void loadButton_Clicked(object sender, EventArgs e)
    {
		if (sender == loadSampleDataButton)
		{
            if (_trackerDb == null)
            {
                await StartDB();
            }
			LoadSampleData(demoTerm, demoCourse, demoOA, demoPA);
		}

    }

    #region Sample Data For Evaluation
    private readonly Term demoTerm = new Term
    {
        TermId = 999, // Should be a safe number to use for testing
        TermName = "Demo Term",
        TermStart = new DateTime(2024, 01, 01),
        TermEnd = new DateTime(2024, 06, 30),
        NotificationsEnabled = true,
        CourseCount = 0
    };

    private Course demoCourse = new Course
    {
        CourseId = 555, // Should be a safe number to use for testing
        CourseName = "C6 Requirements re: C3 Course",
        CourseStatus = "In Progress",
        CourseStart = new DateTime(2024, 01, 01),
        CourseEnd = new DateTime(2024, 06, 30),
        InstructorEmail = "anika.patel@strimeuniversity.edu",
        InstructorPhone = "555-123-4567",
        InstructorName = "Anika Patel",
        CourseNotes = "This addresses requirement C6",
        NotificationsEnabled = true,
        CourseAssessmentCount = 2
    };

    private readonly Assessment demoOA = new Assessment
    {
        AssessmentId = 888, // Should be a safe number to use for testing
        AssessmentName = "C6 OA",
        AssessmentType = "Objective",
        AssessmentStartDate = new DateTime(2024, 01, 01),
        AssessmentEndDate = new DateTime(2024, 02, 29),
        RelatedCourseId = 555,
        NotificationsEnabled = true
    };

    private readonly Assessment demoPA = new Assessment
    {
        AssessmentId = 777, // Should be a safe number to use for testing
        AssessmentName = "C6 PA",
        AssessmentType = "Performance",
        AssessmentStartDate = new DateTime(2024, 03, 01),
        AssessmentEndDate = new DateTime(2024, 04, 30),
        RelatedCourseId = 555,
        NotificationsEnabled = true
    };

    #endregion

    private async void resetDbButton_Clicked(object sender, EventArgs e)
    {
        await TrackerDb.ResetDatabaseFileAsync();
    }
}
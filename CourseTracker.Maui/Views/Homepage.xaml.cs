using System.Diagnostics;
using System.Text.RegularExpressions;
using CourseTracker.Maui.Factories;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Services;
using CourseTracker.Maui.Supplemental;

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

    private async Task LoadSampleDataAsync(Term demoTerm, Course demoCourse, Assessment demoAssessment1, Assessment demoAssessment2)
    {
        if (connection == null)
        {
            connection = new ();
            connection.GetAsyncConnection(); // Ensure this method sets up the connection properly and is awaited
        }
        var existing = await Validation.DataExistsInTables(connection);
        if (existing)
        {
            await DisplayAlert("Table Already Has Data", "Table data has already been loaded. You should reset the database to avoid errors with sample data creation.", "OK");
            return;
        }

        try
        {
            var termId = await connection.InsertAndGetIdAsync(demoTerm);
            demoCourse.TermId = termId;
            var courseId = await connection.InsertAndGetIdAsync(demoCourse);

            demoAssessment1.RelatedCourseId = courseId;
            demoAssessment2.RelatedCourseId = courseId;

            await connection.InsertAsync(demoAssessment1);
            await connection.InsertAsync(demoAssessment2);

            await DisplayAlert("Sample Data Loaded", "Sample data has been loaded successfully. Please relaunch the application to complete setup.", "OK");
            loadSampleDataButton.IsEnabled = false; // Assume this exists in your context to disable the button

        }
        catch (Exception ex)
        {
            await DisplayAlert("Sample Data Error", "$There was an error while loading sample data: {ex.Message}", "Ok");
        }
    }


    private async void loadButton_Clicked(object sender, EventArgs e)
    {
		if (sender == loadSampleDataButton)
		{
            if (_trackerDb == null)
            {
                await StartDB();
            }
			await LoadSampleDataAsync(demoTerm, demoCourse, demoOA, demoPA);
		}

    }

    #region Sample Data For Evaluation
    private readonly Term demoTerm = new Term
    {
        TermId = GetNextAutoIncrementID("Term") ?? 1500,
        TermName = "Demo Term",
        TermStart = new DateTime(2024, 01, 01),
        TermEnd = new DateTime(2024, 06, 30),
        NotificationsEnabled = true,
        CourseCount = 0
    };

    private readonly Term demoTerm2 = new Term
    {
        TermId = GetNextAutoIncrementID("Term") ?? 1500,
        TermName = "Another Term",
        TermStart = new DateTime(2023, 01, 01),
        TermEnd = new DateTime(2023, 06, 30),
        NotificationsEnabled = true,
        CourseCount = 0
    };

    private Course demoCourse = new Course
    {
        CourseId = GetNextAutoIncrementID("Course") ?? 1600,
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
        AssessmentId = GetNextAutoIncrementID("Assessment") ?? 1700,
        AssessmentName = "C6 OA",
        AssessmentType = "Objective",
        AssessmentStartDate = new DateTime(2024, 01, 01),
        AssessmentEndDate = new DateTime(2024, 02, 29),
        RelatedCourseId = 555,
        NotificationsEnabled = true
    };

    private readonly Assessment demoPA = new Assessment
    {
        AssessmentId = GetNextAutoIncrementID("Assessment") ?? 1700,
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

    private static int? GetNextAutoIncrementID(string tableName)
    {
        switch (tableName)
        {
            case "Term":
                return TrackerDb.GetNextAutoIncrementID("Term");
            case "Course":
                return TrackerDb.GetNextAutoIncrementID("Course");
            case "Assessment":
                return TrackerDb.GetNextAutoIncrementID("Assessment");
            default:
                return null;
        }
    }
}
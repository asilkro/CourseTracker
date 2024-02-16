using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using CourseTracker.Maui.Factories;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Services;
using CourseTracker.Maui.Supplemental;
using Microsoft.Maui.Layouts;

namespace CourseTracker.Maui.Views;

public partial class Homepage : ContentPage
{
    #region Fields / Properties
    TrackerDb _trackerDb = new();
	Connection connection = new();
    #endregion

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

    private async void loadButton_Clicked(object sender, EventArgs e)
    {
		if (sender == loadSampleDataButton)
		{
            if (_trackerDb == null)
            {
                await StartDB();
            }
			await LoadSampleDataAsync();
		}

    }

    private async void resetDbButton_Clicked(object sender, EventArgs e)
    {
        //TODO: add confirmation to reset DB
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

    #region Sample Data For Evaluation
    private static int courseId = GetNextAutoIncrementID("Course") ?? 1600;

    private static readonly Term demoTerm = new()
    {
        TermId = GetNextAutoIncrementID("Term") ?? 1500,
        TermName = "Demo Term",
        TermStart = new DateTime(2024, 01, 01),
        TermEnd = new DateTime(2024, 06, 30),
        CourseCount = 1
    };

    private static readonly Term demoTerm2 = new()
    {
        TermId = GetNextAutoIncrementID("Term") ?? 1501,
        TermName = "Term One 2023",
        TermStart = new DateTime(2023, 01, 01),
        TermEnd = new DateTime(2023, 06, 30),
        CourseCount = 2
    };

    private static readonly Course demoCourse = new()
    {
        CourseId = courseId,
        CourseName = "Example Course for Evaluation",
        CourseStatus = "In Progress",
        CourseStart = new DateTime(2024, 01, 01),
        CourseEnd = new DateTime(2024, 06, 30),
        InstructorEmail = "anika.patel@strimeuniversity.edu",
        InstructorPhone = "555-123-4567",
        InstructorName = "Anika Patel",
        CourseNotes = "This addresses assessment requirement C6 re: C3",
        NotificationsEnabled = true,
        TermId = demoTerm.TermId,
        CourseAssessmentCount = 2
    };

    private static readonly Course demoCourse2 = new()
    {
        CourseId = GetNextAutoIncrementID("Course") ?? 1601,
        CourseName = "The Zed Pandemic",
        CourseStatus = "Completed",
        CourseStart = new DateTime(2023, 01, 01),
        CourseEnd = new DateTime(2023, 02, 28),
        InstructorEmail = "rickgrimes@uwalkers.edu",
        InstructorPhone = "555-987-6543",
        InstructorName = "Rick Grimes",
        CourseNotes = "This is a course that was completed and covered the fictional outbreak of a zombie disease",
        NotificationsEnabled = false,
        TermId = demoTerm2.TermId,
        CourseAssessmentCount = 1
    };

    private static readonly Course demoCourse3 = new()
    {
        CourseId = GetNextAutoIncrementID("Course") ?? 1602,
        CourseName = "Screen Writing for Community",
        CourseStatus = "Dropped",
        CourseStart = new DateTime(2023, 03, 01),
        CourseEnd = new DateTime(2023, 04, 30),
        InstructorEmail = "dharmon@greendale.edu",
        InstructorPhone = "555-424-1565",
        InstructorName = "Dan Harmon",
        CourseNotes = "This course was dropped due to a scheduling conflict with Dean Pelton.",
        NotificationsEnabled = false,
        TermId = demoTerm2.TermId,
        CourseAssessmentCount = 1
    };

    private static readonly Assessment demoOA = new Assessment
    {
        AssessmentId = GetNextAutoIncrementID("Assessment") ?? 1700,
        AssessmentName = "C6 OA",
        AssessmentType = "Objective",
        AssessmentStartDate = new DateTime(2024, 01, 01),
        AssessmentEndDate = new DateTime(2024, 02, 29),
        RelatedCourseId = courseId,
        NotificationsEnabled = true
    };
    
    private static readonly Assessment demoPA = new Assessment
    {
        AssessmentId = GetNextAutoIncrementID("Assessment") ?? 1701,
        AssessmentName = "C6 PA",
        AssessmentType = "Performance",
        AssessmentStartDate = new DateTime(2024, 03, 01),
        AssessmentEndDate = new DateTime(2024, 04, 30),
        RelatedCourseId = courseId,
        NotificationsEnabled = true
    };
    private static readonly Assessment demoOA2 = new Assessment
    {
        AssessmentId = GetNextAutoIncrementID("Assessment") ?? 1702,
        AssessmentName = "Zed Pandemic OA",
        AssessmentType = "Objective",
        AssessmentStartDate = new DateTime(2023, 01, 01),
        AssessmentEndDate = new DateTime(2023, 02, 28),
        RelatedCourseId = demoCourse2.CourseId,
        NotificationsEnabled = false
    };

    private static readonly Assessment demoPA2 = new Assessment
    {
        AssessmentId = GetNextAutoIncrementID("Assessment") ?? 1703,
        AssessmentName = "Community Script PA",
        AssessmentType = "Performance",
        AssessmentStartDate = new DateTime(2023, 03, 01),
        AssessmentEndDate = new DateTime(2024, 02, 29),
        RelatedCourseId = demoCourse3.CourseId,
        NotificationsEnabled = false
    };
    
    private async Task LoadSampleDataAsync()
    {
        if (connection == null)
        {
            connection = new();
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
            //First term, course for assessment C6 and C3 requirements
            var termId = await connection.InsertAndGetIdAsync(demoTerm);
            Debug.WriteLine("Inserted " + demoTerm.TermName);

            demoCourse.TermId = termId;
            var courseId = await connection.InsertAndGetIdAsync(demoCourse);
            Debug.WriteLine("Inserted " + demoCourse.CourseName);

            demoOA.RelatedCourseId = courseId;
            demoPA.RelatedCourseId = courseId;
            await connection.InsertAsync(demoOA);
            Debug.WriteLine("Inserted " + demoOA.AssessmentName);
            await connection.InsertAsync(demoPA);
            Debug.WriteLine("Inserted " + demoPA.AssessmentName);

            //Second term and courses to provide a more robust demo set of data for evaluator
            termId = await connection.InsertAndGetIdAsync(demoTerm2);
            Debug.WriteLine("Inserted " + demoTerm2.TermName);
            demoCourse2.TermId = termId;
            demoCourse3.TermId = termId;
            courseId = await connection.InsertAndGetIdAsync(demoCourse2);
            Debug.WriteLine("Inserted " + demoCourse2.CourseName);
            demoOA2.RelatedCourseId = demoCourse2.CourseId;
            courseId = await connection.InsertAndGetIdAsync(demoCourse3);
            Debug.WriteLine("Inserted " + demoCourse3.CourseName);
            demoPA2.RelatedCourseId = demoCourse3.CourseId;

            //TODO: add validation for insert during debug

            await DisplayAlert("Sample Data Loaded", "Sample data has been loaded successfully. Please relaunch the application to complete setup.", "OK");
            isSampleDataLoaded();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Sample Data Error", "$There was an error while loading sample data: " + ex.Message, "OK");
            return;
        }
    }

    public bool isSampleDataLoaded()
    {
        bool result = false;
        if (connection == null)
        {
            connection = new();
            connection.GetAsyncConnection();
        }
        var demotermexists = connection.FindAsync<Term>(demoTerm.TermId);
        var democourseexists = connection.FindAsync<Course>(demoCourse.CourseId);
        var demoOAexists = connection.FindAsync<Assessment>(demoOA.AssessmentId);
        var demoPAexists = connection.FindAsync<Assessment>(demoPA.AssessmentId);

        if (demotermexists != null && democourseexists != null && demoOAexists != null && demoPAexists != null)
        {
            result = true;
        }

        return result;
    }
    #endregion
}
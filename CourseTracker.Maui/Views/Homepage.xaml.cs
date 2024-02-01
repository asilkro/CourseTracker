using System.Diagnostics;
using CourseTracker.Maui.Factories;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Services;

namespace CourseTracker.Maui.Views;

public partial class Homepage : ContentPage
{
	TrackerDb _trackerDb = new TrackerDb();
	Connection connection = new Connection();

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

    private async void LoadSampleData(Term demoTerm, Instructor demoInstructor,
        Course demoCourse, Assessment demoAssessment1, Assessment demoAssessment2)
    {
        if (connection == null)
        {
            connection = new Connection();
            connection.GetAsyncConnection(); // Ensure this method sets up the connection properly
        }

        var termId = await connection.InsertAndGetIdAsync<Term>(demoTerm);
        var instructorId = await connection.InsertAndGetIdAsync<Instructor>(demoInstructor);

        demoCourse.TermId = termId;
        demoCourse.InstructorId = instructorId;

        var courseId = await connection.InsertAndGetIdAsync<Course>(demoCourse);

        demoAssessment1.RelatedCourseId = courseId;
        demoAssessment2.RelatedCourseId = courseId;

        await connection.InsertAsync<Assessment>(demoAssessment1);
        await connection.InsertAsync<Assessment>(demoAssessment2);

		await DisplayAlert("Sample Data Loaded", "Sample data has been loaded", "OK");
    }


    private async void loadButton_Clicked(object sender, EventArgs e)
    {
		if (sender == loadSampleDataButton)
		{
            if (_trackerDb == null)
            {
                await StartDB();
            }
			LoadSampleData(demoTerm, demoInstructor, demoCourse, demoOA, demoPA);
		}

    }

    #region Sample Data For Evaluation
    private readonly Term demoTerm = new Term
    {
        TermName = "Demo Term",
        TermStart = new DateTime(2024, 01, 01),
        TermEnd = new DateTime(2024, 06, 30),
        NotificationsEnabled = true,
        CourseCount = 0
    };

    private Course demoCourse = new Course
    {
        CourseName = "C6 Requirements re: C3 Course",
        CourseStatus = "In Progress",

        CourseStart = new DateTime(2024, 01, 01),
        CourseEnd = new DateTime(2024, 06, 30),
        CourseNotes = "This addresses requirement C6",
        NotificationsEnabled = true,
        CourseAssessmentCount = 2
    };

    private readonly Instructor demoInstructor = new Instructor
    {
        InstructorName = "Anika Patel",
        InstructorEmail = "anika.patel@strimeuniversity.edu",
        InstructorPhone = "555-123-4567"
    };

    private readonly Assessment demoOA = new Assessment
    {
        AssessmentName = "C6 OA",
        AssessmentType = "Objective",
        AssessmentStartDate = new DateTime(2024, 01, 01),
        AssessmentEndDate = new DateTime(2024, 02, 29),
        NotificationsEnabled = true
    };

    private readonly Assessment demoPA = new Assessment
    {
        AssessmentName = "C6 PA",
        AssessmentType = "Performance",
        AssessmentStartDate = new DateTime(2024, 03, 01),
        AssessmentEndDate = new DateTime(2024, 04, 30),
        NotificationsEnabled = true
    };
    #endregion
}
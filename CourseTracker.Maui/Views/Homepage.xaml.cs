using System.Diagnostics;
using CourseTracker.Maui.Data;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Services;
using CourseTracker.Maui.Supplemental;

namespace CourseTracker.Maui.Views;

public partial class Homepage : ContentPage
{
    #region Fields / Properties
    TrackerDb _trackerDb = new();
	Connection connection = new();
    CourseDB courseDB;
    TermsDB termsDB;
    AssessmentDB assessmentDB;
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
        bool confirmed = await DisplayAlert("Reset Database", "This will delete all data in the database. Are you sure you want to continue?", "Yes", "No");
        if (confirmed)
        {
            await TrackerDb.ResetDatabaseFileAsync();

        }  
    }

    private int? GetNextAutoIncrementID(string tableName)
    {
        switch (tableName)
        {
            case "Term":
                return termsDB.GetNextId().Result;
            case "Course":
                return courseDB.GetNextId().Result;
            case "Assessment":
                return assessmentDB.GetNextId().Result;
            default:
                return null;
        }
    }

    #region Sample Data For Evaluation

    private Term MakeDemoTerm()
    {
        Term demoTerm = new()
        {
            TermId = GetNextAutoIncrementID("Term") ?? 1500,
            TermName = "Demo Term",
            TermStart = new DateTime(2024, 01, 01),
            TermEnd = new DateTime(2024, 06, 30),
            CourseCount = 1
        };
        return demoTerm;
    }

    private Term MakeDemoTerm2()
    {
        Term demoTerm2 = new()
        {
            TermId = termsDB.GetNextId().Result,
            TermName = "Term One 2023",
            TermStart = new DateTime(2023, 01, 01),
            TermEnd = new DateTime(2023, 06, 30),
            CourseCount = 2
        };
        return demoTerm2;
    }

    private int GetNextCourseId()
    {
        int id = courseDB.GetNextId().Result;
        return id;
    }
    private int GetNextAssessmentId()
    {
        int id = assessmentDB.GetNextId().Result;
        return id;
    }
    private int GetNextTermId()
    {
        int id = termsDB.GetNextId().Result;
        return id;
    }

    private Course MakeDemoCourse()
    {
        Course course = new()
        {
            CourseId = GetNextCourseId(),
            CourseName = "Example Course for Evaluation",
            CourseStatus = "In Progress",
            CourseStart = new DateTime(2024, 01, 01),
            CourseEnd = new DateTime(2024, 06, 30),
            InstructorEmail = "anika.patel@strimeuniversity.edu",
            InstructorPhone = "555-123-4567",
            InstructorName = "Anika Patel",
            CourseNotes = "This addresses assessment requirement C6 re: C3",
            NotificationsEnabled = true,
            TermId = 1,
            CourseAssessmentCount = 0
        };

        return course;
    }

    private Course MakeDemoCourse2()
    {
        Course course = new()
        {
            CourseId = GetNextCourseId(),
            CourseName = "The Zed Pandemic",
            CourseStatus = "Completed",
            CourseStart = new DateTime(2023, 01, 01),
            CourseEnd = new DateTime(2023, 02, 28),
            InstructorEmail = "rickgrimes@uwalkers.edu",
            InstructorPhone = "555-987-6543",
            InstructorName = "Rick Grimes",
            CourseNotes = "This is a course that was completed and covered the fictional outbreak of a zombie disease",
            NotificationsEnabled = false,
            TermId = 2,
            CourseAssessmentCount = 1
        };
        return course;
    }

    private Course MakeDemoCourse3()
    {
        Course course = new()
        {
            CourseId = GetNextCourseId(),
            CourseName = "Screen Writing for Community",
            CourseStatus = "Dropped",
            CourseStart = new DateTime(2023, 03, 01),
            CourseEnd = new DateTime(2023, 04, 30),
            InstructorEmail = "dharmon@greendale.edu",
            InstructorPhone = "555-424-1565",
            InstructorName = "Dan Harmon",
            CourseNotes = "This course was dropped due to a scheduling conflict with Dean Pelton.",
            NotificationsEnabled = false,
            TermId = 2,
            CourseAssessmentCount = 1
        };
        return course;
    }

    private Assessment MakeDemoOA()
    {
        Assessment demoOA = new()
        {
            AssessmentId = GetNextAssessmentId(),
            AssessmentName = "C6 OA",
            AssessmentType = "Objective",
            AssessmentStartDate = new DateTime(2024, 01, 01),
            AssessmentEndDate = new DateTime(2024, 02, 29),
            RelatedCourseId = 1,
            NotificationsEnabled = true
        };
        return demoOA;
    }
    
    private Assessment MakeDemoPA()
    {
        Assessment demoPA = new()
        {
        AssessmentId = GetNextAssessmentId(),
        AssessmentName = "C6 PA",
        AssessmentType = "Performance",
        AssessmentStartDate = new DateTime(2024, 03, 01),
        AssessmentEndDate = new DateTime(2024, 04, 30),
        RelatedCourseId = 1,
        NotificationsEnabled = true
    };
        return demoPA;
    }

    private Assessment MakeDemoOA2()
    {
        Assessment demoOA2 = new()
        {
            AssessmentId = GetNextAssessmentId(),
            AssessmentName = "Zed Pandemic OA",
            AssessmentType = "Objective",
            AssessmentStartDate = new DateTime(2023, 01, 01),
            AssessmentEndDate = new DateTime(2023, 02, 28),
            RelatedCourseId = 2,
            NotificationsEnabled = false
        };
        return demoOA2;
    }

    private Assessment MakeDemoPA2()
    {
        Assessment demoPA2 = new()
        {
            AssessmentId = GetNextAssessmentId(),
            AssessmentName = "Community Script PA",
            AssessmentType = "Performance",
            AssessmentStartDate = new DateTime(2023, 03, 01),
            AssessmentEndDate = new DateTime(2024, 02, 29),
            RelatedCourseId = 3,
            NotificationsEnabled = false
        };
        return demoPA2;
    }
    
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
            //Term, Course and Assessments for C6 and C3 requirements
            var DemoTerm = MakeDemoTerm();
            DemoTerm.TermId = GetNextTermId();
            var termId = await connection.InsertAndGetIdAsync(DemoTerm);
            Debug.WriteLine("Inserted " + DemoTerm.TermName);
            var demoCourse = MakeDemoCourse();
            demoCourse.TermId = termId;
            var courseId = await connection.InsertAndGetIdAsync(demoCourse);
            Debug.WriteLine("Inserted " + demoCourse.CourseName);
            var demoOA = MakeDemoOA();
            var demoPA = MakeDemoPA();
            demoOA.RelatedCourseId = courseId;
            demoPA.RelatedCourseId = courseId;
            await assessmentDB.InsertAssessmentAndUpdateCourseCount(demoOA);
            Debug.WriteLine("Inserted " + demoOA.AssessmentName);
            await assessmentDB.InsertAssessmentAndUpdateCourseCount(demoPA);
            Debug.WriteLine("Inserted " + demoPA.AssessmentName);

            //Second term and courses to provide a more robust demo set of data for evaluator
            var demoTerm2 = MakeDemoTerm2();
            var termId2 = await connection.InsertAndGetIdAsync(demoTerm2);
            Debug.WriteLine("Inserted " + demoTerm2.TermName);

            var demoCourse2 = MakeDemoCourse2();
            var demoCourse3 = MakeDemoCourse3();
            
            demoCourse2.TermId = termId2;
            demoCourse3.TermId = termId2;
            var courseId2 = await connection.InsertAndGetIdAsync(demoCourse2);
            Debug.WriteLine("Inserted " + demoCourse2.CourseName);
            await courseDB.UpdateCourseAndUpdateTermCount(demoCourse2);
            
            var demoOA2 = MakeDemoOA2();
            var demoPA2 = MakeDemoPA2();

            demoOA2.RelatedCourseId = courseId2;
            demoPA2.RelatedCourseId = demoCourse3.CourseId;

            await assessmentDB.InsertAssessmentAndUpdateCourseCount(demoOA2);
            Debug.WriteLine("Inserted " + demoOA2.AssessmentName);
            var courseId3 = await connection.InsertAndGetIdAsync(demoCourse3);
            Debug.WriteLine("Inserted " + demoCourse3.CourseName);

            demoPA2.RelatedCourseId = courseId3;
            await courseDB.UpdateCourseAndUpdateTermCount(demoCourse3);
            await assessmentDB.InsertAssessmentAndUpdateCourseCount(demoPA2);
            Debug.WriteLine("Inserted " + demoPA2.AssessmentName);

            await DisplayAlert("Sample Data Loaded", "Sample data has been loaded successfully. Please relaunch the application to complete setup.", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Sample Data Error", "$There was an error while loading sample data: " + ex.Message, "OK");
            return;
        }
    }
    #endregion
}
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Xml;
using Android.Views;
using Android.Widget;
using CourseTracker.Maui.Data;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Services;
using CourseTracker.Maui.Supplemental;
using GoogleGson.Annotations;
using Newtonsoft.Json;
using Org.Json;

namespace CourseTracker.Maui.Views;

public partial class Homepage : ContentPage
{
    #region Fields / Properties
    TrackerDb _trackerDb = new();
    Connection connection = new();
    public TermsDB termsDB;
    public CourseDB courseDB;
    public AssessmentDB assessmentDB;
    #endregion

    public Homepage()
    {
        InitializeComponent();
        termsDB = new TermsDB();
        courseDB = new CourseDB();
        assessmentDB = new AssessmentDB();
        StartDB();

    }

    private async Task StartDB()
    {
        _trackerDb ??= new TrackerDb();
        await TrackerDb.Initialize();
        
        Toast.MakeText(Android.App.Application.Context, "Database Initialized", ToastLength.Short).Show();
    }

    private async void loadButton_Clicked(object sender, EventArgs e)
    {
        if (sender == loadSampleDataButton)
        {
            if (_trackerDb == null)
            {
                await StartDB();
            }
            Debug.WriteLine("Starting to load Data");
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

    #region Sample Data For Evaluation

    private async Task<Term> MakeDemoTerm()
    {
        Term demoTerm = new()
        {
            TermId = await termsDB.GetNextId(),
            TermName = "Demo Term",
            TermStart = new DateTime(2024, 01, 01),
            TermEnd = new DateTime(2024, 06, 30),
            CourseCount = 1
        };
        return demoTerm;
    }

    private async Task<Term> MakeDemoTerm2()
    {
        Term demoTerm2 = new()
        {
            TermId = await termsDB.GetNextId(),
            TermName = "Term One 2023",
            TermStart = new DateTime(2023, 01, 01),
            TermEnd = new DateTime(2023, 06, 30),
            CourseCount = 2
        };
        return demoTerm2;
    }


    private async Task<Course> MakeDemoCourse()
    {
        Course course = new()
        {
            CourseId = await courseDB.GetNextId(),
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

    private async Task<Course> MakeDemoCourse2()
    {
        Course course = new()
        {
            CourseId = await courseDB.GetNextId(),
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

    private async Task<Course> MakeDemoCourse3()
    {
        Course course = new()
        {
            CourseId = await courseDB.GetNextId(),
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

    private async Task<Assessment> MakeDemoOA()
    {
        Assessment demoOA = new()
        {
            AssessmentId = await assessmentDB.GetNextId(),
            AssessmentName = "C6 OA",
            AssessmentType = "Objective",
            AssessmentStartDate = new DateTime(2024, 01, 01),
            AssessmentEndDate = new DateTime(2024, 02, 29),
            RelatedCourseId = 1,
            NotificationsEnabled = true
        };
        return demoOA;
    }

    private async Task<Assessment> MakeDemoPA()
    {
        Assessment demoPA = new()
        {
            AssessmentId = await assessmentDB.GetNextId(),
            AssessmentName = "C6 PA",
            AssessmentType = "Performance",
            AssessmentStartDate = new DateTime(2024, 03, 01),
            AssessmentEndDate = new DateTime(2024, 04, 30),
            RelatedCourseId = 1,
            NotificationsEnabled = true
        };
        return demoPA;
    }

    private async Task<Assessment> MakeDemoOA2()
    {
        Assessment demoOA2 = new()
        {
            AssessmentId = await assessmentDB.GetNextId(),
            AssessmentName = "Zed Pandemic OA",
            AssessmentType = "Objective",
            AssessmentStartDate = new DateTime(2023, 01, 01),
            AssessmentEndDate = new DateTime(2023, 02, 28),
            RelatedCourseId = 2,
            NotificationsEnabled = false
        };
        return demoOA2;
    }

    private async Task<Assessment> MakeDemoPA2()
    {
        Assessment demoPA2 = new()
        {
            AssessmentId = await assessmentDB.GetNextId(),
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
        var validation = new Validation();
        var existing = await validation.DataExistsInTables();
        Debug.WriteLine("Existing check: " + existing);
        if (existing)
        {
            //await DisplayAlert("Table Already Has Data", "Table data has already been loaded. " +
            //    "You should reset the database to avoid errors with sample data creation.", "OK");
            //return;
        }

        try
        {
            //Term, Course and Assessments for C6 and C3 requirements
            
            var DemoTerm = await MakeDemoTerm();
            DemoTerm.TermId = await termsDB.GetNextId();
            
            var termId = await connection.InsertAndGetIdAsync(DemoTerm);
            Debug.WriteLine("Inserted " + DemoTerm.TermName);

            var demoCourse = await MakeDemoCourse();
            demoCourse.TermId = termId;
            await courseDB.SaveCourseAsync(demoCourse);
            var courseId = demoCourse.CourseId;
            Debug.WriteLine("Inserted " + demoCourse.CourseName);

            var demoOA = await MakeDemoOA();
            var demoPA = await MakeDemoPA();
            demoOA.RelatedCourseId = courseId;
            demoPA.RelatedCourseId = courseId;
            await assessmentDB.UpdateAssessmentAndUpdateCourse(demoOA);
            Debug.WriteLine("Inserted " + demoOA.AssessmentName);
            await assessmentDB.UpdateAssessmentAndUpdateCourse(demoPA);
            Debug.WriteLine("Inserted " + demoPA.AssessmentName);

            //Second term and courses to provide a more robust demo set of data for evaluator
            var demoTerm2 = await MakeDemoTerm2();
            var termId2 = await connection.InsertAndGetIdAsync(demoTerm2);
            Debug.WriteLine("Inserted " + demoTerm2.TermName);

            var demoCourse2 = await MakeDemoCourse2();
            var demoCourse3 = await MakeDemoCourse3();

            demoCourse2.TermId = termId2;
            demoCourse3.TermId = termId2;
            var courseId2 = await connection.InsertAndGetIdAsync(demoCourse2);
            Debug.WriteLine("Inserted " + demoCourse2.CourseName);
            await courseDB.UpdateCourseAndUpdateTermCount(demoCourse2);

            var demoOA2 = await MakeDemoOA2();
            var demoPA2 = await MakeDemoPA2();

            demoOA2.RelatedCourseId = courseId2;
            demoPA2.RelatedCourseId = demoCourse3.CourseId;

            await assessmentDB.UpdateAssessmentAndUpdateCourse(demoOA2);
            Debug.WriteLine("Inserted " + demoOA2.AssessmentName);
            var courseId3 = await connection.InsertAndGetIdAsync(demoCourse3);
            Debug.WriteLine("Inserted " + demoCourse3.CourseName);

            demoPA2.RelatedCourseId = courseId3;
            await courseDB.UpdateCourseAndUpdateTermCount(demoCourse3);
            await assessmentDB.UpdateAssessmentAndUpdateCourse(demoPA2);
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
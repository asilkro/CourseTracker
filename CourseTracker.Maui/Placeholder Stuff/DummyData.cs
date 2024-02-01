using CourseTracker.Maui.Models;
using CourseTracker.Maui.Services;

namespace CourseTracker.Maui.Placeholder_Stuff;
public class DummyData
{
    #region Lists of Data
    public List<String> CourseNames = new List<string>
    {
        "Social Psychology",
        "Comparative Religion",
        "The Art of Discourse",
        "Aerodynamics of Gender"
    };

    public List<String> InstructorNames = new List<string>
    {
        "Jeff Winger",
        "Britta Perry",
        "Troy Barnes",
        "Annie Edison"
    };

    public List<String> InstructorEmails = new List<string>
    {
        "jwinger@greendalecc.edu",
        "bperry@greendalecc.edu",
        "troyb@greendalecc.edu",
        "aedison13@greendalecc.edu"
    };

    public List<String> InstructorPhones = new List<string>
    {
        "553-555-9052",
        "552-555-1892",
        "551-555-2189",
        "554-555-7556"
    };

    public List<String> TermNames = new List<string>
    {
        "The First Term",
        "Some Second Term",
        "Terrible Third Term",
        "Fantastic Fourth Term"
    };

    public List<String> CourseStatuses = new List<string>
    {
        "Planned",
        "Planned",
        "Completed",
        "In Progress"
    };

    public List<String> AssessmentNames = new List<string>
    {
        "OA 123",
        "PA 555",
        "PA 988",
        "OA 789"
    };

    public List<DateTime> TermStarts = new List<DateTime>
    {
        new DateTime(2023, 09, 01),
        new DateTime(2023, 03, 01),
        new DateTime(2022, 11, 01),
        new DateTime(2024, 01, 01),
    };

    public List<DateTime> TermEnds = new List<DateTime>
    {
        DateTime.Now.Date.AddDays(-90),
        new DateTime(2023, 09, 30),
        new DateTime(2023, 04, 30),
        new DateTime(2024, 06, 30),
    };

    public List<DateTime> CourseStarts = new List<DateTime>
    {
        DateTime.Now.Date.AddDays(-34),
        DateTime.Now.Date.AddDays(-50),
        DateTime.Now.Date.AddDays(-110),
        DateTime.Now.Date.AddDays(-60)
    };

    public List<DateTime> CourseEnds = new List<DateTime>
    {
        DateTime.Now.Date.AddDays(90),
        DateTime.Now.Date.AddDays(140),
        DateTime.Now.Date.AddDays(41),
        DateTime.Now.Date.AddDays(65)
    };

    #endregion

    #region Required Stuff

    public static void AddRequiredStuff()
    {


        Course demoCourse = new Course
        {
            CourseName = "C6 Requirements re: C3 Course",
            CourseStatus = "In Progress",
            
            CourseStart = new DateTime(2024, 01, 01),
            CourseEnd = new DateTime(2024, 06, 30),
            CourseNotes = "This addresses requirement C6",
            NotificationsEnabled = true,
            CourseAssessmentCount = 2
        };

        Assessment demoOA = new Assessment
        {
            AssessmentName = "C6 OA",
            AssessmentType = "Objective",
            AssessmentStartDate = new DateTime(2024, 01, 01),
            AssessmentEndDate = new DateTime(2024, 02, 29),
            NotificationsEnabled = true
        };

        Assessment demoPA = new Assessment
        {
            AssessmentName = "C6 PA",
            AssessmentType = "Performance",
            AssessmentStartDate = new DateTime(2024, 03, 01),
            AssessmentEndDate = new DateTime(2024, 04, 30),
            NotificationsEnabled = true
        };

        Instructor demoInstructor = new Instructor
        {
            InstructorName = "Anika Patel",
            InstructorEmail = "anika.patel@strimeuniversity.edu",
            InstructorPhone = "555-123-4567"
        };




    }


    #endregion

}



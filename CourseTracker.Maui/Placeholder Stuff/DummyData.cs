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
        DateTime.Now.Date.AddDays(-320),
        DateTime.Now.Date.AddDays(-190),
        DateTime.Now.Date.AddDays(-210),
        DateTime.Now.Date.AddDays(-60)
    };

    public List<DateTime> TermEnds = new List<DateTime>
    {
        DateTime.Now.Date.AddDays(-90),
        DateTime.Now.Date.AddDays(-140),
        DateTime.Now.Date.AddDays(-120),
        DateTime.Now.Date.AddDays(-45)
    };

    public List<DateTime> CourseStarts = new List<DateTime>
    {
        DateTime.Now.Date.AddDays(-320),
        DateTime.Now.Date.AddDays(-190),
        DateTime.Now.Date.AddDays(-210),
        DateTime.Now.Date.AddDays(-60)
    };

    public List<DateTime> CourseEnds = new List<DateTime>
    {
        DateTime.Now.Date.AddDays(-90),
        DateTime.Now.Date.AddDays(-140),
        DateTime.Now.Date.AddDays(-120),
        DateTime.Now.Date.AddDays(-45)
    };

    #endregion

}



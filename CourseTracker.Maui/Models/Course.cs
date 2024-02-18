namespace CourseTracker.Maui.Models;

[Table("Course")]
public class Course
{
    #region Properties

    [PrimaryKey]
    [AutoIncrement]
    public int CourseId { get; set; } = -1; // -1 means new course
    public string CourseName { get; set; } = "Course Name Placeholder";
    public int TermId { get; set; } = -1; // -1 means not set to actual term
    public string CourseStatus { get; set; } = "In Progress";
    public DateTime CourseStart { get; set; } = DateTime.Now.Date;
    public DateTime CourseEnd { get; set; } = DateTime.Now.Date.AddDays(90);
    public string CourseNotes { get; set; } = "Course Notes Placeholder";
    public string InstructorName { get; set; } = "Instructor Name Placeholder";
    public string InstructorEmail { get; set; } = "placeholder@school.edu";
    public string InstructorPhone { get; set; } = "555-555-1234";
    public bool NotificationsEnabled { get; set; } = false;
    public int CourseAssessmentCount { get; set; } = 0; // 0 will never be a valid count, can only be 1/2

    #endregion

    #region Constructors

    public Course()
    {

    }

    public Course(int courseId, string courseName, int termId, string courseStatus, DateTime courseStart, DateTime courseEnd, string courseNotes, string instructorName, string instructorEmail, string instructorPhone, bool notificationsEnabled, int courseAssessmentCount)
    {
        CourseId = courseId;
        CourseName = courseName;
        TermId = termId;
        CourseStatus = courseStatus;
        CourseStart = courseStart;
        CourseEnd = courseEnd;
        CourseNotes = courseNotes;
        InstructorName = instructorName;
        InstructorEmail = instructorEmail;
        InstructorPhone = instructorPhone;
        NotificationsEnabled = notificationsEnabled;
        CourseAssessmentCount = courseAssessmentCount;
    }

    #endregion
}
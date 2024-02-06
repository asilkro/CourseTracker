using SQLite;

namespace CourseTracker.Maui.Models;

[Table("Course")]
public class Course
{
    #region Properties

    [PrimaryKey]
    [Column("CourseId")] public int CourseId { get; set; } = -1; // -1 means new course
    [Column("CourseName")] public string CourseName { get; set; } = "Course Name Placeholder";
    [Column("TermId")] public int TermId { get; set; } = -1; // -1 means not set to actual term
    [Column("CourseStatus")] public string CourseStatus { get; set; } = "In Progress";
    [Column("CourseStart")] public DateTime CourseStart { get; set; } = DateTime.Now.Date;
    [Column("CourseEnd")] public DateTime CourseEnd { get; set; } = DateTime.Now.Date.AddDays(90);
    [Column("CourseNotes")] public string CourseNotes { get; set; } = "Course Notes Placeholder";
    [Column("InstructorName")] public string InstructorName { get; set; } = "Instructor Name Placeholder";
    [Column("InstructorEmail")] public string InstructorEmail { get; set; } = "placeholder@school.edu";
    [Column("InstructorPhone")] public string InstructorPhone { get; set; } = "555-555-1234";
    [Column("NotificationsEnabled")] public bool NotificationsEnabled { get; set; } = false;
    [Column("CourseAssessmentCount")] public int CourseAssessmentCount { get; set; } = 0; // 0 will never be a valid count, can only be 1/2

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
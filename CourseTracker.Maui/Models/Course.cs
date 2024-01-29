﻿using SQLite;

namespace CourseTracker.Maui.Models;

[Table("Course")]
public class Course
{
    #region Properties

    [PrimaryKey]
    [Column("CourseId")] public int CourseId { get; set; } = -1; // -1 means new course
    [Column("TermId")] public int TermId { get; set; } = -1; // -1 means not set to actual term
    [Column("InstructorId")] public int InstructorId { get; set; } = -1; // -1 means not set to actual instructor
    [Column("CourseName")] public string CourseName { get; set; } = "Course Name Placeholder";
    [Column("CourseStatus")] public string CourseStatus { get; set; } = "Course Status Placeholder";
    [Column("CourseStart")] public DateTime CourseStart { get; set; } = DateTime.Now.Date;
    [Column("CourseEnd")] public DateTime CourseEnd { get; set; } = DateTime.Now.Date.AddDays(90);
    [Column("CourseNotes")] public string CourseNotes { get; set; } = "Course Notes Placeholder";
    [Column("NotificationsEnabled")] public bool NotificationsEnabled { get; set; } = false;
    [Column("CourseAssessmentCount")] public int CourseAssessmentCount { get; set; } = -1; // -1 will never be a valid count, can only be 1/2

    #endregion

    #region Constructors

    public Course()
    {

    }

    public Course(int courseId, int termId, int instructorId, string courseName, string courseStatus, DateTime courseStart, DateTime courseEnd, string courseNotes, bool notificationsEnabled, int courseAssessmentCount)
    {
        CourseId = courseId;
        TermId = termId;
        InstructorId = instructorId;
        CourseName = courseName;
        CourseStatus = courseStatus;
        CourseStart = courseStart;
        CourseEnd = courseEnd;
        CourseNotes = courseNotes;
        NotificationsEnabled = notificationsEnabled;
        CourseAssessmentCount = courseAssessmentCount;
    }


    #endregion
}
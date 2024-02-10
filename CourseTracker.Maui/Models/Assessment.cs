using SQLite;

namespace CourseTracker.Maui.Models;

[Table("Assessment")]
public class Assessment
{
    #region Properties
    [PrimaryKey]
    [AutoIncrement]
    [Column("AssessmentId")] public int AssessmentId { get; set; } = -1;
    // -1 means new assessment
    [Column("AssessmentName")] public string AssessmentName { get; set; } = "Assessment Name Placeholder";
    [Column("AssessmentType")] public string AssessmentType { get; set; } = "Objective";
    [Column("AssessmentStartDate")] public DateTime AssessmentStartDate { get; set; } = DateTime.Today.Date;
    [Column("AssessmentEndDate")] public DateTime AssessmentEndDate { get; set; } = DateTime.Today.Date.AddDays(30);
    [Column("RelatedCourseId")] public int RelatedCourseId { get; set; } = -1;
    // -1 means not set to actual course
    [Column("NotificationsEnabled")] public bool NotificationsEnabled { get; set; } = false;

    #endregion

    #region Constructors
    public Assessment()
    {
    }

    public Assessment(int assessmentId, string assessmentName, string assessmentType, DateTime assessmentStartDate, DateTime assessmentEndDate, int relatedCourseId, bool notificationsEnabled)
    {
        AssessmentId = assessmentId;
        AssessmentName = assessmentName;
        AssessmentType = assessmentType;
        AssessmentStartDate = assessmentStartDate;
        AssessmentEndDate = assessmentEndDate;
        RelatedCourseId = relatedCourseId;
        NotificationsEnabled = notificationsEnabled;
    }
    #endregion
}

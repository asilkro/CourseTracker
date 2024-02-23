using SQLite;
namespace CourseTracker.Maui.Models;

[Table("Assessment")]
public class Assessment
{
    #region Properties
    [PrimaryKey]
    [AutoIncrement]
    public int AssessmentId { get; set; }
    public string AssessmentName { get; set; } = "Assessment Name";
    public string AssessmentType { get; set; } = "Objective";
    public DateTime AssessmentStartDate { get; set; }
    public DateTime AssessmentEndDate { get; set; }
    public int RelatedCourseId { get; set; }
    public bool NotificationsEnabled { get; set; } = false;

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

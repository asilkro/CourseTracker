using System.ComponentModel.DataAnnotations.Schema;

namespace CourseTracker.Maui.Models
{
    [Table("Assessment")]
    public class Assessment
    {
        #region Properties
        [Column("AssessmentId")]
        public int AssessmentId { get; set; } = -1; // -1 means new assessment

        [Column("AssessmentName")] public string AssessmentName { get; set; } = "Assessment Name Placeholder";

        [Column("AssessmentType")] public string AssessmentType { get; set; } = "Objective"; // Objective or Performance?

        [Column("AssessmentStartDate")] public DateTime AssessmentStartDate { get; set; } = DateTime.Today.Date;

        [Column("AssessmentEndDate")] public DateTime AssessmentEndDate { get; set; } = DateTime.Today.Date.AddDays(30);

        [Column("RelatedCourseId")] public int RelatedCourseId { get; set; } = -1; // -1 means not set to actual course

        [Column("NotificationsEnabled")] public bool NotificationsEnabled { get; set; } = false;

        #endregion

        #region Constructors
        public Assessment()
        {
        }
        #endregion

        #region Methods
        public bool AssessmentTypeIsValid(string assessmentType)
        {
            var result = assessmentType switch
            {
                "Objective" => true,
                "Performance" => true,
                "None" => true, //Covers cases where a course may only have one assessment
                _ => false
            };

            return result;
        }

        #endregion
    }
}

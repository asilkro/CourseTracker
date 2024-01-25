using CourseTracker.Maui.ViewModels;
using CourseTracker.Maui.Models;

namespace CourseTracker.Maui.Factories
{
    internal class AssessmentFactory : FactoryBase<Assessment>
    {
        public Assessment? CreateAssessment(int assessmentId, string assessmentName, string assessmentType, DateTime assessmentStartDate, DateTime assessmentEndDate, int relatedCourseId, bool notificationsEnabled, out string errorMessage)
        {
            if (!IsValidAssessment(assessmentId, assessmentName, assessmentType, assessmentStartDate, assessmentEndDate, relatedCourseId, notificationsEnabled, out errorMessage))
            {
                return null;
            }
            var assessment = CreateObject();
            assessment.AssessmentId = assessmentId;
            assessment.AssessmentName = assessmentName;
            assessment.AssessmentType = assessmentType;
            assessment.AssessmentStartDate = assessmentStartDate;
            assessment.AssessmentEndDate = assessmentEndDate;
            assessment.RelatedCourseId = relatedCourseId;
            assessment.NotificationsEnabled = notificationsEnabled;

            return assessment;
        }

        private bool IsValidAssessment(int id, string assessmentName, string assessmentType, DateTime assessmentStartDate, DateTime assessmentEndDate, int relatedCourseId, bool notificationsEnabled, out string errorMessage)
        {
            errorMessage = "";
            //TODO: Add validation logic

            return string.IsNullOrEmpty(errorMessage);
        }

        protected override Assessment? CreateDefaultObject()
        {
            return new Assessment();
        }
    }
}

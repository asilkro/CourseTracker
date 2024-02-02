using CourseTracker.Maui.ViewModels;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Supplemental;
using System.Diagnostics;
using CourseTracker.Maui.Services;

namespace CourseTracker.Maui.Factories
{
    public class AssessmentFactory : FactoryBase<Assessment>
    {

        public AssessmentFactory(IAsyncSqLite database) : base(database)
        {
        }

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

            if (!Validation.IdWasSet(id))
                errorMessage = "Assessment ID must be greater than 0.";

            else if (!Validation.IdWasSet(relatedCourseId))
                errorMessage = "Related course ID must be greater than 0.";
            
            else if (!Validation.NotNull(assessmentName))
                errorMessage = "Assessment name cannot be empty.";
            
            else if (!Validation.AssessmentTypeIsValid(assessmentType))
                errorMessage = "Assessment type is invalid, must be Objective or Performance.";
            
            else if (!Validation.DatesAreValid(assessmentStartDate, assessmentEndDate))
                errorMessage = "Assessment start and end dates must be valid.";

            Debug.WriteLine(errorMessage);
            return string.IsNullOrEmpty(errorMessage);
        }

        protected override Assessment? CreateDefaultObject()
        {
            return new Assessment();
        }

        public async Task<string> InsertAssessmentAndUpdateCourseCount(Assessment newAssessment)
        {
            var connection = new Connection();
            var course = await connection.FindAsync<Course>(newAssessment.RelatedCourseId);
            if (course == null)
            {
                return "Course not found.";
            }

            if (course.CourseAssessmentCount >= 2)
            {
                return "Courses may have no more than two assessments.";
            }

            course.CourseAssessmentCount += 1;
            await connection.UpdateAsync(course);
            await connection.InsertAsync(newAssessment);

            return "Assessment added successfully.";
        }

    }
}

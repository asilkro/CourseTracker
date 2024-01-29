using CourseTracker.Maui.ViewModels;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Placeholder_Stuff;
using CourseTracker.Maui.Supplemental;
using System.Diagnostics;
using CourseTracker.Maui.Services;

namespace CourseTracker.Maui.Factories
{
    internal class AssessmentFactory : FactoryBase<Assessment>
    {
        private readonly DummyData _dummyData;
        public AssessmentFactory(IAsyncSqLite database, DummyData dummyData) : base(database)
        {
            _dummyData = dummyData;
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

        public async Task<List<Assessment>> GenerateSampleTerms(int numberOfAssessments)
        {
            var assessments = new List<Assessment>();
            for (int i = 0; i < numberOfAssessments; i++)
            {
                var assessment = new Assessment
                {
                    AssessmentId = i,
                    AssessmentName = _dummyData.AssessmentNames[i % _dummyData.AssessmentNames.Count],
                    AssessmentType = "Objective",
                    AssessmentStartDate = _dummyData.CourseStarts[i % _dummyData.CourseStarts.Count],   
                    AssessmentEndDate = _dummyData.CourseStarts[i % _dummyData.CourseStarts.Count],
                    RelatedCourseId = i,
                    NotificationsEnabled = i % 2 == 0
                };

                // Add the assessment to the list
                assessments.Add(assessment);

                // Add to the database
                await AddObject(assessment);
            }
            return assessments;
        }
    }
}

using CourseTracker.Maui.ViewModels;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Services;
using CourseTracker.Maui.Supplemental;
using System.Diagnostics;
using Plugin.LocalNotification;

namespace CourseTracker.Maui.Factories
{
    public class AssessmentFactory : FactoryBase<Assessment>
    {
        readonly Connection _database = new();
        public AssessmentFactory(IAsyncSqLite database) : base(database)
        {
        }

        public async Task<AssessmentCreationOut> CreateAssessmentAsync(AssessmentVM assessmentVM)
        {
            AssessmentCreationOut result = await CreateAssessmentAsync(assessmentVM.AssessmentId,
                                               assessmentVM.AssessmentName,
                                               assessmentVM.AssessmentType,
                                               assessmentVM.AssessmentStartDate,
                                               assessmentVM.AssessmentEndDate,
                                               assessmentVM.RelatedCourseId,
                                               assessmentVM.NotificationsEnabled);
            if (result != null)
            {
                return result;
            }
            else
            {
                return new AssessmentCreationOut { ErrorMessage = "Error creating assessment." };
            }
        }

        public async Task<AssessmentCreationOut>? CreateAssessmentAsync(int assessmentId, string assessmentName,
            string assessmentType, DateTime assessmentStartDate, DateTime assessmentEndDate,
            int relatedCourseId, bool notificationsEnabled)
        {
            string errorMessage = string.Empty;
            if (!IsValidAssessment(assessmentId, assessmentName, assessmentType,
                assessmentStartDate, assessmentEndDate, relatedCourseId, notificationsEnabled))
            {
                return new AssessmentCreationOut { ErrorMessage = errorMessage };
            }

            Assessment assessment = new()
            {
                AssessmentId = assessmentId,
                AssessmentName = assessmentName,
                AssessmentType = assessmentType,
                AssessmentStartDate = assessmentStartDate,
                AssessmentEndDate = assessmentEndDate,
                RelatedCourseId = relatedCourseId,
                NotificationsEnabled = notificationsEnabled
            };

            await InsertAssessmentAndUpdateCourseCount(assessment);

            if (assessment.NotificationsEnabled)
            {
                await ScheduleAssessmentNotifications(assessment);
            }

            return new AssessmentCreationOut  { Assessment = assessment };
        }

        private bool IsValidAssessment(int id, string assessmentName, string assessmentType, DateTime assessmentStartDate, DateTime assessmentEndDate, int relatedCourseId, bool notificationsEnabled)
        {
            var errorMessage = "";

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

        public async Task LowerCourseAssessmentCount(int courseId) // Used when an assessment is deleted or moved to another course
        {
            var connection = new Connection().GetAsyncConnection();
            var course = await connection.FindAsync<Course>(courseId);
            if (course == null)
            {
                return;
            }

            if (course.CourseAssessmentCount > 0)
            {
                course.CourseAssessmentCount -= 1;
                await connection.UpdateAsync(course);
            }
        }

        public async Task<string> UpdateAssessmentAndUpdateCourseCount(Assessment newAssessment)
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
            await connection.UpdateAsync(newAssessment);

            return "Assessment updated successfully.";
        }

        public async Task ScheduleAssessmentNotifications(Assessment assessment)
        {
            // Assumptions: Using full days 
            if (assessment.NotificationsEnabled)
            {
                //var notificationId = (assessment.AssessmentId + DateTime.Now.Second); // Ideally, generate a unique ID for each notification.
                var title = $"Reminder for {assessment.AssessmentName}";

                // Schedule notifications for start date reminders
                var startReminders = new[] { 14, 7, 1 }; // These could be different from the end reminders or configurable in-app in a future version.
                foreach (var daysBefore in startReminders)
                {
                    var notificationId = (assessment.AssessmentId + DateTime.Now.Second);
                    var notifyTime = assessment.AssessmentStartDate.AddDays(-daysBefore);
                    var subtitle = $"{assessment.AssessmentName} starts in {daysBefore} day(s)";
                    await Notification.ScheduleNotificationAsync(notificationId++, title, subtitle, notifyTime, NotificationCategoryType.Reminder);
                }

                // Schedule notifications for end date reminders
                var endReminders = new[] { 14, 7, 1 }; // These could be different from the start reminders or configurable in-app in a future version.
                foreach (var daysBefore in endReminders)
                {
                    var notificationId = (assessment.AssessmentId + DateTime.Now.Millisecond);
                    var notifyTime = assessment.AssessmentEndDate.AddDays(-daysBefore);
                    var subtitle = $"{assessment.AssessmentName} due in {daysBefore} day(s)";
                    await Notification.ScheduleNotificationAsync(notificationId++, title, subtitle, notifyTime, NotificationCategoryType.Reminder);
                }
            }
            return;
        }

        public class AssessmentCreationOut
        {
            public Assessment? Assessment { get; set; }
            public string ErrorMessage { get; set; } = string.Empty;
        }

    }
}

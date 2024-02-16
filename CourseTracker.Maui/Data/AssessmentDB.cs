using System.Collections.ObjectModel;
using System.Diagnostics;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.ViewModels;
using CourseTracker.Maui.Services;
using CourseTracker.Maui.Supplemental;
using Plugin.LocalNotification;

namespace CourseTracker.Maui.Data
{
    public class AssessmentDB
    {
        SQLiteAsyncConnection _database;
        public AssessmentDB()
        {
        }
        async Task Init()
        {
            if (_database != null)
                return;
            _database = new SQLiteAsyncConnection(TrackerDb.DatabasePath, TrackerDb.Flags);
            await _database.CreateTableAsync<Assessment>();
        }
        public async Task<List<Assessment>> GetAssessmentsAsync()
        {
            await Init();
            return await _database.Table<Assessment>().ToListAsync();
        }

        public async Task<Assessment> GetAssessmentsAsync(int id)
        {
            await Init();
            return await _database.Table<Assessment>()
                .Where(i => i.AssessmentId == id)
                .FirstOrDefaultAsync();
        }

        public async Task SaveAssessmentAsync(Assessment assessment)
        {
            await Init();
            if (assessment.AssessmentId != 0)
            {
                await _database.UpdateAsync(assessment);
                //TODO: Update assessment count on courses, return error if course not found or over limit of 2

            }
            else
            {
                await _database.InsertAsync(assessment);
                //TODO: Update assessment count on courses, return error if course not found or over limit of 2

            }
        }
        public async Task<int> DeleteAssessmentAsync(Assessment assessment)
        {
            await Init();
            return await _database.DeleteAsync(assessment);
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
            //if (!IsValidAssessment(assessmentId, assessmentName, assessmentType,
            //    assessmentStartDate, assessmentEndDate, relatedCourseId, notificationsEnabled))
            //{
            //    return new AssessmentCreationOut { ErrorMessage = errorMessage };
            //}

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

            return new AssessmentCreationOut { Assessment = assessment };
        }

        public string IsValidAssessment(Assessment assessment)
        { 
            var errorMessage = string.Empty;

            if (!Validation.IdWasSet(assessment.AssessmentId))
                errorMessage = "Assessment ID must be greater than 0.";
            else if (!Validation.IdWasSet(assessment.RelatedCourseId))
                errorMessage = "Related course ID must be greater than 0.";
            else if (!Validation.NotNull(assessment.AssessmentName))
                errorMessage = "Assessment name cannot be empty.";
            else if (!Validation.AssessmentTypeIsValid(assessment.AssessmentType))
                errorMessage = "Assessment type is invalid, must be Objective or Performance.";
            else if (!Validation.DatesAreValid(assessment.AssessmentStartDate, assessment.AssessmentEndDate))
                errorMessage = "Assessment start and end dates must be valid.";

            Debug.WriteLine(errorMessage);
            return errorMessage;
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

        public async Task<int> GetNextId()
        {
            await Init();
            List<Assessment> assessments = await GetAssessmentsAsync();
            if (assessments.Count == 0)
            {
                return 1;
            }
            else
            {
                int maxId = assessments.Max(t => t.AssessmentId);
                maxId++;
                return maxId;
            }
        }
    }
}

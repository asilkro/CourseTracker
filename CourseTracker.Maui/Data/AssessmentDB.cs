using System.Diagnostics;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Services;
using CourseTracker.Maui.Supplemental;
using Newtonsoft.Json;
using Plugin.LocalNotification;

namespace CourseTracker.Maui.Data
{
    public class AssessmentDB
    {
        #region Fields
        SQLiteAsyncConnection _database;
        CourseDB courseDB;
        #endregion

        #region Constructors
        public AssessmentDB()
        {
            courseDB = new CourseDB();
        }
        #endregion

        #region Methods
        public async Task Init()
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

        public async Task<Assessment> GetAssessmentByIdAsync(int id)
        {
            await Init();
            return await _database.Table<Assessment>()
                .Where(i => i.AssessmentId == id)
                .FirstOrDefaultAsync();
        }

        public async Task<int> DeleteAssessmentAsync(Assessment assessment)
        {
            await Init();
            return await _database.DeleteAsync(assessment);
        }

        public async Task<AssessmentCreationOut>? CreateAssessmentAsync(int assessmentId, string assessmentName,
            string assessmentType, DateTime assessmentStartDate, DateTime assessmentEndDate,
            int relatedCourseId, bool notificationsEnabled)
        {
            string errorMessage = string.Empty;

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

            await UpdateAssessmentAndUpdateCourse(assessment);

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
                errorMessage = "Assessment ID does not appear to have been set.";
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

        public async Task<string> UpdateAssessmentAndUpdateCourse(Assessment assessment)
        {
            try
            {
                var course = await _database.Table<Course>().Where(e => e.CourseId == assessment.RelatedCourseId).FirstOrDefaultAsync();
                if (course == null)
                {
                    return "Course not found.";
                }

                if (course.CourseAssessmentCount >= 2)
                {
                    return "Courses may have no more than two assessments.";
                }

                course.CourseAssessmentCount += 1;
                Debug.WriteLine(JsonConvert.SerializeObject(course, Formatting.Indented));
                await courseDB.SaveCourseAsync(course);
                await SaveAssessmentAsync(assessment);

                return "Assessment added successfully.";
            }
            catch (Exception e)
            {
                return "Error updating assessment: " + e.Message;
            }

        }

        public async Task<string> DeleteAssessmentAndUpdateCourse(Assessment assessment)
        {
            var course = await _database.FindAsync<Course>(assessment.RelatedCourseId);
            if (course == null)
            {
                return "Course not found.";
            }

            if (course.CourseAssessmentCount <= 0)
            {
                return "Course assessment count is already 0.";
            }

            course.CourseAssessmentCount -= 1;
            await courseDB.SaveCourseAsync(course);
            var result = await _database.DeleteAsync(assessment);

            if (result != 1)
            {
                return "Error deleting assessment.";
            }
            return "Assessment deleted successfully.";
        }

        public async Task<string> LowerCourseAssessmentCount(Course course) // Used when an assessment is deleted or moved to another course
        {
            if (course == null)
            {
                return "Course not found.";
            }

            if (course.CourseAssessmentCount > 0)
            {
                try
                {
                    course.CourseAssessmentCount -= 1;
                    await courseDB.SaveCourseAsync(course);
                    return "Course " + course.CourseName + " assessment count lowered successfully.";
                }
                catch (Exception e)
                {
                    return "Error updating assessment count: " + e.Message;
                }
            }
            else return "Course assessment count is already 0.";
        }


        public async Task ScheduleAssessmentNotifications(Assessment assessment)
        {
            // Assumptions: Using full days 
            if (assessment.NotificationsEnabled)
            {
                //var notificationId = (assessment.AssessmentId + DateTime.Now.Second); // Ideally, generate a unique ID for each notification.
                var title = $"Reminder for {assessment.AssessmentName}";

                // Schedule notifications for start date reminders
                var startReminders = new[] { 3, 1 }; // These could be different from the end reminders or configurable in-app in a future version.
                foreach (var daysBefore in startReminders)
                {
                    var notificationId = (assessment.AssessmentId + DateTime.Now.Second);
                    var notifyTime = assessment.AssessmentStartDate.AddDays(-daysBefore);
                    var subtitle = $"{assessment.AssessmentName} starts in {daysBefore} day(s)";
                    await Notification.ScheduleNotificationAsync(notificationId++, title, subtitle, notifyTime, NotificationCategoryType.Reminder);
                }

                // Schedule notifications for end date reminders
                var endReminders = new[] { 3, 1 }; // These could be different from the start reminders or configurable in-app in a future version.
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

        public async Task SaveAssessmentAsync(Assessment assessment)
        {
            await Init();
            var result = _database.FindAsync<Assessment>(assessment.AssessmentId);
            if (result == null)
            {
                await _database.InsertAsync(assessment);
            }
            else
            {
                await _database.UpdateAsync(assessment);
            }
        }
        #endregion

        #region Nested Types

        public class AssessmentCreationOut
        {
            public Assessment? Assessment { get; set; }
            public string ErrorMessage { get; set; } = string.Empty;
        }
        #endregion
    }
}

using System.Diagnostics;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Services;
using CourseTracker.Maui.Supplemental;
using SQLite;

namespace CourseTracker.Maui.Data
{
    public class AssessmentDB
    {
        #region Fields
        SQLiteAsyncConnection _database;
        readonly CourseDB courseDB;
        readonly NotifyDB notifyDB;
        #endregion

        #region Constructors
        public AssessmentDB()
        {
            courseDB = new CourseDB();
            notifyDB = new NotifyDB();
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
        public async Task<List<Assessment>> GetAssessmentsByCourseIdAsync(int CourseId)
        {
            await Init();
            return await _database.Table<Assessment>()
                .Where(i => i.RelatedCourseId == CourseId)
                .ToListAsync();
        }

        public async Task<int> DeleteAssessmentAsync(Assessment assessment)
        {
            await Init();
            return await _database.DeleteAsync(assessment);
        }

        public static string IsValidAssessment(Assessment assessment)
        {
            var errorMessage = string.Empty;

            if (!Validation.IdWasSet(assessment.AssessmentId))
                errorMessage = "Assessment ID does not appear to have been set."; //Should never trigger
            else if (!Validation.IdWasSet(assessment.RelatedCourseId))
                errorMessage = "Related course ID must be greater than 0.";
            else if (!Validation.NotNull(assessment.AssessmentName))
                errorMessage = "Assessment name cannot be empty.";
            else if (!Validation.AssessmentTypeIsValid(assessment.AssessmentType))
                errorMessage = "Assessment type is invalid, must be Objective or Performance.";
            else if (!Validation.DatesAreValid(assessment.AssessmentStartDate, assessment.AssessmentEndDate))
                errorMessage = "Assessment start and end dates must be valid.";

            return errorMessage;
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

        public async Task ScheduleAssessmentNotifications(Assessment assessment)
        {
            // Assumptions: Using full days 
            {
                var title = $"Assessment Reminder: {assessment.AssessmentName}";

                // Schedule notifications for start date reminders
                var startReminders = new[] { 1 }; // These could be different from the end reminders or configurable in-app in a future version.
                foreach (var daysBefore in startReminders)
                {
                    try
                    {
                        Notification notification = new()
                        {
                            NotificationTitle = title,
                            NotificationDate = assessment.AssessmentStartDate.AddDays(-daysBefore),
                            RelatedItemType = "Assessment",
                            NotificationMessage = $"{assessment.AssessmentName} starts in {daysBefore} day(s)",
                            NotificationTriggered = 0
                        };
                        await notifyDB.SaveNotificationAsync(notification);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }

                }

                // Schedule notifications for end date reminders
                var endReminders = new[] { 1 }; // These could be different from the start reminders or configurable in-app in a future version.
                foreach (var daysBefore in endReminders)
                {
                    Notification notification = new()
                    {
                        NotificationTitle = title,
                        NotificationDate = assessment.AssessmentEndDate.AddDays(-daysBefore),
                        RelatedItemType = "Assessment",
                        NotificationMessage = $"Assessment: {assessment.AssessmentName} is due in {daysBefore} day(s)",
                        NotificationTriggered = 0
                    };
                    await notifyDB.SaveNotificationAsync(notification);
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
            var result = await _database.FindAsync<Assessment>(assessment.AssessmentId);
            if (result == null)
            {
                if (assessment.NotificationsEnabled)
                {
                    Debug.WriteLine("Notification enabled for assessment: " + assessment.AssessmentName);
                    await ScheduleAssessmentNotifications(assessment);
                }
                await _database.InsertAsync(assessment);
            }
            else
            {
                if (assessment.NotificationsEnabled)
                {
                    await ScheduleAssessmentNotifications(assessment);
                }
                await _database.UpdateAsync(assessment);
            }
        }
        #endregion
    }
}

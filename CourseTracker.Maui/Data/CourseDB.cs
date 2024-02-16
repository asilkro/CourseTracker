using System.Diagnostics;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.ViewModels;
using CourseTracker.Maui.Services;
using CourseTracker.Maui.Supplemental;
using Plugin.LocalNotification;

namespace CourseTracker.Maui.Data
{
    public class CourseDB
    {
        SQLiteAsyncConnection _database;
        public CourseDB()
        {
        }

        public async Task Init()
        {
            if (_database != null)
                return;
            _database = new SQLiteAsyncConnection(TrackerDb.DatabasePath, TrackerDb.Flags);
            await _database.CreateTableAsync<Course>();
        }

        public async Task<List<Course>> GetCoursesAsync()
        {
            await Init();
            return await _database.Table<Course>().ToListAsync();
        }
        public async Task<Course> GetCourseByIdAsync(int id)
        {
            await Init();
            return await _database.Table<Course>()
                .Where(i => i.CourseId == id)
                .FirstOrDefaultAsync();
        }

        public async Task<int> DeleteCourseAsync(Course course)
        {
            await Init();
            return await _database.DeleteAsync(course);
        }

        public string IsValidCourse(Course course)
        {
            var errorMessage = string.Empty;
            if (!Validation.NotNull(course.CourseName))
                errorMessage = "Course name cannot be empty or undefined.";
            else if (!Validation.NotNull(course.CourseStatus))
                errorMessage = "Course status cannot be empty or undefined.";
            else if (!Validation.CourseStatusIsValid(course.CourseStatus))
                errorMessage = "Course status is not valid.";
            else if (!Validation.DatesAreValid(course.CourseStart, course.CourseEnd))
                errorMessage = "Course start and end dates are not valid.";
            else if (!Validation.IdWasSet(course.CourseId))
                errorMessage = "Valid Course ID must be greater than 0.";
            else if (!Validation.IdWasSet(course.TermId))
                errorMessage = "Valid Term ID must be greater than 0.";
            else if (!Validation.NotNull(course.InstructorName))
                errorMessage = "Instructor name cannot be empty.";
            else if (!Validation.NotNull(course.InstructorEmail))
                errorMessage = "Instructor email cannot be empty.";
            else if (!Validation.EmailIsValid(course.InstructorEmail))
                errorMessage = "Instructor email format is not valid.";
            else if (!Validation.NotTryingToDropTables(course.CourseNotes)) // Not particularly robust, but it's a start
                errorMessage = "Invalid input in notes detected.";
            else if (!Validation.NotNull(course.InstructorPhone))
                errorMessage = "Instructor phone cannot be empty.";
            else if (!Validation.ValidPhoneNumber(course.InstructorPhone))
                errorMessage = "Instructor phone is not valid. Use xxx-xxx-xxxx format.";
            else if (!Validation.ValidCourseAssessmentCount(course.CourseAssessmentCount))
                errorMessage = "Course can only have 1 or 2 assessments.";

            Debug.WriteLine(errorMessage);
            return errorMessage;
        }
        public async Task<string> InsertCourseAndUpdateTermCount(Course course)
        {
            var connection = new Connection();
            var term = await connection.FindAsync<Term>(course.TermId);
            if (term == null)
            {
                return "Associated term not found.";
            }

            if (term.CourseCount >= 6)
            {
                return "Terms may NOT consist of more than six courses.";
            }

            term.CourseCount += 1;
            await connection.UpdateAsync(term);
            await connection.InsertAsync(course);

            return "Course added successfully.";
        }

        public async Task<string> UpdateCourseAndUpdateTermCount(Course course)
        {
            var connection = new Connection();
            var term = await connection.FindAsync<Term>(course.TermId);
            if (term == null)
            {
                return "Associated term not found.";
            }

            if (term.CourseCount >= 6)
            {
                return "Terms may NOT consist of more than six courses.";
            }

            term.CourseCount += 1;
            await connection.UpdateAsync(term);
            await connection.UpdateAsync(course);

            return "Course Updated successfully.";
        }

        public async void RemoveCourseAsync(Course course)
        {
            if (course == null)
                return;
            var result = await App.Current.MainPage.DisplayAlert("Delete Course", $"Are you sure you want to delete {course.CourseName}?", "Yes", "No");
            if (result)
            {
                int confirm = await DeleteCourseAsync(course);
                if (confirm == 1)
                { await App.Current.MainPage.DisplayAlert("Course Deleted", $"{course.CourseName} has been deleted.", "OK"); }
                else
                { await App.Current.MainPage.DisplayAlert("Error", "Course was not deleted.", "OK");}
            }
        }

        public async Task ScheduleCourseNotifications(Course course)
        {
            if (course.NotificationsEnabled)
            {
                var notificationId = course.CourseId; // Ideally, generate a unique ID for each notification.
                var title = $"Reminder for {course.CourseName}";

                // Schedule notifications for start date reminders
                var startReminders = new[] { 7, 3, 1 };
                foreach (var daysBefore in startReminders)
                {
                    var notifyTime = course.CourseStart.AddDays(-daysBefore);
                    var subtitle = $"Starts in {daysBefore} days";
                    await Notification.ScheduleNotificationAsync(notificationId++, title, subtitle, notifyTime, NotificationCategoryType.Reminder);
                }

                // Schedule notifications for end date reminders
                var endReminders = new[] { 7, 3, 1 };
                foreach (var daysBefore in endReminders)
                {
                    var notifyTime = course.CourseEnd.AddDays(-daysBefore);
                    var subtitle = $"Ends in {daysBefore} days";
                    await Notification.ScheduleNotificationAsync(notificationId++, title, subtitle, notifyTime, NotificationCategoryType.Reminder);
                }
            }
        }

        public class CourseOperationOut
        {
            public Course? Course { get; set; }
            public string ErrorMessage { get; set; } = string.Empty;
        }

        public async Task SaveCourseAsync(Course course)
        {
            await Init();
            if (course.CourseId != 0)
            {
                await _database.UpdateAsync(course);
            }
            else
            {
                await _database.InsertAsync(course);
            }
        }
    }
}

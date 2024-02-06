using System.Diagnostics;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Services;
using CourseTracker.Maui.Supplemental;
using CourseTracker.Maui.ViewModels;
using Plugin.LocalNotification;

namespace CourseTracker.Maui.Factories
{
    public class CourseFactory : FactoryBase<Course>
    {
        public CourseFactory(IAsyncSqLite database) : base(database)
        {

        }

        public Course? CreateCourse(CourseVM courseVM, out string errorMessage)
        {
            return CreateCourse(courseVM.Course.CourseId, courseVM.Course.TermId,
                courseVM.Course.CourseName, courseVM.Course.CourseStatus,
                courseVM.Course.CourseStart, courseVM.Course.CourseEnd,
                courseVM.Course.CourseNotes, courseVM.Course.InstructorName,
                courseVM.Course.InstructorEmail, courseVM.Course.InstructorPhone,
                courseVM.Course.NotificationsEnabled, courseVM.Course.CourseAssessmentCount, out errorMessage);
        }

        public Course? CreateCourse(int courseId, int termId, string courseName, string courseStatus,
    DateTime courseStart, DateTime courseEnd, string courseNotes,
    string instructorName, string instructorEmail, string instructorPhone,
    bool notificationEnabled, int courseAssessmentCount, out string errorMessage)
        {
            if (!IsValidCourse(courseId, termId, courseName, courseStatus,
                courseStart, courseEnd, courseNotes, instructorName,
                instructorEmail, instructorPhone, notificationEnabled, courseAssessmentCount, out errorMessage))
            {
                return null;
            }

            Course course = new()
            {
                TermId = termId,
                CourseName = courseName,
                CourseStatus = courseStatus,
                CourseStart = courseStart,
                CourseEnd = courseEnd,
                CourseNotes = courseNotes,
                InstructorName = instructorName,
                InstructorEmail = instructorEmail,
                InstructorPhone = instructorPhone,
                NotificationsEnabled = notificationEnabled,
                CourseAssessmentCount = courseAssessmentCount
            };

            if (course.NotificationsEnabled)
            {
                ScheduleCourseNotifications(course);
            }

            return course;
        }

        public Course? EditCourse(CourseVM courseVM, out string errorMessage)
        {
            return EditCourse(courseVM.Course.CourseId, courseVM.Course.TermId,
                courseVM.Course.CourseName, courseVM.Course.CourseStatus,
                courseVM.Course.CourseStart, courseVM.Course.CourseEnd,
                courseVM.Course.CourseNotes, courseVM.Course.InstructorName,
                courseVM.Course.InstructorEmail, courseVM.Course.InstructorPhone,
                courseVM.Course.NotificationsEnabled, courseVM.Course.CourseAssessmentCount, out errorMessage);
        }

        public Course? EditCourse(int courseId, int termId, string courseName, string courseStatus,
    DateTime courseStart, DateTime courseEnd, string courseNotes,
    string instructorName, string instructorEmail, string instructorPhone,
    bool notificationEnabled, int courseAssessmentCount, out string errorMessage)
        {
            if (!IsValidCourse(courseId, termId, courseName, courseStatus,
                courseStart, courseEnd, courseNotes, instructorName,
                instructorEmail, instructorPhone, notificationEnabled, courseAssessmentCount, out errorMessage))
            {
                return null;
            }

            Course course = new()
            {
                CourseId = courseId,
                TermId = termId,
                CourseName = courseName,
                CourseStatus = courseStatus,
                CourseStart = courseStart,
                CourseEnd = courseEnd,
                CourseNotes = courseNotes,
                InstructorName = instructorName,
                InstructorEmail = instructorEmail,
                InstructorPhone = instructorPhone,
                NotificationsEnabled = notificationEnabled,
                CourseAssessmentCount = courseAssessmentCount
            };

            if (course.NotificationsEnabled)
            {
                ScheduleCourseNotifications(course);
            }

            return course;
        }

        public bool IsValidCourse(int courseId, int termId, string courseName, string courseStatus,
    DateTime courseStart, DateTime courseEnd, string courseNotes,
    string instructorName, string instructorEmail, string instructorPhone,
    bool notificationEnabled, int courseAssessmentCount, out string errorMessage)
        {
            errorMessage = "";
            if (!Validation.NotNull(courseName))
                errorMessage = "Course name cannot be empty.";
            else if (!Validation.NotNull(courseStatus))
                errorMessage = "Course status cannot be empty.";
            else if (!Validation.CourseStatusIsValid(courseStatus))
                errorMessage = "Course status is not valid.";
            else if (!Validation.DatesAreValid(courseStart, courseEnd))
                errorMessage = "Course start and end dates are not valid.";
            else if (!Validation.IdWasSet(courseAssessmentCount))
                errorMessage = "Course ID must be greater than 0.";
            else if (!Validation.IdWasSet(termId))
                errorMessage = "Term ID must be greater than 0.";
            else if (!Validation.NotNull(instructorName))
                errorMessage = "Instructor name cannot be empty.";
            else if (!Validation.NotNull(instructorEmail))
                errorMessage = "Instructor email cannot be empty.";
            else if (!Validation.NotNull(instructorPhone))
                errorMessage = "Instructor phone cannot be empty.";
            else if (!Validation.ValidCourseAssessmentCount(courseAssessmentCount))
                errorMessage = "Course can only have 1 or 2 assessments.";

            Debug.WriteLine(errorMessage);
            return string.IsNullOrEmpty(errorMessage);
        }

        protected override Course? CreateDefaultObject()
        {
            return new Course();
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

    }
}

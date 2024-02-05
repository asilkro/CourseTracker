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

        public Course? CreateCourse(AddCoursesVM addCoursesVM, out string errorMessage)
        {
            return CreateCourse(addCoursesVM.Course.CourseId, addCoursesVM.Course.TermId, 
                addCoursesVM.Course.InstructorId, addCoursesVM.Course.CourseName, 
                addCoursesVM.Course.CourseStatus, addCoursesVM.Course.CourseStart, 
                addCoursesVM.Course.CourseEnd, addCoursesVM.Course.CourseNotes, 
                addCoursesVM.Course.NotificationsEnabled, addCoursesVM.Course.CourseAssessmentCount, out errorMessage);
        }

        public Course? CreateCourse(int courseId, int termId, int instructorId, string courseName,
            string courseStatus, DateTime courseStart, DateTime courseEnd, string courseNotes,
            bool notificationEnabled, int courseAssessmentCount, out string errorMessage)
        {
            if (!IsValidCourse(courseId,termId, instructorId, courseName, courseStatus,
                courseStart, courseEnd, courseNotes, notificationEnabled, courseAssessmentCount, out errorMessage))
            {
                return null;
            }

            var course = CreateObject();
            course.CourseStart = courseStart;
            course.CourseEnd = courseEnd;
            course.CourseName = courseName;
            course.CourseNotes = courseNotes;
            course.CourseStatus = courseStatus;
            course.CourseId = courseId;
            course.TermId = termId;
            course.InstructorId = instructorId;
            course.NotificationsEnabled = notificationEnabled;
            course.CourseAssessmentCount = courseAssessmentCount;
            return course;
        }

        public Course? EditCourse(EditCoursesVM editCoursesVM, out string errorMessage)
        {
            return EditCourse(editCoursesVM.Course.CourseId, editCoursesVM.Course.TermId,
                editCoursesVM.Course.InstructorId, editCoursesVM.Course.CourseName,
                editCoursesVM.Course.CourseStatus, editCoursesVM.Course.CourseStart,
                editCoursesVM.Course.CourseEnd, editCoursesVM.Course.CourseNotes,
                editCoursesVM.Course.NotificationsEnabled, editCoursesVM.Course.CourseAssessmentCount, out errorMessage);
        }

        public Course? EditCourse(int courseId, int termId, int instructorId, string courseName,
            string courseStatus, DateTime courseStart, DateTime courseEnd, string courseNotes,
            bool notificationEnabled, int courseAssessmentCount, out string errorMessage)
        {
            if (!IsValidCourse(courseId, termId, instructorId, courseName, courseStatus,
                courseStart, courseEnd, courseNotes, notificationEnabled, courseAssessmentCount, out errorMessage))
            {
                return null;
            }

            Course course = new()
            {
                CourseStart = courseStart,
                CourseEnd = courseEnd,
                CourseName = courseName,
                CourseNotes = courseNotes,
                CourseStatus = courseStatus,
                CourseId = courseId,
                TermId = termId,
                InstructorId = instructorId,
                NotificationsEnabled = notificationEnabled,
                CourseAssessmentCount = courseAssessmentCount
            };

            if (course.NotificationsEnabled)
            {
                ScheduleCourseNotifications(course);
            }

            return course;
        }

        public bool IsValidCourse(int courseId, int termId, int instructorId, string courseName,
            string courseStatus, DateTime courseStart, DateTime courseEnd, string courseNotes,
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

            else if (!Validation.IdWasSet(instructorId))
                errorMessage = "Instructor ID must be greater than 0.";

            else if (!Validation.ValidCourseAssessmentCount(courseAssessmentCount))
                errorMessage = "Course can only have 1 or 2 assessments.";

            Debug.WriteLine(errorMessage);
            return string.IsNullOrEmpty(errorMessage);
        }

        protected override Course? CreateDefaultObject()
        {
            return new Course();
        }

        public async Task<string> InsertCourseAndUpdateTermCount(Course newCourse)
        {
            var connection = new Connection();
            var term = await connection.FindAsync<Term>(newCourse.TermId);
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
            await connection.InsertAsync(newCourse);

            return "Course added successfully.";
        }

        public async Task ScheduleCourseNotifications(Course course)
        {
            if (course.NotificationsEnabled)
            {
                var notificationId = course.CourseId; // Ideally, generate a unique ID for each notification.
                var title = $"Reminder for {course.CourseName}";

                // Schedule notifications for start date reminders
                var startReminders = new[] { 14, 7, 1 };
                foreach (var daysBefore in startReminders)
                {
                    var notifyTime = course.CourseStart.AddDays(-daysBefore);
                    var subtitle = $"Starts in {daysBefore} days";
                    await Notification.ScheduleNotificationAsync(notificationId++, title, subtitle, notifyTime, NotificationCategoryType.Reminder);
                }

                // Schedule notifications for end date reminders
                var endReminders = new[] { 14, 7, 1 };
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

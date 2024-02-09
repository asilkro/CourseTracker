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
        readonly Connection _database = new();
        public CourseFactory(IAsyncSqLite database) : base(database)
        {
        }

        public async Task<CourseOperationOut> CreateCourseAsync(CourseVM courseVM)
        {
            return await CreateCourseAsync(
                courseVM.Course.CourseId, courseVM.Course.TermId,
                courseVM.Course.CourseName, courseVM.Course.CourseStatus,
                courseVM.Course.CourseStart, courseVM.Course.CourseEnd,
                courseVM.Course.CourseNotes, courseVM.Course.InstructorName,
                courseVM.Course.InstructorEmail, courseVM.Course.InstructorPhone,
                courseVM.Course.NotificationsEnabled, courseVM.Course.CourseAssessmentCount);
        }

        public async Task<CourseOperationOut> CreateCourseAsync(int courseId, int termId, string courseName, string courseStatus, DateTime courseStart, DateTime courseEnd, string courseNotes, string instructorName, string instructorEmail, string instructorPhone, bool notificationEnabled, int courseAssessmentCount)
        {
            string errorMessage;
            if (!IsValidCourse(courseId, termId, courseName, courseStatus, courseStart, courseEnd, courseNotes, instructorName, instructorEmail, instructorPhone, notificationEnabled, courseAssessmentCount, out errorMessage))
            {
                return new CourseOperationOut { ErrorMessage = errorMessage };
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

            var insertResult = await InsertCourseAndUpdateTermCount(course);
            if (insertResult != "Course added successfully.")
            {
                return new CourseOperationOut { ErrorMessage = insertResult };
            }

            if (course.NotificationsEnabled)
            {
                await ScheduleCourseNotifications(course);
            }

            return new CourseOperationOut { Course = course };
        }


        public async Task<CourseOperationOut> EditCourseAsync(CourseVM courseVM)
        {
            return await EditCourseAsync(courseVM.Course.CourseId, courseVM.Course.TermId,
                courseVM.Course.CourseName, courseVM.Course.CourseStatus,
                courseVM.Course.CourseStart, courseVM.Course.CourseEnd,
                courseVM.Course.CourseNotes, courseVM.Course.InstructorName,
                courseVM.Course.InstructorEmail, courseVM.Course.InstructorPhone,
                courseVM.Course.NotificationsEnabled, courseVM.Course.CourseAssessmentCount);
        }

        public async Task<CourseOperationOut> EditCourseAsync(int courseId, int termId, string courseName, string courseStatus, DateTime courseStart, DateTime courseEnd, string courseNotes, string instructorName, string instructorEmail, string instructorPhone, bool notificationEnabled, int courseAssessmentCount)
        {
            string errorMessage;
            if (!IsValidCourse(courseId, termId, courseName, courseStatus, courseStart, courseEnd, courseNotes, instructorName, instructorEmail, instructorPhone, notificationEnabled, courseAssessmentCount, out errorMessage))
            {
                return new CourseOperationOut { ErrorMessage = errorMessage };
            }

            var existingCourse = await _database.FindAsync<Course>(courseId);
            if (existingCourse == null)
            {
                return new CourseOperationOut { ErrorMessage = "Course not found." };
            }

            // Properties get updated here
            existingCourse.TermId = termId;
            existingCourse.CourseName = courseName;
            existingCourse.CourseStatus = courseStatus;
            existingCourse.CourseStart = courseStart;
            existingCourse.CourseEnd = courseEnd;
            existingCourse.CourseNotes = courseNotes;
            existingCourse.InstructorName = instructorName;
            existingCourse.InstructorEmail = instructorEmail;
            existingCourse.InstructorPhone = instructorPhone;
            existingCourse.NotificationsEnabled = notificationEnabled;
            existingCourse.CourseAssessmentCount = courseAssessmentCount;

            // Save changes to the database
            var updateResult = await _database.UpdateAsync<Course>(existingCourse);
            if (updateResult == 0) //
            {
                return new CourseOperationOut { ErrorMessage = "Failed to update the course." };
            }

            if (existingCourse.NotificationsEnabled)
            {
                await ScheduleCourseNotifications(existingCourse);
            }

            return new CourseOperationOut { Course = existingCourse };
        }



        public bool IsValidCourse(
            int courseId,
            int termId,
            string courseName,
            string courseStatus,
            DateTime courseStart,
            DateTime courseEnd,
            string courseNotes,
            string instructorName,
            string instructorEmail,
            string instructorPhone,
            bool notificationEnabled,
            int courseAssessmentCount,
            out string errorMessage)
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
            else if (!Validation.IdWasSet(courseId))
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

        public class CourseOperationOut
        {
            public Course? Course { get; set; }
            public string ErrorMessage { get; set; } = string.Empty;
        }


    }
}

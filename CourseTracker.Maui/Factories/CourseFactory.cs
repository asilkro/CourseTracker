using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.ViewModels;

namespace CourseTracker.Maui.Factories
{
    internal class CourseFactory : FactoryBase<Course>
    {
        public Course? CreateCourse(AddCoursesVM addCoursesVM, out string errorMessage)
        {
            return CreateCourse(addCoursesVM.Course.CourseId, addCoursesVM.Course.TermId, addCoursesVM.Course.InstructorId, addCoursesVM.Course.CourseName, addCoursesVM.Course.CourseStatus, addCoursesVM.Course.CourseStart, addCoursesVM.Course.CourseEnd, addCoursesVM.Course.CourseNotes, addCoursesVM.Course.NotificationsEnabled, addCoursesVM.Course.CourseAssessmentCount, out errorMessage);
        }

        public Course? CreateCourse(int courseId, int termId, int instructorId, string courseName, string courseStatus,
            DateTime courseStart, DateTime courseEnd, string courseNotes, bool notificationEnabled, int courseAssessmentCount, out string errorMessage)
        {
            if (!IsValidCourse(courseId,termId, instructorId, courseName, courseStatus, courseStart, courseEnd, courseNotes, notificationEnabled, courseAssessmentCount, out errorMessage))
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

        public bool IsValidCourse(int courseId, int termId, int instructorId, string courseName, string courseStatus,
            DateTime courseStart, DateTime courseEnd, string courseNotes, bool notificationEnabled, int courseAssessmentCount, out string errorMessage)
        {
            errorMessage = "";
            if (string.IsNullOrEmpty(courseName))
                errorMessage = "Course name cannot be empty.";
            else if (string.IsNullOrEmpty(courseStatus))
                errorMessage = "Course status cannot be empty.";







            return string.IsNullOrEmpty(errorMessage);
        }

        protected override Course? CreateDefaultObject()
        {
            return new Course();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Services;
using CourseTracker.Maui.Supplemental;
using CourseTracker.Maui.ViewModels;

namespace CourseTracker.Maui.Factories
{
    internal class CourseFactory : FactoryBase<Course>
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
    }
}

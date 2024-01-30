using System.Diagnostics;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Placeholder_Stuff;
using CourseTracker.Maui.Services;
using CourseTracker.Maui.Supplemental;
using CourseTracker.Maui.ViewModels;

namespace CourseTracker.Maui.Factories
{
    public class CourseFactory : FactoryBase<Course>
    {
        private readonly DummyData _dummyData;
        public CourseFactory(IAsyncSqLite database, DummyData dummyData) : base(database)
        {
            _dummyData = dummyData;
        }

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

        public async Task<List<Course>> GenerateSampleCourses(int numberOfCourses)
        {
            var sampleCourses = new List<Course>();
            // Loop to create the specified number of sample courses
            for (int i = 0; i < numberOfCourses; i++)
            {
                // Generate sample courses
                var course = new Course
                {
                    CourseId = i + 1,
                    TermId = i + 1,
                    InstructorId = i + 1,
                    CourseName = _dummyData.CourseNames[i % _dummyData.CourseNames.Count],
                    CourseStatus = _dummyData.CourseStatuses[i % _dummyData.CourseStatuses.Count],
                    CourseStart = _dummyData.CourseStatuses[i % _dummyData.CourseStatuses.Count] == "In Progress" ? DateTime.Now : DateTime.Now.AddDays(-30),
                    CourseEnd = _dummyData.CourseStatuses[i % _dummyData.CourseStatuses.Count] == "In Progress" ? DateTime.Now.AddDays(30) : DateTime.Now,
                    CourseNotes = "These are placeholder notes for the sample courses",
                    NotificationsEnabled = i % 2 == 0,
                    CourseAssessmentCount = 1
                };
                if (!IsValidCourse(course.CourseId, course.TermId, course.InstructorId, course.CourseName,
                                       course.CourseStatus, course.CourseStart, course.CourseEnd, course.CourseNotes,
                                                          course.NotificationsEnabled, course.CourseAssessmentCount, out string errorMessage))
                {
                    return sampleCourses; // Return empty list if the generated
                                          // sample data would be invalid
                }

                // Add the sample course to the list
                sampleCourses.Add(course);

                // Insert the sample course into the database
                await AddObject(course);
            }
                return sampleCourses;
        }
    }
}

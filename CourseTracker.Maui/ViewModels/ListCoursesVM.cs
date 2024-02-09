﻿using System.Collections.ObjectModel;
using System.Diagnostics;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Services;

namespace CourseTracker.Maui.ViewModels
{
    public class ListCoursesVM : ViewModelBase
    {
        private Connection _database;

        public ObservableCollection<Course> Courses { get; private set; } = new ObservableCollection<Course>();

        public ListCoursesVM()
        {
            LoadCourses();
        }

        public async Task LoadCourses()
        {
            try
            {
                _database = _database ?? new Connection();
                _database.GetAsyncConnection();
                var updatedCoursesList = await _database.Table<Course>();
                Courses.Clear();
                foreach (var course in updatedCoursesList)
                {
                    Courses.Add(course);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Issue loading courses: " + ex.Message);
                return;
            }
        }

        private Course course = new();

		public Course Course
		{
            get { return course; }
            set
			{
                if (course != value)
				{
                    course = value;
                    OnPropertyChanged(nameof(Course));
                }
            }
        }

        private int courseId;
        public int CourseId
        {
            get { return courseId; }
            set
            {
                if (courseId != value)
                {
                    courseId = value;
                    OnPropertyChanged(nameof(CourseId));
                }
            }
        }

        private string courseName;
        public string CourseName
        {
            get { return courseName; }
            set
            {
                if (courseName != value)
                {
                    courseName = value;
                    OnPropertyChanged(nameof(CourseName));
                }
            }
        }
        private int termId;
        public int TermId
        {
            get { return termId; }
            set
            {
                if (termId != value)
                {
                    termId = value;
                    OnPropertyChanged(nameof(TermId));
                }
            }
        }
        private string instructorName;
        public string InstructorName
        {
            get { return instructorName; }
            set
            {
                if (instructorName != value)
                {
                    instructorName = value;
                    OnPropertyChanged(nameof(InstructorName));
                }
            }
        }
        private string instructorPhone;
        public string InstructorPhone
        {
            get { return instructorPhone; }
            set
            {
                if (instructorPhone != value)
                {
                    instructorPhone = value;
                    OnPropertyChanged(nameof(InstructorPhone));
                }
            }
        }
        private string instructorEmail;
        public string InstructorEmail
        {
            get { return instructorEmail; }
            set
            {
                if (instructorEmail != value)
                {
                    instructorEmail = value;
                    OnPropertyChanged(nameof(InstructorEmail));
                }
            }
        }
        private string courseStatus;
        public string CourseStatus
        {
            get { return courseStatus; }
            set
            {
                if (courseStatus != value)
                {
                    courseStatus = value;
                    OnPropertyChanged(nameof(CourseStatus));
                }
            }
        }
        private DateTime courseStart;
        public DateTime CourseStart
        {
            get { return courseStart; }
            set
            {
                if (courseStart != value)
                {
                    courseStart = value;
                    OnPropertyChanged(nameof(CourseStart));
                }
            }
        }

        private DateTime courseEnd;
        public DateTime CourseEnd
        {
            get { return courseEnd; }
            set
            {
                if (courseEnd != value)
                {
                    courseEnd = value;
                    OnPropertyChanged(nameof(CourseEnd));
                }
            }
        }

        public int courseAssessmentCount;
        public int CourseAssessmentCount
        {
            get { return courseAssessmentCount; }
            set
            {
                if (courseAssessmentCount != value)
                {
                    courseAssessmentCount = value;
                    OnPropertyChanged(nameof(CourseAssessmentCount));
                }
            }
        }

        public bool notificationsEnabled;
        public bool NotificationsEnabled
        {
            get { return notificationsEnabled; }
            set
            {
                if (notificationsEnabled != value)
                {
                    notificationsEnabled = value;
                    OnPropertyChanged(nameof(NotificationsEnabled));
                }
            }
        }
	}
}
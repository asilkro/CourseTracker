using System.Collections.ObjectModel;
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
                    OnPropertyChanged("Course");
                }
            }
        }
	}
}
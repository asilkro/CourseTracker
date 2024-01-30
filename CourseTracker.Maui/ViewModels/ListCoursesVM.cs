using CourseTracker.Maui.Models;

namespace CourseTracker.Maui.ViewModels
{
    public class ListCoursesVM : ViewModelBase
    {
		private List<Course> courses;

		public List<Course> Courses
		{
			get { return courses; }
			set 
			{ 
				if (courses != value)
				{
                    courses = value;
                    OnPropertyChanged("Courses");
                }
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
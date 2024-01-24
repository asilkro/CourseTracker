using CourseTracker.Maui.Models;

namespace CourseTracker.Maui.ViewModels
{
    internal class AddCoursesVM : ViewModelBase
    {
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

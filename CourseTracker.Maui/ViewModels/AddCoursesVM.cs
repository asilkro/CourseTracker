using CourseTracker.Maui.Models;

namespace CourseTracker.Maui.ViewModels
{
    public class AddCoursesVM : ViewModelBase
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

		private Instructor instructor = new();
		public Instructor Instructor
		{
            get { return instructor; }
            set
			{
                if (instructor != value)
				{
                    instructor = value;
                    OnPropertyChanged("Instructor");
                }
            }
        }

		private Assessment assessment = new();
		public Assessment Assessment
		{
            get { return assessment; }
            set
			{
                if (assessment != value)
				{
                    assessment = value;
                    OnPropertyChanged("Assessment");
                }
            }
        }

		private DateTime minimumDate = DateTime.Parse("01/01/2020");
		public DateTime MinimumDate
		{
			get { return minimumDate; }
			set
			{
				if (minimumDate != value)
				{
					minimumDate = value;
					OnPropertyChanged("MinimumDate");
				}
			}
		}

		private DateTime maximumDate = DateTime.Parse("12/31/4020");
		public DateTime MaximumDate
		{ 
			get { return maximumDate; }
		    set
			{
				if(maximumDate != value)
				{
					maximumDate = value;
					OnPropertyChanged("MaximumDate");
				}
			}
		}

		private DateTime date = DateTime.Now.Date;
		public DateTime Date
		{
			get { return date; }
			set
			{
                if(date != value)
				{
                    date = value;
                    OnPropertyChanged("Date");
                }
            }
		}
	}
}

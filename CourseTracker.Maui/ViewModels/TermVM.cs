using CourseTracker.Maui.Models;

namespace CourseTracker.Maui.ViewModels
{
    public class TermVM : ViewModelBase
    {
        private Term term = new();

        public Term Term
        {
            get { return term; }
            set
            {
                if (term != value)
                {
                    term = value;
                    OnPropertyChanged("Term");
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
                if (maximumDate != value)
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
                if (date != value)
                {
                    date = value;
                    OnPropertyChanged("Date");
                }
            }
        }

    }
}

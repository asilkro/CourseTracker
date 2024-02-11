using CourseTracker.Maui.Models;

namespace CourseTracker.Maui.ViewModels
{
    public class TermVM : ViewModelBase
    {
        public TermVM(Term term) { }
        public TermVM() { }

        private Term term = new();

        public Term Term
        {
            get { return term; }
            set
            {
                if (term != value)
                {
                    term = value;
                    OnPropertyChanged(nameof(Term));
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
        private string termName;
        public string TermName
        {
            get { return termName; }
            set
            {
                if (termName != value)
                {
                    termName = value;
                    OnPropertyChanged(nameof(TermName));
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
                    OnPropertyChanged(nameof(MinimumDate));
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
                    OnPropertyChanged(nameof(MaximumDate));
                }
            }
        }

        private static DateTime dateStart = DateTime.Now.Date;
        private DateTime termStart = new DateTime(dateStart.Year, dateStart.Month, 1);
        public DateTime TermStart
        {
            get { return termStart; }
            set
            {
                if (termStart != value)
                {
                    termStart = value;
                    OnPropertyChanged(nameof(TermStart));
                }
            }
        }

        private static DateTime dateEnd = DateTime.Now.Date;
        private DateTime termEnd = new DateTime(dateEnd.Year, dateEnd.Month, DateTime.DaysInMonth(dateEnd.Year, dateEnd.Month)).AddMonths(6).AddTicks(-1);
        public DateTime TermEnd
        {
            get { return termEnd; }
            set
            {
                if (termEnd != value)
                {
                    termEnd = value;
                    OnPropertyChanged(nameof(TermEnd));
                }
            }
        }

        private int courseCount;
        public int CourseCount
        {
            get { return courseCount; }
            set
            {
                if (courseCount != value)
                {
                    courseCount = value;
                    OnPropertyChanged(nameof(CourseCount));
                }
            }
        }

        private bool notificationsEnabled;
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

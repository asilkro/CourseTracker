using System.Diagnostics;
using CourseTracker.Maui.Data;
using CourseTracker.Maui.Factories;
using CourseTracker.Maui.Models;

namespace CourseTracker.Maui.ViewModels
{
    [QueryProperty(nameof(EditTermId), nameof(EditTermId))]
    public class TermVM : ViewModelBase
    {
        private Term term = new();
        public TermVM()
        {
            OnTermSubmitButtonClick = new Command(async () => await SubmitButtonClicked());
            OnTermCancelButtonClick = new Command(async () => await CancelButtonClicked());
        }

        private async Task SubmitButtonClicked()
        {
            throw new NotImplementedException();
        }

        public int termId;
        public int editTermId;
        public Command OnTermSubmitButtonClick { get; set; }
        public Command OnTermCancelButtonClick { get; set; }


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
        public int EditTermId
        {
            get { return editTermId; }
            set
            {
                if (editTermId != value)
                {
                    editTermId = value;
                    PerformOperation(value);
                }
            }
        }
        private async Task PerformOperation(int Id)
        {
            if (Id <= 0)
            {
                TermId = await termsDB.GetNextId();
            }
            else
            {
                Debug.WriteLine("TermId: " + Id);
                Term temp = await termsDB.GetTermByIdAsync(Id);
                if (temp == null)
                {
                    return;
                }
                TermName = temp.TermName;
                TermStart = temp.TermStart;
                TermEnd = temp.TermEnd;
                CourseCount = temp.CourseCount;
                TermId = temp.TermId;
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

        public async void OnAppearing()
        {
            if (EditTermId <= 0)
            {
                TermId = await termsDB.GetNextId();
            }
        }
    }
}

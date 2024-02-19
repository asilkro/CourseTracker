using System.Diagnostics;
using CourseTracker.Maui.Data;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Supplemental;
using CourseTracker.Maui.Views;

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
            Term term = new()
            {
                TermId = TermId,
                TermName = TermName,
                TermStart = TermStart,
                TermEnd = TermEnd,
                CourseCount = CourseCount
            };
            string message = IsValidTerm(term);
            if (!string.IsNullOrEmpty(message))
            {
                ShowToast(message);
                return;
            }

            //await sharedDB.SaveTerm(term);

            bool anotherTermWanted = await Application.Current.MainPage.DisplayAlert("Another Term?", "Would you like to add another term?", "Yes", "No");
            if (anotherTermWanted)
            {
                await Shell.Current.GoToAsync(nameof(TermPage));
            }
            else
            {
                await Shell.Current.GoToAsync("//homepage");
            }
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


        private static readonly DateTime dateStart = DateTime.Now.Date;
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

        private static readonly DateTime dateEnd = DateTime.Now.Date;
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

        public string IsValidTerm(Term term)
        {
            var errorMessage = string.Empty;

            if (!Validation.IdWasSet(termId))
                errorMessage = "Term ID must be greater than 0.";
            else if (!Validation.NotNull(termName))
                errorMessage = "Term name cannot be empty.";
            else if (!Validation.TermsAreValid(termStart, termEnd))
                errorMessage = "Term start and end dates must be the first and last days of the month, respectively.";
            else if (!Validation.CourseCountIsValid(courseCount))
                errorMessage = "Terms must have between 1 and 6 courses.";
            else if (!Validation.DatesAreValid(termStart, termEnd))
                errorMessage = "Term start and end dates are not valid.";
            else if (Validation.IsUniqueTermName(termName, new()).Result == false)
                errorMessage = "Term name must be unique.";

            if (!string.IsNullOrEmpty(errorMessage))
                ShowToast(errorMessage);
            return errorMessage;
        }
    }
}

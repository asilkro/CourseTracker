using System.Collections.ObjectModel;
using System.Diagnostics;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Supplemental;
using CourseTracker.Maui.Views;

namespace CourseTracker.Maui.ViewModels
{
    [QueryProperty(nameof(EditTermId), nameof(EditTermId))]
    public class TermVM : ViewModelBase
    {
        #region Fields
        private Term term = new();
        private int termId;
        private int editTermId;
        private string courseName;
        private string courseStatus;
        private string termName;
        private static readonly new DateTime dateStart = DateTime.Now.Date;
        private static readonly new DateTime dateEnd = DateTime.Now.Date;
        private DateTime termStart = new DateTime(dateStart.Year, dateStart.Month, 1);
        private DateTime termEnd = DateTime.Now.Date.AddDays(30);
        private int courseCount;
        private bool notificationsEnabled;
        #endregion

        #region Properties
        public ObservableCollection<Course> RelatedCourses { get; private set; } = [];
        public Command OnTermSubmitButtonClick { get; set; }
        public Command OnLoadCourseButtonClick { get; set; }
        public Command OnTermCancelButtonClick { get; set; }

        public Term Term
        {
            get => term;
            set => SetProperty(ref term, value);
        }

        public int TermId
        {
            get => termId;
            set => SetProperty(ref termId, value);
        }

        public int EditTermId
        {
            get => editTermId;
            set => SetProperty(ref editTermId, value, onChanged: () => PerformOperation(value));
        }

        public string TermName
        {
            get => termName;
            set => SetProperty(ref termName, value);
        }

        public DateTime TermStart
        {
            get => termStart;
            set => SetProperty(ref termStart, value);
        }

        public DateTime TermEnd
        {
            get => termEnd;
            set => SetProperty(ref termEnd, value);
        }

        public string CourseName
        {
            get => courseName;
            set => SetProperty(ref courseName, value);
        }

        public string CourseStatus
        {
            get => courseStatus;
            set => SetProperty(ref courseStatus, value);
        }

        public int CourseCount
        {
            get => courseCount;
            set => SetProperty(ref courseCount, value);
        }

        #endregion

        #region Constructors
        public TermVM()
        {
            OnTermSubmitButtonClick = new Command(async () => await SubmitButtonClicked());
            OnLoadCourseButtonClick = new Command(async () => await LoadCourses());
            OnTermCancelButtonClick = new Command(async () => await CancelButtonClicked());
        }
        #endregion

        #region Methods
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

            await sharedDB.SaveTerm(term);

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

        private async Task LoadCourses()
        {
            Debug.WriteLine(RelatedCourses.Count + " related courses count.");
            try
            {
                RelatedCourses ??= [];
                if (RelatedCourses.Count > 0)
                {
                    RelatedCourses.Clear();
                }
                List<Course> coursesForGivenTerm = await courseDB.GetCoursesByTermIdAsync(TermId);
                Debug.WriteLine(TermId + " is your termId.");
                Debug.WriteLine("Count of courses for term: " + coursesForGivenTerm.Count);
                switch (coursesForGivenTerm)
                {
                    case null:
                        ShowToast("Course count returned null.");
                        return;
                    case { Count: 0 }:
                        ShowToast("No courses found for term.");
                        return;
                }
                foreach (var course in coursesForGivenTerm)
                {
                    int i = 1;
                    Debug.WriteLine(course.CourseName + " loaded, number " + i);
                    RelatedCourses.Add(course);
                    i++;
                }
                OnPropertyChanged(nameof(RelatedCourses));
                CourseCount = RelatedCourses.Count;
            }
            catch (Exception ex)
            {
                ShowToast("Issue loading courses for " + TermName + ": " + ex.Message);
                return;
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
                errorMessage = "Terms should have between 1 and 6 courses.";
            else if (!Validation.DatesAreValid(termStart, termEnd))
                errorMessage = "Term start and end dates are not valid.";

            if (!string.IsNullOrEmpty(errorMessage))
                ShowToast(errorMessage);
            return errorMessage;
        }

        public async void OnAppearing()
        {
            if (EditTermId <= 0)
            {
                TermId = await termsDB.GetNextId();
            }
            await LoadCourses();
        }
        #endregion
    }
}

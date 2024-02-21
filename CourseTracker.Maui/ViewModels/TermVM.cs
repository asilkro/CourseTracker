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
        public ObservableCollection<Course> RelatedCourses { get; private set; } = [];
        private Term term = new();
        public TermVM()
        {
            OnTermSubmitButtonClick = new Command(async () => await SubmitButtonClicked());
            OnTermCancelButtonClick = new Command(async () => await CancelButtonClicked());
            OnLoadCourseButtonClick = new Command(async () => await LoadCourses());
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

        public int termId;
        public int editTermId;
        public Command OnTermSubmitButtonClick { get; set; }
        public Command OnTermCancelButtonClick { get; set; }
        public Command OnLoadCourseButtonClick { get; set; }

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


        private static readonly new DateTime dateStart = DateTime.Now.Date;
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

        private static readonly new DateTime dateEnd = DateTime.Now.Date;
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
            await LoadCourses();
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

        public async Task LoadCourses()
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
                    Debug.WriteLine(Course.CourseName + " loaded, number " + i);
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

    }
}

using System.Collections.ObjectModel;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Supplemental;
using CourseTracker.Maui.Views;

namespace CourseTracker.Maui.ViewModels
{
    [QueryProperty(nameof(EditCourseId), nameof(EditCourseId))]
    public class CourseVM : ViewModelBase
    {
        #region Fields
        private Course course;
        private int editCourseId;
        private Assessment assessment = new();
        private readonly DateTime minimumDate = DateTime.Parse("01/01/2020");
        private readonly DateTime maximumDate = DateTime.Parse("12/31/4020");
        private DateTime date = DateTime.Now.Date;
        private Term term = new();
        private Term _selectedTerm;
        private int courseId;
        private int termId;
        private string instructorName;
        private string instructorPhone;
        private string instructorEmail;
        private string courseName;
        private string courseStatus;
        private string termName;
        private string courseNotes;
        private DateTime courseStart = DateTime.Now.Date;
        private DateTime courseEnd = DateTime.Now.Date.AddDays(90);
        private bool notificationsEnabled;
        private int courseAssessmentCount;

        #endregion

        #region Properties

        public Command CourseNoteShareButton_Clicked { get; set; }
        public Command OnCourseSubmitButtonClick { get; set; }
        public Command OnCourseCancelButtonClick { get; set; }

        public Course Course
        {
            get => course;
            set => SetProperty(ref course, value);
        }

        public Assessment Assessment
        {
            get => assessment;
            set => SetProperty(ref assessment, value);
        }

        public DateTime Date
        {
            get => date;
            set => SetProperty(ref date, value);
        }

        public Term Term
        {
            get => term;
            set => SetProperty(ref term, value);
        }

        public ObservableCollection<Term> Terms { get; } = [];

        public Term SelectedTerm
        {
            get => _selectedTerm;
            set
            {
                SetProperty(ref _selectedTerm, value, onChanged: () =>
                {
                    if (Course != null && value != null)
                    {
                        Course.TermId = value.TermId;
                    }
                });
            }
        }

        public int CourseId
        {
            get => courseId;
            set => SetProperty(ref courseId, value);
        }

        public int EditCourseId
        {
            get => editCourseId;
            set => SetProperty(ref editCourseId, value, onChanged: () => PerformOperation(value));
        }

        public int TermId
        {
            get => termId;
            set => SetProperty(ref termId, value);
        }

        public string InstructorName
        {
            get => instructorName;
            set => SetProperty(ref instructorName, value);
        }

        public string InstructorPhone
        {
            get => instructorPhone;
            set => SetProperty(ref instructorPhone, value);
        }

        public string InstructorEmail
        {
            get => instructorEmail;
            set => SetProperty(ref instructorEmail, value);
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

        public string TermName
        {
            get => termName;
            set => SetProperty(ref termName, value);
        }

        public string CourseNotes
        {
            get => courseNotes;
            set => SetProperty(ref courseNotes, value);
        }

        public DateTime CourseStart
        {
            get => courseStart;
            set => SetProperty(ref courseStart, value);
        }

        public DateTime CourseEnd
        {
            get => courseEnd;
            set => SetProperty(ref courseEnd, value);
        }

        public bool NotificationsEnabled
        {
            get => notificationsEnabled;
            set => SetProperty(ref notificationsEnabled, value);
        }

        public int CourseAssessmentCount
        {
            get => courseAssessmentCount;
            set => SetProperty(ref courseAssessmentCount, value);
        }

        #endregion

        #region Constructors

        public CourseVM()
        {
            LoadTerms();
            CourseNoteShareButton_Clicked = new Command(async () => await CourseNoteShareButtonClicked());
            OnCourseSubmitButtonClick = new Command(async () => await SubmitButtonClicked());
            OnCourseCancelButtonClick = new Command(async () => await CancelButtonClicked());
        }

        #endregion

        private async Task LoadTerms()
        {
            try
            {
                var terms = await termsDB.GetTermsAsync();
                Course course = await courseDB.GetCourseByIdAsync(EditCourseId);
                Terms.Clear();
                Term term1 = new();
                foreach (var term in terms)
                {
                    if (course.TermId == term.TermId)
                    {
                        term1 = term;
                    }
                    Terms.Add(term);
                }
                SelectedTerm = term1;
            }
            catch (Exception ex)
            {
                ShowToast("Issue loading terms: " + ex.Message);
            }
        }

        private async Task PerformOperation(int Id)
        {
            if (Id <= 0)
            {
                CourseId = await courseDB.GetNextId();
                CourseStart = dateStart;
                CourseEnd = dateEnd;
                return;
            }
            Course? courseCounter = await courseDB.GetCourseByIdAsync(Id);
            int count = 0;
            
            Course temp = await courseDB.GetCourseByIdAsync(Id);
            CourseName = temp.CourseName;
            InstructorName = temp.InstructorName;
            InstructorPhone = temp.InstructorPhone;
            InstructorEmail = temp.InstructorEmail;
            CourseStatus = temp.CourseStatus;
            CourseStart = temp.CourseStart;
            CourseEnd = temp.CourseEnd;
            CourseNotes = temp.CourseNotes;
            NotificationsEnabled = temp.NotificationsEnabled;
            CourseId = temp.CourseId;
            TermId = temp.TermId;
            if (courseCounter != null)
            {
                count =  courseCounter.CourseAssessmentCount;
                CourseAssessmentCount = count;
            }
            else
            {
                CourseAssessmentCount = count;
            }
        }

        public async Task SubmitButtonClicked()
        {
            Course course = new()
            {
                CourseId = CourseId,
                CourseName = CourseName,
                TermId = SelectedTerm.TermId,
                CourseStatus = CourseStatus,
                CourseStart = CourseStart,
                CourseEnd = CourseEnd,
                CourseNotes = CourseNotes,
                InstructorName = InstructorName,
                InstructorEmail = InstructorEmail,
                InstructorPhone = InstructorPhone,
                NotificationsEnabled = NotificationsEnabled,
                CourseAssessmentCount = CourseAssessmentCount
            };
            string message = IsValidCourse(course);
            if (!string.IsNullOrEmpty(message))
            {
                ShowToast(message);
                return;
            }

            await sharedDB.InsertCourseAndUpdateTerm(course);

            bool anotherCourseWanted = await Application.Current.MainPage.DisplayAlert("Course Saved", "Would you like to add another Course?", "Yes", "No");
            if (anotherCourseWanted)
            {
                await Shell.Current.GoToAsync(nameof(CoursePage));
            }
            else
            {
                await Shell.Current.GoToAsync("//homepage");
            }
        }

        private async Task CourseNoteShareButtonClicked()
        {
            var notes = Course.CourseNotes;
            var course = Course.CourseName;

            if (!Validation.NotNull(notes))
            {
                ShowToast("No course notes found to share.");
            }
            else
            {
                await ShareText(notes, course);
            }
        }

        public static async Task ShareText(string notes, string source)
        {
            try
            {
                await Share.Default.RequestAsync(new ShareTextRequest
                {
                    Text = notes + Environment.NewLine + "SharedDB on " + DateTime.Now.ToShortDateString(),
                    Title = "Course Notes for " + source,
                });
                ShowToast("Notes shared successfully.");
            }
            catch (Exception ex)
            {
                ShowToast("Error sharing notes. " + ex.Message);
            }
        }

        public static string IsValidCourse(Course course)
        {
            var errorMessage = string.Empty;
            if (!Validation.NotNull(course.CourseName))
                errorMessage = "Course name cannot be empty or undefined.";
            else if (!Validation.IsUniqueCourseName(course.CourseName, new()).Result == false)
                errorMessage = "Course name must be unique.";
            else if (!Validation.NotNull(course.CourseStatus))
                errorMessage = "Course status cannot be empty or undefined.";
            else if (!Validation.CourseStatusIsValid(course.CourseStatus))
                errorMessage = "Course status is not valid.";
            else if (!Validation.DatesAreValid(course.CourseStart, course.CourseEnd))
                errorMessage = "Course start and end dates are not valid.";
            else if (!Validation.IdWasSet(course.CourseId))
                errorMessage = "Valid Course ID must be greater than 0.";
            else if (!Validation.IdWasSet(course.TermId))
                errorMessage = "Valid Term ID must be greater than 0.";
            else if (!Validation.NotNull(course.InstructorName))
                errorMessage = "Instructor name cannot be empty.";
            else if (!Validation.NotNull(course.InstructorEmail))
                errorMessage = "Instructor email cannot be empty.";
            else if (!Validation.EmailIsValid(course.InstructorEmail))
                errorMessage = "Instructor email format is not valid.";
            else if (!Validation.NotNull(course.InstructorPhone))
                errorMessage = "Instructor phone cannot be empty.";
            else if (!Validation.ValidPhoneNumber(course.InstructorPhone))
                errorMessage = "Instructor phone is not valid. Use xxx-xxx-xxxx format.";
            else if (!Validation.ValidCourseAssessmentCount(course.CourseAssessmentCount))
                errorMessage = "Course can only have 1 or 2 assessments."; //

            if (!string.IsNullOrEmpty(errorMessage))
                ShowToast(errorMessage);
            return errorMessage;
        }

        public async void OnAppearing()
        {
            if (EditCourseId <= 0)
            {
                CourseId = await courseDB.GetNextId();
            }

        }

    }
}

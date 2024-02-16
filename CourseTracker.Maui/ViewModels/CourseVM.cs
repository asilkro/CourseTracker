using System.Collections.ObjectModel;
using System.Diagnostics;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Services;
using CourseTracker.Maui.Supplemental;

namespace CourseTracker.Maui.ViewModels
{
    [QueryProperty(nameof(EditCourseId), nameof(EditCourseId))]
    public class CourseVM : ViewModelBase
    {
        private Course course;
        public int editCourseId;
        public Command CourseNoteShareButton_Clicked { get; set; }
        public Command OnCourseSubmitButtonClick { get; set; }
        public Command OnCourseCancelButtonClick { get; set; }

        public CourseVM()
        {
            LoadTerms();
            CourseNoteShareButton_Clicked = new Command(async () => await CourseNoteShareButtonClicked());
            OnCourseSubmitButtonClick = new Command(async () => await SubmitButtonClicked());
            OnCourseCancelButtonClick = new Command(async () => await CancelButtonClicked());
            
        }

        public async Task InitializeAsync()
        {
            if (courseId > 0)
            {
                await LoadCourseDetails();
            }
            else
            {
                await LoadTerms();
            }
        }

        public Course Course
        {
            get => course;
            set
            {
                if (course != value)
                {
                    course = value;
                    OnPropertyChanged(nameof(Course));
                }
            }
        }


        private async Task LoadCourseDetails()
        {

            Connection DatabaseService = new();
            DatabaseService.GetAsyncConnection();
            
            if (courseId > 0)
            {
                Course = await DatabaseService.FindAsync<Course>(courseId);
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
                    OnPropertyChanged(nameof(Assessment));
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

        private DateTime date = DateTime.Now.Date;
        public DateTime Date
        {
            get { return date; }
            set
            {
                if (date != value)
                {
                    date = value;
                    OnPropertyChanged(nameof(Date));
                }
            }
        }

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

        public ObservableCollection<Term> Terms { get; } = new ObservableCollection<Term>();
        Connection _connection;


        private async Task LoadTerms()
        {
            try
            {
                _connection = _connection ?? new Connection();
                _connection.GetAsyncConnection();
                var terms = await _connection.Table<Term>();
                Terms.Clear();
                foreach (var term in terms)
                {
                    Terms.Add(term);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Issue loading terms: " + ex.Message);
            }
        }



        private Term _selectedTerm;
        public Term SelectedTerm
        {
            get => _selectedTerm;
            set
            {
                if (_selectedTerm != value)
                {
                    _selectedTerm = value;
                    Course.TermId = value.TermId;
                    OnPropertyChanged(nameof(SelectedTerm));
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
        public int EditCourseId
        {
            get { return editCourseId; }
            set
            {
                if (editCourseId != value)
                {
                    editCourseId = value;
                    PerformOperation(value);
                }
            }
        }

        private async Task PerformOperation(int Id)
        {
            Debug.WriteLine("CourseId: " + Id);
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

        private string instructorPhone;
        public string InstructorPhone
        {
            get { return instructorPhone; }
            set
            {
                if (instructorPhone != value)
                {
                    instructorPhone = value;
                    OnPropertyChanged(nameof(InstructorPhone));
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

        private string courseNotes;
        public string CourseNotes
        {
            get { return courseNotes; }
            set
            {
                if (courseNotes != value)
                {
                    courseNotes = value;
                    OnPropertyChanged(nameof(CourseNotes));
                }
            }
        }

        private DateTime courseStart = DateTime.Now.Date;
        public DateTime CourseStart
        {
            get { return courseStart; }
            set
            {
                if (courseStart != value)
                {
                    courseStart = value;
                    OnPropertyChanged(nameof(CourseStart));
                }
            }
        }

        private DateTime courseEnd = DateTime.Now.Date.AddDays(90);
        public DateTime CourseEnd
        {
            get { return courseEnd; }
            set
            {
                if (courseEnd != value)
                {
                    courseEnd = value;
                    OnPropertyChanged(nameof(CourseEnd));
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

        private int courseAssessmentCount;
        public int CourseAssessmentCount
        {
            get { return courseAssessmentCount; }
            set
            {
                if (courseAssessmentCount != value)
                {
                    courseAssessmentCount = value;
                    OnPropertyChanged(nameof(CourseAssessmentCount));
                }
            }
        }

        public async Task SubmitButtonClicked()
        {
            Course course = new Course()
            {
                CourseId = CourseId,
                CourseName = CourseName,
                TermId = TermId,
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
            await courseDB.SaveCourseAsync(course);
            
        }

        private async Task CourseNoteShareButtonClicked()
        {
            var notes = Course.CourseNotes;
            var course = Course.CourseName;

            Debug.WriteLine("Course.CourseNotes = " + notes);


            if (!Validation.NotNull(notes))
            {
                App.Current.MainPage.DisplayAlert("Note Validation Error", "No course notes found to share.", "OK");
            }
            else
            {
                await ShareText(notes, course);
            }
        }

        public async Task ShareText(string notes, string source) //
        {
            try 
            {
                await Share.Default.RequestAsync(new ShareTextRequest
                {
                    Text = notes + Environment.NewLine + "Shared on " + DateTime.Now.ToShortDateString(),
                    Title = "Course Notes for " + source,
                });
                ShowToast("Notes shared successfully.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error sharing notes: " + ex.Message);
                ShowToast("Error sharing notes. " + ex.Message);
            }
        }
        public string IsValidCourse(Course course)
        {
            var errorMessage = string.Empty;
            if (!Validation.NotNull(course.CourseName))
                errorMessage = "Course name cannot be empty or undefined.";
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
            else if (!Validation.NotTryingToDropTables(course.CourseNotes)) // Not particularly robust, but it's a start
                errorMessage = "Invalid input in notes detected.";
            else if (!Validation.NotNull(course.InstructorPhone))
                errorMessage = "Instructor phone cannot be empty.";
            else if (!Validation.ValidPhoneNumber(course.InstructorPhone))
                errorMessage = "Instructor phone is not valid. Use xxx-xxx-xxxx format.";
            else if (!Validation.ValidCourseAssessmentCount(course.CourseAssessmentCount))
                errorMessage = "Course can only have 1 or 2 assessments.";

            Debug.WriteLine(errorMessage);
            return errorMessage;
        }
        public async Task<string> InsertCourseAndUpdateTermCount(Course course)
        {
            Term term = await termsDB.GetTermByIdAsync(course.TermId);
            if (term == null)
            {
                return "Associated term not found.";
            }

            if (term.CourseCount >= 6)
            {
                return "Terms may NOT consist of more than six courses.";
            }

            term.CourseCount += 1;
            await termsDB.SaveTermAsync(term);
            await courseDB.SaveCourseAsync(course);

            return "Course added successfully.";
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
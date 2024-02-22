using System.Collections.ObjectModel;
using System.Diagnostics;
using CourseTracker.Maui.Data;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Views;

namespace CourseTracker.Maui.ViewModels
{
    [QueryProperty(nameof(EditAssessmentId), nameof(EditAssessmentId))]
    public class AssessmentVM : ViewModelBase
    {
        #region Fields
        private Assessment assessment = new();
        private int editAssessmentId;
        private int assessmentId;
        private string assessmentName;
        private string assessmentType;
        private DateTime assessmentStartDate = DateTime.Now.Date;
        private DateTime assessmentEndDate = DateTime.Now.Date.AddDays(30);
        private int relatedCourseId;
        private bool notificationsEnabled;
        private Course _selectedCourse;
        #endregion

        #region Properties
        public Command OnAssessmentSubmitButtonClick { get; set; }
        public Command OnAssessmentCancelButtonClick { get; set; }
        public ObservableCollection<Course> Courses { get; } = new ObservableCollection<Course>();

        public Assessment Assessment
        {
            get => assessment;
            set => SetProperty(ref assessment, value);
        }

        public int EditAssessmentId
        {
            get => editAssessmentId;
            set => SetProperty(ref editAssessmentId, value, onChanged: () => PerformOperation(value));
        }

        public int AssessmentId
        {
            get => assessmentId;
            set => SetProperty(ref assessmentId, value);
        }

        public string AssessmentName
        {
            get => assessmentName;
            set => SetProperty(ref assessmentName, value);
        }

        public string AssessmentType
        {
            get => assessmentType;
            set => SetProperty(ref assessmentType, value);
        }

        public DateTime AssessmentStartDate
        {
            get => assessmentStartDate;
            set => SetProperty(ref assessmentStartDate, value);
        }

        public DateTime AssessmentEndDate
        {
            get => assessmentEndDate;
            set => SetProperty(ref assessmentEndDate, value);
        }

        public int RelatedCourseId
        {
            get => relatedCourseId;
            set => SetProperty(ref relatedCourseId, value);
        }

        public bool NotificationsEnabled
        {
            get => notificationsEnabled;
            set => SetProperty(ref notificationsEnabled, value);
        }

        public Course SelectedCourse
        {
            get => _selectedCourse;
            set
            {
                SetProperty(ref _selectedCourse, value, onChanged: () =>
                {
                    if (Assessment != null && value != null)
                    {
                        Assessment.RelatedCourseId = value.CourseId;
                    }
                });
            }
        }
        #endregion

        #region Constructors
        public AssessmentVM()
        {
            LoadCourses();
            OnAssessmentSubmitButtonClick = new Command(async () => await SubmitButtonClicked());
            OnAssessmentCancelButtonClick = new Command(async () => await CancelButtonClicked());
        }
        #endregion

        #region Private Methods
        private async Task LoadCourses()
        {
            try
            {
                var courses = await courseDB.GetCoursesAsync();
                Assessment assessment = await assessmentDB.GetAssessmentByIdAsync(EditAssessmentId);
                if (assessment == null)
                {
                    assessment = new Assessment();
                }
                Courses.Clear();
                Course course1 = new();
                foreach (var course in courses)
                {
                    if (course.CourseId == assessment.RelatedCourseId)
                    {
                        course1 = course;
                    }
                    Courses.Add(course);
                }
                SelectedCourse = course1;
            }
            catch (Exception ex)
            {
                ShowToast("Issue loading courses: " + ex.Message);
            }
        }

        private async Task PerformOperation(int Id)
        {
            if (Id <= 0)
            {
                AssessmentId = await assessmentDB.GetNextId();
                AssessmentStartDate = dateStart;
                AssessmentEndDate = dateEnd;
                return;
            }

            Debug.WriteLine("AssessmentId: " + Id);
            Assessment temp = await assessmentDB.GetAssessmentByIdAsync(Id);
            AssessmentId = temp.AssessmentId;
            AssessmentName = temp.AssessmentName;
            AssessmentType = temp.AssessmentType;
            AssessmentStartDate = temp.AssessmentStartDate;
            AssessmentEndDate = temp.AssessmentEndDate;
            RelatedCourseId = temp.RelatedCourseId;
            NotificationsEnabled = temp.NotificationsEnabled;
        }

        public async Task SubmitButtonClicked()
        {
            Assessment assessment = new()
            {
                AssessmentId = AssessmentId,
                AssessmentName = AssessmentName,
                AssessmentType = AssessmentType,
                AssessmentStartDate = AssessmentStartDate,
                AssessmentEndDate = AssessmentEndDate,
                RelatedCourseId = SelectedCourse.CourseId,
                NotificationsEnabled = NotificationsEnabled
            };
            string message = AssessmentDB.IsValidAssessment(assessment);
            string duplicateAssessmentKind = await DuplicateAssessmentKind(assessment);
            if (!string.IsNullOrEmpty(message))
            {
                ShowToast(message);
                return;
            }
            if (!string.IsNullOrEmpty(duplicateAssessmentKind))
            {
                ShowToast(duplicateAssessmentKind);
                return;
            }
            await sharedDB.SaveAssessmentAndUpdateCourse(assessment);

            bool anotherAssessmentWanted = await Application.Current.MainPage.DisplayAlert("Assessment Saved", "Would you like to add another assessment?", "Yes", "No");
            if (anotherAssessmentWanted)
            {
                await Shell.Current.GoToAsync(nameof(AssessmentPage));
            }
            else
            {
                await Shell.Current.GoToAsync("//homepage");
            }
        }

        public async void OnAppearing()
        {
            if (EditAssessmentId <= 0)
            {
                AssessmentId = await assessmentDB.GetNextId();
            }
        }

        private async Task<string> DuplicateAssessmentKind(Assessment assessment)
        {
            var message = string.Empty;
            try
            {
                var assessments = await assessmentDB.GetAssessmentsByCourseIdAsync(assessment.RelatedCourseId);
                if (assessments == null)
                {
                    return message;
                }
                foreach (var a in assessments)
                {
                    Debug.WriteLine("Assessment " + a.AssessmentName + " is " + a.AssessmentType);
                    if (a.AssessmentType == assessment.AssessmentType)
                    {
                        message = "This course already has a " + assessment.AssessmentType + " assessment.";
                    }
                }
            }
            catch (Exception ex)
            {
                message = "Issue checking for duplicate assessment type: " + ex.Message;
            }
            return message;
        }
        
        #endregion
    }
}

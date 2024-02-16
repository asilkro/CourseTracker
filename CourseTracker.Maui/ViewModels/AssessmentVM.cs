using System.Collections.ObjectModel;
using System.Diagnostics;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Services;
using System.Threading.Tasks;
using System.Windows.Input;
using CourseTracker.Maui.Supplemental;
using CourseTracker.Maui.Views;

namespace CourseTracker.Maui.ViewModels
{
    [QueryProperty(nameof(EditAssessmentId), nameof(EditAssessmentId))]
    public class AssessmentVM : ViewModelBase
    {
        public int editAssessmentId;
        public Command OnAssessmentSubmitButtonClick { get; set; }
        public Command OnAssessmentCancelButtonClick { get; set; }
        public AssessmentVM()
        {
            OnAssessmentSubmitButtonClick = new Command(async () => await submitButton_Clicked());
            OnAssessmentCancelButtonClick = new Command(async () => await cancelButton_Clicked());
        }



        private Assessment assessment;
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

        public int EditAssessmentId
        {
            get { return editAssessmentId; }
            set
            {
                if (editAssessmentId != value)
                {
                    editAssessmentId = value;
                    PerformOperation(value);
                }
            }
        }

        private async Task PerformOperation(int Id)
        {
            Debug.WriteLine("AssessmentId: " + Id);
            Assessment temp = await assessmentDB.GetAssessmentsAsync(Id);
            AssessmentId = temp.AssessmentId;
            AssessmentName = temp.AssessmentName;
            AssessmentType = temp.AssessmentType;
            AssessmentStartDate = temp.AssessmentStartDate;
            AssessmentEndDate = temp.AssessmentEndDate;
            RelatedCourseId = temp.RelatedCourseId;
            NotificationsEnabled = temp.NotificationsEnabled;
        }

        private int assessmentId;
        public int AssessmentId
        {
            get { return assessmentId; }
            set
            {
                if (assessmentId != value)
                {
                    assessmentId = value;
                    OnPropertyChanged(nameof(AssessmentId));
                }
            }
        }
        private string assessmentName;
        public string AssessmentName
        {
            get { return assessmentName; }
            set
            {
                if (assessmentName != value)
                {
                    assessmentName = value;
                    OnPropertyChanged(nameof(AssessmentName));
                }
            }
        }
        private string assessmentType;
        public string AssessmentType
        {
            get { return assessmentType; }
            set
            {
                if (assessmentType != value)
                {
                    assessmentType = value;
                    OnPropertyChanged(nameof(AssessmentType));
                }
            }
        }
        private DateTime assessmentStartDate;
        public DateTime AssessmentStartDate
        {
            get { return assessmentStartDate; }
            set
            {
                if (assessmentStartDate != value)
                {
                    assessmentStartDate = value;
                    OnPropertyChanged(nameof(AssessmentStartDate));
                }
            }
        }
        private DateTime assessmentEndDate;
        public DateTime AssessmentEndDate
        {
            get { return assessmentEndDate; }
            set
            {
                if (assessmentEndDate != value)
                {
                    assessmentEndDate = value;
                    OnPropertyChanged(nameof(AssessmentEndDate));
                }
            }
        }
        private int relatedCourseId;
        public int RelatedCourseId
        {
            get { return relatedCourseId; }
            set
            {
                if (relatedCourseId != value)
                {
                    relatedCourseId = value;
                    OnPropertyChanged(nameof(RelatedCourseId));
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



        #region Methods
        private async Task LoadAssessmentDetails()
        {
            Connection database = new();
            database.GetAsyncConnection();

            if (assessmentId <= 0)
            {
                assessment = await database.FindAsync<Assessment>(assessmentId);
            }
        }

        public async Task InitializeAsync()
        {
            await LoadAssessmentDetails();
        }

        public async void ShowActionSheet(Assessment assessment)
        {
            string action = await Application.Current.MainPage.DisplayActionSheet("Assessment Options", "Cancel", null, "Edit", "Delete");
            switch (action)
            {
                case "Edit":
                    await Shell.Current.GoToAsync($"editassessment?assessmentId={assessment.AssessmentId}");
                    break;
                case "Delete":
                    bool answer = await Application.Current.MainPage.DisplayAlert("Delete Assessment", "Are you sure you want to delete this assessment?", "Yes", "No");
                    if (answer)
                    {
                        await assessmentDB.DeleteAssessmentAsync(assessment);
                        await Shell.Current.GoToAsync("..");
                    }
                    break;
                default:
                    break;
            }
        }

        public async Task submitButton_Clicked()
        {
           
            Assessment assessment = new()
            {
                AssessmentId = AssessmentId,
                AssessmentName = AssessmentName,
                AssessmentType = AssessmentType,
                AssessmentStartDate = AssessmentStartDate,
                AssessmentEndDate = AssessmentEndDate,
                RelatedCourseId = RelatedCourseId,
                NotificationsEnabled = NotificationsEnabled
            };
            string message = assessmentDB.IsValidAssessment(assessment);
            if (!string.IsNullOrEmpty(message))
            {
                ShowToast(message);
                return;
            }
            await assessmentDB.SaveAssessmentAsync(assessment);

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

        private async Task cancelButton_Clicked()
        {
            await Shell.Current.GoToAsync("//homepage");
        }

        public async void OnAppearing()
        {
            if (EditAssessmentId <= 0)
            {
                AssessmentId = await assessmentDB.GetNextId();
            }
        }

        #endregion
    }
}

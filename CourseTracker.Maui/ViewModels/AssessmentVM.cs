using System.Collections.ObjectModel;
using System.Diagnostics;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Services;
using System.Threading.Tasks;

namespace CourseTracker.Maui.ViewModels
{
    [QueryProperty(nameof(EditAssessmentId), nameof(EditAssessmentId))]
    public class AssessmentVM : ViewModelBase
    {
        public AssessmentVM()
        {
        }
        public int editAssessmentId;

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
        #endregion
    }
}

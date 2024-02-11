using System.Collections.ObjectModel;
using System.Diagnostics;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Services;
using System.Threading.Tasks;

namespace CourseTracker.Maui.ViewModels
{
    public class AssessmentVM : ViewModelBase
    {
        #region Fields
        private Assessment assessment;

        private int assessmentId;
        private string assessmentName;
        private string assessmentType;
        private DateTime assessmentStartDate = DateTime.Now.Date;
        private DateTime assessmentEndDate = DateTime.Now.Date.AddDays(90);
        private int relatedCourseId;
        private bool notificationsEnabled;
        private Course course;
        #endregion

        #region Constructors
        public AssessmentVM(Assessment assessment) 
        {
            this.assessment = assessment;
            assessmentName = assessment.AssessmentName;
            assessmentType = assessment.AssessmentType;
            assessmentStartDate = assessment.AssessmentStartDate;
            assessmentEndDate = assessment.AssessmentEndDate;
            relatedCourseId = assessment.RelatedCourseId;
            notificationsEnabled = assessment.NotificationsEnabled;
        }

        public AssessmentVM() { }
        #endregion

        #region Properties
        public int AssessmentId
        {
            get => assessmentId;
            set => SetProperty(ref assessmentId, value, nameof(AssessmentId));
        }

        public string AssessmentName
        {
            get => assessmentName;
            set => SetProperty(ref assessmentName, value, nameof(AssessmentName));
        }

        public string AssessmentType
        {
            get => assessmentType;
            set => SetProperty(ref assessmentType, value, nameof(AssessmentType));
        }

        public DateTime AssessmentStartDate
        {
            get => assessmentStartDate;
            set => SetProperty(ref assessmentStartDate, value, nameof(AssessmentStartDate));
        }

        public DateTime AssessmentEndDate
        {
            get => assessmentEndDate;
            set => SetProperty(ref assessmentEndDate, value, nameof(AssessmentEndDate));
        }

        public int RelatedCourseId
        {
            get => relatedCourseId;
            set => SetProperty(ref relatedCourseId, value, nameof(RelatedCourseId));
        }

        public bool NotificationsEnabled
        {
            get => notificationsEnabled;
            set => SetProperty(ref notificationsEnabled, value, nameof(NotificationsEnabled));
        }

        public Course Course
        {
            get => course;
            set => SetProperty(ref course, value, nameof(Course));
        }
        #endregion

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

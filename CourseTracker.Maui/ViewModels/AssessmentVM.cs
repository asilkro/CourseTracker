using System.Collections.ObjectModel;
using System.Diagnostics;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Services;

namespace CourseTracker.Maui.ViewModels
{
    public class AssessmentVM : ViewModelBase
    {
        private Assessment assessment;
        private int assessmentId;

        public AssessmentVM()
        {
            LoadAssessmentDetails();
        }

        public AssessmentVM(Assessment assessment)
        {
            AssessmentId = assessment.AssessmentId;
            LoadAssessmentDetails();
        }

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
        private DateTime assessmentStartDate = DateTime.Now.Date;
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
        private DateTime assessmentEndDate = DateTime.Now.Date.AddDays(90);
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

        private Course course;
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

        private async Task LoadAssessmentDetails()
        {
            Connection DatabaseService = new();
            DatabaseService.GetAsyncConnection();

            if (assessmentId <= 0)
            {
                assessment = await DatabaseService.FindAsync<Assessment>(assessmentId);
            }
        }

        public async Task InitializeAsync()
        {
            await LoadAssessmentDetails();
        }
    }
}

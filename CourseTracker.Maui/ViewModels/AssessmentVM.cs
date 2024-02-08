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
                    OnPropertyChanged("AssessmentId");
                }
            }
        }
        public string AssessmentName
        {
            get { return AssessmentName; }
            set
            {
                if (AssessmentName != value)
                {
                    AssessmentName = value;
                    OnPropertyChanged("AssessmentName");
                }
            }
        }
        public string AssessmentType
        {
            get { return AssessmentType; }
            set
            {
                if (AssessmentType != value)
                {
                    AssessmentType = value;
                    OnPropertyChanged("AssessmentType");
                }
            }
        }
        public DateTime AssessmentStartDate
        {
            get { return AssessmentStartDate; }
            set
            {
                if (AssessmentStartDate != value)
                {
                    AssessmentStartDate = value;
                    OnPropertyChanged("AssessmentStartDate");
                }
            }
        }
        public DateTime AssessmentEndDate
        {
            get { return AssessmentEndDate; }
            set
            {
                if (AssessmentEndDate != value)
                {
                    AssessmentEndDate = value;
                    OnPropertyChanged("AssessmentEndDate");
                }
            }
        }
        public int RelatedCourseId
        {
            get { return RelatedCourseId; }
            set
            {
                if (RelatedCourseId != value)
                {
                    RelatedCourseId = value;
                    OnPropertyChanged("RelatedCourseId");
                }
            }
        }
        public bool NotificationsEnabled
        {
            get { return NotificationsEnabled; }
            set
            {
                if (NotificationsEnabled != value)
                {
                    NotificationsEnabled = value;
                    OnPropertyChanged("NotificationsEnabled");
                }
            }
        }

        public Course course
        {
            get { return course; }
            set
            {
                if (course != value)
                {
                    course = value;
                    OnPropertyChanged("Course");
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

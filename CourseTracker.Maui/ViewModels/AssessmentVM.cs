using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTracker.Maui.ViewModels
{
    public class AssessmentVM : ViewModelBase
    {
        private int _assessmentId = -1;
        private string _assessmentName = string.Empty;
        private string _assessmentType = "Objective";
        private DateTime _assessmentStartDate = DateTime.Today.Date;
        private DateTime _assessmentEndDate = DateTime.Today.Date.AddDays(30);
        private int _relatedCourseId = -1;
        private bool _notificationsEnabled = false;

        public int AssessmentId
        {
            get { return _assessmentId; }
            set
            {
                if (_assessmentId != value)
                {
                    _assessmentId = value;
                    OnPropertyChanged("AssessmentId");
                }
            }
        }
        public string AssessmentName
        {
            get { return _assessmentName; }
            set
            {
                if (_assessmentName != value)
                {
                    _assessmentName = value;
                    OnPropertyChanged("AssessmentName");
                }
            }
        }
        public string AssessmentType
        {
            get { return _assessmentType; }
            set
            {
                if (_assessmentType != value)
                {
                    _assessmentType = value;
                    OnPropertyChanged("AssessmentType");
                }
            }
        }
        public DateTime AssessmentStartDate
        {
            get { return _assessmentStartDate; }
            set
            {
                if (_assessmentStartDate != value)
                {
                    _assessmentStartDate = value;
                    OnPropertyChanged("AssessmentStartDate");
                }
            }
        }
        public DateTime AssessmentEndDate
        {
            get { return _assessmentEndDate; }
            set
            {
                if (_assessmentEndDate != value)
                {
                    _assessmentEndDate = value;
                    OnPropertyChanged("AssessmentEndDate");
                }
            }
        }
        public int RelatedCourseId
        {
            get { return _relatedCourseId; }
            set
            {
                if (_relatedCourseId != value)
                {
                    _relatedCourseId = value;
                    OnPropertyChanged("RelatedCourseId");
                }
            }
        }
        public bool NotificationsEnabled
        {
            get { return _notificationsEnabled; }
            set
            {
                if (_notificationsEnabled != value)
                {
                    _notificationsEnabled = value;
                    OnPropertyChanged("NotificationsEnabled");
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTracker.Maui.ViewModels
{
    public class InstructorVM : ViewModelBase
    {
        private int _instructorId = -1;
        private string _instructorName = string.Empty;
        private string _instructorEmail = string.Empty;
        private string _instructorPhone = string.Empty;

        public int InstructorId
        {
            get { return _instructorId; }
            set
            {
                if (_instructorId != value)
                {
                    _instructorId = value;
                    OnPropertyChanged("InstructorId");
                }
            }
        }

        public string InstructorName
        {
            get { return _instructorName; }
            set
            {
                if (_instructorName != value)
                {
                    _instructorName = value;
                    OnPropertyChanged("InstructorName");
                }
            }
        }

        public string InstructorEmail
        {
            get { return _instructorEmail; }
            set
            {
                if (_instructorEmail != value)
                {
                    _instructorEmail = value;
                    OnPropertyChanged("InstructorEmail");
                }
            }
        }

        public string InstructorPhone
        {
            get { return _instructorPhone; }
            set
            {
                if (_instructorPhone != value)
                {
                    _instructorPhone = value;
                    OnPropertyChanged("InstructorPhone");
                }
            }
        }
    }
}

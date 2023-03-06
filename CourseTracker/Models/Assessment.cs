using System;
using System.Collections.Generic;
using System.Text;

namespace CourseTracker.Models
{
    public class Assessment
    {
        #region Properties
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Name { get; set; }
        public string AssessmentKind { get; set; }
        public bool NotificationEnabled { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        #endregion

        #region Constructors

        public Assessment()
        {

        }

        public Assessment(int id, int courseId, string name, string assessmentKind,
            bool notificationEnabled, DateTime startDate, DateTime endDate)
        {
            Id = id;
            CourseId = courseId;
            Name = name;
            AssessmentKind = assessmentKind;
            NotificationEnabled = notificationEnabled;
            StartDate = startDate;
            EndDate = endDate;
        }

        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace CourseTracker.Models
{
    public class Assessment
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Name { get; set; }
        public string AssessmentKind { get; set; }
        public bool NotificationEnabled { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}

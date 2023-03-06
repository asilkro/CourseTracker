using System;
using System.Collections.Generic;
using System.Text;

namespace CourseTracker.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string CIName { get; set; }      //CI = Course Instructor
        public string CIPhone { get; set; }     //CI = Course Instructor
        public string CIEmail { get; set; }     //CI = Course Instructor
        public string Note { get; set; }
        public bool NotificationEnabled { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}

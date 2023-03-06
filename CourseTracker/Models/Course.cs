using System;
using System.Collections.Generic;
using System.Text;

namespace CourseTracker.Models
{
    public class Course
    {
        #region Properties
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

        #endregion

        #region Constructors

        public Course()
        {

        }

        public Course(int id, string name, string status, string ciName, string ciPhone, string ciEmail,
            string note, bool notificationEnabled, DateTime startDate, DateTime endDate)
        {
            Id = id;
            Name = name;
            Status = status;
            CIName = ciName;
            CIPhone = ciPhone;
            CIEmail = ciEmail;
            Note = note;

        }

        #endregion

    }
}

using System;

namespace CourseTracker.Models
{
    public class Term
    {
        #region Properties
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        #endregion

        #region Constructors

        public Term()
        {

        }

        public Term(int id, string name, DateTime startDate, DateTime endDate)
        {
            Id = id;
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
        }

        #endregion

    }
}
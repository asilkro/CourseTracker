using SQLite;

namespace CourseTracker.Maui.Models;

    [Table("Term")]
    public class Term
    {
    #region Properties

    [Column("TermId")] public int TermId { get; set; } = -1; // -1 means new term
    [Column("TermName")] public string TermName { get; set; } = "Term Name Placeholder";
    [Column("TermStart")] public DateTime TermStart { get; set; } = DateTime.Now.Date;
    [Column("TermEnd")] public DateTime TermEnd { get; set; } = DateTime.Now.Date.AddDays(90);
    [Column("NotificationsEnabled")] public bool NotificationsEnabled { get; set; } = false;
    [Column("CourseCount")] public int CourseCount { get; set; } = 0;

    #endregion

    #region Constructors
    
    public Term()
    {
    
    }

     #endregion
    }

using SQLite;

namespace CourseTracker.Maui.Models;

    [Table("Term")]
    public class Term
    {
    #region Properties
    [PrimaryKey]
    [AutoIncrement]
    [Column("TermId")] public int TermId { get; set; } = -1; // -1 means new term
    [Column("TermName")] public string TermName { get; set; } = "Term Name Placeholder";
    [Column("TermStart")] public DateTime TermStart { get; set; } = DateTime.Now.Date;
    [Column("TermEnd")] public DateTime TermEnd { get; set; } = DateTime.Now.Date.AddDays(90);
    [Column("CourseCount")] public int CourseCount { get; set; } = 0;

    #endregion

    #region Constructors
    
    public Term()
    {
    
    }

    public Term(int termId, string termName, DateTime termStart,
        DateTime termEnd, int courseCount)
    {
        TermId = termId;
        TermName = termName;
        TermStart = termStart;
        TermEnd = termEnd;
        CourseCount = courseCount;
    }

     #endregion
    }

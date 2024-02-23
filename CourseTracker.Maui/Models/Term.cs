using SQLite;
namespace CourseTracker.Maui.Models;

[Table("Term")]
public class Term
{
    #region Properties
    [PrimaryKey]
    [AutoIncrement]
    public int TermId { get; set; }
    public string TermName { get; set; } = "Term Name Placeholder";
    public DateTime TermStart { get; set; } = DateTime.Now.Date;
    public DateTime TermEnd { get; set; } = DateTime.Now.Date.AddDays(90);
    public int CourseCount { get; set; } = 0;

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

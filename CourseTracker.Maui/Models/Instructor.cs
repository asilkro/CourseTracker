using System.ComponentModel.DataAnnotations.Schema;

namespace CourseTracker.Maui.Models
{
    [Table("Instructor")]
    public class Instructor
    {
        #region Properties

        [Column("InstructorId")] public int InstructorId { get; set; } = -1; // -1 means new instructor
        [Column("InstructorName")] public string InstructorName { get; set; } = "Instructor Name Placeholder";
        [Column("InstructorEmail")] public string InstructorEmail { get; set; } = "placeholder@contoso.com";
        [Column("InstructorPhone")] public string InstructorPhone { get; set; } = "555-555-1234";

        #endregion

        #region Constructors
        public Instructor()
        {

        }

        #endregion
    }
}

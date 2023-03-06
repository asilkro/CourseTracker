using System;
using System.Collections.Generic;
using System.Text;

namespace CourseTracker.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string NoteContent { get; set; }
        public int CourseID { get; set; }
    }
}

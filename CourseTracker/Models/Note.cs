using System;
using System.Collections.Generic;
using System.Text;

namespace CourseTracker.Models
{
    public class Note
    {
        #region Properties
        public int Id { get; set; }
        public string NoteContent { get; set; }
        public int CourseID { get; set; }

        #endregion

        #region Constructors

        public Note()
        {

        }

        public Note(int id, string noteContent, int courseId)
        {
            Id = id;
            NoteContent = noteContent;
            CourseID = courseId;
        }

        #endregion

    }
}

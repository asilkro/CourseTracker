using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTracker.Views.Note
{
    public class NoteFlyoutMenuItem
    {
        public NoteFlyoutMenuItem()
        {
            TargetType = typeof(NoteFlyoutMenuItem);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}
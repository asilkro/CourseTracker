using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTracker.Views.Term
{
    public class TermFlyoutMenuItem
    {
        public TermFlyoutMenuItem()
        {
            TargetType = typeof(TermFlyoutMenuItem);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}
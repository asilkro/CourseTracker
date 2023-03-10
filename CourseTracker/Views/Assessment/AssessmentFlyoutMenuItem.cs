using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseTracker.Views.Assessment
{
    public class AssessmentFlyoutMenuItem
    {
        public AssessmentFlyoutMenuItem()
        {
            TargetType = typeof(AssessmentFlyoutMenuItem);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}
using CourseTracker.Maui.ViewModels;
using CourseTracker.Maui.Models;

namespace CourseTracker.Maui.Factories
{
    internal class AssessmentFactory : FactoryBase<Assessment>
    {
        protected override Assessment? CreateDefaultObject()
        {
            return new Assessment();
        }
    }
}

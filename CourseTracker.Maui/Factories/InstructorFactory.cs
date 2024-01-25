using CourseTracker.Maui.ViewModels;
using CourseTracker.Maui.Models;

namespace CourseTracker.Maui.Factories
{
    internal class InstructorFactory : FactoryBase<Instructor>
    {
        protected override Instructor? CreateDefaultObject()
        {
            return new Instructor();
        }
    }
}

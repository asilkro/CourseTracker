using CourseTracker.Maui.Views;

namespace CourseTracker.Maui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(TermPage), typeof(TermPage));
            Routing.RegisterRoute(nameof(CoursePage), typeof(CoursePage));
            Routing.RegisterRoute(nameof(AssessmentPage), typeof(AssessmentPage));
        }
    }
}

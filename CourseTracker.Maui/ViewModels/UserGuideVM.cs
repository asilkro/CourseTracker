namespace CourseTracker.Maui.ViewModels
{
    public class UserGuideVM : ViewModelBase
    {
        private string _userGuideText;
        public string UserGuideText
        {
            get => _userGuideText;
            set
            {
                if (_userGuideText != value)
                {
                    _userGuideText = value;
                    OnPropertyChanged(nameof(UserGuideText));
                }
            }
        }
        public UserGuideVM()
        {
            LoadGuideText();
        }

        private void LoadGuideText()
        {
            _userGuideText =
                "Welcome to the CourseTracker App for C971.\n\n" +
                "If you are the evaluator, please use the data setup button on the main page before beginning to guarantee the configuration.\r\n\r\n" +
                "IMPORTANT: There is a known issue with Visual Studio 2022 that may cause ListViews to not populate properly without changing the XAML file containing that ListView.\r\n + " +
                "If you encounter this issue, please open the XAML file (ie ListTerms.xaml) and make a small change (changing CachingStrategy=\"RecycleElement\" to \"RetainElement\" for example) and then save the file. It should display correctly after.\r\n\r\n" +

                "Overview:\r\n" + "A term may have up to six courses and each course may have up to two assessments. \r\n\r\n" +
                "Removal of a term will also remove all courses and assessments associated with it. \r\n\r\n" +
                "Removal of a course will also remove all assessments associated with it. \r\n\r\n" +

                "Create a Term:\r\n" +
                "Start by defining a Term; it's the foundation of your planning. Navigate to the Term creation section and fill in the necessary details.\r\n\r\n" +
                "Remember, every course and assessment you plan will need to be linked to a Term. \r\n\r\n" +
                "Add a Course:\r\n" +
                "Once you've established your Term, it's time to add a \"Course\". Go to the Course section, select the appropriate Term from the dropdown, and input your course details. \r\n\r\n" +
                "Link an Assessment:\r\n" +
                "An Assessment can technically be created anytime but should ideally follow Terms and Courses for a streamlined experience. \r\n\r\n" +
                "In the Assessment section, use the \"Related Course\" field to connect each assessment to its respective course. You can enter the CourseId manually from the course above or select the course from a dropdown, if available.\r\n\r\n";
        }
    }
}

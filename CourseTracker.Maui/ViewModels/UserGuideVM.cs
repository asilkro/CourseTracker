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
                "IMPORTANT: There is a known issue with Visual Studio 2022 that may cause ListViews to not populate properly without changing the XAML file containing that ListView.\r\n\r\n" +
                "If you encounter this issue, please open the XAML file (ie ListTerms.xaml) and make a small change (changing CachingStrategy=\"RecycleElement\" to \"RetainElement\" for example) and then save the file. The ListView should display correctly after.\r\n\r\n" +

                "Overview:\r\n" + "A term may have up to six courses and each course may have up to two assessments. \r\n\r\n" +
                "Removal of a term will also remove all courses and assessments associated with it. \r\n\r\n" +
                "Removal of a course will also remove all assessments associated with it. \r\n\r\n" +

                "Add a Term:\r\n" +
                "Start by defining a Term; it's the foundation of your planning. Navigate to the Term creation section and fill in the necessary details.\r\n\r\n" +
                "Remember, every course and assessment you plan will need to be linked to a Term. \r\n\r\n" +

                "Add a Course:\r\n" +
                "Once you've established your Term, it's time to add a Course.\r\n" +
                "Go to the Course section and click new. Enter all the information and select the appropriate Term from the dropdown to associate it with the course. \r\n\r\n" +

                "Add an Assessment:\r\n" +
                "Once you have a course, you can make an Assessment.\r\n\r\n" +
                "Go to the Assessment section and click new. Enter all the information and select the appropriate Course from the dropdown to associate it with the assessment.\r\n\r\n" +

                "Editing & Removing Items:\r\n" +
                "Select the item you wish to modify or remove from the respective list (Term/Course/Assessment) and tap edit or delete.\r\n\r\n" +
                "Edit will open the editing view for the item you tapped, and when you are done, click SUBMIT to save.\r\n\r\n" +
                "Delete will prompt you to confirm and then begin removing items in a cascade delete of related items.";

        }
    }
}

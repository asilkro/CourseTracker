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
                "**NOTE**:\r\nThe TermList view seems to impacted by the MAUI bug documented in the README, but works properly if you click New Term then back to List.\r\n\r\n" +

                "Overview:\r\n" + "A term may have up to six courses and each course may have up to two assessments. \r\n\r\n" +
                "Removal of a term will also remove all associated courses and assessments. \r\n\r\n" +
                "Removal of a course will also remove all assessments associated with it. \r\n\r\n" +

                                "Notifications:\r\n" +
                "Notification dates are 1 day before those in the pickers (Start / End).\r\nIf this notification date is the same as the current date, no notification will be generated - this is a limitation of the plugin used for notifications.\r\n\r\n" +
                "Notifications are generated when the app's main screen launches and matches the notification date against the system date.\r\n\r\n" +

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

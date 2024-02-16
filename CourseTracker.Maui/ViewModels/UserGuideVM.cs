﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

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
            UserGuideText = "CourseTracker User Guide\n\n" +
                "Welcome to the CourseTracker. Follow these simple steps to set up your educational tracking efficiently:\n\n"
                + "Create a Term:\r\n" +
                "Start by defining a \"Term\"; it's the foundation of your planning. Navigate to the Term creation section and fill in the necessary details.\r\n" +
                " Remember, every course and assessment you plan will need to be linked to a Term. \r\n\r\n" +
                "Add a Course:\r\n" +
                "Once you've established your Term, it's time to add a \"Course\". Go to the Course section, select the appropriate Term from the dropdown, and input your course details. \r\nMake a note of the CourseId - you'll need it soon!\r\n\r\n" +
                "Link an Assessment:\r\n" +
                "An \"Assessment\" can technically be created anytime but should ideally follow Terms and Courses for a streamlined experience. \r\n" +
                "In the Assessment section, use the \"Related Course\" field to connect each assessment to its respective course. You can enter the CourseId manually from the course above or select the course from a dropdown, if available.\r\n\r\n" +
                "\r\n" + "Important:\r\nUse the buttons on the main page with care as they are very powerful. Use these buttons to reset the application database(requires closing and relaunching) or \r\n loading the required evaluation data.";
        }
    }
}

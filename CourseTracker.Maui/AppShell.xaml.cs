﻿namespace CourseTracker.Maui
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Shell.Current.GoToAsync("//homepage");
        }
    }
}

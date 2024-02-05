using Microsoft.Extensions.DependencyInjection;
using CourseTracker.Maui.Services;
using CourseTracker.Maui.Factories;
using CourseTracker.Maui.Views;
using Microsoft.Maui.Platform;
using System;
using Plugin.LocalNotification;
using Plugin.LocalNotification.EventArgs;

namespace CourseTracker.Maui
{
    public partial class App : Application
    {        
        public App()
        {
            InitializeComponent();

            LocalNotificationCenter.Current.NotificationActionTapped += OnNotificationActionTapped;
            MainPage = new AppShell();
        }

        private void OnNotificationActionTapped(NotificationActionEventArgs e)
        {
            if (e.IsDismissed)
            {
                // your code goes here
                return;
            }
            if (e.IsTapped)
            {
                // your code goes here
                return;
            }
            // if Notification Action are setup
            switch (e.ActionId)
            {
                // your code goes here
            }
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using CourseTracker.Maui.Services;
using CourseTracker.Maui.Factories;
using CourseTracker.Maui.Views;
using CourseTracker.Maui.Supplemental;
using Microsoft.Maui.Platform;
using System;
using Plugin.LocalNotification;
using Plugin.LocalNotification.EventArgs;
using System.Diagnostics;

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
                e.Request.Cancel();
                Debug.WriteLine(e.ActionId);
                return;
            }
            if (e.IsTapped)
            {
                e.Request.Show();
                Debug.WriteLine(e.ActionId);
                return;
            }
        }
    }
}

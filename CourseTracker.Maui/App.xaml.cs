using System.Diagnostics;
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

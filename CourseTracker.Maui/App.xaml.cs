using CommunityToolkit.Maui.Alerts;
using Plugin.LocalNotification;
using Plugin.LocalNotification.EventArgs;

namespace CourseTracker.Maui
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Current.UserAppTheme = AppTheme.Light;
            this.RequestedThemeChanged += (s, e) => { Application.Current.UserAppTheme = AppTheme.Light; };
            LocalNotificationCenter.Current.NotificationActionTapped += OnNotificationActionTapped;
            Task.Run(RequestNotificationPermissions);
            MainPage = new AppShell();
        }

        private static async Task RequestNotificationPermissions()
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                if (await LocalNotificationCenter.Current.AreNotificationsEnabled() == false)
                {
                    var result = await LocalNotificationCenter.Current.RequestNotificationPermission();
                    if (!result)
                    {
                        await Toast.Make("Notifications require approval.").Show();
                        return;
                    }
                    var notification = new NotificationRequest
                    {
                        NotificationId = 10109,
                        Title = "Notifications Working",
                        Description = "This notification verifies that you have the required permissions.",
                        ReturningData = null,
                        Schedule =
                        {
                            NotifyTime = DateTime.Now.AddSeconds(5) // Triggers Notification in 5 seconds
                        }
                    };

                    await LocalNotificationCenter.Current.Show(notification);
                }
            });
        }

        private void OnNotificationActionTapped(NotificationActionEventArgs e)
        {
            if (e.IsDismissed)
            {
                e.Request.Cancel();
                Toast.Make("Notification: " + e.ToString() + " dismissed.").Show();
                return;
            }
            if (e.IsTapped)
            {
                e.Request.Show();
                Toast.Make("Notification: " + e.ToString() + " tapped.").Show();
                return;
            }
        }
    }
}

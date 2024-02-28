using CommunityToolkit.Maui.Alerts;
using CourseTracker.Maui.Data;
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
            NotificationRequest notification = new()
            {
                Android = 
                {
                    AutoCancel = true,
                    ChannelId = "CourseTracker"
                },
                Title = "Notifications Working",
                Description = "This notification verifies that you have the required permissions.",
                Schedule =
                        {
                            NotifyTime = DateTime.Now // Triggers Notification in 5 seconds
                        }
                
            };

            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                if (!await LocalNotificationCenter.Current.AreNotificationsEnabled())
                {
                    var result = await LocalNotificationCenter.Current.RequestNotificationPermission();
                    if (!result)
                    {
                        await Toast.Make("Notifications require permission approval.").Show();
                        return;
                    }
                }
                    
            });
            await LocalNotificationCenter.Current.Show(notification);
        }

        private void OnNotificationActionTapped(NotificationActionEventArgs e)
        {

            if (e.IsDismissed)
            {
                e.Request.Cancel();
                Snackbar.Make("Notification " + e.Request.Title + " dismissed.").Show();
                return;
            }
            if (e.IsTapped)
            {
                e.Request.Show();
                Snackbar.Make("Notification " + e.Request.Title + " tapped.").Show();
                return;
            }
        }
    }
}

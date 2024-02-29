using System.Diagnostics;
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
                    
                },
                Title = "Notifications Working",
                Description = "Your permissions are set correctly.",
                Schedule =
                        {
                            NotifyTime = DateTime.Now, // Triggers Notification
                            NotifyAutoCancelTime = DateTime.Now.AddSeconds(30) // Removes Notification
                        }
            };
#if DEBUG
            Debug.WriteLine("NotifyAutoCancelTime is: " + notification.Schedule.NotifyAutoCancelTime.ToString());
                #endif

            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                if (!await LocalNotificationCenter.Current.AreNotificationsEnabled())
                {
                    var result = await LocalNotificationCenter.Current.RequestNotificationPermission();
                    if (!result)
                    {
                        await Application.Current.MainPage.DisplayAlert("Notifications Are Not Enabled","You must accept the request to enable permissions for notifications to function.","Acknowledge");
                        return;
                    }
                }
                await LocalNotificationCenter.Current.Show(notification);
            });
            
        }

        private void OnNotificationActionTapped(NotificationActionEventArgs e)
        {
            if (e.IsDismissed)
            {
                e.Request.Cancel();
                return;
            }
            if (e.IsTapped)
            {
                e.Request.Show();
                return;
            }
        }
    }
}

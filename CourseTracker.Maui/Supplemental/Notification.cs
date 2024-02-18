using Plugin.LocalNotification;

namespace CourseTracker.Maui.Supplemental
{

    public class Notification
    {
        // Used to track the notification ID, would be a nice refactor to use a generator.
        public static async Task ScheduleNotificationAsync(int notificationId, string title, string subtitle,
            DateTime notifyTime, NotificationCategoryType type)
        {
            var notification = new NotificationRequest
            {
                NotificationId = notificationId,
                Title = title,
                Subtitle = subtitle,
                CategoryType = type,
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = notifyTime.Date,
                    NotifyAutoCancelTime = notifyTime.Date.AddDays(1)
                }
            };

            if (await Validation.IsValidNotification(notification))
            {
                await LocalNotificationCenter.Current.Show(notification);
            }
            else return;
        }

        public static async Task<bool> IsValidNotification(NotificationRequest notification)
        {
            return notification.Schedule.NotifyTime > DateTime.Now;
        }
    }
}

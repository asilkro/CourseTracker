using Plugin.LocalNotification;

namespace CourseTracker.Maui.Supplemental
{
    
    public class Notification
    {
        int notificationId = 0; // Used to track the notification ID, would be a nice refactor to use a generator.
        public async Task ScheduleNotificationAsync(string title, string subtitle, 
            DateTime notifyTime, NotificationCategoryType type)
        {
            var notification = new NotificationRequest
            {
                NotificationId = notificationId,
                Title = title,
                Subtitle = subtitle,
                CategoryType = type,
                CategoryType = NotificationCategoryType
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = notifyTime.Date,
                    NotifyAutoCancelTime = notifyTime.Date.AddDays(1)
                }
            };

            if (await Validation.IsValidNotification(notification))
            {
                await LocalNotificationCenter.Current.Show(notification);
                notificationId++;
            }
            else return;
        }
    }
}

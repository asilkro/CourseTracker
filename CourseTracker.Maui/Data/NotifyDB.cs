using System.Diagnostics;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Services;
using Plugin.LocalNotification;
using SQLite;

namespace CourseTracker.Maui.Data
{
    public class NotifyDB
    {
        SQLiteAsyncConnection _database;

        public NotifyDB()
        {
        }

        public async Task Init()
        {
            if (_database != null)
                return;
            _database = new SQLiteAsyncConnection(TrackerDb.DatabasePath, TrackerDb.Flags);
            await _database.CreateTableAsync<Notification>();
        }

        public async Task<List<Notification>> GetNotificationsAsync()
        {
            await Init();
            return await _database.Table<Notification>().ToListAsync();
        }

        public async Task<Notification> GetNotificationByIdAsync(int id)
        {
            await Init();
            return await _database.Table<Notification>()
                .Where(i => i.NotificationId == id)
                .FirstOrDefaultAsync();
        }

        public async void NotificationRunner()
        {
            var matchingNotifications = await GetNotificationByDate(DateTime.Now.Date);
#if DEBUG
            Debug.WriteLine("There are: " + matchingNotifications.Count + " notifications for today.");
#endif
            if (matchingNotifications.Count > 0)
            {
#if DEBUG
                Debug.WriteLine("There are: " + matchingNotifications.Count + " notifications for today.");
#endif
                foreach (var notification in matchingNotifications)
                {
                    await MakeAndroidNotification(notification);

#if DEBUG
                    Debug.WriteLine("Notification");
                    PrintOutPendingRequests();
#endif
                }
            }
        }

        public async Task<List<Notification>> GetNotificationByDate(DateTime date)
        {
            await Init();
            return await _database.Table<Notification>()
                .Where(i => i.NotificationDate == date && i.NotificationTriggered != 1)
                .ToListAsync();
        }

        public static async void PrintOutPendingRequests()
        {
            IList<NotificationRequest> requests = await LocalNotificationCenter.Current.GetPendingNotificationList();
            foreach (var request in requests)
            {
                Debug.WriteLine("--");
                Debug.WriteLine("NotificationId: " + request.NotificationId);
                Debug.WriteLine("Title: " + request.Title);
                Debug.WriteLine("Subtitle: " + request.Subtitle);
                Debug.WriteLine("Description: " + request.Description);
                Debug.WriteLine("NotifyTime: " + request.Schedule.NotifyTime);
                Debug.WriteLine("NotifyAutoCancelTime: " + request.Schedule.NotifyAutoCancelTime);
                Debug.WriteLine("Repeat Interval: " + request.Schedule.NotifyRepeatInterval);
                Debug.WriteLine("--");
            }
        }

        public async Task MakeAndroidNotification(Notification notification)
        {
            try
            {
                NotificationRequest request = new()
                {
                    Android =
                            {
                                ChannelId = "CourseTracker",
                                When = notification.NotificationDate,
                                TimeoutAfter = notification.NotificationDate.AddDays(3).TimeOfDay,
                                VisibilityType = Plugin.LocalNotification.AndroidOption.AndroidVisibilityType.Public
                            },

                    NotificationId = notification.NotificationId,
                    Title = notification.NotificationTitle,
                    Subtitle = notification.NotificationMessage,
                    CategoryType = NotificationCategoryType.Reminder,
                    Schedule = schedule,
                };
                await LocalNotificationCenter.Current.Show(request);
#if DEBUG
                PrintOutPendingRequests();
#endif
                notification.NotificationTriggered = 1;
                await SaveNotificationAsync(notification);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("There was an issue scheduling: " + notification.NotificationTitle + " : " + ex.Message);
                return;
            }
        }

        public async Task<int> DeleteNotificationAsync(Notification notification)
        {
            await Init();
            return await _database.DeleteAsync(notification);
        }

        public async Task SaveNotificationAsync(Notification notification)
        {
            await Init();
            var result = await _database.FindAsync<Notification>(notification.NotificationId);

            if (result == null)
            {
                await _database.InsertAsync(notification);
            }
            else
            {
                await _database.UpdateAsync(notification);
                // Notifications are not editable currently
                // but this will allow future expansions to the app
            }
            Debug.WriteLine(result);
        }

        public async Task<int> GetNextId()
        {
            await Init();
            List<Notification> notifications = await GetNotificationsAsync();
            if (notifications.Count == 0)
            {
                return 1;
            }
            else
            {
                int maxId = notifications.Max(t => t.NotificationId);
                maxId++;
                return maxId;
            }
        }

        public NotificationRequestSchedule schedule = new()
        {
            NotifyTime = DateTime.Now,
            NotifyAutoCancelTime = DateTime.Now.AddDays(3),
            RepeatType = NotificationRepeat.TimeInterval,
            NotifyRepeatInterval = TimeSpan.FromHours(4)
        };
    }
}

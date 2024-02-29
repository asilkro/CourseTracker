﻿using System.Diagnostics;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Services;
using CourseTracker.Maui.Supplemental;
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

        public async Task ScheduleNotificationAsync(Notification notification)
        {
            NotificationRequest notificationRequest = new()
            {
                Android =
                    {
                    AutoCancel = true,
                    ChannelId = "CourseTracker",
                    LaunchAppWhenTapped = true,
                    When = notification.NotificationDate,
                    Priority = Plugin.LocalNotification.AndroidOption.AndroidPriority.Default,
                    TimeoutAfter = notification.NotificationDate.AddDays(7).TimeOfDay,
                    VisibilityType = Plugin.LocalNotification.AndroidOption.AndroidVisibilityType.Public
                    },

                NotificationId = notification.NotificationId,
                Title = notification.NotificationTitle,
                Subtitle = notification.NotificationMessage,
                CategoryType = NotificationCategoryType.Reminder,
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = notification.NotificationDate,
                    NotifyAutoCancelTime = notification.NotificationDate.AddDays(7),
                    RepeatType = NotificationRepeat.TimeInterval,
                    NotifyRepeatInterval = TimeSpan.FromDays(1)
                }
            };

            var result = Validation.IsValidNotification(notificationRequest);
#if DEBUG
            Debug.WriteLine(notificationRequest.Title + " returns " + result);
#endif
            if (result == string.Empty)
            {
                await SaveNotificationAsync(notification);
                await LocalNotificationCenter.Current.Show(notificationRequest);
                List<Notification> notificationCount = await GetNotificationsAsync();
#if DEBUG
                Debug.WriteLine(notificationCount.Count + " was the number of notifications");
#endif

                return;
            }
        }

    }
}

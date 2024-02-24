﻿using System.Diagnostics;
using System.Security.Cryptography;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Services;
using CourseTracker.Maui.Supplemental;
using Plugin.LocalNotification;
using SQLite;

namespace CourseTracker.Maui.Data
{
    public class CourseDB
    {
        SQLiteAsyncConnection _database;

        public CourseDB()
        {
        }

        public async Task Init()
        {
            if (_database != null)
                return;
            _database = new SQLiteAsyncConnection(TrackerDb.DatabasePath, TrackerDb.Flags);
            await _database.CreateTableAsync<Course>();
        }

        public async Task<List<Course>> GetCoursesAsync()
        {
            await Init();
            return await _database.Table<Course>().ToListAsync();
        }
        public async Task<Course> GetCourseByIdAsync(int id)
        {
            await Init();
            return await _database.Table<Course>()
                .Where(i => i.CourseId == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Course>> GetCoursesByTermIdAsync(int TermId)
        {
            await Init();
            return await _database.Table<Course>()
                .Where(i => i.TermId == TermId)
                .ToListAsync();
        }

        public async Task<int> DeleteCourseAsync(Course course)
        {
            await Init();
            return await _database.DeleteAsync(course);
        }

        public async Task RemoveCourseAsync(Course course)
        {
            if (course == null)
                return;
            var result = await App.Current.MainPage.DisplayAlert("Delete Course", $"Are you sure you want to delete {course.CourseName}?", "Yes", "No");
            if (result)
            {
                int confirm = await DeleteCourseAsync(course);
                if (confirm == 1)
                { await App.Current.MainPage.DisplayAlert("Course Deleted", $"{course.CourseName} has been deleted.", "OK"); }
                else
                { await App.Current.MainPage.DisplayAlert("Error", "Course was not deleted.", "OK"); }
            }
        }


        public static async Task ScheduleCourseNotifications(Course course)
        {
            if (course.NotificationsEnabled)
            {
                var notificationId = course.CourseId + RandomNumberGenerator.GetInt32(999); // Ideally, generate a unique ID for each notification.
                var title = $"Reminder for {course.CourseName}";

                // Schedule notifications for start date reminders
                var startReminders = new[] { 3, 1 };
                foreach (var daysBefore in startReminders)
                {
                    var notifyTime = course.CourseStart.AddDays(-daysBefore);
                    var subtitle = $"Starts in {daysBefore} days";
                    await Notification.ScheduleNotificationAsync(notificationId++, title, subtitle, notifyTime, NotificationCategoryType.Reminder);
                }

                // Schedule notifications for end date reminders
                var endReminders = new[] { 3, 1 };
                foreach (var daysBefore in endReminders)
                {
                    var notifyTime = course.CourseEnd.AddDays(-daysBefore);
                    var subtitle = $"Ends in {daysBefore} days";
                    await Notification.ScheduleNotificationAsync(notificationId++, title, subtitle, notifyTime, NotificationCategoryType.Reminder);
                }
            }
        }

        public class CourseOperationOut
        {
            public Course? Course { get; set; }
            public string ErrorMessage { get; set; } = string.Empty;
        }

        public async Task SaveCourseAsync(Course course)
        {
            await Init();
            var result = await _database.FindAsync<Course>(course.CourseId);

            if (result == null)
            {
                await _database.InsertAsync(course);
                if (course.NotificationsEnabled)
                {
                    await ScheduleCourseNotifications(course);
                }
            }
            else
            {
                await _database.UpdateAsync(course);
                if (course.NotificationsEnabled)
                {
                    await ScheduleCourseNotifications(course);
                }
            }
            Debug.WriteLine(result);
        }

        public async Task<int> GetNextId()
        {
            await Init();
            List<Course> courses = await GetCoursesAsync();
            if (courses.Count == 0)
            {
                return 1;
            }
            else
            {
                int maxId = courses.Max(t => t.CourseId);
                maxId++;
                return maxId;
            }
        }
    }
}

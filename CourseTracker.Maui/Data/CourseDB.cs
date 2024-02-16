﻿using System.Diagnostics;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.ViewModels;
using CourseTracker.Maui.Services;
using CourseTracker.Maui.Supplemental;
using Plugin.LocalNotification;

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

        public async Task<int> DeleteCourseAsync(Course course)
        {
            await Init();
            return await _database.DeleteAsync(course);
        }


        public async Task<string> UpdateCourseAndUpdateTermCount(Course course)
        {
            var connection = new Connection();
            var term = await connection.FindAsync<Term>(course.TermId);
            if (term == null)
            {
                return "Associated term not found.";
            }

            if (term.CourseCount >= 6)
            {
                return "Terms may NOT consist of more than six courses.";
            }

            term.CourseCount += 1;
            await connection.UpdateAsync(term);
            await connection.UpdateAsync(course);

            return "Course Updated successfully.";
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
                { await App.Current.MainPage.DisplayAlert("Error", "Course was not deleted.", "OK");}
            }
        }

        public async Task ScheduleCourseNotifications(Course course)
        {
            if (course.NotificationsEnabled)
            {
                var notificationId = course.CourseId; // Ideally, generate a unique ID for each notification.
                var title = $"Reminder for {course.CourseName}";

                // Schedule notifications for start date reminders
                var startReminders = new[] { 7, 3, 1 };
                foreach (var daysBefore in startReminders)
                {
                    var notifyTime = course.CourseStart.AddDays(-daysBefore);
                    var subtitle = $"Starts in {daysBefore} days";
                    await Notification.ScheduleNotificationAsync(notificationId++, title, subtitle, notifyTime, NotificationCategoryType.Reminder);
                }

                // Schedule notifications for end date reminders
                var endReminders = new[] { 7, 3, 1 };
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
            if (course.CourseId != 0)
            {
                await _database.UpdateAsync(course);
            }
            else
            {
                await _database.InsertAsync(course);
            }
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
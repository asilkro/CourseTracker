using System.Diagnostics;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Services;
using SQLite;

namespace CourseTracker.Maui.Data;

public class CourseDB
{
    SQLiteAsyncConnection _database;
    readonly NotifyDB notifyDB;

    public CourseDB()
    {
        notifyDB = new NotifyDB();
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


    public async Task ScheduleCourseNotifications(Course course)
    {
        var title = $"Course Reminder: {course.CourseName}";

        // Schedule notifications for start date reminders
        var startReminders = new[] { 1 };
        foreach (var daysBefore in startReminders)
        {
            try
            {
                Notification notification = new()
                {
                    NotificationTitle = title,
                    NotificationDate = course.CourseStart.AddDays(-daysBefore),
                    RelatedItemType = "Course",
                    NotificationMessage = $"{course.CourseName} begins in {daysBefore} day(s)",
                    NotificationTriggered = 0
                };
                await notifyDB.SaveNotificationAsync(notification);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        // Schedule notifications for end date reminders
        var endReminders = new[] { 1 };
        foreach (var daysBefore in endReminders)
        {
            try
            {
                Notification notification = new()
                {
                    NotificationTitle = title,
                    NotificationDate = course.CourseEnd.AddDays(-daysBefore),
                    RelatedItemType = "Course",
                    NotificationMessage = $"{course.CourseName} ends in {daysBefore} day(s)",
                    NotificationTriggered = 0
                };

                await notifyDB.SaveNotificationAsync(notification);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}

public class CourseOperationOut
{
    public Course? Course { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
}
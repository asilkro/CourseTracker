using System.Collections.ObjectModel;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Services;

namespace CourseTracker.Maui.Data
{
    public class CourseDB
    {
        SQLiteAsyncConnection _database;
        public CourseDB()
        {
            async Task Init()
            {
                if (_database != null)
                    return;
                _database = new SQLiteAsyncConnection(TrackerDb.DatabasePath, TrackerDb.Flags);
                await _database.CreateTableAsync<Course>();
            }

            async Task<List<Course>> GetCoursesAsync()
            {
                await Init();
                return await _database.Table<Course>().ToListAsync();
            }
        }
    }
}

using CourseTracker.Maui.Models;
using CourseTracker.Maui.Services;

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

        public async Task<int> DeleteCourseAsync(Course course)
        {
            await Init();
            return await _database.DeleteAsync(course);
        }

        public async Task<Course> GetCourseByIdAsync(int id)
        {
            await Init();
            return await _database.Table<Course>()
                .Where(i => i.CourseId == id)
                .FirstOrDefaultAsync();
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
        }

}

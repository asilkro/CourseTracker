using System.Collections.ObjectModel;
using CourseTracker.Maui.Factories;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Services;

namespace CourseTracker.Maui.Data
{
    public class AssessmentDB
    {
        SQLiteAsyncConnection _database;
        public AssessmentDB()
        {
        }
        async Task Init()
        {
            if (_database != null)
                return;
            _database = new SQLiteAsyncConnection(TrackerDb.DatabasePath, TrackerDb.Flags);
            await _database.CreateTableAsync<Assessment>();
        }
        public async Task<List<Assessment>> GetAssessmentsAsync()
        {
            await Init();
            return await _database.Table<Assessment>().ToListAsync();
        }

        public async Task<Assessment> GetAssessmentsAsync(int id)
        {
            await Init();
            return await _database.Table<Assessment>()
                .Where(i => i.AssessmentId == id)
                .FirstOrDefaultAsync();
        }

        public async Task SaveAssessmentAsync(Assessment assessment)
        {
            await Init();
            if (assessment.AssessmentId != 0)
            {
                await _database.UpdateAsync(assessment);
                //TODO: Update assessment count on courses, return error if course not found or over limit of 2

            }
            else
            {
                await _database.InsertAsync(assessment);
                //TODO: Update assessment count on courses, return error if course not found or over limit of 2

            }
        }
        public async Task<int> DeleteAssessmentAsync(Assessment assessment)
        {
            await Init();
            return await _database.DeleteAsync(assessment);
        }

    }
}

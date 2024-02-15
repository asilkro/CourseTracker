using System.Collections.ObjectModel;
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
        public async Task SaveAssessmentAsync(Assessment assessment)
        {
            await Init();
            if (assessment.AssessmentId != 0)
            {
                await _database.UpdateAsync(assessment);
            }
            else
            {
                await _database.InsertAsync(assessment);
            }
        }
        public async Task<int> DeleteAssessmentAsync(Assessment assessment)
        {
            await Init();
            return await _database.DeleteAsync(assessment);
        }
    }
}

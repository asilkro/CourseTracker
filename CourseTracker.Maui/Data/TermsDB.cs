using CourseTracker.Maui.Models;
using CourseTracker.Maui.Services;

namespace CourseTracker.Maui.Data
{
    public class TermsDB
    {
        SQLiteAsyncConnection _database;
        CourseDB courseDB;
        public TermsDB()
        {
        }
        public async Task Init()
        {
            if (_database != null)
                return;
            _database = new SQLiteAsyncConnection(TrackerDb.DatabasePath, TrackerDb.Flags);
            await _database.CreateTableAsync<Term>();
        }
        public async Task<List<Term>> GetTermsAsync()
        {
            await Init();
            return await _database.Table<Term>().ToListAsync();
        }
        public async Task<Term> GetTermByIdAsync(int id)
        {
            await Init();
            return await _database.Table<Term>()
                .Where(i => i.TermId == id)
                .FirstOrDefaultAsync();
        }
        public async Task<int> GetNextId()
        {
            await Init();
            List<Term> terms = await GetTermsAsync();
            if (terms.Count == 0)
            {
                return 1;
            }
            else
            {
                int maxId = terms.Max(t => t.TermId);
                maxId++;
                return maxId;
            }
        }
        public async Task SaveTermAsync(Term term)
        {
            await Init();
            var result = await _database.FindAsync<Term>(term.TermId);
            if (result == null)
            {
                await _database.InsertAsync(term);
            }
            else
            {
                await _database.UpdateAsync(term);
            }
        }
        public async Task<int> DeleteTermAsync(Term term)
        {
            await Init();
            
            int count = term.CourseCount;
            var result = await Application.Current.MainPage.DisplayAlert(
                "WARNING: DELETING TERM", "Are you SURE you want to delete this term?" +
                " It will also remove " + count + " courses." +
                "If you wish to keep this data, please move the existing courses to another term first.", "Yes", "No");
            if (result)
            {
                List<Course> allCourses = await courseDB.GetCoursesAsync();
                foreach (Course course in allCourses)
                {
                    if (course.TermId == term.TermId)
                    {
                        await courseDB.DeleteCourseAsync(course);                 
                    }
                }

              return await _database.DeleteAsync(term);
            }
            return 0;
        }



    }
}

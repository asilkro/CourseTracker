using SQLite;
using CourseTracker.Maui.Models;
using static Microsoft.Maui.Storage.FileSystem;

namespace CourseTracker.Maui.Services
{
    public class TrackerDb
    {
        private static Connection _connection = new();
        private static SQLiteConnection _dbConnection;
        private static SQLiteAsyncConnection _db;

        #region SQLite setup

        public const string DatabaseFilename = "TrackerDB.db3";

        public const SQLiteOpenFlags Flags =
            // Create our SQLite DB if it doesn't exist
            SQLiteOpenFlags.Create |
            // Set multi-thread DB access for performance
            SQLiteOpenFlags.SharedCache |
            // We need to be able to read from and write to DB
            SQLiteOpenFlags.ReadWrite |
            // Configure data protection mode on the DB
            // Device must be unlocked for access
            SQLiteOpenFlags.ProtectionComplete;

        public static string DatabasePath =>
            Path.Combine(AppDataDirectory, DatabaseFilename);
        // This is the path to the database on the device and is used by the SQLiteConnection object
        // to connect to the database.

        public static string DatabasePathPlatform =>
            Path.Combine(CacheDirectory, DatabaseFilename);
        // This is the platform-specific path(?) to the database on the device and is used by the SQLiteConnection object
        // to connect to the database. It may also be just a cache directory, but I'm not sure.

        public static async Task Initialize()
        {
            if (_db != null) return; // If the database is already initialized, don't do it again.

            _db = new SQLiteAsyncConnection(DatabasePath);
            _dbConnection = new SQLiteConnection(DatabasePath);

            if (_dbConnection.Table<Term>().Count() != 0) return; // If the database already has data, don't add more.

            await _db.EnableLoadExtensionAsync(true);
            _dbConnection.EnableLoadExtension(true); // cannot async a void method
            await SetupTables(_db); // Will create tables if they don't exist
        }

        public static async Task SetupTables(SQLiteAsyncConnection db)
        {
            if (_db == null)
            {
                return;
            }
            else
            {
                if (await db.GetTableInfoAsync("Term") == null)
                {
                    await db.CreateTableAsync<Term>();
                }
                if (await db.GetTableInfoAsync("Course") == null)
                {
                    await db.CreateTableAsync<Course>();
                }
                if (await db.GetTableInfoAsync("Assessment") == null)
                {
                    await db.CreateTableAsync<Assessment>();
                }
                if (await db.GetTableInfoAsync("Instructor") == null)
                {
                    await db.CreateTableAsync<Instructor>();
                }
            }
            
        }
        
        #endregion

        #region Utility

        public static int GetNextAutoIncrementID(string tableName)
        {
            var query = $"SELECT seq FROM sqlite_sequence WHERE name = '{tableName}'";
            var result = _dbConnection.ExecuteScalar<int>(query);
            return result + 1;
        }

        public static void ResetDatabase()
        {
            _dbConnection.DropTable<Term>();
            _dbConnection.DropTable<Course>();
            _dbConnection.DropTable<Assessment>();
            _dbConnection.DropTable<Instructor>();
            _dbConnection.CreateTable<Term>();
            _dbConnection.CreateTable<Course>();
            _dbConnection.CreateTable<Assessment>();
            _dbConnection.CreateTable<Instructor>();
        }

        #endregion

        #region Term CRUD

        public static async Task<List<Term>> GetTermsAsync()
        {
            return await _db.Table<Term>().ToListAsync();
        }

        public static async Task<Term> GetTermAsync(int id) // ID = PK
        {
            return await _db.FindAsync<Term>(id);
        }

        public static async Task<int> SaveTermAsync(Term term)
        {
            if (term.TermId == -1 || term == null)
            {
                return await _db.InsertAsync(term);
            }
            else
            {
                return await _db.UpdateAsync(term);
                
            }
        }

        public static async Task<int> DeleteTermAsync(int id)
        {
            return await _db.DeleteAsync<Term>(id);
        }

        #endregion

        #region Course CRUD

        public static async Task<List<Course>> GetCoursesAsync()
        {
            return await _db.Table<Course>().ToListAsync();
        }

        public static async Task<Course> GetCourseAsync(int id) // ID = PK
        {
            return await _db.FindAsync<Course>(id);
        }

        public static async Task<int> SaveCourseAsync(Course course)
        {
            if (course.CourseId == -1 || course == null)
            {
                return await _db.InsertAsync(course);
            }
            else
            {
                return await _db.UpdateAsync(course);
            }
        }

        public static async Task<int> DeleteCourseAsync(int id)
        {
            return await _db.DeleteAsync<Course>(id);
        }

        #endregion

        #region Assessment CRUD

        public static async Task<List<Assessment>> GetAssessmentsAsync()
        {
            return await _db.Table<Assessment>().ToListAsync();
        }

        public static async Task<Assessment> GetAssessmentAsync(int id) // ID = PK
        {
            return await _db.FindAsync<Assessment>(id);
        }

        public static async Task<int> SaveAssessmentAsync(Assessment assessment)
        {
            if (assessment.AssessmentId == -1 || assessment == null)
            {
                return await _db.InsertAsync(assessment);
            }
            else
            {
                return await _db.UpdateAsync(assessment);
            }
        }

        public static async Task<int> DeleteAssessmentAsync(int id)
        {
            return await _db.DeleteAsync<Assessment>(id);
        }

        #endregion

        #region Instructor CRUD

        public static async Task<List<Instructor>> GetInstructorsAsync()
        {
            return await _db.Table<Instructor>().ToListAsync();
        }

        public static async Task<Instructor> GetInstructorAsync(int id) // ID = PK
        {
            return await _db.FindAsync<Instructor>(id);
        }

        public static async Task<int> SaveInstructorAsync(Instructor instructor)
        {
            if (instructor.InstructorId == -1 || instructor == null)
            {
                return await _db.InsertAsync(instructor);
            }
            else
            {
                return await _db.UpdateAsync(instructor);
            }
        }

        public static async Task<int> DeleteInstructorAsync(int id)
        {
            return await _db.DeleteAsync<Instructor>(id);
        }

        #endregion

        #region Required Sample Data

        private static string _demoTermName = "Big League Term";
        private static string _demoTermId = "5";
        private static DateTime _demoStartDate = new(2024, 2, 1);
        private static DateTime _demoEndDate = new(2024, 7, 31);
        private static string _demoInstructorName = "Anika Patel";
        private static string _demoInstructorEmail = "anika.patel@strimeuniversity.edu";
        private static string _demoInstructorPhone = "555-123-4567";
        private static string _demoCourseName = "Intro to Felt Surrogacy";

        private static string _demoNotes =
            "Lecture series discussing the use of puppets to relive the traumatic events of our past.";

        #endregion
    }
}
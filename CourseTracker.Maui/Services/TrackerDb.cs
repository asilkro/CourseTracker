using SQLite;
using CourseTracker.Maui.Models;
using static Microsoft.Maui.Storage.FileSystem;
using System.Diagnostics;

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

            _db = new SQLiteAsyncConnection(DatabasePath, Flags);
            _dbConnection = new SQLiteConnection(DatabasePath, Flags);

            await SetupTables(_db); // Will create tables if they don't exist
            if (_dbConnection.Table<Term>().Count() != 0) return; // If the database already has data, don't add more.

            await _db.EnableLoadExtensionAsync(true);
            
        }

        public static async Task SetupTables(SQLiteAsyncConnection db)
        {
            // Check if the 'Term' table exists and create it if it doesn't.
            var termTableInfo = await db.GetTableInfoAsync(nameof(Term));
            if (!termTableInfo.Any())
            {
                await db.CreateTableAsync<Term>();
            }

            // Check if the 'Course' table exists and create it if it doesn't.
            var courseTableInfo = await db.GetTableInfoAsync(nameof(Course));
            if (!courseTableInfo.Any())
            {
                await db.CreateTableAsync<Course>();
            }

            // Check if the 'Assessment' table exists and create it if it doesn't.
            var assessmentTableInfo = await db.GetTableInfoAsync(nameof(Assessment));
            if (!assessmentTableInfo.Any())
            {
                await db.CreateTableAsync<Assessment>();
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
            if (_connection == null)
            {
                _connection = new Connection();
            }

            using var _dbConnection = _connection.GetConnection();

            _dbConnection.DropTable<Term>();
            Debug.WriteLine("Dropped Term table");
            _dbConnection.DropTable<Course>();
            Debug.WriteLine("Dropped Course table");
            _dbConnection.DropTable<Assessment>();
            Debug.WriteLine("Dropped Assessment table");
            _dbConnection.CreateTable<Term>();
            Debug.WriteLine("Created Term table");
            _dbConnection.CreateTable<Course>();
            Debug.WriteLine("Created Course table");
            _dbConnection.CreateTable<Assessment>();
            Debug.WriteLine("Created Assessment table");
        }

        #endregion
    }
}
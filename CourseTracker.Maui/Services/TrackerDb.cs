using SQLite;
using CourseTracker.Maui.Models;
using static Microsoft.Maui.Storage.FileSystem;

namespace CourseTracker.Maui.Services
{
    public static class TrackerDb
    {
        private static Connection _connection;
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

        private static async Task Initialize()
        {
            if (_db != null) return; // If the database is already initialized, don't do it again.

            _db = new SQLiteAsyncConnection(DatabasePath);
            _dbConnection = new SQLiteConnection(DatabasePath);

            await _db.EnableLoadExtensionAsync(true);
            _dbConnection.EnableLoadExtension(true); // cannot async a void method
            await SetupTables(_db);
        }

        private static async Task SetupTables(SQLiteAsyncConnection db)
        {
            await db.CreateTableAsync<Term>();
            await db.CreateTableAsync<Course>();
            await db.CreateTableAsync<Assessment>();
            await db.CreateTableAsync<Instructor>();
        }

        #endregion

        #region Utility

        public static int GetNextAutoIncrementID(string tableName)
        {
            var query = $"SELECT seq FROM sqlite_sequence WHERE name = '{tableName}'";
            var result = _dbConnection.ExecuteScalar<int>(query);
            return result + 1;
        }

        //TODO: Add CRUD methods using Factories

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
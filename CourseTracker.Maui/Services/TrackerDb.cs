using System.Diagnostics;
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

            _db = new SQLiteAsyncConnection(DatabasePath, Flags);

            await SetupTables(_db); // Will create tables if they don't exist
            if (await _db.Table<Term>().CountAsync() != 0) return; // If the database already has data, don't add more.
            if (await _db.Table<Course>().CountAsync() != 0) return;
            if (await _db.Table<Assessment>().CountAsync() != 0) return;

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

            // Check if the 'sqlite_sequence' table exists and create it if it doesn't.
            // It should be getting created automatically, but this is here just in case.
            var sqlite_sequenceTableInfo = await db.GetTableInfoAsync("sqlite_sequence");
            if (!sqlite_sequenceTableInfo.Any())
            {
                await db.ExecuteAsync("CREATE TABLE sqlite_sequence(name, seq)");
            }
        }

        #endregion

        #region Utility

        public static async Task ResetDatabaseFileAsync()
        {
            try
            {
                if (File.Exists(DatabasePath)) // If the database file exists, delete it
                {
                    Debug.WriteLine("Erasing file at: " + DatabasePath);
                    File.Delete(DatabasePath);
                }

                // Re-run the initialization of the database to make a new DB file and tables
                Debug.WriteLine("Preparing to reinitialize database at: " + DatabasePath);
                await Initialize();
                Debug.WriteLine("Database reinitialized at: " + DatabasePath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to reset database: " + ex.Message);
                // This should be unnecessary, but it's here for debugging purposes
            }
        }

        #endregion
    }
}
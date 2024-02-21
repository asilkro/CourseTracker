using System.Diagnostics;
using static Microsoft.Maui.Storage.FileSystem;

namespace CourseTracker.Maui.Services
{
    public class TrackerDb
    {
        private static Connection _connection = new();

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

        #endregion

        #region Utility

        public static async Task ResetDatabaseFileAsync()
        {
            try
            {
                _connection.GetConnection().Execute("Delete from Term");
                _connection.GetConnection().Execute("Delete from Course");
                _connection.GetConnection().Execute("Delete from Assessment");
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
using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace CourseTracker.Services
{
    public class AppDatabase
    {
        static SQLiteAsyncConnection Database; // Using a single connection to minimize overhead
        
        static string connectionString = "Data Source=CourseTrackerDB.db3;";

        //static string setupDB = "";

        public int SubmitQuery(string query)
        {
            var result = -1;
            SQLiteCommand command;

            return result;
        }
    }
}

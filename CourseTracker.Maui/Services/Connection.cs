using SQLite;

namespace CourseTracker.Maui.Services;

public interface IAsyncSqLite
{
    SQLiteAsyncConnection GetAsyncConnection(); // ASync Implementation

    SQLiteConnection GetConnection(); // Non-Async Implementation should it be useful for some case
}

class Connection : IAsyncSqLite
{
    public SQLiteAsyncConnection GetAsyncConnection()
    {
        return new SQLiteAsyncConnection(TrackerDb.DatabasePath, TrackerDb.Flags);
    }
    public SQLiteConnection GetConnection()
    {
        return new SQLiteConnection(TrackerDb.DatabasePath, TrackerDb.Flags);
    }
}
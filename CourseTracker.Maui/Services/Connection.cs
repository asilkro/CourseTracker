using SQLite;

namespace CourseTracker.Maui.Services;
public interface IAsyncSqLite
{
    SQLiteAsyncConnection GetAsyncConnection(); //Async Connection
    SQLiteConnection GetConnection(); //Non-Async Connection
    Task InsertAsync<T>(T obj);
    Task<List<T>> Table<T>() where T : new();
    Task<T> FindAsync<T>(int id) where T : new();
    Task UpdateAsync<T>(T obj);
    Task DeleteAsync<T>(T obj);
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

    public async Task InsertAsync<T>(T obj)
    {
        var asyncConnection = GetAsyncConnection();
        await asyncConnection.InsertAsync(obj);
    }

    public async Task<List<T>> Table<T>() where T : new()
    {
        var asyncConnection = GetAsyncConnection();
        return await asyncConnection.Table<T>().ToListAsync();
    }

    public async Task<T> FindAsync<T>(int id) where T : new()
    {
        var asyncConnection = GetAsyncConnection();
        return await asyncConnection.FindAsync<T>(id);
    }

    public async Task UpdateAsync<T>(T obj)
    {
        var asyncConnection = GetAsyncConnection();
        await asyncConnection.UpdateAsync(obj);
    }

    public async Task DeleteAsync<T>(T obj)
    {
        var asyncConnection = GetAsyncConnection();
        await asyncConnection.DeleteAsync(obj);
    }
}
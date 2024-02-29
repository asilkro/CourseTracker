using System.Diagnostics;
using SQLite;

namespace CourseTracker.Maui.Services;
public interface IAsyncSqLite
{
    SQLiteAsyncConnection GetAsyncConnection(); //Async Connection
    SQLiteConnection GetConnection(); //Non-Async Connection
    Task InsertAsync<T>(T obj);
    Task<List<T>> Table<T>() where T : new();
    Task<T> FindAsync<T>(int id) where T : new();
    Task<T> FindAsync<T>(string stringBeingChecked) where T : new();
    Task<int> UpdateAsync<T>(T obj);
    Task DeleteAsync<T>(T obj);
    Task<T> ExecuteScalarAsync<T>(string query);
    Task<bool> AnyAsync<T>() where T : new();
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

    public async Task<T> FindAsync<T>(string stringBeingChecked) where T : new()
    {
        var asyncConnection = GetAsyncConnection();
        return await asyncConnection.FindAsync<T>(stringBeingChecked);
    }

    public async Task<int> UpdateAsync<T>(T obj)
    {
        var asyncConnection = GetAsyncConnection();
        return await asyncConnection.UpdateAsync(obj);
    }

    public async Task DeleteAsync<T>(T obj)
    {
        var asyncConnection = GetAsyncConnection();
        await asyncConnection.DeleteAsync(obj);
    }

    public async Task<T> ExecuteScalarAsync<T>(string query)
    {
        var asyncConnection = GetAsyncConnection();
        return await asyncConnection.ExecuteScalarAsync<T>(query);
    }

    public async Task<bool> AnyAsync<T>() where T : new()
    {
        var asyncConnection = GetAsyncConnection();
        try
        {
            var record = await asyncConnection.Table<T>().FirstOrDefaultAsync();
            return record != null;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error in AnyAsync: {ex.Message}"); // Never came up in testing
            return false;
        }
    }
}
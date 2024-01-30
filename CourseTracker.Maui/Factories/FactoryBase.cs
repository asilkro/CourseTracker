using CourseTracker.Maui.Services;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseTracker.Maui.Factories
{
    public abstract class FactoryBase<T>
    where T : new()
    {
        readonly IAsyncSqLite _database;
        protected List<T> createdObjects = new();

        protected FactoryBase(IAsyncSqLite database)
        {
            _database = database;
        }

        public T? CreateObject()
        {
            var obj = CreateDefaultObject();
            if (obj != null)
            {
                createdObjects.Add(obj);
            }

            return obj;
        }

        public async Task AddObject(T obj)
        {
            await _database.InsertAsync(obj);
            createdObjects.Add(obj);
        }

        public async Task<List<T>> GetAllObjects()
        {
            // Ensure the 'new()' constraint is applied here
            return await _database.Table<T>();
        }

        public async Task<T> GetObjectById(int oid)
        {
            // Ensure the 'new()' constraint is applied here
            return await _database.FindAsync<T>(oid);
        }

        public async Task UpdateObject(T obj)
        {
            await _database.UpdateAsync(obj);
        }

        public async Task DeleteObject(T obj)
        {
            await _database.DeleteAsync(obj);
        }

        protected abstract T? CreateDefaultObject();
    }

}
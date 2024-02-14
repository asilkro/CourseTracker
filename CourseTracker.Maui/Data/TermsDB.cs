﻿using System.Collections.ObjectModel;
using CourseTracker.Maui.Models;
using CourseTracker.Maui.Services;

namespace CourseTracker.Maui.Data
{
    public class TermsDB
    {
        SQLiteAsyncConnection _database;
        public TermsDB()
        {
        }
        async Task Init()
        {
            if (_database != null)
                return;
            _database = new SQLiteAsyncConnection(TrackerDb.DatabasePath, TrackerDb.Flags);
            await _database.CreateTableAsync<Term>();
        }
        public async Task<List<Term>> GetTermsAsync()
        {
            await Init();
            return await _database.Table<Term>().ToListAsync();
        }
        public async Task SaveTermAsync(Term term)
        {
            await Init();
            if (term.TermId != 0)
            {
                await _database.UpdateAsync(term);
            }
            else
            {
                await _database.InsertAsync(term);
            }
        }
        public async Task<int> DeleteTermAsync(Term term)
        {
            await Init();
            return await _database.DeleteAsync(term);
        }
        public async Task<Term> GetTermByIdAsync(int id)
        {
            await Init();
            return await _database.Table<Term>()
                .Where(i => i.TermId == id)
                .FirstOrDefaultAsync();
        }
    }
}

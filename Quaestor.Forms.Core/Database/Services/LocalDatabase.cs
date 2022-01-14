using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Quaestor.Forms.Core.Database.Models.Interfaces;
using SQLite;
using Polly;

namespace Quaestor.Forms.Core.Database.Services
{
    public class LocalDatabase : IDatabaseService
    {
        readonly SQLiteAsyncConnection _database;

        #region Constructor

        public LocalDatabase()
        {
            _database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags, false);
        }

        #endregion

        #region Public

        public async Task<List<T>> GetListAsync<T>() where T : class, new()
        {
            await CheckTable<T>().ConfigureAwait(false);

            var data = await AttemptAndRetry(() => _database.Table<T>().ToListAsync())
                                 .ConfigureAwait(false);
            return data;
        }

        public async Task<List<T>> GetListAsync<T>(Expression<Func<T, bool>> expression) where T : class, new()
        {
            await CheckTable<T>().ConfigureAwait(false);

            var data = await AttemptAndRetry(() => _database.Table<T>()
                                                            .Where(expression)
                                                            .ToListAsync())
                                            .ConfigureAwait(false);

            return data;
        }

        public async Task<T> GetAsync<T>(int id) where T : class, IBaseDb, new()
        {
            await CheckTable<T>().ConfigureAwait(false);

            var data = await AttemptAndRetry(() => _database.Table<T>()
                                                            .Where(i => i.Id.Equals(id))
                                                            .FirstOrDefaultAsync())
                                            .ConfigureAwait(false);
            return data;
        }

        public async Task<bool> SaveAsync<T>(T data) where T : class, IBaseDb, new()
        {
            await CheckTable<T>().ConfigureAwait(false);

            //Create
            if (data.Id == 0)
            {
                var updated = await AttemptAndRetry(() => _database.InsertAsync(data)).ConfigureAwait(false);
                return updated == 1;
            }
            else //Update
            {
                var updated = await AttemptAndRetry(() => _database.UpdateAsync(data)).ConfigureAwait(false);
                return updated == 1;
            }
        }

        public async Task<bool> DeleteAsync<T>(T data) where T : class, new()
        {
            await CheckTable<T>().ConfigureAwait(false);

            var updated = await AttemptAndRetry(() => _database.DeleteAsync(data)).ConfigureAwait(false);
            return updated == 1;
        }

        #endregion

        #region Private

        async Task CheckTable<T>()
        {
            if (!_database.TableMappings.Any(x => x.MappedType == typeof(T)))
            {
                await _database.EnableWriteAheadLoggingAsync().ConfigureAwait(false);
                await _database.CreateTablesAsync(CreateFlags.None, typeof(T)).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Retry task if a SQLiteException is thrown. Using Exponential backoff the error will be thrown after about 4s.
        /// https://codetraveler.io/2019/11/26/efficiently-initializing-sqlite-database/
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="numRetries"></param>
        /// <returns></returns>
        static Task<T> AttemptAndRetry<T>(Func<Task<T>> action, int numRetries = 10)
        {
            return Policy.Handle<SQLiteException>().WaitAndRetryAsync(numRetries, pollyRetryAttempt).ExecuteAsync(action);

            static TimeSpan pollyRetryAttempt(int attemptNumber) => TimeSpan.FromMilliseconds(Math.Pow(2, attemptNumber));
        }

        #endregion
    }
}

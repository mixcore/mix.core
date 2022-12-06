using Microsoft.Extensions.Options;
using Mix.RepoDb.Interfaces;
using Mix.RepoDb.Models;
using RepoDb;
using RepoDb.Interfaces;
using System.Data;
using System.Data.Common;

namespace Mix.RepoDb.Repositories
{
    public class RepoDbBase<TDbConnection, TEntity> : IRepoDb<TDbConnection, TEntity>
    where TDbConnection : DbConnection
    where TEntity : class
    {
        private readonly AppSetting _settings;

        public RepoDbBase(IOptions<AppSetting> settings,
            ICache cache,
            ITrace trace)
        {
            _settings = settings.Value;
            Cache = cache;
            Trace = trace;
        }
        public RepoDbBase()
        {

        }
        /*** Properties ***/

        public ITrace Trace { get; }

        public ICache Cache { get; }

        /*** Methods ***/

        public TDbConnection CreateConnection()
        {
            var connection = Activator.CreateInstance<TDbConnection>();
            connection.ConnectionString = _settings.ConnectionString;
            return connection;
        }

        /*** Non-Async ***/

        // Get (Many)

        public IEnumerable<TEntity> GetAll(string? cacheKey = null)
        {
            using (var connection = CreateConnection())
            {
                return connection.QueryAll<TEntity>(cacheKey: cacheKey,
                    commandTimeout: _settings.CommandTimeout,
                    cache: Cache,
                    cacheItemExpiration: _settings.CacheItemExpiration,
                    trace: Trace);
            }
        }

        // Get

        public TEntity Get(int id)
        {
            using (var connection = CreateConnection())
            {
                return connection.Query<TEntity>(id,
                    commandTimeout: _settings.CommandTimeout,
                    trace: Trace).First();
            }
        }

        // Delete

        public int Delete(int id)
        {
            using (var connection = CreateConnection())
            {
                return connection.Delete<TEntity>(id,
                    commandTimeout: _settings.CommandTimeout,
                    trace: Trace);
            }
        }

        // Merge

        public int Merge(TEntity entity,
            IDbTransaction? transaction = null)
        {
            using (var connection = CreateConnection())
            {
                return connection.Merge<TEntity, int>(entity,
                    commandTimeout: _settings.CommandTimeout,
                    trace: Trace);
            }
        }

        // Save

        public object Insert(TEntity entity,
            IDbTransaction? transaction = null)
        {
            using (var connection = CreateConnection())
            {
                return connection.Insert(entity,
                    commandTimeout: _settings.CommandTimeout,
                    trace: Trace);
            }
        }

        // Update

        public int Update(TEntity entity,
            IDbTransaction? transaction = null)
        {
            using (var connection = CreateConnection())
            {
                return Update(entity,
                    transaction: transaction);
            }
        }

        /*** Async ***/

        // Get (Many)

        public async Task<IEnumerable<TEntity>> GetAllAsync(string? cacheKey = null)
        {
            using (var connection = CreateConnection())
            {
                return await connection.QueryAllAsync<TEntity>(cacheKey: cacheKey,
                    commandTimeout: _settings.CommandTimeout,
                    cache: Cache,
                    cacheItemExpiration: _settings.CacheItemExpiration,
                    trace: Trace);
            }
        }

        // Get

        public async Task<TEntity> GetAsync(int id)
        {
            using (var connection = CreateConnection())
            {
                return (await connection.QueryAsync<TEntity>(id,
                    commandTimeout: _settings.CommandTimeout,
                    trace: Trace)).First();
            }
        }

        // Delete

        public async Task<int> DeleteAsync(int id)
        {
            using (var connection = CreateConnection())
            {
                return await connection.DeleteAsync<TEntity>(id,
                    commandTimeout: _settings.CommandTimeout,
                    trace: Trace);
            }
        }

        // Merge

        public async Task<object> MergeAsync(TEntity entity)
        {
            using (var connection = CreateConnection())
            {
                return await connection.MergeAsync(entity,
                    commandTimeout: _settings.CommandTimeout,
                    trace: Trace);
            }
        }

        // Save

        public async Task<object> InsertAsync(TEntity entity)
        {
            using (var connection = CreateConnection())
            {
                return await connection.InsertAsync(entity,
                    commandTimeout: _settings.CommandTimeout,
                    trace: Trace);
            }
        }

        // Update

        public async Task<int> UpdateAsync(TEntity entity)
        {
            using (var connection = CreateConnection())
            {
                return await connection.UpdateAsync(entity,
                    commandTimeout: _settings.CommandTimeout,
                    trace: Trace);
            }
        }
    }
}

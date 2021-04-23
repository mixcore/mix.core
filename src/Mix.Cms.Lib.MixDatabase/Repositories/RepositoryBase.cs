using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Mix.Cms.Lib.MixDatabase.Interfaces;
using Mix.Cms.Lib.MixDatabase.Models;
using RepoDb;
using RepoDb.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.MixDatabase.Repositories
{
    public class RepositoryBase<TDbConnection, TEntity> : IRepositoryBase<TDbConnection, TEntity>
    where TDbConnection : DbConnection
    where TEntity : class
    {
        private AppSetting _settings;

        public RepositoryBase([FromServices]IOptions<AppSetting> settings,
            [FromServices] ICache cache,
            [FromServices] ITrace trace)
        {
            _settings = settings.Value;
            Cache = cache;
            Trace = trace;
        }
        public RepositoryBase()
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

        public IEnumerable<TEntity> GetAll(string cacheKey = null)
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
            IDbTransaction transaction = null)
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
            IDbTransaction transaction = null)
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
            IDbTransaction transaction = null)
        {
            using (var connection = CreateConnection())
            {
                return Update(entity,
                    transaction: transaction);
            }
        }

        /*** Async ***/

        // Get (Many)

        public async Task<IEnumerable<TEntity>> GetAllAsync(string cacheKey = null)
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

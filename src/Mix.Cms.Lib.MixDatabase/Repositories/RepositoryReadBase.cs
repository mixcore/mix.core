using Microsoft.AspNetCore.Mvc;
using Mix.Cms.Lib.MixDatabase.Models;
using RepoDb;
using RepoDb.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.MixDatabase.Repositories
{
    public class RepositoryReadBase<TDbConnection>
        where TDbConnection : DbConnection
    {
        private string _tableName;
        private AppSetting _settings;
        public RepositoryReadBase(
            string tableName,
            AppSetting settings,
            [FromServices] ICache cache,
            [FromServices] ITrace trace)
        {
            _tableName = tableName;
            _settings = settings;
            Cache = cache;
            Trace = trace;
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

        public IEnumerable<dynamic> GetAll(string cacheKey = null)
        {
            using (var connection = CreateConnection())
            {
                return connection.QueryAll(_tableName, 
                    cacheKey: cacheKey,
                    commandTimeout: _settings.CommandTimeout,
                    cache: Cache,
                    cacheItemExpiration: _settings.CacheItemExpiration,
                    trace: Trace);
            }
        }

        // Get

        public dynamic Get(int id)
        {
            using (var connection = CreateConnection())
            {
                return connection.Query<dynamic>(_tableName, id,
                    commandTimeout: _settings.CommandTimeout,
                    trace: Trace).First();
            }
        }

        // Get (Many)

        public async Task<IEnumerable<dynamic>> GetAllAsync(string cacheKey = null)
        {
            using (var connection = CreateConnection())
            {
                return await connection.QueryAllAsync<dynamic>(
                    _tableName,
                    cacheKey: cacheKey,
                    commandTimeout: _settings.CommandTimeout,
                    cache: Cache,
                    cacheItemExpiration: _settings.CacheItemExpiration,
                    trace: Trace);
            }
        }

        // Get

        public async Task<dynamic> GetAsync(int id)
        {
            using (var connection = CreateConnection())
            {
                return (await connection.QueryAsync<dynamic>(
                    _tableName, 
                    id,
                    commandTimeout: _settings.CommandTimeout,
                    trace: Trace)).First();
            }
        }

    }
}

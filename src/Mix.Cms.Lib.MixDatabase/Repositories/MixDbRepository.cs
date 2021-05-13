using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.MixDatabase.Models;
using Mix.Cms.Lib.Models.Common;
using Mix.Cms.Lib.Services;
using Mix.Heart.Enums;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using Npgsql;
using RepoDb;
using RepoDb.Enumerations;
using RepoDb.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Mix.Heart.Models;

namespace Mix.Cms.Lib.MixDatabase.Repositories
{
    public class MixDbRepository
    {
        private AppSetting _settings;
        public MixDbRepository([FromServices] ICache cache)
        {
            Cache = cache;
            _settings = new AppSetting()
            {
                CacheItemExpiration = 10,
                CommandTimeout = 1000
            };
        }

        /*** Properties ***/

        public ITrace Trace { get; }

        public ICache Cache { get; }

        /*** Methods ***/

        public IDbConnection CreateConnection()
        {
            var provider = MixService.GetEnumConfig<MixDatabaseProvider>(MixConstants.CONST_SETTING_DATABASE_PROVIDER);
            var connectionType = GetDbConnectionType(provider);
            var connection = Activator.CreateInstance(connectionType) as IDbConnection;
            string cnn = MixService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION);
            connection.ConnectionString = cnn;
            return connection;
        }

        static Type GetDbConnectionType(MixDatabaseProvider dbProvider)
        {
            switch (dbProvider)
            {
                case MixDatabaseProvider.MSSQL:
                    return typeof(SqlConnection);
                case MixDatabaseProvider.MySQL:
                    return typeof(MySqlConnection);
                case MixDatabaseProvider.PostgreSQL:
                    return typeof(NpgsqlConnection);
                case MixDatabaseProvider.SQLITE:
                    return typeof(SqliteConnection);
                default:
                    return typeof(SqliteConnection);
            }
        }

        /*** Non-Async ***/

        // Get (Many)

        public IEnumerable<dynamic> GetAll(string _tableName, dynamic query, PagingRequest pagingData, string cacheKey = null)
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

        public dynamic Get(string _tableName, string id)
        {
            using (var connection = CreateConnection())
            {
                return connection.Query<dynamic>(_tableName, id,
                    commandTimeout: _settings.CommandTimeout,
                    trace: Trace).First();
            }
        }

        // Get (Many)

        public async Task<PaginationModel<dynamic>> GetAllAsync(string _tableName, JObject query, PagingRequest pagingRequest, string cacheKey = null)
        {
            using (var connection = CreateConnection())
            {
                IEnumerable<QueryField> queryFields = ParseQuery(query);
                List<OrderField> orderFields = new List<OrderField>() {
                    new OrderField(pagingRequest.OrderBy, pagingRequest.Direction == DisplayDirection.Asc ? Order.Ascending: Order.Descending)
                };
                var count = (int)connection.Count(_tableName, queryFields);
                var data = await connection.BatchQueryAsync(_tableName, pagingRequest.PageIndex,
                    pagingRequest.PageSize, orderFields, queryFields, null, null, _settings.CommandTimeout);
                return new PaginationModel<dynamic>()
                {
                    Items = data.ToList(),
                    PageIndex = pagingRequest.PageIndex,
                    PageSize = pagingRequest.PageSize,
                    TotalItems = count,
                    TotalPage = (count / pagingRequest.PageSize) + (count % pagingRequest.PageSize > 0 ? 1 : 0)
                };
            }
        }

        private IEnumerable<QueryField> ParseQuery(JObject query)
        {
            List<QueryField> result = null;
            if (query != null)
            {
                result = new List<QueryField>();
                foreach (var item in query.Properties())
                {
                    QueryField field = new QueryField(item.Name, query.Value<string>(item.Name));
                    result.Add(field);
                }
            }
            return result;
        }

        // Get

        public async Task<dynamic> GetAsync(string _tableName, string id)
        {
            using (var connection = CreateConnection())
            {
                return (await connection.QueryAsync<dynamic>(
                    _tableName,
                    new
                    {
                        id = id
                    },
                    commandTimeout: _settings.CommandTimeout,
                    trace: Trace)).First();
            }
        }

    }
}

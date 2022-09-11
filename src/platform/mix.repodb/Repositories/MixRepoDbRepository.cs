using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Mix.Constant.Constants;
using Mix.Constant.Enums;
using Mix.Database.Entities.Cms;
using Mix.Database.Services;
using Mix.Heart.Enums;
using Mix.Heart.Models;
using Mix.Heart.UnitOfWork;
using Mix.RepoDb.Models;
using Mix.Service.Services;
using Mix.Shared.Models;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using Npgsql;
using RepoDb;
using RepoDb.Enumerations;
using RepoDb.Interfaces;
using System.Data;

namespace Mix.RepoDb.Repositories
{
    public class MixRepoDbRepository
    {
        #region Properties

        public ITrace Trace { get; }

        public ICache Cache { get; }

        public string ConnectionString { get; set; }
        public MixDatabaseProvider DatabaseProvider { get; set; }
        readonly DatabaseService _databaseService;
        private AppSetting _settings;
        private string _tableName;
        #endregion

        public MixRepoDbRepository(ICache cache, DatabaseService databaseService)
        {
            Cache = cache;
            _settings = new AppSetting()
            {
                CacheItemExpiration = 10,
                CommandTimeout = 1000
            };
            _databaseService = databaseService;
            ConnectionString = _databaseService.GetConnectionString(MixConstants.CONST_MIXDB_CONNECTION);
            DatabaseProvider = _databaseService.DatabaseProvider;
            InitializeRepoDb();
        }

        #region Methods
        public void Init(string tableName)
        {
            _tableName = tableName.ToLower();
            ConnectionString = _databaseService.GetConnectionString(MixConstants.CONST_MIXDB_CONNECTION);
            DatabaseProvider = _databaseService.DatabaseProvider;
        }

        public void Init(string tableName, MixDatabaseProvider databaseProvider, string connectionString)
        {
            DatabaseProvider = databaseProvider;
            ConnectionString = connectionString;
            _tableName = tableName;
            InitializeRepoDb();
        }


        public Task<int> ExecuteCommand(string commandSql)
        {
            using (var connection = CreateConnection())
            {
                return connection.ExecuteNonQueryAsync(commandSql);
            }
        }

        public async Task<PagingResponseModel<dynamic>> GetPagingAsync(IEnumerable<QueryField> queryFields, PagingRequestModel pagingRequest)
        {
            using (var connection = CreateConnection())
            {
                List<OrderField> orderFields = new List<OrderField>() {
                    new OrderField(pagingRequest.SortBy ?? "id", pagingRequest.SortDirection == SortDirection.Asc ? Order.Ascending: Order.Descending)
                };
                var count = (int)connection.Count(_tableName, queryFields);
                int pageSize = pagingRequest.PageSize.HasValue ? pagingRequest.PageSize.Value : 100;
                var data = await connection.BatchQueryAsync(_tableName, pagingRequest.PageIndex,
                    pageSize, orderFields, queryFields, null, null, commandTimeout: _settings.CommandTimeout);
                return new PagingResponseModel<dynamic>()
                {
                    Items = data.ToList(),
                    PagingData = new()
                    {
                        PageIndex = pagingRequest.PageIndex,
                        PageSize = pagingRequest.PageSize,
                        Total = count,
                        TotalPage = (int)Math.Ceiling((double)pagingRequest.Total / pageSize)
                    }
                };
            }
        }

        public async Task<List<dynamic>?> GetListByAsync(List<QueryField> queryFields)
        {
            using (var connection = CreateConnection())
            {
                try
                {
                    var data = await connection.QueryAsync(_tableName, queryFields);
                    return data.ToList();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.Message);
                    return default;
                }
            }
        }

        public async Task<List<dynamic>?> GetAllAsync()
        {
            using (var connection = CreateConnection())
            {
                try
                {
                    var data = await connection.QueryAllAsync(_tableName, null, null, commandTimeout: _settings.CommandTimeout);
                    return data.ToList();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.Message);
                    return default;
                }
            }
        }

        public async Task<dynamic?> GetSingleByParentAsync(MixContentType parentType, object parentId)
        {
            using (var connection = CreateConnection())
            {
                try
                {
                    return (await connection.QueryAsync<dynamic>(
                        _tableName,
                        new List<QueryField>() {
                    new QueryField("parentType", parentType.ToString()),
                    new QueryField("parentId", parentId)
                        },
                        commandTimeout: _settings.CommandTimeout,
                        trace: Trace))?.SingleOrDefault();
                }
                catch(Exception ex)
                {
                    MixService.LogException(ex);
                    return default;
                }
            }
        }

        // Get

        public async Task<dynamic?> GetSingleAsync(int id)
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
                    trace: Trace))?.SingleOrDefault();
            }
        }

        public async Task<int> InsertAsync(JObject entity,
            IDbTransaction? transaction = null)
        {
            using (var connection = CreateConnection())
            {
                var obj = entity.ToObject<Dictionary<string, object>>()!;
                var result = await connection.InsertAsync(
                        _tableName,
                        entity: obj,
                        commandTimeout: _settings.CommandTimeout,
                        trace: Trace);

                return int.Parse(result?.ToString() ?? "0");
            }
        }

        public async Task<int?> InsertManyAsync(List<dynamic> entities,
            IDbTransaction? transaction = null)
        {
            using (var connection = CreateConnection())
            {
                var result = await connection.InsertAllAsync(
                        _tableName,
                        entities: entities,
                        commandTimeout: _settings.CommandTimeout,
                        trace: Trace);
                return result;
            }
        }

        public async Task<object?> UpdateAsync(JObject entity,
            IDbTransaction? transaction = null)
        {
            using (var connection = CreateConnection())
            {
                if (connection.Exists(_tableName, new { id = entity.Value<int>("id") }))
                {
                    object obj = entity.ToObject<Dictionary<string, object>>()!;
                    return await connection.UpdateAsync(_tableName, obj,
                        commandTimeout: _settings.CommandTimeout,
                        trace: Trace);
                }
                return null;
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            using (var connection = CreateConnection())
            {
                if (connection.Exists(_tableName, new { id = id }))
                {
                    return await connection.DeleteAsync(_tableName, id,
                        commandTimeout: _settings.CommandTimeout,
                        trace: Trace);
                }
                return 0;
            }
        }
        
        public async Task<int> DeleteAsync(List<QueryField> queries)
        {
            using (var connection = CreateConnection())
            {
                if (connection.Exists(_tableName, queries))
                {
                    return await connection.DeleteAsync(_tableName, queries,
                        commandTimeout: _settings.CommandTimeout,
                        trace: Trace);
                }
                return 0;
            }
        }

        #endregion

        #region private
        private IEnumerable<QueryField>? ParseQuery(JObject query)
        {
            List<QueryField>? result = null;
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


        private void InitializeRepoDb()
        {
            switch (DatabaseProvider)
            {
                case MixDatabaseProvider.SQLSERVER:
                    SqlServerBootstrap.Initialize();
                    break;
                case MixDatabaseProvider.MySQL:
                    MySqlBootstrap.Initialize();
                    break;
                case MixDatabaseProvider.PostgreSQL:
                    PostgreSqlBootstrap.Initialize();
                    break;
                case MixDatabaseProvider.SQLITE:
                    SqLiteBootstrap.Initialize();
                    break;
                default:
                    SqLiteBootstrap.Initialize();
                    break;
            }
        }
        public IDbConnection CreateConnection()
        {
            var connectionType = GetDbConnectionType(DatabaseProvider);
            var connection = Activator.CreateInstance(connectionType) as IDbConnection;
            connection!.ConnectionString = ConnectionString;
            return connection;
        }

        static Type GetDbConnectionType(MixDatabaseProvider dbProvider)
        {
            switch (dbProvider)
            {
                case MixDatabaseProvider.SQLSERVER:
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

        #endregion
    }
}

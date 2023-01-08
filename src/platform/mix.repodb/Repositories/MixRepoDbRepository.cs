using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Constant.Constants;
using Mix.Constant.Enums;
using Mix.Database.Entities.Cms;
using Mix.Database.Services;
using Mix.Heart.Enums;
using Mix.Heart.Exceptions;
using Mix.Heart.Extensions;
using Mix.Heart.Models;
using Mix.Heart.UnitOfWork;
using Mix.RepoDb.Models;
using Mix.Service.Services;
using Mix.Shared.Dtos;
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
    public class MixRepoDbRepository : IDisposable
    {
        #region Properties
        private readonly UnitOfWorkInfo<MixCmsContext> _cmsUow;
        private IDbConnection _connection;
        private IDbTransaction _dbTransaction;
        public ITrace Trace { get; }

        public ICache Cache { get; }

        public string ConnectionString { get; set; }
        public MixDatabaseProvider DatabaseProvider { get; set; }
        readonly DatabaseService _databaseService;
        private readonly AppSetting _settings;
        private string _tableName;
        private bool _isRoot;
        #endregion

        public MixRepoDbRepository(ICache cache, DatabaseService databaseService, UnitOfWorkInfo<MixCmsContext> cmsUow)
        {
            Cache = cache;
            _settings = new AppSetting()
            {
                CacheItemExpiration = 10,
                CommandTimeout = 1000
            };
            _databaseService = databaseService;

            _cmsUow = cmsUow;
            DatabaseProvider = _databaseService.DatabaseProvider;
            ConnectionString = _databaseService.GetConnectionString(MixConstants.CONST_MIXDB_CONNECTION);
            if (!string.IsNullOrEmpty(ConnectionString))
            {
                InitializeRepoDb();
                CreateConnection();
            }
        }


        #region Methods
        public void InitTableName(string tableName)
        {
            _tableName = tableName.ToLower();
            ConnectionString = _databaseService.GetConnectionString(MixConstants.CONST_MIXDB_CONNECTION);
            DatabaseProvider = _databaseService.DatabaseProvider;
            CreateConnection();
        }

        public void Init(string tableName, MixDatabaseProvider databaseProvider, string connectionString)
        {
            DatabaseProvider = databaseProvider;
            ConnectionString = connectionString;
            _tableName = tableName;
            InitializeRepoDb();
            CreateConnection(true);
        }


        public Task<int> ExecuteCommand(string commandSql)
        {
            try
            {
                if (_connection.State == ConnectionState.Closed)
                {
                    _connection.Open();

                }
                return _connection.ExecuteNonQueryAsync(commandSql, transaction: _dbTransaction);
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.ServerError, ex);
            }
        }

        public async Task<PagingResponseModel<dynamic>> GetPagingAsync(IEnumerable<QueryField> queryFields, PagingRequestModel pagingRequest)
        {
            List<OrderField> orderFields = new List<OrderField>() {
                    new OrderField(pagingRequest.SortBy ?? "Id", pagingRequest.SortDirection == SortDirection.Asc ? Order.Ascending: Order.Descending)
                };
            var count = (int)_connection.Count(_tableName, queryFields, transaction: _dbTransaction);
            int pageSize = pagingRequest.PageSize.HasValue ? pagingRequest.PageSize.Value : 100;
            var data = await _connection.BatchQueryAsync(_tableName, pagingRequest.PageIndex,
                pageSize, orderFields, queryFields, null, null, commandTimeout: _settings.CommandTimeout, transaction: _dbTransaction);
            return new PagingResponseModel<dynamic>()
            {
                Items = data.ToList(),
                PagingData = new()
                {
                    Page = pagingRequest.PageIndex + 1,
                    PageIndex = pagingRequest.PageIndex,
                    PageSize = pagingRequest.PageSize,
                    Total = count,
                    TotalPage = (int)Math.Ceiling((double)pagingRequest.Total / pageSize)
                }
            };
        }

        public Task<List<dynamic>?> GetListByAsync(IEnumerable<SearchQueryField> searchQueryFields, string? fields = null)
        {
            List<QueryField> queries = ParseSearchQuery(searchQueryFields);
            return GetListByAsync(queries, fields);
        }

        private List<QueryField> ParseSearchQuery(IEnumerable<SearchQueryField> searchQueryFields)
        {
            List<QueryField> queries = new();
            foreach (var item in searchQueryFields)
            {
                queries.Add(new QueryField(item.FieldName, item.Value));
            }
            return queries;
        }

        public async Task<List<dynamic>?> GetListByAsync(List<QueryField> queryFields, string? fields = null)
        {
            try
            {
                List<Field>? selectedFields = null;
                if (!string.IsNullOrEmpty(fields))
                {
                    selectedFields = new();
                    var arrField = fields.Split(',', StringSplitOptions.TrimEntries);
                    foreach (var item in arrField)
                    {
                        selectedFields.Add(new Field(item));
                    }
                }
                var data = await _connection.QueryAsync(_tableName, queryFields, selectedFields, transaction: _dbTransaction);
                return data.ToList();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return default;
            }
        }

        public async Task<List<dynamic>?> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var data = await _connection.QueryAllAsync(_tableName, null, null, commandTimeout: _settings.CommandTimeout, transaction: _dbTransaction, cancellationToken: cancellationToken);
                return data.ToList();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return default;
            }
        }

        public async Task<dynamic?> GetSingleByParentAsync(MixContentType parentType, object parentId)
        {
            try
            {
                if (_connection.State != ConnectionState.Open)
                {
                    _connection.Open();
                }
                return (await _connection.QueryAsync<dynamic>(
                    _tableName,
                    new List<QueryField>() {
                    new QueryField("ParentType", parentType.ToString()),
                    new QueryField("ParentId", parentId.ToString())
                    },
                    commandTimeout: _settings.CommandTimeout,
                    transaction: _dbTransaction,
                    trace: Trace))?.SingleOrDefault();
            }
            catch (Exception ex)
            {
                MixService.LogException(ex);
                return default;
            }
        }
        public async Task<dynamic?> GetListByParentAsync(MixContentType parentType, object parentId)
        {
            try
            {
                return (await _connection.QueryAsync<dynamic>(
                    _tableName,
                    new List<QueryField>() {
                    new QueryField("parentType", parentType.ToString()),
                    new QueryField("parentId", parentId)
                    },
                    commandTimeout: _settings.CommandTimeout,
                    transaction: _dbTransaction,
                    trace: Trace))?.ToList();
            }
            catch (Exception ex)
            {
                MixService.LogException(ex);
                return default;
            }
        }

        // Get

        public async Task<dynamic?> GetSingleAsync(int id)
        {
            try
            {
                return (await _connection.QueryAsync<dynamic>(
                    _tableName,
                    new
                    {
                        Id = id
                    },
                    commandTimeout: _settings.CommandTimeout,
                    transaction: _dbTransaction,
                    trace: Trace))?.SingleOrDefault();
            }
            catch (Exception ex)
            {
                MixService.LogException(ex);
                return default;
            }
        }

        public async Task<int> InsertAsync(JObject entity,
            IDbTransaction? transaction = null)
        {
            try
            {
                JObject obj = new JObject();
                foreach (var pr in entity.Properties())
                {
                    obj.Add(new JProperty(pr.Name.ToTitleCase(), pr.Value));
                }
                var dicObj = obj.ToObject<Dictionary<string, object>>();
                var result = await _connection.InsertAsync(
                        _tableName,
                        entity: dicObj,
                        fields: null,
                        commandTimeout: _settings.CommandTimeout,
                        transaction: _dbTransaction,
                        trace: Trace);

                return int.Parse(result?.ToString() ?? "0");
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.Badrequest, ex.Message);
            }
        }

        public async Task<int?> InsertManyAsync(List<dynamic> entities,
            IDbTransaction? transaction = null)
        {
            try
            {
                var result = await _connection.InsertAllAsync(
                        _tableName,
                        entities: entities,
                        commandTimeout: _settings.CommandTimeout,
                        transaction: _dbTransaction,
                        trace: Trace);
                return result;
            }
            catch (Exception ex)
            {
                MixService.LogException(ex);
                return default;
            }
        }

        public async Task<object?> UpdateAsync(JObject entity,
            IDbTransaction? transaction = null)
        {
            try
            {
                if (_connection.Exists(_tableName, new { Id = entity.Value<int>("Id") }, transaction: _dbTransaction))
                {
                    object obj = entity.ToObject<Dictionary<string, object>>()!;
                    return await _connection.UpdateAsync(_tableName, obj,
                        commandTimeout: _settings.CommandTimeout,
                        trace: Trace,
                        transaction: _dbTransaction);
                }
                return null;
            }
            catch (Exception ex)
            {
                MixService.LogException(ex);
                return default;
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            try
            {
                if (_connection.Exists(_tableName, new { Id = id }, transaction: _dbTransaction))
                {
                    return await _connection.DeleteAsync(_tableName, id,
                        commandTimeout: _settings.CommandTimeout,
                        transaction: _dbTransaction,
                        trace: Trace);
                }
                return 0;
            }
            catch (Exception ex)
            {
                MixService.LogException(ex);
                return default;
            }
        }

        public async Task<int> DeleteAsync(List<QueryField> queries)
        {
            try
            {
                if (_connection.Exists(_tableName, queries, transaction: _dbTransaction))
                {
                    return await _connection.DeleteAsync(_tableName, queries,
                        commandTimeout: _settings.CommandTimeout,
                        transaction: _dbTransaction,
                        trace: Trace);
                }
                return 0;
            }
            catch (Exception ex)
            {
                MixService.LogException(ex);
                return default;
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
                    GlobalConfiguration.Setup().UseSqlServer();
                    break;
                case MixDatabaseProvider.MySQL:
                    GlobalConfiguration.Setup().UseMySql();
                    break;
                case MixDatabaseProvider.PostgreSQL:
                    GlobalConfiguration.Setup().UsePostgreSql();
                    break;
                case MixDatabaseProvider.SQLITE:
                    GlobalConfiguration.Setup().UseSqlite();
                    break;
                default:
                    GlobalConfiguration.Setup().UseSqlite();
                    break;
            }
        }

        private void SetDbConnection()
        {
            _cmsUow.Begin();
            _connection = _cmsUow.DbContext.Database.GetDbConnection();
            _dbTransaction = _cmsUow.ActiveTransaction.GetDbTransaction();
            _isRoot = false;
            switch (DatabaseProvider)
            {
                case MixDatabaseProvider.SQLSERVER:
                    GlobalConfiguration.Setup().UseSqlServer();
                    break;
                case MixDatabaseProvider.MySQL:
                    GlobalConfiguration.Setup().UseMySqlConnector();
                    break;
                case MixDatabaseProvider.PostgreSQL:
                    GlobalConfiguration.Setup().UsePostgreSql();
                    break;
                case MixDatabaseProvider.SQLITE:
                    GlobalConfiguration.Setup().UseSqlite();
                    break;
                default:
                    GlobalConfiguration.Setup().UseSqlite();
                    break;
            }
        }

        public IDbConnection CreateConnection(bool isRoot = false)
        {
            _isRoot = isRoot;
            var connectionType = GetDbConnectionType(DatabaseProvider);

            if (_isRoot)
            {
                _connection = Activator.CreateInstance(connectionType) as IDbConnection;
                _connection!.ConnectionString = ConnectionString;
                _connection.Open();
                _dbTransaction = _connection.BeginTransaction();
            }
            else
            {
                SetDbConnection();
            }
            return _connection;
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

        public void Dispose()
        {
            if (_connection != null)
            {
                if (_isRoot && _dbTransaction.Connection != null)
                {
                    _dbTransaction.Commit();
                }
                _connection.Close();
            }
        }
    }
}

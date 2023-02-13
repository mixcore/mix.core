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
        private readonly AppSetting _settings;
        private string _tableName;
        private bool _isRoot = true;
        #endregion

        public MixRepoDbRepository(ICache cache, DatabaseService databaseService, UnitOfWorkInfo<MixCmsContext> cmsUow)
        {
            Cache = cache;
            _settings = new AppSetting()
            {
                CacheItemExpiration = 10,
                CommandTimeout = 1000
            };

            _cmsUow = cmsUow;
            DatabaseProvider = databaseService.DatabaseProvider;
            ConnectionString = databaseService.GetConnectionString(MixConstants.CONST_MIXDB_CONNECTION);
            InitializeRepoDb();
            CreateConnection();
        }

        public MixRepoDbRepository(ICache cache, MixDatabaseProvider databaseProvider, string connectionString, UnitOfWorkInfo<MixCmsContext> cmsUow)
        {
            Cache = cache;
            _settings = new AppSetting()
            {
                CacheItemExpiration = 10,
                CommandTimeout = 1000
            };

            _cmsUow = cmsUow;
            DatabaseProvider = databaseProvider;
            ConnectionString = connectionString;
            InitializeRepoDb();
            CreateConnection();
        }


        #region Methods
        public void InitTableName(string tableName)
        {
            _tableName = tableName.ToLower();
            InitializeRepoDb();
            CreateConnection();
        }

        public void Init(string tableName, MixDatabaseProvider databaseProvider, string connectionString)
        {
            DatabaseProvider = databaseProvider;
            ConnectionString = connectionString;
            _tableName = tableName;
            InitializeRepoDb();
            CreateConnection(true, true);
        }


        public Task<int> ExecuteCommand(string commandSql)
        {
            try
            {
                BeginTransaction();
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
                Operation op = ParseMixOperator(item.CompareOperator);
                queries.Add(new QueryField(item.FieldName, op, item.Value));
            }
            return queries;
        }

        private Operation ParseMixOperator(MixCompareOperator compareOperator)
        {
            return compareOperator switch
            {
                MixCompareOperator.InRange => Operation.In,
                _ => Operation.Equal
            };
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
                BeginTransaction();
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
                BeginTransaction();
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
            finally
            {
                CompleteTransaction();
            }
        }

        public async Task<dynamic?> GetSingleByAsync(List<QueryField> queries)
        {
            try
            {
                BeginTransaction();
                return (await _connection.QueryAsync<dynamic>(
                    _tableName,
                    queries,
                    commandTimeout: _settings.CommandTimeout,
                    transaction: _dbTransaction,
                    trace: Trace))?.SingleOrDefault();
            }
            catch (Exception ex)
            {
                MixService.LogException(ex);
                return default;
            }
            finally
            {
                CompleteTransaction();
            }
        }


        public async Task<dynamic?> GetListByParentAsync(MixContentType parentType, object parentId)
        {
            try
            {
                BeginTransaction();
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
            finally
            {
                CompleteTransaction();
            }
        }

        // Get

        public async Task<dynamic?> GetSingleAsync(int id)
        {
            try
            {
                BeginTransaction();
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
            finally
            {
                CompleteTransaction();
            }
        }

        public async Task<int> InsertAsync(JObject entity)
        {
            try
            {
                BeginTransaction();
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
            finally
            {
                CompleteTransaction();
            }
        }

        public async Task<int?> InsertManyAsync(List<dynamic> entities)
        {
            try
            {
                BeginTransaction();
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
            finally
            {
                CompleteTransaction();
            }
        }

        public async Task<object?> UpdateAsync(JObject entity)
        {
            try
            {
                BeginTransaction();
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
            finally
            {
                CompleteTransaction();
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            try
            {
                BeginTransaction();
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
            finally
            {
                CompleteTransaction();
            }
        }

        public async Task<int> DeleteAsync(List<QueryField> queries)
        {
            try
            {
                BeginTransaction();
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
            finally
            {
                CompleteTransaction();
            }
        }

        #endregion

        #region private
        private void BeginTransaction()
        {
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
                _dbTransaction = _connection.BeginTransaction();
            }
        }


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
            if (DatabaseProvider != MixDatabaseProvider.SQLITE)
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
        }

        public void SetDbConnection(UnitOfWorkInfo dbUow)
        {
            
            if (DatabaseProvider == MixDatabaseProvider.SQLITE)
            {

                dbUow.Begin();
                if (_connection != null)
                {
                    _connection.Close();
                    _connection.Dispose();
                }

                _connection = dbUow.ActiveDbContext.Database.GetDbConnection();
                _dbTransaction = dbUow.ActiveTransaction.GetDbTransaction();
                _isRoot = false;
            }
        }

        public IDbConnection CreateConnection(bool isRoot = false, bool isRenew = false)
        {
            if (!isRenew && _connection != null)
            {
                return _connection;
            }

            _isRoot = isRoot;
            var connectionType = GetDbConnectionType(DatabaseProvider);

            if (_isRoot || DatabaseProvider == MixDatabaseProvider.SQLITE)
            {
                _connection = Activator.CreateInstance(connectionType) as IDbConnection;
                _connection!.ConnectionString = ConnectionString;
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
        public void CompleteTransaction()
        {
            if ((_isRoot || DatabaseProvider == MixDatabaseProvider.SQLITE) && _dbTransaction?.Connection != null)
            {
                _dbTransaction.Commit();
            }
        }
        public void Dispose()
        {
            if (_connection != null)
            {
                if ((_isRoot || DatabaseProvider == MixDatabaseProvider.SQLITE) && _dbTransaction?.Connection != null)
                {
                    _dbTransaction.Commit();
                }
                _connection.Close();
            }
        }
    }
}

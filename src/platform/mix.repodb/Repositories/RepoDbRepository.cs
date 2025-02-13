using Azure.Core;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Constant.Enums;
using Mix.Heart.Enums;
using Mix.Heart.Exceptions;
using Mix.Heart.Models;
using Mix.Heart.UnitOfWork;
using Mix.RepoDb.Helpers;
using Mix.RepoDb.Models;
using Mix.Shared.Models;
using MySqlConnector;
using Newtonsoft.Json.Linq;
using Npgsql;
using RepoDb;
using RepoDb.Enumerations;
using RepoDb.Interfaces;
using System.Data;

namespace Mix.RepoDb.Repositories
{
    public class RepoDbRepository: IDisposable
    {
        #region Properties
        private bool _isRoot;
        private IDbTransaction? _dbTransaction;
        private readonly AppSetting _settings;
        private IDbConnection _connection;
        private MixDatabaseProvider _databaseProvider;
        private MixdbTrace _trace;
        #endregion

        public RepoDbRepository(string connectionString, MixDatabaseProvider databaseProvider, AppSetting settings)
        {
            _settings = settings;
            InitializeRepoDb(connectionString, databaseProvider);
        }

        public void InitializeRepoDb(string connectionString, MixDatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider;
            CreateConnection(connectionString);
            InitializeRepoDb();
        }

        #region Methods

        #region Connection
        public void Dispose()
        {
            if (_connection != null)
            {
                if (_dbTransaction != null)
                {
                    try
                    {
                        if ((_isRoot || _databaseProvider == MixDatabaseProvider.SQLITE) && _dbTransaction?.Connection != null)
                        {
                            _dbTransaction.Commit();
                        }
                        _dbTransaction!.Dispose();
                        _dbTransaction = null;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }
                _connection.Close();
                _connection.Dispose();
            }
        }

        public void BeginTransaction()
        {
            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
                _dbTransaction = _connection.BeginTransaction();

            }
            if (_dbTransaction == null || _dbTransaction.Connection == null)
            {
                _dbTransaction = _connection.BeginTransaction();
            }
        }

        public void SetDbConnection(UnitOfWorkInfo dbUow)
        {
            dbUow.Begin();

            if (_databaseProvider == MixDatabaseProvider.SQLITE)
            {
                if (_connection != null)
                {
                    _connection.Close();
                    _connection.Dispose();
                }
            }
            _connection = dbUow.ActiveDbContext.Database.GetDbConnection();
            _dbTransaction = dbUow.ActiveTransaction.GetDbTransaction();
            _isRoot = false;
        }

        public IDbConnection? CreateConnection(string connectionString, bool isRoot = true, bool isRenew = false)
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                _trace = new();

                if (!isRenew && _connection != null && _connection.ConnectionString == connectionString)
                {
                    return _connection;
                }

                _isRoot = isRoot;
                var connectionType = GetDbConnectionType(_databaseProvider);

                _connection = (Activator.CreateInstance(connectionType) as IDbConnection)!;
                _connection.ConnectionString = connectionString;
            }
            return _connection;
        }
        static Type GetDbConnectionType(MixDatabaseProvider dbProvider)
        {
            return dbProvider switch
            {
                MixDatabaseProvider.SQLSERVER => typeof(SqlConnection),
                MixDatabaseProvider.MySQL => typeof(MySqlConnection),
                MixDatabaseProvider.PostgreSQL => typeof(NpgsqlConnection),
                MixDatabaseProvider.SQLITE => typeof(SqliteConnection),
                _ => typeof(SqliteConnection),
            };
        }
        private void InitializeRepoDb()
        {
            switch (_databaseProvider)
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

        public void CompleteTransaction()
        {
            if (_isRoot && _dbTransaction?.Connection != null)
            {
                _dbTransaction.Commit();
                _dbTransaction = null;
            }
        }

        public void RollbackTransaction()
        {
            if (_isRoot && _dbTransaction?.Connection != null)
            {
                _dbTransaction.Rollback();
            }
        }

        #endregion

        public async Task<int> ExecuteCommand(string commandSql, IDbTransaction? _dbTransaction = null, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            BeginTransaction();
            var result = await _connection.ExecuteNonQueryAsync(commandSql, transaction: _dbTransaction, cancellationToken: cancellationToken);
            CompleteTransaction();
            return result;
        }

        public async Task<PagingResponseModel<JObject>> GetPagingAsync(
            string tableName,
            IEnumerable<QueryField> queryFields,
            PagingRequestModel pagingRequest,
            MixConjunction conjunction,
            IEnumerable<Field>? selectFields = null,
            IStatementBuilder? builder = null,
            IDbTransaction? transaction = null,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (pagingRequest.SortByColumns == null)
            {
                throw new MixException(MixErrorStatus.Badrequest, $"{nameof(pagingRequest.SortByColumns)} must have value");
            }
            
            List<OrderField> sortByColumns = pagingRequest.SortByColumns
                .Select(m => new OrderField(m.FieldName, m.Direction == SortDirection.Asc ? Order.Ascending : Order.Descending))
                .ToList();
            builder = _databaseProvider == MixDatabaseProvider.PostgreSQL ? new OptimizedPostgresSqlStatementBuilder(true) : _connection.GetStatementBuilder();

            int pageSize = pagingRequest.PageSize ?? 100;

            var countCommandText = builder.CreateCount(tableName, new QueryGroup(queryFields, conjunction == MixConjunction.Or ? Conjunction.Or : Conjunction.And));
            var count = (long)await _connection.ExecuteScalarAsync(countCommandText, queryFields, transaction: transaction ?? _dbTransaction, cancellationToken: cancellationToken);

            var commandText = builder.CreateBatchQuery(tableName, selectFields, pagingRequest.PageIndex, pageSize, sortByColumns, new QueryGroup(queryFields, conjunction == MixConjunction.Or ? Conjunction.Or : Conjunction.And));
            var data = await _connection.ExecuteQueryAsync(commandText, queryFields, transaction: transaction ?? _dbTransaction, cancellationToken: cancellationToken);

            return new PagingResponseModel<JObject>()
            {
                Items = RepoDbHelper.ParseListJObject(data),
                PagingData = new()
                {
                    Page = pagingRequest.PageIndex + 1,
                    PageIndex = pagingRequest.PageIndex,
                    PageSize = pagingRequest.PageSize,
                    Total = count,
                    TotalPage = (int)Math.Ceiling((double)count / pageSize),
                    SortByColumns= pagingRequest.SortByColumns
                }
            };
        }

        public async Task<List<JObject>?> GetListByAsync(
            string tableName,
            List<QueryField> queryFields,
            string? fields = null,
            List<OrderField>? orderFields = null,
            IDbTransaction? transaction = null,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
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
            var data = await _connection.QueryAsync(tableName, queryFields, selectedFields, orderFields, transaction: transaction ?? _dbTransaction, cancellationToken: cancellationToken);

            return RepoDbHelper.ParseListJObject(data);
        }

        public async Task<List<JObject>?> GetAllAsync(
            string tableName,
            IDbTransaction? _dbTransaction = null,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var data = await _connection.QueryAllAsync(tableName, null, null, commandTimeout: _settings.CommandTimeout, transaction: _dbTransaction, cancellationToken: cancellationToken);
            return RepoDbHelper.ParseListJObject(data);
        }



        public async Task<JObject?> GetSingleAsync(
            string tableName,
            List<QueryField> queries,
            IEnumerable<Field>? selectFields = null,
            IDbTransaction? transaction = null,
            ITrace? trace = null,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var data = (await _connection.QueryAsync<dynamic>(
                tableName,
                queries,
                fields: selectFields,
                commandTimeout: _settings.CommandTimeout,
                transaction: transaction ?? _dbTransaction,
                cancellationToken: cancellationToken,
                trace: trace))?.SingleOrDefault();
            return RepoDbHelper.ParseJObject(data);
        }

        // Get

        public async Task<JObject?> GetSingleAsync(
            string tableName,
            QueryField idQuery,
            IEnumerable<Field>? selectFields = null,
             IDbTransaction? transaction = null,
            ITrace? trace = null,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var data = (await _connection.QueryAsync<dynamic>(
                tableName,
                idQuery,
                fields: selectFields,
                commandTimeout: _settings.CommandTimeout,
                transaction: transaction ?? _dbTransaction,
                trace: trace))?.SingleOrDefault();
            return RepoDbHelper.ParseJObject(data);
        }

        public async Task<object> InsertAsync(
            string tableName,
            Dictionary<string, object> dicObj,
            IDbTransaction? transaction = null,
            ITrace? trace = null,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var fields = dicObj!.Keys.Select(m => new Field(m)).ToList();
            var result = await _connection.InsertAsync(
                    tableName,
                    entity: dicObj,
                    fields: fields,
                    commandTimeout: _settings.CommandTimeout,
                    transaction: transaction ?? _dbTransaction,
                    trace: trace,
                    cancellationToken: cancellationToken);
            return result;
        }


        public async Task<int?> InsertManyAsync(
            string tableName,
            List<Dictionary<string, object>> entities,
            IDbTransaction? transaction = null,
            ITrace? trace = null,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (entities.Count == 0)
            {
                return default;
            }

            var fields = entities[0].Keys.Select(m => new Field(m)).ToList();

            var result = await _connection.InsertAllAsync(
                    tableName,
                    entities: entities,
                    fields: fields,
                    commandTimeout: _settings.CommandTimeout,
                    transaction: transaction ?? _dbTransaction,
                    trace: trace,
                    cancellationToken: cancellationToken);
            return result;
        }

        public async Task<int?> InsertManyAsync(
            string tableName,
            List<dynamic> entities,
            List<Field>? fields = null,
            IDbTransaction? transaction = null,
            ITrace? trace = null,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var result = await _connection.InsertAllAsync(
                    tableName,
                    entities: entities,
                    fields: fields,
                    commandTimeout: _settings.CommandTimeout,
                    transaction: transaction ?? _dbTransaction,
                    trace: trace,
                    cancellationToken: cancellationToken);
            return result;
        }

        public async Task<int?> UpdateManyAsync(
            string tableName,
            List<Dictionary<string, object>> entities,
            IDbTransaction? transaction = null,
            ITrace? trace = null,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (entities.Count == 0)
            {
                return default;
            }

            var fields = entities[0].Keys.Select(m => new Field(m)).ToList();

            var result = await _connection.UpdateAllAsync(
                    tableName,
                    entities: entities,
                    fields: fields,
                    commandTimeout: _settings.CommandTimeout,
                    transaction: transaction ?? _dbTransaction,
                    trace: trace,
                    cancellationToken: cancellationToken);
            return result;
        }

        public async Task<object?> UpdateAsync(
            string tableName,
            QueryField id,
            Dictionary<string, object> entity,
            IDbTransaction? transaction = null,
            ITrace? trace = null,
            IEnumerable<string>? fieldNames = default,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (Exists(tableName, id))
            {
                return await _connection.UpdateAsync(
                    tableName,
                    entity,
                    fields: fieldNames?.Select(m => new Field(m)),
                    commandTimeout: _settings.CommandTimeout,
                    trace: trace,
                    transaction: transaction ?? _dbTransaction,
                    cancellationToken: cancellationToken);
            }
            return null;
        }

        public bool Exists(string tableName, QueryField id)
        {
            return _connection.Exists(tableName, id, transaction: _dbTransaction);
        }

        public async Task<int> DeleteAsync(
            string tableName,
            QueryField idQuery,
            IDbTransaction? transaction = null,
            ITrace? trace = null,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (_connection.Exists(tableName, idQuery, transaction: transaction ?? _dbTransaction, trace: trace))
            {
                return await _connection.DeleteAsync(tableName, idQuery,
                    commandTimeout: _settings.CommandTimeout,
                    transaction: transaction ?? _dbTransaction,
                    trace: trace,
                    cancellationToken: cancellationToken);
            }
            return 0;
        }

        public async Task<int> DeleteAsync(
            string tableName,
            List<QueryField> queries,
            IDbTransaction? transaction = null,
            ITrace? trace = null,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (_connection.Exists(tableName, queries, transaction: transaction ?? _dbTransaction, trace: trace))
            {
                return await _connection.DeleteAsync(tableName, queries,
                    commandTimeout: _settings.CommandTimeout,
                    transaction: transaction ?? _dbTransaction,
                    trace: trace,
                    cancellationToken: cancellationToken);
            }
            return 0;
        }

        public Operation ParseSearchOperation(ExpressionMethod? searchMethod)
        {
            return searchMethod switch
            {
                ExpressionMethod.Like => Operation.Like,
                ExpressionMethod.Equal => Operation.Equal,
                ExpressionMethod.NotEqual => Operation.NotEqual,
                ExpressionMethod.LessThanOrEqual => Operation.LessThanOrEqual,
                ExpressionMethod.LessThan => Operation.LessThan,
                ExpressionMethod.GreaterThan => Operation.GreaterThan,
                ExpressionMethod.GreaterThanOrEqual => Operation.GreaterThanOrEqual,
                ExpressionMethod.In => Operation.In,
                _ => Operation.Equal
            };
        }

        public MixCompareOperator ParseMixCompareOperator(ExpressionMethod? searchMethod)
        {
            return searchMethod switch
            {
                ExpressionMethod.Like => MixCompareOperator.Like,
                ExpressionMethod.Equal => MixCompareOperator.Equal,
                ExpressionMethod.NotEqual => MixCompareOperator.NotEqual,
                ExpressionMethod.LessThanOrEqual => MixCompareOperator.LessThanOrEqual,
                ExpressionMethod.LessThan => MixCompareOperator.LessThan,
                ExpressionMethod.GreaterThan => MixCompareOperator.GreaterThan,
                ExpressionMethod.GreaterThanOrEqual => MixCompareOperator.GreaterThanOrEqual,
                ExpressionMethod.In => MixCompareOperator.InRange,
                _ => MixCompareOperator.Equal
            };
        }

        #endregion
    }
}

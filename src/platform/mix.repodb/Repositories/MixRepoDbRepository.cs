using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Mix.Constant.Constants;
using Mix.Database.Entities.Cms;
using Mix.Database.Services;
using Mix.Heart.Enums;
using Mix.Heart.Models;
using Mix.Heart.UnitOfWork;
using Mix.RepoDb.Models;
using Mix.Shared.Models;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
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

        UnitOfWorkInfo<MixCmsContext> _uow;
        private static string _connectionString;
        private static MixDatabaseProvider _databaseProvider;
        readonly DatabaseService _databaseService;
        private AppSetting _settings;
        private string _tableName;
        #endregion

        public MixRepoDbRepository(ICache cache, DatabaseService databaseService, UnitOfWorkInfo<MixCmsContext> uow)
        {
            Cache = cache;
            _settings = new AppSetting()
            {
                CacheItemExpiration = 10,
                CommandTimeout = 1000
            };
            _databaseService = databaseService;
            _connectionString = _databaseService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION);
            _databaseProvider = _databaseService.DatabaseProvider;
            InitializeRepoDb();
            _uow = uow;
        }

        #region Methods
        public void Init(string tableName)
        {
            _tableName = $"{MixConstants.CONST_MIXDB_PREFIX}{tableName}";
        }

        public async Task<PagingResponseModel<dynamic>> GetPagingAsync(JObject query, PagingRequestModel pagingRequest)
        {
            using (var connection = CreateConnection())
            {
                IEnumerable<QueryField> queryFields = ParseQuery(query);
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

        // Get

        public async Task<dynamic> GetAsync(int id)
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

        public async Task<object> InsertAsync(JObject entity,
            IDbTransaction? transaction = null)
        {
            using (var connection = CreateConnection())
            {
                List<Field> fields = ParseFields(entity);
                var obj = JsonConvert.DeserializeObject(entity.ToString());
                var result = await connection.InsertAsync(
                        _tableName,
                        entity: obj,
                        commandTimeout: _settings.CommandTimeout,
                        trace: Trace);

                return result;
            }
        }

        private List<Field> ParseFields(JObject entity)
        {
            var fields = new List<Field>();
            foreach (var prop in entity.Properties())
            {
                fields.Add(new Field(prop.Name));
            }
            return fields;
        }

        public async Task<object> UpdateAsync(object entity,
            IDbTransaction? transaction = null)
        {
            using (var connection = CreateConnection())
            {
                return await connection.UpdateAsync(_tableName, entity,
                    commandTimeout: _settings.CommandTimeout,
                    trace: Trace);
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            using (var connection = CreateConnection())
            {
                return await connection.DeleteAsync(_tableName, id,
                    commandTimeout: _settings.CommandTimeout,
                    trace: Trace);
            }
        }

        #endregion

        #region private
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


        private static void InitializeRepoDb()
        {
            switch (_databaseProvider)
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
            var connectionType = GetDbConnectionType(_databaseProvider);
            var connection = Activator.CreateInstance(connectionType) as IDbConnection;
            connection.ConnectionString = _connectionString;
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

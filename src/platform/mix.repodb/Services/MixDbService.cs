using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Mix.Constant.Constants;
using Mix.Constant.Enums;
using Mix.Database.Entities.Cms;
using Mix.Database.Services;
using Mix.Heart.Enums;
using Mix.Heart.Models;
using Mix.Heart.UnitOfWork;
using Mix.RepoDb.Models;
using Mix.RepoDb.ViewModels;
using Mix.Shared.Models;
using MySql.Data.MySqlClient;
using Newtonsoft.Json.Linq;
using Npgsql;
using RepoDb;
using RepoDb.Enumerations;
using RepoDb.Interfaces;
using System.Data;

namespace Mix.RepoDb.Services
{
    public class MixDbService
    {
        UnitOfWorkInfo<MixCmsContext> _uow;
        private static string _connectionString;
        private static MixDatabaseProvider _databaseProvider;
        readonly DatabaseService _databaseService;
        private AppSetting _settings;
        public MixDbService(ICache cache, DatabaseService databaseService, UnitOfWorkInfo<MixCmsContext> uow)
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

        /*** Properties ***/

        public ITrace Trace { get; }

        public ICache Cache { get; }

        /*** Methods ***/
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


        #region Migrate

        public async Task<bool> MigrateDatabase(int databaseId)
        {
            MixDatabaseViewModel database = await MixDatabaseViewModel.GetRepository(_uow).GetSingleAsync(databaseId);
            if (database!= null && database.Columns.Count > 0)
            {

                List<string> colSqls = new List<string>();
                foreach (var col in database.Columns)
                {
                    colSqls.Add(GenerateColumnSql(col));
                }
                string tableName = $"{MixConstants.CONST_MIXDB_PREFIX}{database.SystemName}";
                string commandText = $"DROP TABLE IF EXISTS {tableName}; " +
                    $"CREATE TABLE {tableName} " +
                    $"(id varchar(50) NOT NULL Unique, specificulture varchar(50), Status varchar(50), createdDateTime {GetColumnType(MixDataType.DateTime)}, " +
                    $" {string.Join(",", colSqls.ToArray())})";
                if (!string.IsNullOrEmpty(commandText))
                {
                    using (var ctx = new MixCmsContext())
                    {
                        await ctx.Database.ExecuteSqlRawAsync(commandText);
                        return true;
                    }
                }
                return false;
            }
            return false;
        }

        private static string GenerateColumnSql(MixDatabaseColumnViewModel col)
        {

            string colType = GetColumnType(col.DataType, col.ColumnConfigurations.MaxLength);
            string nullable = col.ColumnConfigurations.IsRequire ? "NOT NUll" : "NULL";
            return $"{col.SystemName} {colType} {nullable}";
        }

        private static string GetColumnType(MixDataType dataType, int? maxLength = null)
        {
            switch (dataType)
            {
                case MixDataType.DateTime:
                case MixDataType.Date:
                case MixDataType.Time:
                    return _databaseProvider switch
                    {
                        MixDatabaseProvider.PostgreSQL => "timestamp without time zone",
                        _ => "datetime"
                    };
                case MixDataType.Double:
                    return "float";
                case MixDataType.Integer:
                    return "int";
                case MixDataType.Reference: // JObject - { ref_table_name: "", children: [] }
                case MixDataType.Html:
                    return _databaseProvider switch
                    {
                        MixDatabaseProvider.SQLSERVER => "ntext",
                        _ => "text"
                    };
                case MixDataType.Duration:
                case MixDataType.Custom:
                case MixDataType.PhoneNumber:
                case MixDataType.Text:
                case MixDataType.MultilineText:
                case MixDataType.EmailAddress:
                case MixDataType.Password:
                case MixDataType.Url:
                case MixDataType.ImageUrl:
                case MixDataType.CreditCard:
                case MixDataType.PostalCode:
                case MixDataType.Upload:
                case MixDataType.Color:
                case MixDataType.Boolean:
                case MixDataType.Icon:
                case MixDataType.VideoYoutube:
                case MixDataType.TuiEditor:
                case MixDataType.QRCode:
                default:
                    return $"varchar({maxLength ?? 250})";
            }
        }

        #endregion
        // Get (Many)

        public IEnumerable<dynamic> GetAll(string _tableName, dynamic query, PagingRequestModel pagingData, string? cacheKey = null)
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

        public async Task<PagingResponseModel<dynamic>> GetAllAsync(string _tableName, JObject query, PagingRequestModel pagingRequest, string? cacheKey = null)
        {
            using (var connection = CreateConnection())
            {
                IEnumerable<QueryField> queryFields = ParseQuery(query);
                List<OrderField> orderFields = new List<OrderField>() {
                    new OrderField(pagingRequest.SortBy, pagingRequest.SortDirection == SortDirection.Asc ? Order.Ascending: Order.Descending)
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

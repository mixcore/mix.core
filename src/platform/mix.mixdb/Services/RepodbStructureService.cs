using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Mix.Constant.Constants;
using Mix.Constant.Enums;
using Mix.Database.Base;
using Mix.Database.Entities.Cms;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Enums;
using Mix.Heart.Exceptions;
using Mix.Heart.Extensions;
using Mix.Heart.Services;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Extensions;
using Mix.Lib.Interfaces;
using Mix.Mixdb.Entities;
using Mix.Mixdb.Helpers;
using Mix.Mixdb.Interfaces;
using Mix.Mixdb.ViewModels;
using Mix.RepoDb.Models;
using Mix.RepoDb.Repositories;
using Mix.RepoDb.ViewModels;
using Mix.Service.Interfaces;
using Mix.Service.Services;
using Mix.Shared.Dtos;
using RepoDb;
using RepoDb.Enumerations;
using RepoDb.Interfaces;
using System.Data;
using System.Dynamic;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Mix.Mixdb.Services
{
    public class RepodbStructureService : IMixdbStructureService
    {
        #region Properties
        public MixDatabaseProvider DbProvider { get; }
        protected IDatabaseConstants _databaseConstant;
        protected MixDatabaseProvider _databaseProvider;
        protected readonly UnitOfWorkInfo<MixCmsContext> _cmsUow;
        private readonly IConfiguration _configuration;
        protected readonly DatabaseService _databaseService;
        private readonly ICache _cache;
        private RepoDbRepository _repository;
        private RepoDbRepository _backupRepository;
        private readonly AppSetting _settings;
        private IMixMemoryCacheService _memoryCache;

        public RepodbStructureService(
                IConfiguration configuration,
                IHttpContextAccessor httpContextAccessor,
                UnitOfWorkInfo<MixCmsContext> uow,
                DatabaseService databaseService,
                IMixMemoryCacheService memoryCache,
                MixCacheService cacheService,
                IMixTenantService mixTenantService,
                ICache cache
           )
        {
            _cmsUow = uow;
            _configuration = configuration;
            _databaseService = databaseService;
            _databaseProvider = _databaseService.DatabaseProvider;
            _databaseConstant = _databaseProvider switch
            {
                MixDatabaseProvider.SQLSERVER => new SqlServerDatabaseConstants(),
                MixDatabaseProvider.MySQL => new MySqlDatabaseConstants(),
                MixDatabaseProvider.PostgreSQL => new PostgresDatabaseConstants(),
                MixDatabaseProvider.SQLITE => new SqliteDatabaseConstants(),
                MixDatabaseProvider.SCYLLADB => new CassandraDatabaseConstants(),
                _ => throw new NotImplementedException()
            };
            _memoryCache = memoryCache;
            _cache = cache;
            _settings = new AppSetting()
            {
                CacheItemExpiration = 10,
                CommandTimeout = 1000
            };
        }

        #endregion

        #region Implements
        public void Init(string connectionString, MixDatabaseProvider dbProvider)
        {
            _repository = new RepoDbRepository(connectionString, dbProvider, _settings);
            _repository.CreateConnection(connectionString, true, true);
        }
        public async Task<int> ExecuteCommand(string commandText)
        {
            var result = await _repository.ExecuteCommand(commandText);
            _repository.CompleteTransaction();
            return result;
        }

        public async Task AddColumn(MixDbDatabaseViewModel database, MixdbDatabaseColumnViewModel col, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (database == null)
            {
                throw new MixException(MixErrorStatus.Badrequest, $"Invalid table {col.MixDatabaseName}");
            }
            var fieldNameService = new FieldNameService(database.NamingConvention);
           
            var commandText = GenerateAddColumnSql(col, database.DatabaseProvider, database.Type, fieldNameService);
            await ExecuteCommand(commandText);
        }

        public async Task AlterColumn(MixDbDatabaseViewModel database, MixdbDatabaseColumnViewModel col, bool isDrop, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (database == null)
            {
                throw new MixException(MixErrorStatus.Badrequest, $"Invalid table {col.MixDatabaseName}");
            }
            if (database.MixDatabaseContext != null)
            {
                _repository.InitializeRepoDb(database.MixDatabaseContext.ConnectionString.Decrypt(_configuration.AesKey()), database.MixDatabaseContext.DatabaseProvider);
            }
            var fieldNameService = new FieldNameService(database.NamingConvention);
            var alterCommandText = isDrop
                ? $"{GenerateDropColumnSql(col)} {GenerateAddColumnSql(col, database.DatabaseProvider, database.Type, fieldNameService)}"
                : GenerateAlterColumnSql(col);
            await ExecuteCommand(alterCommandText);
        }

        public async Task DropColumn(MixDbDatabaseViewModel database, MixdbDatabaseColumnViewModel col, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (database == null)
            {
                throw new MixException(MixErrorStatus.Badrequest, $"Invalid table {col.MixDatabaseName}");
            }
            if (database.MixDatabaseContext != null)
            {
                _repository.InitializeRepoDb(
                    database.MixDatabaseContext.ConnectionString.Decrypt(_configuration.AesKey())
                    , database.MixDatabaseContext.DatabaseProvider);
            }
            var alterCommandText = GenerateDropColumnSql(col);
            await ExecuteCommand(alterCommandText);
        }

        public async Task Migrate(
            MixDbDatabaseViewModel database,
            MixDatabaseProvider databaseProvider,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var fieldNameService = new FieldNameService(database.NamingConvention);
            var colsSql = new List<string>();
            var tableName = database.SystemName;
            var databaseConstant = MixDbHelper.GetDatabaseConstant(databaseProvider);
            foreach (var col in database.Columns)
            {
                colsSql.Add(GenerateColumnSql(col, databaseProvider, database.Type, fieldNameService));
            }

            var commandText = GetMigrateTableSql(tableName, colsSql);
            if (!string.IsNullOrEmpty(commandText))
            {
                string sql = $"DROP TABLE IF EXISTS {_databaseConstant.BacktickOpen}{tableName}{_databaseConstant.BacktickClose};";
                await _cmsUow.DbContext.Database.ExecuteSqlRawAsync(sql);
                await _cmsUow.DbContext.Database.ExecuteSqlRawAsync($"{commandText}");
            }
        }

        #endregion

        #region Methods



        #region Repodb Helpers

        //private void InitBackupRepository(string databaseName)
        //{
        //    MixFileHelper.CreateFolderIfNotExist(MixFolders.BackupFolder);
        //    string cnn = $"Data Source={MixFolders.BackupFolder}/backup_{databaseName}.sqlite";
        //    using var ctx = new BackupDbContext(cnn);
        //    ctx.Database.EnsureCreated();
        //    ctx.Dispose();
        //    _backupRepository.Init(databaseName, MixDatabaseProvider.SQLITE, cnn);
        //}

        private async Task<List<QueryField>> BuildSearchQueryAsync(MixDbDatabaseViewModel database, MixDbDatabaseViewModel parentDb, SearchMixDbRequestDto request,
            FieldNameService fieldNameService)
        {
            var queries = new List<QueryField>();
            if (request.ObjParentId != null)
            {
                //var parentDb = await LoadMixDb(request.ParentName);
                if (database is null)
                {
                    return queries;
                }

                if (database.Type == MixDatabaseType.AdditionalData ||
                    database.Type == MixDatabaseType.GuidAdditionalData)
                {
                    queries.Add(new(fieldNameService.ParentId, request.ParentId));
                }
                else
                {
                    Expression<Func<MixDatabaseAssociation, bool>> predicate = m =>
                        m.ParentDatabaseName == request.ParentName
                        && m.ChildDatabaseName == database.SystemName;
                    predicate = predicate.AndAlsoIf(parentDb.Type == MixDatabaseType.GuidService,
                        m => m.GuidParentId == (Guid)request.ObjParentId);
                    predicate = predicate.AndAlsoIf(parentDb.Type != MixDatabaseType.GuidService,
                        m => m.ParentId == (int)request.ObjParentId);

                    var childIdsQuery = _cmsUow.DbContext.MixDatabaseAssociation
                        .Where(predicate);
                    if (database.Type == MixDatabaseType.GuidService)
                    {
                        queries.Add(new(fieldNameService.Id, Operation.In,
                            childIdsQuery.Select(m => m.GuidChildId).ToList()));
                    }
                    else
                    {
                        queries.Add(new(fieldNameService.Id, Operation.In,
                            childIdsQuery.Select(m => m.ChildId).ToList()));
                    }
                }
            }

            if (request.Queries != null)
            {
                foreach (var query in request.Queries)
                {
                    Operation op = ParseOperator(query.CompareOperator);
                    queries.Add(new(query.FieldName, op, query.Value));
                }
            }

            return queries;
        }

        private Operation ParseOperator(MixCompareOperator? compareOperator)
        {
            switch (compareOperator)
            {
                case MixCompareOperator.Equal:
                    return Operation.Equal;
                case MixCompareOperator.Like:
                    return Operation.Like;
                case MixCompareOperator.NotEqual:
                    return Operation.NotEqual;
                case MixCompareOperator.Contain:
                    return Operation.In;
                case MixCompareOperator.NotContain:
                    return Operation.NotIn;
                case MixCompareOperator.InRange:
                    return Operation.In;
                case MixCompareOperator.NotInRange:
                    return Operation.NotIn;
                case MixCompareOperator.GreaterThanOrEqual:
                    return Operation.GreaterThanOrEqual;
                case MixCompareOperator.GreaterThan:
                    return Operation.GreaterThan;
                case MixCompareOperator.LessThanOrEqual:
                    return Operation.LessThanOrEqual;
                case MixCompareOperator.LessThan:
                    return Operation.LessThan;
                default:
                    return Operation.Equal;
            }
        }

        protected virtual Task SwitchDbContext(MixDatabaseContextReadViewModel dbContext)
        {
            if (dbContext == null)
            {
                throw new MixException(MixErrorStatus.Badrequest, $"Invalid MixDatabaseContext");
            }

            _databaseProvider = dbContext.DatabaseProvider;
            _databaseConstant = MixDbHelper.GetDatabaseConstant(dbContext.DatabaseProvider);
            _repository = new RepoDbRepository(
                dbContext.ConnectionString.Decrypt(_configuration.AesKey()), 
                dbContext.DatabaseProvider, new AppSetting()
            {
                CacheItemExpiration = 10,
                CommandTimeout = 1000
            });
            return Task.CompletedTask;
        }

        #endregion

        #endregion

        #region Private

        private string GetInsertQuery(ExpandoObject obj, List<string> selectMembers)
        {
            var result = obj.ToList();
            List<string> values = new();
            foreach (var col in selectMembers)
            {
                if (obj.Any(m => m.Key == col))
                {
                    var val = obj.First(m => m.Key == col).Value;
                    if (val != null)
                    {
                        if (val is string)
                        {
                            values.Add($"'{val.ToString()!.Replace("'", "\\'")}'");
                        }
                        else
                        {
                            values.Add($"'{val}'");
                        }

                        continue;
                    }
                }

                values.Add("NULL");
            }

            return $"({string.Join(',', values)})";
        }


        private string GetMigrateTableSql(string tableName, List<string> colsSql)
        {
            return $"CREATE TABLE {_databaseConstant.BacktickOpen}{tableName}{_databaseConstant.BacktickClose} (" +
                   //$"({_databaseConstant.BacktickOpen}{fieldNameService.Id}{_databaseConstant.BacktickClose} {GetIdSyntax(databaseProvider, dbType)}, " +
                   //$"{_databaseConstant.BacktickOpen}{fieldNameService.CreatedDateTime}{_databaseConstant.BacktickClose} {GetColumnType(MixDataType.DateTime)}, " +
                   //$"{_databaseConstant.BacktickOpen}{fieldNameService.LastModified}{_databaseConstant.BacktickClose} {GetColumnType(MixDataType.DateTime)} NULL, " +
                   //$"{_databaseConstant.BacktickOpen}{fieldNameService.TenantId}{_databaseConstant.BacktickClose} {GetColumnType(MixDataType.Integer)} NULL, " +
                   //$"{_databaseConstant.BacktickOpen}{fieldNameService.CreatedBy}{_databaseConstant.BacktickClose} {GetColumnType(MixDataType.String)} NULL, " +
                   //$"{_databaseConstant.BacktickOpen}{fieldNameService.ModifiedBy}{_databaseConstant.BacktickClose} {GetColumnType(MixDataType.String)} NULL, " +
                   //$"{_databaseConstant.BacktickOpen}{fieldNameService.Priority}{_databaseConstant.BacktickClose} {GetColumnType(MixDataType.Integer)} NOT NULL, " +
                   //$"{_databaseConstant.BacktickOpen}{fieldNameService.Status}{_databaseConstant.BacktickClose} {GetColumnType(MixDataType.String)} NOT NULL, " +
                   //$"{_databaseConstant.BacktickOpen}{fieldNameService.IsDeleted}{_databaseConstant.BacktickClose} {GetColumnType(MixDataType.Boolean)} NOT NULL, " +
                   $" {string.Join(",", colsSql.ToArray())})";
        }

        private string GetIdSyntax(MixDatabaseProvider databaseProvider, MixDatabaseType dbType)
        {
            if (dbType == MixDatabaseType.GuidService)
            {
                return
                    $"{_databaseConstant.BacktickOpen}{_databaseConstant.Guid}{_databaseConstant.BacktickClose} PRIMARY KEY";
            }

            return databaseProvider switch
            {
                MixDatabaseProvider.SQLSERVER => $"{GetColumnType(MixDataType.Integer)} IDENTITY(1,1) PRIMARY KEY",
                MixDatabaseProvider.SQLITE => $"integer PRIMARY KEY AUTOINCREMENT",
                MixDatabaseProvider.PostgreSQL => "SERIAL PRIMARY KEY",
                MixDatabaseProvider.MySQL =>
                    $"{GetColumnType(MixDataType.Integer)} NOT NULL AUTO_INCREMENT PRIMARY KEY",
                _ => string.Empty
            };
        }

        private string GenerateColumnSql(MixdbDatabaseColumnViewModel col, MixDatabaseProvider databaseProvider,
            MixDatabaseType dbType, FieldNameService fieldNameService)
        {
            if (string.Equals(col.SystemName, "id", StringComparison.OrdinalIgnoreCase))
            {
                return $"{_databaseConstant.BacktickOpen}id{_databaseConstant.BacktickClose} {GetIdSyntax(databaseProvider, dbType)}";
            }
            string colType = GetColumnType(col.DataType, col.ColumnConfigurations.MaxLength);
            string nullable = col.ColumnConfigurations.IsRequire ? "NOT NUll" : "NULL";
            string unique = col.ColumnConfigurations.IsUnique ? "Unique" : "";
            string defaultValue = !IsLongTextColumn(col) && !string.IsNullOrEmpty(col.DefaultValue)
                ? $"DEFAULT '{@col.DefaultValue}'"
                : string.Empty;
            return
                $"{_databaseConstant.BacktickOpen}{col.SystemName}{_databaseConstant.BacktickClose} {colType} {nullable} {unique} {defaultValue}";
        }

        private string GenerateAddColumnSql(MixdbDatabaseColumnViewModel col, MixDatabaseProvider databaseProvider, MixDatabaseType type, FieldNameService fieldNameService)
        {
            return
                $"ALTER TABLE {_databaseConstant.BacktickOpen}{col.MixDatabaseName}{_databaseConstant.BacktickClose}" +
                $" ADD {GenerateColumnSql(col, databaseProvider, type, fieldNameService)};";
        }

        private string GenerateDropColumnSql(MixdbDatabaseColumnViewModel col)
        {
            return
                $"ALTER TABLE {_databaseConstant.BacktickOpen}{col.MixDatabaseName}{_databaseConstant.BacktickClose}" +
                $" DROP COLUMN IF EXISTS {_databaseConstant.BacktickOpen}{col.SystemName}{_databaseConstant.BacktickClose};";
        }

        private string GenerateAlterColumnSql(MixdbDatabaseColumnViewModel col)
        {
            string colType = GetColumnType(col.DataType, col.ColumnConfigurations.MaxLength);
            string colName = $"{_databaseConstant.BacktickOpen}{col.SystemName}{_databaseConstant.BacktickClose}";
            string tableName =
                $"{_databaseConstant.BacktickOpen}{col.MixDatabaseName}{_databaseConstant.BacktickClose}";
            string uniqueKey =
                $"{_databaseConstant.BacktickOpen}{col.MixDatabaseName}_{col.SystemName}_key{_databaseConstant.BacktickClose}";
            string defaultValue = !IsLongTextColumn(col) && !string.IsNullOrEmpty(col.DefaultValue)
                ? $"DEFAULT '{@col.DefaultValue}'"
                : string.Empty;
            string alterTable = $"ALTER TABLE {tableName} ";
            var result = alterTable +
                         $" ALTER COLUMN {colName} TYPE {colType} USING {colName}::{colType};"
                         + alterTable
                         + $" ALTER COLUMN {colName} DROP NOT NULL;";

            if (col.ColumnConfigurations.IsRequire)
            {
                result += alterTable
                          + $" ALTER COLUMN {colName} SET NOT NULL;";
            }

            if (!string.IsNullOrEmpty(defaultValue))
            {
                result += alterTable
                          + $" ALTER COLUMN {colName} SET {defaultValue};";
            }

            result += alterTable
                      + $" DROP CONSTRAINT IF EXISTS {uniqueKey};";
            if (col.ColumnConfigurations.IsUnique)
            {
                result += alterTable
                          + $" ADD CONSTRAINT {uniqueKey} UNIQUE ({colName});";
            }

            return result;
        }

        private bool IsLongTextColumn(MixdbDatabaseColumnViewModel col)
        {
            return col.DataType == MixDataType.Text
                   | col.DataType == MixDataType.Array
                   | col.DataType == MixDataType.ArrayMedia
                   | col.DataType == MixDataType.ArrayRadio
                   | col.DataType == MixDataType.Html
                   | col.DataType == MixDataType.Json
                   | col.DataType == MixDataType.TuiEditor;
        }

        private string GetColumnType(MixDataType dataType, int? maxLength = null)
        {
            switch (dataType)
            {
                case MixDataType.Date:
                    return _databaseConstant.Date;
                case MixDataType.DateTime:
                case MixDataType.Time:
                    return _databaseConstant.DateTime;
                case MixDataType.Double:
                    return "float";
                case MixDataType.Reference:
                case MixDataType.Integer:
                    return _databaseConstant.Integer;
                case MixDataType.Long:
                    return _databaseConstant.Long;
                case MixDataType.Guid:
                    return _databaseConstant.Guid;
                case MixDataType.Html:
                    return _databaseConstant.Text;
                case MixDataType.Boolean:
                    return _databaseConstant.Boolean;
                case MixDataType.Json:
                case MixDataType.Array:
                case MixDataType.ArrayMedia:
                case MixDataType.ArrayRadio:
                case MixDataType.Text:
                    return _databaseConstant.Text;
                case MixDataType.Duration:
                case MixDataType.Custom:
                case MixDataType.PhoneNumber:
                case MixDataType.String:
                case MixDataType.MultilineText:
                case MixDataType.EmailAddress:
                case MixDataType.Password:
                case MixDataType.Url:
                case MixDataType.ImageUrl:
                case MixDataType.CreditCard:
                case MixDataType.PostalCode:
                case MixDataType.Upload:
                case MixDataType.Color:
                case MixDataType.Icon:
                case MixDataType.VideoYoutube:
                case MixDataType.TuiEditor:
                case MixDataType.QRCode:
                case MixDataType.BarCode:
                default:
                    return $"{_databaseConstant.NString}({maxLength ?? 250})";
            }
        }

        public void Dispose()
        {
            _repository.Dispose();
            _backupRepository.Dispose();
            _cmsUow.Dispose();
        }



        #endregion
    }
}

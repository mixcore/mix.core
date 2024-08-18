using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Mix.Constant.Constants;
using Mix.Constant.Enums;
using Mix.Database.Base;
using Mix.Database.Entities.Cms;
using Mix.Database.Services;
using Mix.Heart.Constants;
using Mix.Heart.Enums;
using Mix.Heart.Exceptions;
using Mix.Heart.Extensions;
using Mix.Heart.Helpers;
using Mix.Heart.Models;
using Mix.Heart.Services;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Interfaces;
using Mix.RepoDb.Dtos;
using Mix.RepoDb.Entities;
using Mix.RepoDb.Interfaces;
using Mix.RepoDb.Repositories;
using Mix.RepoDb.ViewModels;
using Mix.Service.Interfaces;
using Mix.Service.Services;
using Mix.Shared.Dtos;
using Mix.Shared.Models;
using Mix.Shared.Services;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json.Linq;
using Npgsql;
using RepoDb;
using RepoDb.Enumerations;
using RepoDb.Interfaces;
using System.Dynamic;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using ZstdSharp.Unsafe;

namespace Mix.RepoDb.Services
{
    public class MixDbService : TenantServiceBase, IMixDbService, IDisposable
    {
        #region Properties
        private IDatabaseConstants _databaseConstant;
        private MixDatabaseProvider _databaseProvider;
        private readonly ICache _cache;
        private MixRepoDbRepository _repository;
        private MixRepoDbRepository _backupRepository;
        private IMixMemoryCacheService _memoryCache;


        private readonly UnitOfWorkInfo<MixCmsContext> _cmsUow;
        private readonly DatabaseService _databaseService;

        #endregion

        public MixDbService(
            IHttpContextAccessor httpContextAccessor,
            UnitOfWorkInfo<MixCmsContext> uow,
            DatabaseService databaseService,
            MixRepoDbRepository repository,
            ICache cache,
            IMixMemoryCacheService memoryCache,
            MixCacheService cacheService,
            IMixTenantService mixTenantService)
            : base(httpContextAccessor, cacheService, mixTenantService)
        {
            _cache = cache;
            _cmsUow = uow;
            _databaseService = databaseService;
            _repository = repository;
            _backupRepository = new MixRepoDbRepository(cache, databaseService, uow);
            _databaseProvider = _databaseService.DatabaseProvider;
            _databaseConstant = _databaseProvider switch
            {
                MixDatabaseProvider.SQLSERVER => new SqlServerDatabaseConstants(),
                MixDatabaseProvider.MySQL => new MySqlDatabaseConstants(),
                MixDatabaseProvider.PostgreSQL => new PostgresDatabaseConstants(),
                MixDatabaseProvider.SQLITE => new SqliteDatabaseConstants(),
                _ => throw new NotImplementedException()
            };
            _memoryCache = memoryCache;
        }

        #region Methods

        #region Implements

        // TODO: check why need to restart application to load new database schema for Repo Db Context !important
        public async Task<bool> AddColumn(RepoDbMixDatabaseColumnViewModel col)
        {
            var database = await GetMixDatabase(col.MixDatabaseName);
            if (database == null)
            {
                throw new MixException(MixErrorStatus.Badrequest, $"Invalid table {col.MixDatabaseName}");
            }
            if (database.MixDatabaseContextId.HasValue)
            {
                await SwitchDbContext(database.MixDatabaseContext);
            }
            var commandText = GenerateAddColumnSql(col);
            var result = await _repository.ExecuteCommand(commandText);
            _repository.CompleteTransaction();
            return result >= 0;
        }

        public async Task<bool> DropColumn(RepoDbMixDatabaseColumnViewModel col)
        {
            var database = await GetMixDatabase(col.MixDatabaseName);
            if (database == null)
            {
                throw new MixException(MixErrorStatus.Badrequest, $"Invalid table {col.MixDatabaseName}");
            }
            if (database.MixDatabaseContextId.HasValue)
            {
                await SwitchDbContext(database.MixDatabaseContext);
            }
            var commandText = GenerateDropColumnSql(col);
            var result = await _repository.ExecuteCommand(commandText);
            _repository.CompleteTransaction();
            return result >= 0;
        }

        public async Task<bool> AlterColumn(AlterColumnDto colDto)
        {
            var database = await GetMixDatabase(colDto.MixDatabaseName);
            if (database == null)
            {
                throw new MixException(MixErrorStatus.Badrequest, $"Invalid table {colDto.MixDatabaseName}");
            }
            var col = new RepoDbMixDatabaseColumnViewModel(colDto);
            if (database.MixDatabaseContextId.HasValue)
            {
                await SwitchDbContext(database.MixDatabaseContext);
            }
            var dropCommandText = GenerateAlterColumnSql(col);
            var result = await _repository.ExecuteCommand(dropCommandText);
            _repository.CompleteTransaction();
            return result >= 0;
        }

        public async Task<bool> MigrateDatabase(RepoDbMixDatabaseViewModel database)
        {
            if (database.MixDatabaseContextId.HasValue)
            {
                if (database.MixDatabaseContextId.HasValue)
                {
                    await SwitchDbContext(database.MixDatabaseContext);
                }

                if (database is { Columns.Count: > 0 })
                {
                    await Migrate(database, database.MixDatabaseContext.DatabaseProvider, _repository);
                    _repository.CompleteTransaction();
                    return true;
                }
                return false;
            }

            if (database is { Columns.Count: > 0 })
            {
                await Migrate(database, _databaseProvider, _repository);
                _repository.CompleteTransaction();
                return true;
            }
            return false;
        }
        public async Task<bool> BackupDatabase(RepoDbMixDatabaseViewModel database, CancellationToken cancellationToken = default)
        {
            if (database != null)
            {
                var data = await GetCurrentData(database, cancellationToken);
                var fieldNameService = new FieldNameService(database.NamingConvention);
                if (data is { Count: > 0 })
                {
                    InitBackupRepository(database.SystemName);
                    await Migrate(database, _backupRepository.DatabaseProvider, _backupRepository);
                    foreach (var item in data)
                    {
                        GetMembers(item, database.Columns.Select(c => c.SystemName).ToList(), fieldNameService);
                    }
                    var result = await _backupRepository.InsertManyAsync(data);
                    return result > 0;
                }
                return true;
            }
            return false;
        }

        public async Task<bool> RestoreFromLocal(RepoDbMixDatabaseViewModel database, FieldNameService fieldNameService)
        {
            try
            {
                if (File.Exists($"{MixFolders.BackupFolder}/backup_{database.SystemName}.sqlite"))
                {
                    InitBackupRepository(database.SystemName);
                    var data = await _backupRepository.GetAllAsync();
                    if (data is { Count: > 0 })
                    {
                        var dbColumns = database.Columns.Select(c => c.SystemName).Union(fieldNameService.GetAllFieldName()).ToList();
                        string insertQuery = $"INSERT INTO {_databaseConstant.BacktickOpen}{database.SystemName}{_databaseConstant.BacktickClose} ({string.Join(',', dbColumns.Select(m => $"{_databaseConstant.BacktickOpen}{m}{_databaseConstant.BacktickClose}"))}) VALUES ";
                        List<string> queries = new();
                        foreach (var item in data)
                        {
                            queries.Add(GetInsertQuery(item, dbColumns));
                        }
                        insertQuery += string.Join(',', queries);
                        if (database.MixDatabaseContextId.HasValue)
                        {
                            _repository.Init(database.SystemName, database.MixDatabaseContext.DatabaseProvider, database.MixDatabaseContext.DecryptedConnectionString);
                        }
                        else
                        {
                            _repository.InitTableName(database.SystemName);
                        }
                        var result = await _repository.ExecuteCommand(insertQuery);
                        return result >= 0;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.ServerError, ex);
            }
        }
        public async Task<bool> MigrateSystemDatabases(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var strMixDbs = MixFileHelper.GetFile(
                    "system-databases", MixFileExtensions.Json, MixFolders.JsonDataFolder);
            var obj = JObject.Parse(strMixDbs.Content);
            var databases = obj.Value<JArray>("databases")?.ToObject<List<MixDatabase>>();
            var columns = obj.Value<JArray>("columns")?.ToObject<List<MixDatabaseColumn>>();
            if (databases != null)
            {
                foreach (var database in databases)
                {
                    if (!_cmsUow.DbContext.MixDatabase.Any(m => m.SystemName == database.SystemName))
                    {

                        RepoDbMixDatabaseViewModel currentDb = new(database, _cmsUow);
                        currentDb.Id = 0;
                        currentDb.MixTenantId = CurrentTenant?.Id ?? 1;
                        currentDb.CreatedDateTime = DateTime.UtcNow;
                        currentDb.Columns = new();

                        if (columns is not null)
                        {
                            var cols = columns.Where(c => c.MixDatabaseName == database.SystemName).ToList();
                            foreach (var col in cols)
                            {
                                col.Id = 0;
                                currentDb.Columns.Add(new(col, _cmsUow));
                            }
                        }

                        await currentDb.SaveAsync(cancellationToken);

                        if (currentDb is { Columns.Count: > 0 })
                        {
                            await Migrate(currentDb, _databaseService.DatabaseProvider, _repository);

                        }
                    }
                }
                return true;

            }
            return false;
        }

        public async Task<bool> MigrateInitNewDbContextDatabases(MixDatabaseContext dbContext, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                var cnn = AesEncryptionHelper.DecryptString(dbContext.ConnectionString, GlobalConfigService.Instance.AesKey);
                var strMixDbs = MixFileHelper.GetFile(
                        "init-new-dbcontext-databases", MixFileExtensions.Json, MixFolders.JsonDataFolder);
                var obj = JObject.Parse(strMixDbs.Content);
                var databases = obj.Value<JArray>("databases")?.ToObject<List<MixDatabase>>();
                var columns = obj.Value<JArray>("columns")?.ToObject<List<MixDatabaseColumn>>();
                if (databases != null)
                {

                    foreach (var database in databases)
                    {
                        string newDbName = dbContext.NamingConvention == MixDatabaseNamingConvention.SnakeCase
                            ? $"{dbContext.SystemName}_{database.DisplayName.ToColumnName(false)}"
                            : $"{dbContext.SystemName.ToTitleCase()}{database.DisplayName.ToColumnName(true)}";
                        _repository.Init(newDbName, dbContext.DatabaseProvider, cnn);
                        var currentDb = await RepoDbMixDatabaseViewModel.GetRepository(_cmsUow, CacheService)
                                            .GetSingleAsync(m => m.SystemName == newDbName);
                        if (currentDb == null)
                        {

                            currentDb = new(database, _cmsUow);
                            currentDb.Id = 0;
                            currentDb.NamingConvention = dbContext.NamingConvention;
                            currentDb.SystemName = newDbName;
                            currentDb.MixTenantId = CurrentTenant?.Id ?? 1;
                            currentDb.MixDatabaseContextId = dbContext.Id;
                            currentDb.CreatedDateTime = DateTime.UtcNow;
                            currentDb.Columns = new();

                            if (columns is not null)
                            {
                                var cols = columns.Where(c => c.MixDatabaseName == database.SystemName).ToList();
                                foreach (var col in cols)
                                {
                                    string newColName = dbContext.NamingConvention == MixDatabaseNamingConvention.SnakeCase
                                    ? col.DisplayName.ToColumnName(false)
                                    : col.DisplayName.ToColumnName(true);
                                    col.Id = 0;
                                    col.SystemName = newColName;
                                    col.MixDatabaseName = newDbName;
                                    currentDb.Columns.Add(new(col, _cmsUow));
                                }
                            }

                            await currentDb.SaveAsync(cancellationToken);

                        }
                        if (currentDb is { Columns.Count: > 0 })
                        {
                            await Migrate(currentDb, dbContext.DatabaseProvider, _repository);

                        }
                    }
                    return true;

                }
                return false;
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.Badrequest, ex);
            }
        }
        #endregion

        #region Helper

        private async Task<RepoDbMixDatabaseViewModel> GetMixDatabase(string tableName)
        {
            var result = await _memoryCache.TryGetValueAsync(
                tableName,
                cache =>
                {
                    cache.SlidingExpiration = TimeSpan.FromSeconds(20);
                    return RepoDbMixDatabaseViewModel.GetRepository(_cmsUow, CacheService).GetSingleAsync(m => m.SystemName == tableName);
                }
                );
            if (result == null)
            {
                throw new MixException(MixErrorStatus.Badrequest, "Invalid table name");
            }
            return result;
        }
        private string GetRelationshipDbName(RepoDbMixDatabaseViewModel mixDb)
        {
            string relName = mixDb.NamingConvention == MixDatabaseNamingConvention.SnakeCase
                            ? MixDatabaseNames.DATA_RELATIONSHIP_SNAKE_CASE
                            : MixDatabaseNames.DATA_RELATIONSHIP_TITLE_CASE;
            return mixDb.MixDatabaseContextId.HasValue
                        ? $"{mixDb.MixDatabaseContext.SystemName}_{relName}"
                        : MixDatabaseNames.SYSTEM_DATA_RELATIONSHIP;
        }
        private async Task<List<QueryField>> BuildSearchQueryAsync(string tableName, SearchMixDbRequestDto request, FieldNameService fieldNameService)
        {
            var queries = BuildSearchPredicate(request, fieldNameService);
            if (request.ParentId.HasValue)
            {
                var database = await GetMixDatabase(tableName);
                if (database is null)
                {
                    return queries;
                }

                if (database.Type == MixDatabaseType.AdditionalData || database.Type == MixDatabaseType.GuidAdditionalData)
                {
                    queries.Add(new(fieldNameService.ParentId, request.ParentId));
                }
                else
                {
                    var allowsIds = _cmsUow.DbContext.MixDatabaseAssociation
                            .Where(m => m.ParentDatabaseName == request.ParentName && m.ParentId == request.ParentId.Value && m.ChildDatabaseName == tableName)
                            .Select(m => m.ChildId).ToList();
                    queries.Add(new(fieldNameService.Id, Operation.In, allowsIds));
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

        private List<QueryField> BuildSearchPredicate(SearchMixDbRequestDto req, FieldNameService fieldNameService)
        {
            var queries = new List<QueryField>()
            {
                new QueryField(fieldNameService.TenantId, CurrentTenant.Id)
            };
            if (!string.IsNullOrEmpty(req.SearchColumns) && !string.IsNullOrEmpty(req.Keyword))
            {
                var searchColumns = req.SearchColumns.Replace(" ", string.Empty).Split(',');
                var operation = ParseSearchOperation(req.SearchMethod);
                var keyword = ParseSearchKeyword(req.SearchMethod, req.Keyword);

                foreach (var item in searchColumns)
                {
                    QueryField field = new QueryField(item, operation, keyword);
                    queries.Add(field);
                }
            }
            return queries;
        }

        private object ParseSearchKeyword(MixCompareOperator? searchMethod, string keyword)
        {
            return searchMethod switch
            {
                MixCompareOperator.Like => $"%{keyword}%",
                _ => keyword
            };
        }

        private Operation ParseOperator(MixCompareOperator compareOperator)
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

        private Operation ParseSearchOperation(MixCompareOperator? searchMethod)
        {
            return searchMethod switch
            {
                MixCompareOperator.Like => Operation.Like,
                MixCompareOperator.Equal => Operation.Equal,
                MixCompareOperator.NotEqual => Operation.NotEqual,
                MixCompareOperator.LessThanOrEqual => Operation.LessThanOrEqual,
                MixCompareOperator.LessThan => Operation.LessThan,
                MixCompareOperator.GreaterThan => Operation.GreaterThan,
                MixCompareOperator.GreaterThanOrEqual => Operation.GreaterThanOrEqual,
                MixCompareOperator.InRange => Operation.In,
                MixCompareOperator.NotInRange => Operation.NotIn,
                _ => Operation.Equal
            };
        }


        #endregion
        private async Task SwitchDbContext(MixDatabaseContextReadViewModel dbContext)
        {
            if (dbContext == null)
            {
                throw new MixException(MixErrorStatus.Badrequest, $"Invalid MixDatabaseContext");
            }
            _databaseProvider = dbContext.DatabaseProvider;
            _databaseConstant = _databaseProvider switch
            {
                MixDatabaseProvider.SQLSERVER => new SqlServerDatabaseConstants(),
                MixDatabaseProvider.MySQL => new MySqlDatabaseConstants(),
                MixDatabaseProvider.PostgreSQL => new PostgresDatabaseConstants(),
                MixDatabaseProvider.SQLITE => new SqliteDatabaseConstants(),
                _ => throw new NotImplementedException()
            };
            _repository = new MixRepoDbRepository(_cache, dbContext.DatabaseProvider, dbContext.DecryptedConnectionString, _cmsUow);
            return Task.CompletedTask;
        }

        // Only run after init CMS success
        // TODO: Add version to systemDatabases and update if have newer version


        #endregion

        #region Private


        private void InitBackupRepository(string databaseName)
        {
            MixFileHelper.CreateFolderIfNotExist(MixFolders.BackupFolder);
            string cnn = $"Data Source={MixFolders.BackupFolder}/backup_{databaseName}.sqlite";
            using var ctx = new BackupDbContext(cnn);
            ctx.Database.EnsureCreated();
            ctx.Dispose();
            _backupRepository.Init(databaseName, MixDatabaseProvider.SQLITE, cnn);

        }


        private void GetMembers(ExpandoObject obj, List<string> selectMembers, FieldNameService fieldNameService)
        {
            var result = obj.ToList();
            foreach (KeyValuePair<string, object?> kvp in result)
            {
                if (fieldNameService.GetAllFieldName().All(m => m != kvp.Key) && selectMembers.All(m => m != kvp.Key))
                {
                    obj!.Remove(kvp.Key, out _);
                }
            }
        }
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

        private async Task<List<dynamic>?> GetCurrentData(RepoDbMixDatabaseViewModel database, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (database.MixDatabaseContextId.HasValue)
            {
                _repository.Init(database.SystemName, database.MixDatabaseContext.DatabaseProvider, database.MixDatabaseContext.DecryptedConnectionString);
            }
            else
            {
                _repository.InitTableName(database.SystemName);
            }
            return await _repository.GetAllAsync(cancellationToken);
        }

        private async Task<bool> Migrate(RepoDbMixDatabaseViewModel database,
            MixDatabaseProvider databaseProvider,
            MixRepoDbRepository repo)
        {
            var fieldNameService = new FieldNameService(database.NamingConvention);
            var colsSql = new List<string>();
            var tableName = database.SystemName;
            _databaseConstant = databaseProvider switch
            {
                MixDatabaseProvider.SQLSERVER => new SqlServerDatabaseConstants(),
                MixDatabaseProvider.MySQL => new MySqlDatabaseConstants(),
                MixDatabaseProvider.PostgreSQL => new PostgresDatabaseConstants(),
                MixDatabaseProvider.SQLITE => new SqliteDatabaseConstants(),
                _ => throw new NotImplementedException()
            };
            foreach (var col in database.Columns)
            {
                colsSql.Add(GenerateColumnSql(col));
            }

            var commandText = GetMigrateTableSql(tableName, databaseProvider, colsSql, fieldNameService);
            if (!string.IsNullOrEmpty(commandText))
            {
                await repo.ExecuteCommand($"DROP TABLE IF EXISTS {_databaseConstant.BacktickOpen}{tableName}{_databaseConstant.BacktickClose};");
                var result = await repo.ExecuteCommand(commandText);
                return result >= 0;
            }

            return false;
        }

        private string GetMigrateTableSql(string tableName, MixDatabaseProvider databaseProvider, List<string> colsSql, FieldNameService fieldNameService)
        {
            return $"CREATE TABLE {_databaseConstant.BacktickOpen}{tableName}{_databaseConstant.BacktickClose} " +
                $"({_databaseConstant.BacktickOpen}{fieldNameService.Id}{_databaseConstant.BacktickClose} {GetAutoIncreaseIdSyntax(databaseProvider)}, " +
                $"{_databaseConstant.BacktickOpen}{fieldNameService.CreatedDateTime}{_databaseConstant.BacktickClose} {GetColumnType(MixDataType.DateTime)}, " +
                $"{_databaseConstant.BacktickOpen}{fieldNameService.LastModified}{_databaseConstant.BacktickClose} {GetColumnType(MixDataType.DateTime)} NULL, " +
                $"{_databaseConstant.BacktickOpen}{fieldNameService.TenantId}{_databaseConstant.BacktickClose} {GetColumnType(MixDataType.Integer)} NULL, " +
                $"{_databaseConstant.BacktickOpen}{fieldNameService.CreatedBy}{_databaseConstant.BacktickClose} {GetColumnType(MixDataType.String)} NULL, " +
                $"{_databaseConstant.BacktickOpen}{fieldNameService.ModifiedBy}{_databaseConstant.BacktickClose} {GetColumnType(MixDataType.String)} NULL, " +
                $"{_databaseConstant.BacktickOpen}{fieldNameService.Priority}{_databaseConstant.BacktickClose} {GetColumnType(MixDataType.Integer)} NOT NULL, " +
                $"{_databaseConstant.BacktickOpen}{fieldNameService.Status}{_databaseConstant.BacktickClose} {GetColumnType(MixDataType.String)} NOT NULL, " +
                $"{_databaseConstant.BacktickOpen}{fieldNameService.IsDeleted}{_databaseConstant.BacktickClose} {GetColumnType(MixDataType.Boolean)} NOT NULL, " +
                $" {string.Join(",", colsSql.ToArray())})";
        }

        private string GetAutoIncreaseIdSyntax(MixDatabaseProvider databaseProvider)
        {
            return databaseProvider switch
            {
                MixDatabaseProvider.SQLSERVER => $"{GetColumnType(MixDataType.Integer)} IDENTITY(1,1) PRIMARY KEY",
                MixDatabaseProvider.SQLITE => $"integer PRIMARY KEY AUTOINCREMENT",
                MixDatabaseProvider.PostgreSQL => "SERIAL PRIMARY KEY",
                MixDatabaseProvider.MySQL => $"{GetColumnType(MixDataType.Integer)} NOT NULL AUTO_INCREMENT PRIMARY KEY",
                _ => string.Empty
            };
        }

        private string GenerateColumnSql(RepoDbMixDatabaseColumnViewModel col)
        {
            string colType = GetColumnType(col.DataType, col.ColumnConfigurations.MaxLength);
            string nullable = col.ColumnConfigurations.IsRequire ? "NOT NUll" : "NULL";
            string unique = col.ColumnConfigurations.IsUnique ? "Unique" : "";
            string defaultValue = !IsLongTextColumn(col) && !string.IsNullOrEmpty(col.DefaultValue)
                                    ? $"DEFAULT '{@col.DefaultValue}'" : string.Empty;
            return $"{_databaseConstant.BacktickOpen}{col.SystemName}{_databaseConstant.BacktickClose} {colType} {nullable} {unique} {defaultValue}";
        }

        private string GenerateAddColumnSql(RepoDbMixDatabaseColumnViewModel col)
        {
            return $"ALTER TABLE {_databaseConstant.BacktickOpen}{col.MixDatabaseName}{_databaseConstant.BacktickClose}" +
                $" ADD {GenerateColumnSql(col)};";
        }
        private string GenerateDropColumnSql(RepoDbMixDatabaseColumnViewModel col)
        {
            return $"ALTER TABLE {_databaseConstant.BacktickOpen}{col.MixDatabaseName}{_databaseConstant.BacktickClose}" +
                $" DROP COLUMN IF EXISTS {_databaseConstant.BacktickOpen}{col.SystemName}{_databaseConstant.BacktickClose};";
        }
        private string GenerateAlterColumnSql(RepoDbMixDatabaseColumnViewModel col)
        {
            string colType = GetColumnType(col.DataType, col.ColumnConfigurations.MaxLength);
            string colName = $"{_databaseConstant.BacktickOpen}{col.SystemName}{_databaseConstant.BacktickClose}";
            string tableName = $"{_databaseConstant.BacktickOpen}{col.MixDatabaseName}{_databaseConstant.BacktickClose}";
            string uniqueKey = $"{_databaseConstant.BacktickOpen}{col.MixDatabaseName}_{col.SystemName}_key{_databaseConstant.BacktickClose}";
            string defaultValue = !IsLongTextColumn(col) && !string.IsNullOrEmpty(col.DefaultValue)
                                    ? $"DEFAULT '{@col.DefaultValue}'" : string.Empty;
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

        private bool IsLongTextColumn(RepoDbMixDatabaseColumnViewModel col)
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
                case MixDataType.DateTime:
                case MixDataType.Date:
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

        public Task<bool> RestoreFromLocal(RepoDbMixDatabaseViewModel database)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

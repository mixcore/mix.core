using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Mix.Constant.Enums;
using Mix.Database.Base;
using Mix.Database.Entities.Cms;
using Mix.Database.Services;
using Mix.Heart.Enums;
using Mix.Heart.Extensions;
using Mix.Heart.Helpers;
using Mix.Heart.Models;
using Mix.Heart.UnitOfWork;
using Mix.RepoDb.Entities;
using Mix.RepoDb.Interfaces;
using Mix.RepoDb.Repositories;
using Mix.RepoDb.ViewModels;
using Mix.Service.Interfaces;
using Mix.Service.Services;
using Mix.Shared.Dtos;
using Mix.Shared.Models;
using Newtonsoft.Json.Linq;
using RepoDb;
using RepoDb.Enumerations;
using RepoDb.Interfaces;
using System.Dynamic;

namespace Mix.RepoDb.Services
{
    public class MixDbService : TenantServiceBase, IMixDbService
    {
        private readonly IDatabaseConstants _databaseConstant;
        private readonly MixRepoDbRepository _repository;
        private readonly MixRepoDbRepository _backupRepository;
        private readonly MixRepoDbRepository _associationRepository;
        private readonly IMixMemoryCacheService _memoryCache;

        #region Properties

        private readonly UnitOfWorkInfo<MixCmsContext> _cmsUow;
        private readonly DatabaseService _databaseService;

        private const string CreatedDateFieldName = "CreatedDateTime";
        private const string CreatedByFieldName = "CreatedBy";
        private const string PriorityFieldName = "Priority";
        private const string IdFieldName = "Id";
        private const string ParentIdFieldName = "ParentId";
        private const string ChildIdFieldName = "ChildId";
        private const string TenantIdFieldName = "MixTenantId";
        private const string StatusFieldName = "Status";
        private const string IsDeletedFieldName = "IsDeleted";

        private static readonly string[] DefaultProperties =
        {
            "Id",
            "CreatedDateTime",
            "LastModified",
            "MixTenantId",
            "CreatedBy",
            "ModifiedBy",
            "Priority",
            "Status",
            "IsDeleted"
        };
        #endregion

        public MixDbService(
            IHttpContextAccessor httpContextAccessor,
            UnitOfWorkInfo<MixCmsContext> uow,
            DatabaseService databaseService,
            MixRepoDbRepository repository,
            ICache cache,
            IMixMemoryCacheService memoryCache)
            : base(httpContextAccessor)
        {
            _cmsUow = uow;
            _databaseService = databaseService;
            _repository = repository;
            _associationRepository = new MixRepoDbRepository(cache, databaseService, uow);
            _associationRepository.InitTableName(nameof(MixDatabaseAssociation));
            _backupRepository = new MixRepoDbRepository(cache, databaseService, uow);
            _databaseConstant = _databaseService.DatabaseProvider switch
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

        #region CRUD

        public async Task<PagingResponseModel<JObject>> GetMyData(string tableName, SearchMixDbRequestDto req, string username)
        {
            var paging = new PagingRequestModel()
            {
                PageIndex = req.PageIndex,
                PageSize = req.PageSize,
                SortBy = req.OrderBy,
                SortDirection = req.Direction
            };
            var queries = await BuildSearchQueryAsync(tableName, req);
            queries.Add(new(CreatedByFieldName, Operation.Equal, username));
            return await GetResult(tableName, queries, paging, req.LoadNestedData);
        }

        public async Task<JObject?> GetMyDataById(string tableName, string username, int id, bool loadNestedData)
        {
            _repository.InitTableName(tableName);
            var queries = new List<QueryField>()
            {
                new QueryField(TenantIdFieldName, CurrentTenant.Id),
                new QueryField(IdFieldName, id),
                new QueryField(CreatedByFieldName, username)
            };

            var obj = await _repository.GetSingleByAsync(queries);
            if (obj != null)
            {
                var data = ReflectionHelper.ParseObject(obj);
                if (loadNestedData)
                {
                    await LoadNestedData(id, data, tableName);
                }
                return data;
            }
            return default;
        }

        public async Task<JObject?> GetById(string tableName, int id, bool loadNestedData)
        {
            _repository.InitTableName(tableName);
            var obj = await _repository.GetSingleAsync(id);
            if (obj != null)
            {
                var data = ReflectionHelper.ParseObject(obj);
                if (loadNestedData)
                {
                    await LoadNestedData(id, data, tableName);
                }
                return data;
            }
            return default;
        }


        //public Task<int> CreateData(string tableName, JObject data)
        //{
        //    _repository.InitTableName(tableName);
        //    JObject obj = new JObject();
        //    foreach (var pr in data.Properties())
        //    {
        //        obj.Add(new JProperty(pr.Name.ToTitleCase(), pr.Value));
        //    }
        //    if (!obj.ContainsKey(createdDateFieldName))
        //    {
        //        obj.Add(new JProperty(createdDateFieldName, DateTime.UtcNow));
        //    }
        //    if (!obj.ContainsKey(priorityFieldName))
        //    {
        //        obj.Add(new JProperty(priorityFieldName, 0));
        //    }
        //    if (!obj.ContainsKey(tenantIdFieldName))
        //    {
        //        obj.Add(new JProperty(tenantIdFieldName, CurrentTenant.Id));
        //    }

        //    if (!obj.ContainsKey(statusFieldName))
        //    {
        //        obj.Add(new JProperty(statusFieldName, MixContentStatus.Published.ToString()));
        //    }

        //    if (!obj.ContainsKey(isDeletedFieldName))
        //    {
        //        obj.Add(new JProperty(isDeletedFieldName, false));
        //    }
        //    return await _repository.InsertAsync(obj);
        //}


        #endregion

        #region Helper

        private async Task LoadNestedData(int id, JObject data, string tableName)
        {
            var database = await GetMixDatabase(tableName);
            _repository.InitTableName(tableName);
            foreach (var item in database.Relationships)
            {


                List<QueryField> associationQueries = GetAssociationQueries(item.SourceDatabaseName, item.DestinateDatabaseName, id);
                var associations = await _associationRepository.GetListByAsync(associationQueries);
                if (associations.Count > 0)
                {
                    var nestedIds = JArray.FromObject(associations).Select(m => m.Value<int>(ChildIdFieldName)).ToList();
                    _repository.InitTableName(item.DestinateDatabaseName);
                    List<QueryField> query = new() { new(IdFieldName, Operation.In, nestedIds) };
                    var nestedData = await _repository.GetListByAsync(query);
                    data.Add(new JProperty(item.DisplayName, ReflectionHelper.ParseArray(nestedData)));
                }
                else
                {
                    data.Add(new JProperty(item.DisplayName, new JArray()));
                }
            }
        }

        private async Task<PagingResponseModel<JObject>> GetResult(string tableName,
            IEnumerable<QueryField> queries, PagingRequestModel paging, bool loadNestedData)
        {
            var result = await _repository.GetPagingAsync(queries, paging);

            var items = new List<JObject>();
            var database = await GetMixDatabase(tableName);

            foreach (var item in result.Items)
            {
                var data = ReflectionHelper.ParseObject(item);
                if (loadNestedData)
                {
                    foreach (var rel in database.Relationships)
                    {
                        var id = data.Value<int>("id");

                        List<QueryField> nestedQueries = GetAssociationQueries(rel.SourceDatabaseName, rel.DestinateDatabaseName, id);
                        var associations = await _associationRepository.GetListByAsync(nestedQueries);
                        if (associations is { Count: > 0 })
                        {
                            var nestedIds = JArray.FromObject(associations).Select(m => m.Value<int>(ChildIdFieldName)).ToList();
                            _repository.InitTableName(rel.DestinateDatabaseName);
                            List<QueryField> query = new() { new(IdFieldName, Operation.In, nestedIds) };
                            var nestedData = await _repository.GetListByAsync(query);
                            data.Add(new JProperty(rel.DisplayName, ReflectionHelper.ParseArray(nestedData)));
                        }
                    }
                }
                items.Add(data);
            }
            return new PagingResponseModel<JObject> { Items = items, PagingData = result.PagingData };
        }

        private List<QueryField> GetAssociationQueries(string parentDatabaseName = null, string childDatabaseName = null, int? parentId = null, int? childId = null)
        {
            var queries = new List<QueryField>();
            if (!string.IsNullOrEmpty(parentDatabaseName))
            {
                queries.Add(new QueryField("ParentDatabaseName", parentDatabaseName));
            }
            if (!string.IsNullOrEmpty(childDatabaseName))
            {
                queries.Add(new QueryField("ChildDatabaseName", childDatabaseName));
            }
            if (parentId.HasValue)
            {
                queries.Add(new QueryField(ParentIdFieldName, parentId));
            }
            if (childId.HasValue)
            {
                queries.Add(new QueryField(ChildIdFieldName, parentId));
            }
            return queries;
        }

        private async Task<MixDatabaseViewModel> GetMixDatabase(string tableName)
        {
            return await _memoryCache.TryGetValueAsync(
                tableName,
                cache =>
                {
                    cache.SlidingExpiration = TimeSpan.FromSeconds(20);
                    return MixDatabaseViewModel.GetRepository(_cmsUow).GetSingleAsync(m => m.SystemName == tableName);
                }
                );
        }

        private async Task<List<QueryField>> BuildSearchQueryAsync(string tableName, SearchMixDbRequestDto request)
        {
            var queries = BuildSearchPredicate(request);
            if (request.ParentId.HasValue)
            {
                var database = await GetMixDatabase(tableName);
                if (database.Type == MixDatabaseType.AdditionalData || database.Type == MixDatabaseType.GuidAdditionalData)
                {
                    queries.Add(new(ParentIdFieldName, request.ParentId));
                }
                else
                {
                    var allowsIds = _cmsUow.DbContext.MixDatabaseAssociation
                            .Where(m => m.ParentDatabaseName == request.ParentName && m.ParentId == request.ParentId.Value && m.ChildDatabaseName == tableName)
                            .Select(m => m.ChildId).ToList();
                    queries.Add(new(IdFieldName, Operation.In, allowsIds));
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

        private List<QueryField> BuildSearchPredicate(SearchMixDbRequestDto req)
        {
            var queries = new List<QueryField>()
            {
                new QueryField(TenantIdFieldName, CurrentTenant.Id)
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

        private object ParseSearchKeyword(ExpressionMethod? searchMethod, string keyword)
        {
            return searchMethod switch
            {
                ExpressionMethod.Like => $"%{keyword}%",
                ExpressionMethod.In => keyword.Split(',', StringSplitOptions.TrimEntries),
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

        private Operation ParseSearchOperation(ExpressionMethod? searchMethod)
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


        #endregion
        // TODO: check why need to restart application to load new database schema for Repo Db Context !important
        public async Task<bool> MigrateDatabase(string name)
        {
            MixDatabaseViewModel database = await MixDatabaseViewModel.GetRepository(_cmsUow).GetSingleAsync(m => m.SystemName == name);
            if (database is { Columns.Count: > 0 })
            {
                //await BackupDatabase(database.SystemName);
                await Migrate(database, _databaseService.DatabaseProvider, _repository);
                //await RestoreFromLocal(database);
                return true;
            }
            return false;
        }

        // TODO: check why need to restart application to load new database schema for Repo Db Context !important
        public async Task<bool> RestoreFromLocal(string name)
        {
            MixDatabaseViewModel database = await MixDatabaseViewModel.GetRepository(_cmsUow).GetSingleAsync(m => m.SystemName == name);
            if (database is { Columns.Count: > 0 })
            {
                return await RestoreFromLocal(database);
            }
            return false;
        }

        public async Task<bool> BackupDatabase(string databaseName, CancellationToken cancellationToken = default)
        {
            var database = await MixDatabaseViewModel.GetRepository(_cmsUow).GetSingleAsync(m => m.SystemName == databaseName, cancellationToken);
            if (database != null)
            {
                return await BackupToLocal(database, cancellationToken);
            }
            return false;
        }

        #endregion

        #region Private

        private async Task<bool> BackupToLocal(MixDatabaseViewModel database, CancellationToken cancellationToken = default)
        {
            var data = await GetCurrentData(database.SystemName, cancellationToken);
            if (data is { Count: > 0 })
            {
                InitBackupRepository(database.SystemName);
                await Migrate(database, _backupRepository.DatabaseProvider, _backupRepository);
                foreach (var item in data)
                {
                    GetMembers(item, database.Columns.Select(c => c.SystemName.ToTitleCase()).ToList());
                }
                var result = await _backupRepository.InsertManyAsync(data);
                return result > 0;
            }
            return true;
        }

        private void InitBackupRepository(string databaseName)
        {
            string cnn = $"Data Source=MixContent/Backup/backup_{databaseName}.db";
            using var ctx = new BackupDbContext(cnn);
            ctx.Database.EnsureCreated();
            ctx.Dispose();
            _backupRepository.Init(databaseName, MixDatabaseProvider.SQLITE, cnn);

        }

        private async Task<bool> RestoreFromLocal(MixDatabaseViewModel database)
        {
            InitBackupRepository(database.SystemName);
            var data = await _backupRepository.GetAllAsync();
            if (data is { Count: > 0 })
            {
                foreach (var item in data)
                {
                    GetMembers(item, database.Columns.Select(c => c.SystemName.ToTitleCase()).ToList());
                }
                _repository.InitTableName(database.SystemName);
                var result = await _repository.InsertManyAsync(data);
                return result >= 0;
            }
            return true;
        }

        private void GetMembers(ExpandoObject obj, List<string> selectMembers)
        {
            var result = obj.ToList();
            foreach (KeyValuePair<string, object> kvp in result)
            {
                if (DefaultProperties.All(m => m != kvp.Key) && selectMembers.All(m => m != kvp.Key))
                {
                    obj!.Remove(kvp.Key, out _);
                }
            }
        }

        private async Task<List<dynamic>?> GetCurrentData(string databaseName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _repository.InitTableName(databaseName);
            return await _repository.GetAllAsync(cancellationToken);
        }

        private async Task<bool> Migrate(MixDatabaseViewModel database, MixDatabaseProvider databaseProvider, MixRepoDbRepository repo)
        {
            var colsSql = new List<string>();
            var tableName = database.SystemName.ToTitleCase();

            foreach (var col in database.Columns)
            {
                colsSql.Add(GenerateColumnSql(col));
            }

            var commandText = GetMigrateTableSql(tableName, databaseProvider, colsSql);
            if (!string.IsNullOrEmpty(commandText))
            {
                await repo.ExecuteCommand($"DROP TABLE IF EXISTS {_databaseConstant.BacktickOpen}{tableName}{_databaseConstant.BacktickClose};");
                var result = await repo.ExecuteCommand(commandText);
                return result >= 0;
            }

            return false;
        }

        private string GetMigrateTableSql(string tableName, MixDatabaseProvider databaseProvider, List<string> colsSql)
        {
            return $"CREATE TABLE {_databaseConstant.BacktickOpen}{tableName}{_databaseConstant.BacktickClose} " +
                $"({_databaseConstant.BacktickOpen}Id{_databaseConstant.BacktickClose} {GetAutoIncreaseIdSyntax(databaseProvider)}, " +
                $"{_databaseConstant.BacktickOpen}CreatedDateTime{_databaseConstant.BacktickClose} {GetColumnType(MixDataType.DateTime)}, " +
                $"{_databaseConstant.BacktickOpen}LastModified{_databaseConstant.BacktickClose} {GetColumnType(MixDataType.DateTime)} NULL, " +
                $"{_databaseConstant.BacktickOpen}MixTenantId{_databaseConstant.BacktickClose} {GetColumnType(MixDataType.Integer)} NULL, " +
                $"{_databaseConstant.BacktickOpen}CreatedBy{_databaseConstant.BacktickClose} {GetColumnType(MixDataType.Text)} NULL, " +
                $"{_databaseConstant.BacktickOpen}ModifiedBy{_databaseConstant.BacktickClose} {GetColumnType(MixDataType.Text)} NULL, " +
                $"{_databaseConstant.BacktickOpen}Priority{_databaseConstant.BacktickClose} {GetColumnType(MixDataType.Integer)} NOT NULL, " +
                $"{_databaseConstant.BacktickOpen}Status{_databaseConstant.BacktickClose} {GetColumnType(MixDataType.Text)} NULL, " +
                $"{_databaseConstant.BacktickOpen}IsDeleted{_databaseConstant.BacktickClose} {GetColumnType(MixDataType.Boolean)} NOT NULL, " +
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

        private string GenerateColumnSql(MixDatabaseColumnViewModel col)
        {
            string colType = GetColumnType(col.DataType, col.ColumnConfigurations.MaxLength);
            string nullable = col.ColumnConfigurations.IsRequire ? "NOT NUll" : "NULL";
            string unique = col.ColumnConfigurations.IsUnique ? "Unique" : "";
            string defaultValue = !string.IsNullOrEmpty(col.DefaultValue) ? $"DEFAULT '{@col.DefaultValue}'" : string.Empty;
            return $"{_databaseConstant.BacktickOpen}{col.SystemName.ToTitleCase()}{_databaseConstant.BacktickClose} {colType} {nullable} {unique} {defaultValue}";
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
                    return $"{_databaseConstant.NString}{_databaseConstant.MaxLength}";
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

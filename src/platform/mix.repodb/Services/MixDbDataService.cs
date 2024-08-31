using Microsoft.AspNetCore.Http;
using Mix.Constant.Enums;
using Mix.Database.Entities.Cms;
using Mix.Database.Services;
using Mix.Heart.Enums;
using Mix.Heart.Helpers;
using Mix.Heart.Models;
using Mix.Heart.UnitOfWork;
using Mix.RepoDb.Repositories;
using Mix.RepoDb.ViewModels;
using Mix.Service.Services;
using Mix.Shared.Dtos;
using Mix.Shared.Models;
using Newtonsoft.Json.Linq;
using RepoDb.Interfaces;
using RepoDb;
using RepoDb.Enumerations;
using Mix.Heart.Extensions;
using Mix.RepoDb.Interfaces;
using Mix.Service.Interfaces;
using Mix.Constant.Constants;
using Mix.Heart.Services;
using Mix.Shared.Services;
using Mix.Database.Entities.MixDb;
using Mix.Lib.Interfaces;
using Mix.Heart.Exceptions;
using Mix.RepoDb.Helpers;
using Mix.RepoDb.Dtos;
using System.Linq.Expressions;

namespace Mix.RepoDb.Services
{
    public class MixDbDataService : TenantServiceBase, IMixDbDataService
    {
        #region Properties
        private readonly MixRepoDbRepository _repository;
        private readonly IMixMemoryCacheService _memoryCache;
        private RepoDbMixDatabaseViewModel? _mixDb;
        private FieldNameService _fieldNameService;

        private readonly UnitOfWorkInfo<MixCmsContext> _cmsUow;

        #endregion

        public MixDbDataService(
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
            _cmsUow = uow;
            _repository = repository;
            _memoryCache = memoryCache;
        }

        #region Methods

        public void SetUOW(UnitOfWorkInfo<MixDbDbContext> uow)
        {
            _repository.SetDbConnection(uow);
        }

        #region Implements

        public async Task<JObject?> GetSingleByParent(string tableName, MixContentType parentType, object parentId, bool loadNestedData = false)
        {
            await InitRepository(tableName);
            var data = await _repository.GetSingleByParentAsync(parentType, parentId, _fieldNameService);
            if (data != null)
            {
                JObject result = ReflectionHelper.ParseObject(data);
                if (loadNestedData)
                {
                    var id = result.Value<int>(_fieldNameService.Id);
                    result.Add(await LoadNestedData(_mixDb, _fieldNameService, id));
                }
                return result;
            }
            return default;
        }

        public async Task<JObject?> GetSingleByGuidParent(string tableName, MixContentType parentType, Guid parentId, bool loadNestedData = false)
        {
            await InitRepository(tableName);
            var data = await _repository.GetSingleByParentAsync(parentType, parentId, _fieldNameService);
            if (data != null)
            {
                JObject result = ReflectionHelper.ParseObject(data);
                if (loadNestedData)
                {

                    await LoadNestedData(_mixDb, _fieldNameService, result.Value<int>(_fieldNameService.Id));
                }
                return result;
            }
            return default;
        }

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
            queries.Add(new(_fieldNameService.CreatedBy, Operation.Equal, username));
            return await GetResult(tableName, queries, paging, req.Queries.Any(m => m.CompareOperator == MixCompareOperator.ILike), req.Conjunction, req.LoadNestedData);
        }

        public async Task<JObject?> GetMyDataById(string tableName, string username, object id, bool loadNestedData)
        {
            await InitRepository(tableName);
            var queries = new List<QueryField>()
            {
                new QueryField(_fieldNameService.TenantId, CurrentTenant.Id),
                new QueryField(_fieldNameService.Id, id),
                new QueryField(_fieldNameService.CreatedBy, username)
            };

            var obj = await _repository.GetSingleByAsync(queries);
            if (obj != null)
            {
                JObject data = ReflectionHelper.ParseObject(obj);
                var database = await GetMixDatabase(tableName);

                if (database is null)
                {
                    return default;
                }

                _repository.InitTableName(GetRelationshipDbName(_mixDb));
                if (loadNestedData)
                {
                    var nestedData = await LoadNestedData(_mixDb, _fieldNameService, id);
                    foreach (var d in nestedData)
                    {
                        data.Add(d);
                    }
                }
                return data;
            }

            return default;
        }

        public async Task<JObject?> GetById(string tableName, object id, bool loadNestedData)
        {
            await InitRepository(tableName);
            var obj = await _repository.GetSingleAsync(new QueryField(_fieldNameService.Id, id));
            if (obj != null)
            {
                var mixDb = await GetMixDatabase(tableName);
                var fieldNameService = new FieldNameService(mixDb.NamingConvention);
                var data = await ParseDataAsync(tableName, obj);

                if (loadNestedData)
                {
                    var nestedData = await LoadNestedData(mixDb, fieldNameService, id);
                    foreach (var d in nestedData)
                    {
                        data.Add(d);
                    }
                }
                return data;
            }
            return default;
        }

        public async Task<JObject?> GetSingleBy(string tableName, List<QueryField> queries)
        {
            await InitRepository(tableName);
            var obj = await _repository.GetSingleByAsync(queries);
            if (obj != null)
            {
                return ReflectionHelper.ParseObject(obj);
            }
            return default;
        }

        public async Task<object> CreateData(string tableName, JObject data)
        {
            try
            {
                await InitRepository(tableName);
                var obj = await ParseDto(tableName, data);
                return await _repository.InsertAsync(obj, _mixDb);
            }
            catch (MixException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.Badrequest, ex);
            }
        }

        public async Task<object?> UpdateData(string tableName, JObject data)
        {
            try
            {
                await InitRepository(tableName);
                var obj = await ParseDto(tableName, data);
                var id = data.Value<int?>("id");
                if (!id.HasValue)
                {
                    throw new MixException(MixErrorStatus.NotFound);
                }
                return await _repository.UpdateAsync(id.Value, obj, _mixDb);
            }
            catch (MixException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.Badrequest, ex);
            }
        }
        public async Task<object> DeleteData(string tableName, object id)
        {
            try
            {
                await InitRepository(tableName);
                return await _repository.DeleteAsync(id, _fieldNameService);
            }
            catch (MixException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.Badrequest, ex);
            }
        }
        public async Task<JObject> ParseDataAsync(string tableName, dynamic obj)
        {
            var data = ReflectionHelper.ParseObject(obj);
            var db = await GetMixDatabase(tableName);
            if (db is null)
            {
                return data;
            }

            var jsonColumns = db.Columns.Where(
                                    c => c.DataType == MixDataType.Json
                                            || c.DataType == MixDataType.ArrayMedia
                                            || c.DataType == MixDataType.Array
                                            || c.DataType == MixDataType.ArrayRadio)
                                .ToList();
            foreach (var col in jsonColumns)
            {
                var strValue = data.Value<string>(col.SystemName);
                if (!string.IsNullOrEmpty(strValue))
                {
                    data.Remove(col.SystemName);
                    if (col.DataType == MixDataType.Json || col.DataType == MixDataType.ArrayRadio)
                    {
                        data.Add(new JProperty(col.SystemName, JObject.Parse(strValue)));
                    }
                    else
                    {
                        data.Add(new JProperty(col.SystemName, JArray.Parse(strValue)));
                    }
                }
            }
            return data;
        }
        public async Task<List<JObject>> ParseListDataAsync(string tableName, List<dynamic> objs)
        {
            var result = new List<JObject>();
            foreach (var obj in objs)
            {
                result.Add(await ParseDataAsync(tableName, obj));
            }
            return result;
        }

        public async Task<List<JProperty>> LoadNestedData(RepoDbMixDatabaseViewModel database, FieldNameService fieldNameService,
                        object parentId)
        {
            List<JProperty> result = new();
            foreach (var item in database.Relationships)
            {
                // Many to many
                if (item.Type == MixDatabaseRelationshipType.ManyToMany)
                {
                    var relDb = await GetMixDatabase(GetRelationshipDbName(_mixDb));
                    var relFieldName = new FieldNameService(relDb.NamingConvention);
                    _repository.InitTableName(relDb.SystemName);
                    result.Add(await LoadManyToManyData(relFieldName, item, fieldNameService, parentId));
                }
                else if (item.Type == MixDatabaseRelationshipType.OneToMany)
                {
                    result.Add(await LoadOneToManyData(item, fieldNameService, parentId));
                }
            }
            return result;
        }
        #endregion

        #region Helper
        private string GetRelationshipDbName(RepoDbMixDatabaseViewModel mixdb)
        {
            string relName = mixdb.NamingConvention == MixDatabaseNamingConvention.SnakeCase
                            ? MixDatabaseNames.DATA_RELATIONSHIP_SNAKE_CASE
                            : MixDatabaseNames.DATA_RELATIONSHIP_TITLE_CASE;
            return mixdb.MixDatabaseContextId.HasValue
                        ? $"{mixdb.MixDatabaseContext.SystemName}_{relName}"
                        : MixDatabaseNames.SYSTEM_DATA_RELATIONSHIP;
        }
        public static string GetCacheFolder(string databaseName)
        {
            return $"{MixFolders.MixDbCacheFolder}/{databaseName}";
        }



        private async Task<JProperty> LoadOneToManyData(MixDatabaseRelationshipViewModel item, FieldNameService fieldNameService, object parentId)
        {
            string pIdName = fieldNameService.GetParentId(item.SourceDatabaseName);
            _repository.InitTableName(item.DestinateDatabaseName);
            List<QueryField> query = new() { new(pIdName, Operation.Equal, parentId) };
            var nestedData = await _repository.GetListByAsync(query, orderFields: new List<OrderField>() { new OrderField(_fieldNameService.CreatedDateTime, Order.Ascending) });

            JArray result = new();
            if (nestedData != null)
            {
                foreach (var nd in nestedData)
                {
                    result.Add(await ParseDataAsync(item.DestinateDatabaseName, nd));
                }
            }
            return new JProperty(item.DisplayName, result);
        }


        private async Task<JProperty> LoadManyToManyData(FieldNameService relFieldName, MixDatabaseRelationshipViewModel item, FieldNameService fieldNameService,
            object parentId)
        {
            List<QueryField> queries = GetAssociationQueries(relFieldName, item.SourceDatabaseName, item.DestinateDatabaseName,
                                        parentId: parentId);
            var associations = await _repository.GetListByAsync(queries, orderFields: new List<OrderField>() { new OrderField(_fieldNameService.CreatedDateTime, Order.Ascending) });
            if (associations is { Count: > 0 })
            {
                var childDb = await GetMixDatabase(item.DestinateDatabaseName);
                var nestedIds =
                    childDb.Type == MixDatabaseType.GuidService
                    ? JArray.FromObject(associations).Select(m => m.Value<object>(fieldNameService.GuidChildId)).ToList()
                    : JArray.FromObject(associations).Select(m => m.Value<object>(fieldNameService.ChildId)).ToList();
                _repository.InitTableName(item.DestinateDatabaseName);
                List<QueryField> query = new() { new(fieldNameService.Id, Operation.In, nestedIds) };
                var nestedData = await _repository.GetListByAsync(query);
                if (nestedData != null)
                {
                    return new JProperty(item.DisplayName, JArray.FromObject(nestedData));
                }
            }

            return new JProperty(item.DisplayName, default);
        }


        private async Task<PagingResponseModel<JObject>> GetResult(string tableName, IEnumerable<QueryField> queries, PagingRequestModel paging, bool iLike, MixConjunction conjunction, bool loadNestedData)
        {
            var result = await _repository.GetPagingAsync(queries, paging, iLike, conjunction);

            var items = new List<JObject>();
            var database = await GetMixDatabase(tableName);
            if (database is null)
            {
                return new PagingResponseModel<JObject>();
            }

            foreach (var item in result.Items)
            {
                JObject data = ReflectionHelper.ParseObject(item);
                if (loadNestedData)
                {
                    var id = data.Value<int>(_fieldNameService.Id);
                    await LoadNestedData(_mixDb, _fieldNameService, id);
                }
                items.Add(data);
            }

            return new PagingResponseModel<JObject> { Items = items, PagingData = result.PagingData };
        }

        private List<QueryField> GetAssociationQueries(
                FieldNameService fieldNameService,
                string? parentDatabaseName = null,
                string? childDatabaseName = null,
                object? parentId = null,
                object? childId = null)
        {
            var queries = new List<QueryField>();
            if (!string.IsNullOrEmpty(parentDatabaseName))
            {
                queries.Add(new QueryField(fieldNameService.ParentDatabaseName, parentDatabaseName));
            }
            if (!string.IsNullOrEmpty(childDatabaseName))
            {
                queries.Add(new QueryField(fieldNameService.ChildDatabaseName, childDatabaseName));
            }
            if (parentId != null)
            {
                if (parentId.GetType() == typeof(int))
                {
                    queries.Add(new QueryField(fieldNameService.ParentId, parentId));
                }
                else if (parentId.GetType() == typeof(Guid))
                {
                    queries.Add(new QueryField(fieldNameService.GuidParentId, parentId));
                }
            }
            if (childId != null)
            {
                if (childId.GetType() == typeof(int))
                {
                    queries.Add(new QueryField(fieldNameService.Id, parentId));
                }
                else if (childId.GetType() == typeof(Guid))
                {
                    queries.Add(new QueryField(fieldNameService.GuidChildId, childId));
                }
            }

            return queries;
        }

        private async Task<RepoDbMixDatabaseViewModel?> GetMixDatabase(string tableName)
        {
            return await _memoryCache.TryGetValueAsync(
                tableName,
                cache =>
                {
                    cache.SlidingExpiration = TimeSpan.FromSeconds(20);
                    return RepoDbMixDatabaseViewModel.GetRepository(_cmsUow, CacheService).GetSingleAsync(m => m.SystemName == tableName);
                }
                );
        }

        private async Task<List<QueryField>> BuildSearchQueryAsync(string tableName, SearchMixDbRequestDto request)
        {
            var queries = BuildSearchPredicate(request);
            if (request.ObjParentId != null)
            {
                var database = await GetMixDatabase(tableName);
                var parentDb = await GetMixDatabase(request.ParentName);
                if (database is null)
                {
                    return queries;
                }

                if (database.Type == MixDatabaseType.AdditionalData || database.Type == MixDatabaseType.GuidAdditionalData)
                {
                    queries.Add(new(_fieldNameService.ParentId, request.ParentId));
                }
                else
                {
                    Expression<Func<MixDatabaseAssociation, bool>> predicate = m => m.ParentDatabaseName == request.ParentName
                                                                                        && m.ChildDatabaseName == tableName;
                    predicate = predicate.AndAlsoIf(parentDb.Type == MixDatabaseType.GuidService,
                                            m => m.GuidParentId == (Guid)request.ObjParentId);
                    predicate = predicate.AndAlsoIf(parentDb.Type != MixDatabaseType.GuidService,
                                            m => m.ParentId == (int)request.ObjParentId);

                    var childIdsQuery = _cmsUow.DbContext.MixDatabaseAssociation
                            .Where(predicate);
                    if (database.Type == MixDatabaseType.GuidService)
                    {
                        queries.Add(new(_fieldNameService.Id, Operation.In, childIdsQuery.Select(m => m.GuidChildId).ToList()));
                    }
                    else
                    {
                        queries.Add(new(_fieldNameService.Id, Operation.In, childIdsQuery.Select(m => m.ChildId).ToList()));
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

        private List<QueryField> BuildSearchPredicate(SearchMixDbRequestDto req)
        {
            var queries = new List<QueryField>()
            {
                new QueryField(_fieldNameService.TenantId, CurrentTenant.Id)
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
                MixCompareOperator.InRange => keyword.Split(',', StringSplitOptions.TrimEntries),
                MixCompareOperator.NotInRange => keyword.Split(',', StringSplitOptions.TrimEntries),
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

        #endregion

        #region Private

        private async Task InitRepository(string tableName)
        {
            _mixDb = await GetMixDatabase(tableName);
            _fieldNameService = new FieldNameService(_mixDb.NamingConvention);
            if (_mixDb == null)
            {
                throw new MixException(MixErrorStatus.Badrequest, $"Invalid Mix Db {tableName}");
            }

            if (_mixDb.MixDatabaseContextId.HasValue)
            {
                _repository.Init(tableName, _mixDb.MixDatabaseContext.DatabaseProvider, _mixDb.MixDatabaseContext.DecryptedConnectionString);
            }
            else
            {
                _repository.InitTableName(tableName);
            }
        }

        private async Task<JObject> ParseDto(string tableName, JObject dto)
        {
            JObject result = new();
            var database = await GetMixDatabase(tableName);
            if (database is null)
            {
                return result;
            }
            _fieldNameService = new FieldNameService(database.NamingConvention);
            var encryptedColumnNames = database.Columns
                .Where(m => m.ColumnConfigurations.IsEncrypt)
                .Select(c => c.SystemName)
                .ToList();
            foreach (var pr in dto.Properties())
            {
                var col = database.Columns.FirstOrDefault(c => c.SystemName.Equals(pr.Name, StringComparison.InvariantCultureIgnoreCase));

                if (encryptedColumnNames.Contains(pr.Name))
                {
                    result.Add(
                        new JProperty(
                                pr.Name, AesEncryptionHelper.EncryptString(pr.Value.ToString(),
                                GlobalConfigService.Instance.AppSettings.ApiEncryptKey)));
                }
                else
                {
                    result.Add(new JProperty(pr.Name, col != null ? MixDbHelper.ParseObjectValueToDbType(col.DataType, pr.Value) : pr.Value));
                }
            }

            if (!result.ContainsKey(_fieldNameService.Id))
            {
                result.Add(new JProperty(_fieldNameService.Id, string.Empty));
            }
            if (!result.ContainsKey(_fieldNameService.CreatedDateTime))
            {
                result.Add(new JProperty(_fieldNameService.CreatedDateTime, DateTime.UtcNow));
            }
            else
            {
                result[_fieldNameService.LastModified] = DateTime.UtcNow;
            }
            if (!result.ContainsKey(_fieldNameService.Priority))
            {
                result.Add(new JProperty(_fieldNameService.Priority, 0));
            }
            if (!result.ContainsKey(_fieldNameService.Status))
            {
                result.Add(new JProperty(_fieldNameService.Status, MixContentStatus.Published.ToString()));
            }

            if (!result.ContainsKey(_fieldNameService.IsDeleted))
            {
                result.Add(new JProperty(_fieldNameService.IsDeleted, false));
            }
            return result;
        }

        private object? ParseObjectValue(MixDataType? dataType, JToken value)
        {
            if (value != null)
            {

                switch (dataType)
                {
                    case MixDataType.Integer:
                    case MixDataType.Reference:
                        return int.Parse(value.ToString());
                    case MixDataType.Double:
                        return double.Parse(value.ToString());
                    default:
                        return value.ToString();

                }
            }
            return null;
        }

        public void Dispose()
        {
            _repository.Dispose();
            _cmsUow.Dispose();
        }

        public async Task<object?> CreateDataRelationship(CreateDataRelationshipDto dto, CancellationToken cancellationToken)
        {
            try
            {
                // TODO: Need to add guid id database
                var parentMixDb = await GetMixDatabase(dto.ParentDatabaseName);
                var childMixDb = await GetMixDatabase(dto.ChildDatabaseName);
                if (parentMixDb == null)
                {
                    throw new MixException("Invalid Parent Database");
                }
                if (childMixDb == null)
                {
                    throw new MixException("Invalid Child Database");
                }
                var relDbName = GetRelationshipDbName(parentMixDb);
                var result = await CreateData(relDbName, dto.ToJObject(parentMixDb.NamingConvention));
                return result;
            }
            catch (MixException)
            {
                throw;
            }
            catch (Exception ex)
            {

                throw new MixException(MixErrorStatus.ServerError, ex);
            }
        }

        public async Task DeleteDataRelationship(string relTableName, object id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                await DeleteData(relTableName, id);
            }
            catch (MixException)
            {
                throw;
            }
            catch (Exception ex)
            {

                throw new MixException(MixErrorStatus.ServerError, ex);
            }
        }

        #endregion
    }
}

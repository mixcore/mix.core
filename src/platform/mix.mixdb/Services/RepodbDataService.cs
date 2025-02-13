using Mix.Constant.Enums;
using Mix.Database.Entities.Cms;
using Mix.Heart.Enums;
using Mix.Heart.Models;
using Mix.Heart.UnitOfWork;
using Mix.RepoDb.Repositories;
using Mix.Service.Services;
using Mix.Shared.Dtos;
using Mix.Shared.Models;
using Newtonsoft.Json.Linq;
using RepoDb.Interfaces;
using RepoDb;
using RepoDb.Enumerations;
using Mix.Heart.Extensions;
using Mix.Constant.Constants;
using Mix.Heart.Services;
using Mix.Heart.Exceptions;
using Mix.Mixdb.ViewModels;
using System.Data;
using Mix.RepoDb.Models;
using Mix.Mixdb.Interfaces;
using Mix.Service.Interfaces;
using Mix.Heart.Helpers;
using Newtonsoft.Json;
using NuGet.Protocol;
using Mix.RepoDb.ViewModels;
using Mix.Heart.Model;
using Mix.Mixdb.Dtos;
using Mix.Mixdb.Helpers;
using Mix.Database.Services.MixGlobalSettings;
using Microsoft.Extensions.Configuration;
using Mix.Lib.Extensions;

namespace Mix.Mixdb.Services
{
    public class RepodbDataService : IMixDbDataService
    {
        #region Properties
        public MixDatabaseProvider DbProvider { get; }
        private readonly RepoDbRepository _repository;
        private readonly MixCacheService _cacheSrv;
        private UnitOfWorkInfo<MixCmsContext> _uow;
        private MixDbDatabaseViewModel _mixDb;
        private readonly IConfiguration _configuration;
        private readonly IMixMemoryCacheService _memoryCache;
        private FieldNameService _fieldNameService;
        #endregion

        public RepodbDataService(
            IConfiguration configuration,
            ICache cache, DatabaseService databaseService, IMixMemoryCacheService memoryCache, MixCacheService cacheSrv, UnitOfWorkInfo<MixCmsContext> uow)
        {
            _configuration = configuration;
            DbProvider = databaseService.DatabaseProvider;
            _repository = new(
                databaseService.GetConnectionString(MixConstants.CONST_MIXDB_CONNECTION),
                databaseService.DatabaseProvider,
                new AppSetting()
                {
                    CacheItemExpiration = 10,
                    CommandTimeout = 1000
                });
            _memoryCache = memoryCache;
            _cacheSrv = cacheSrv;
            _uow = uow;
        }

        public RepodbDataService(ICache cache, MixDatabaseProvider databaseProvider, string connectionString, UnitOfWorkInfo<MixCmsContext> cmsUow, IMixMemoryCacheService memoryCache, MixCacheService cacheSrv, UnitOfWorkInfo<MixCmsContext> uow)
        {
            _repository = new(connectionString, databaseProvider, new AppSetting()
            {
                CacheItemExpiration = 10,
                CommandTimeout = 1000
            });
            _memoryCache = memoryCache;
            _cacheSrv = cacheSrv;
            _uow = uow;
        }


        #region Implements

        public void SetDbConnection(UnitOfWorkInfo<MixCmsContext> uow)
        {
            _uow = uow;
        }

        public void Init(string connectionString)
        {
            _repository.CreateConnection(connectionString, true, true);
        }

        #region GET

        #region Get Single

        public async Task<JObject?> GetByIdAsync(string tableName, object objId, string? selectColumns, CancellationToken cancellationToken)
        {
            await LoadMixDb(tableName);
            return await GetSingleByAsync(tableName, new MixQueryField(_fieldNameService.Id, objId, MixCompareOperator.Equal), selectColumns, cancellationToken);
        }

        public async Task<JObject?> GetSingleByAsync(string tableName, MixQueryField query, string? selectColumns, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await LoadMixDb(tableName);
                _repository.BeginTransaction();

                var result = await _repository.GetSingleAsync(
                    tableName,
                    ParseQueryField(query),
                    await ParseFieldName(selectColumns, tableName),
                    cancellationToken: cancellationToken);
                MixDbHelper.ParseRawDataToEntityAsync(result, _mixDb.Columns);
                if (result != null && !string.IsNullOrEmpty(selectColumns))
                {
                    await LoadRelationShips(result, await ParseRelatedDataRequests(selectColumns, tableName), _mixDb, cancellationToken);
                }
                _repository.CompleteTransaction();
                return result;
            }
            throw new TaskCanceledException();
        }

        public async Task<JObject?> GetSingleByAsync(string tableName, List<MixQueryField> queries, string selectColumns, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await LoadMixDb(tableName);
                _repository.BeginTransaction();
                var result = await _repository.GetSingleAsync(tableName, ParseSearchQuery(queries), cancellationToken: cancellationToken);
                if (result != null)
                {
                    MixDbHelper.ParseRawDataToEntityAsync(result, _mixDb.Columns);
                    await LoadRelationShips(result, null, _mixDb, cancellationToken);
                }
                _repository.CompleteTransaction();
                return result;
            }
            throw new TaskCanceledException();
        }

        public async Task<JObject?> GetSingleByParentAsync(string tableName, MixContentType parentType, object parentId, string selectColumns, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await LoadMixDb(tableName);
                var queries = new List<MixQueryField>()
                    {
                        new MixQueryField(_fieldNameService.ParentId, parentId)
                    };
                return await GetSingleByAsync(
                    tableName,
                    queries,
                    selectColumns, cancellationToken);
            }
            throw new TaskCanceledException();
        }

        #endregion

        #region Get List

        public async Task<List<JObject>?> GetListByAsync(SearchMixDbRequestModel request, CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await LoadMixDb(request.TableName);
                List<QueryField> queries = ParseSearchQuery(request.Queries);
                var data = await _repository.GetListByAsync(
                    request.TableName,
                    queries,
                    request.SelectColumns,
                    ParseSortByFields(request.Paging.SortByColumns),
                    cancellationToken: cancellationToken);
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        MixDbHelper.ParseRawDataToEntityAsync(item, _mixDb.Columns);
                        await LoadRelationShips(item, null, _mixDb, cancellationToken);
                    }
                }
                return data;
            }
            throw new TaskCanceledException();
        }

        public async Task<List<JObject>?> GetListByAsync(
            string tableName,
            List<QueryField> queryFields,
            List<MixSortByColumn>? sortByFields = null,
            string? selectFields = null,
            bool loadNestedData = false,
            CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _repository.BeginTransaction();
                await LoadMixDb(tableName);
                var data = await _repository.GetListByAsync(tableName, queryFields, selectFields, ParseSortByFields(sortByFields));
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        MixDbHelper.ParseRawDataToEntityAsync(item, _mixDb.Columns);
                        await LoadRelationShips(item, null, _mixDb, cancellationToken);
                    }
                }
                return data;
            }
            throw new TaskCanceledException();
        }
        public async Task<List<JObject>?> GetListByParentAsync(SearchMixDbRequestModel request, MixContentType parentType, object parentId, CancellationToken cancellationToken = default)
        {
            try
            {
                await LoadMixDb(request.TableName);
                _repository.BeginTransaction();
                var data = await _repository.GetListByAsync(request.TableName, new List<QueryField>() {
                    new QueryField(_fieldNameService.ParentType, parentType.ToString()),
                    new QueryField(_fieldNameService.ParentId, parentId)
                    },
                     orderFields: ParseSortByFields(request.Paging.SortByColumns),
                     cancellationToken: cancellationToken);
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        MixDbHelper.ParseRawDataToEntityAsync(item, _mixDb.Columns);
                        await LoadRelationShips(item, null, _mixDb, cancellationToken);
                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                await MixLogService.LogExceptionAsync(ex);
                _repository.RollbackTransaction();
                throw new MixException(MixErrorStatus.Badrequest, ex);
            }
        }


        public async Task<List<JObject>?> GetAllAsync(SearchMixDbRequestModel request, CancellationToken cancellationToken = default)
        {
            try
            {
                _repository.BeginTransaction();
                await LoadMixDb(request.TableName);
                var data = await _repository.GetAllAsync(request.TableName, cancellationToken: cancellationToken);
                if (data != null && request.RelatedDataRequests != null)
                {
                    foreach (var item in data)
                    {
                        MixDbHelper.ParseRawDataToEntityAsync(item, _mixDb.Columns);
                        await LoadRelationShips(item, request.RelatedDataRequests, _mixDb, cancellationToken);
                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                _repository.RollbackTransaction();
                throw;
            }
        }


        public async Task<PagingResponseModel<JObject>> GetPagingAsync(
            SearchMixDbRequestModel request,
            CancellationToken cancellationToken = default)
        {
            var mixDb = await GetMixDb(request.TableName);
            var fieldNameSrv = new FieldNameService(mixDb.NamingConvention);
            if (request.Paging.SortByColumns.Count == 0)
            {
                request.Paging.SortByColumns.Add(new MixSortByColumn(fieldNameSrv.Id, SortDirection.Desc));
            }

            var data = await _repository.GetPagingAsync(
                request.TableName,
                ParseSearchQuery(request.Queries),
                request.Paging,
                request.Conjunction,
                await ParseFieldName(request.SelectColumns, request.TableName), cancellationToken: cancellationToken);
            foreach (var item in data.Items)
            {
                MixDbHelper.ParseRawDataToEntityAsync(item, mixDb.Columns);
                await LoadRelationShips(item, request.RelatedDataRequests, mixDb, cancellationToken);
            }
            return data;
        }

        #region Load Nested Data
        private async Task LoadRelationShips(JObject? obj, List<SearchMixDbRequestModel>? relatedDataRequests, MixDbDatabaseViewModel? mixDb, CancellationToken cancellationToken)
        {
            if (mixDb != null && obj != null)
            {
                if (relatedDataRequests != null)
                {
                    await LoadNestedDataAsync(mixDb.SystemName, obj, relatedDataRequests, cancellationToken);
                }
            }
        }
        public async Task LoadNestedDataAsync(string tableName, JObject item, List<SearchMixDbRequestModel> relatedDataRequests, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var db = await GetMixDb(tableName);
                foreach (var req in relatedDataRequests)
                {
                    var rel = db.Relationships.FirstOrDefault(m => m.DestinateDatabaseName == req.TableName);
                    if (rel == null)
                    {
                        continue;
                    }

                    if (rel.Type == MixDatabaseRelationshipType.OneToMany)
                    {
                        await LoadOneToMany(tableName, item, rel, req.Clone(), cancellationToken);
                    }
                    if (rel.Type == MixDatabaseRelationshipType.ManyToMany)
                    {
                        await LoadManyToMany(tableName, item, rel, req.Clone(), cancellationToken);
                    }
                }
                return;
            }
            throw new TaskCanceledException();
        }

        private async Task LoadOneToMany(string tableName, JObject item, MixDatabaseRelationshipViewModel rel, SearchMixDbRequestModel req, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var parentId = _fieldNameService.NamingConvention == MixDatabaseNamingConvention.SnakeCase
                    ? $"{rel.SourceDatabaseName}_id"
                    : $"{rel.SourceDatabaseName}Id";
                req.Queries.Add(new MixQueryField(parentId, await ExtractIdAsync(tableName, item)));
                var data = await GetPagingAsync(
                    req,
                    cancellationToken: cancellationToken);
                item.Add(new JProperty(rel.DisplayName, ReflectionHelper.ParseObject(data)));
                return;
            }
            throw new TaskCanceledException();
        }

        private async Task LoadManyToMany(string tableName, JObject item, MixDatabaseRelationshipViewModel rel, SearchMixDbRequestModel req, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var relQuery = new List<QueryField>() {
                        new QueryField(_fieldNameService.ParentDatabaseName, rel.SourceDatabaseName),
                        new QueryField(_fieldNameService.ChildDatabaseName, rel.DestinateDatabaseName)
                };
                var parentDb = await GetMixDb(rel.SourceDatabaseName);
                var childDb = await GetMixDb(rel.DestinateDatabaseName);

                if (parentDb.Type == MixDatabaseType.GuidService)
                {
                    relQuery.Add(new(_fieldNameService.GuidParentId, await ExtractIdAsync(rel.SourceDatabaseName, item)));
                }
                else
                {
                    relQuery.Add(new(_fieldNameService.ParentId, await ExtractIdAsync(rel.DestinateDatabaseName, item)));
                }

                var allowsRels = await GetListByAsync(GetRelationshipDbName(), relQuery);
                if (allowsRels != null)
                {

                    req.Queries ??= new List<MixQueryField>();
                    req.Queries.Add(
                        new(
                            _fieldNameService.Id,
                            childDb.Type == MixDatabaseType.GuidService
                            ? allowsRels.Select(m => m.Value<int>(_fieldNameService.GuidChildId)).ToList()
                            : allowsRels.Select(m => m.Value<int>(_fieldNameService.ChildId)).ToList(),
                            MixCompareOperator.InRange
                    ));
                    var data = await GetPagingAsync(
                        req,
                        cancellationToken: cancellationToken);
                    item.Add(new JProperty(rel.DisplayName, data));
                }
                return;
            }
            throw new TaskCanceledException();
        }


        private async Task<IEnumerable<Field>?> ParseFieldName(string? selectFieldNames, string tableName)
        {
            List<Field>? fields = new();
            await LoadMixDb(tableName);
            string[] fieldNames = string.IsNullOrEmpty(selectFieldNames) ? _mixDb.Columns.Select(m => m.SystemName).ToArray()
                : selectFieldNames?.Split(',', StringSplitOptions.RemoveEmptyEntries) ?? _mixDb.Columns.Select(m => m.SystemName).ToArray();

            if (!fieldNames.Any(m => m.Trim() == _fieldNameService.Id))
            {
                fieldNames = fieldNames.Prepend(_fieldNameService.Id).ToArray();
            }
            foreach (var item in fieldNames)
            {
                fields.Add(new Field(item.Trim()));
            }
            return fields;
        }

        private async Task<List<SearchMixDbRequestModel>?> ParseRelatedDataRequests(string? selectFieldNames, string tableName)
        {
            if (string.IsNullOrEmpty(selectFieldNames))
            {
                return default;
            }
            var fields = selectFieldNames.Split(',');
            await LoadMixDb(tableName);
            return _mixDb.Relationships.Where(m => fields.Contains(m.DisplayName)).Select(m => new SearchMixDbRequestModel()
            {
                TableName = m.DestinateDatabaseName,
                Paging = new PagingRequestModel()
            }).ToList();
        }

        #endregion
        #endregion

        #endregion

        #region CREATE

        public async Task<object> CreateAsync(string tableName, JObject obj, string? createdBy = null, CancellationToken cancellationToken = default)
        {
            try
            {
                await LoadMixDb(tableName);
                _repository.BeginTransaction();
                Dictionary<string, object> dicObj = await ParseDtoToEntityAsync(obj, createdBy);

                var fields = dicObj!.Keys.Select(m => new Field(m)).ToList();
                var result = await _repository.InsertAsync(tableName, dicObj, cancellationToken: cancellationToken);
                _repository.CompleteTransaction();
                return result;
            }
            catch (Exception ex)
            {
                _repository.RollbackTransaction();
                throw new MixException(MixErrorStatus.Badrequest, ex);
            }
        }


        public async Task CreateManyAsync(string tableName, List<JObject> entities, string? createdBy = null, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                await LoadMixDb(tableName);
                if (entities.Count == 0)
                {
                    return;
                }

                List<Dictionary<string, object>> dicObjs = new();

                foreach (var entity in entities)
                {
                    var dicObj = await ParseDtoToEntityAsync(entity, createdBy);
                    if (dicObj != null)
                    {
                        dicObjs.Add(dicObj);
                    }
                }

                _repository.BeginTransaction();
                var result = await _repository.InsertManyAsync(
                        _mixDb.SystemName,
                        entities: dicObjs,
                        cancellationToken: cancellationToken
                        );
                _repository.CompleteTransaction();
            }
            catch (Exception ex)
            {
                _repository.RollbackTransaction();
                throw new MixException(MixErrorStatus.ServerError, ex);
            }
        }

        public async Task CreateDataRelationshipAsync(string tableName, CreateDataRelationshipDto obj, string? createdBy = null, CancellationToken cancellationToken = default)
        {
            await LoadMixDb(tableName);
            string relDbName = GetRelationshipDbName();
            var rel = new Dictionary<string, object?>()
            {
                { _fieldNameService.CreatedBy, createdBy},
                { _fieldNameService.ParentDatabaseName, obj.ParentDatabaseName},
                { _fieldNameService.ChildDatabaseName, obj.ChildDatabaseName },
                { _fieldNameService.GuidParentId, obj.GuidParentId },
                { _fieldNameService.GuidChildId, obj.GuidChildId },
                { _fieldNameService.ParentId, obj.ParentId },
                { _fieldNameService.ChildId, obj.ChildId },
                { _fieldNameService.Priority, 0 },
                { _fieldNameService.Status, MixContentStatus.Published.ToString() },
                { _fieldNameService.CreatedDateTime, DateTime.UtcNow },
                { _fieldNameService.IsDeleted, false },
            };
            await _repository.InsertAsync(relDbName, rel, cancellationToken: cancellationToken);
        }
        #endregion

        #region UPDATE

        public async Task<int?> UpdateManyAsync(string tableName, List<JObject> entities, string? modifiedBy = null, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                await LoadMixDb(tableName);
                if (entities.Count == 0)
                {
                    return default;
                }

                List<Dictionary<string, object>> dicObjs = new();

                foreach (var entity in entities)
                {
                    var dicObj = await ParseDtoToEntityAsync(entity, modifiedBy);
                    if (dicObj != null)
                    {
                        dicObjs.Add(dicObj);
                    }
                }

                _repository.BeginTransaction();
                var result = await _repository.UpdateManyAsync(
                        _mixDb.SystemName,
                        entities: dicObjs,
                        cancellationToken: cancellationToken);
                _repository.CompleteTransaction();
                return result;
            }
            catch (Exception ex)
            {
                _repository.RollbackTransaction();
                throw new MixException(MixErrorStatus.ServerError, ex);
            }
        }

        public async Task<object?> UpdateAsync(string tableName, object id, JObject entity, string? modifiedBy = null, IEnumerable<string>? fieldNames = default,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                await LoadMixDb(tableName);
                _repository.BeginTransaction();
                QueryField idQuery = new QueryField(_fieldNameService.Id, id);
                var result = await _repository.UpdateAsync(tableName, idQuery,
                    await ParseDtoToEntityAsync(entity, modifiedBy), fieldNames: fieldNames, cancellationToken: cancellationToken
                    );
                var cacheFolder = GetCacheFolder(tableName);
                await _cacheSrv.RemoveCacheAsync(id, cacheFolder, cancellationToken);

                return result;
            }
            catch (Exception ex)
            {
                _repository.RollbackTransaction();
                await MixLogService.LogExceptionAsync(ex);
                throw ex;
            }
        }
        #endregion

        #region DELETE
        public async Task<int> DeleteAsync(string tableName, object id, string? modifiedBy = null, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                await LoadMixDb(tableName);
                _repository.BeginTransaction();
                QueryField idQuery = new QueryField(_fieldNameService.Id, ParseId(id, _mixDb));
                return await _repository.DeleteAsync(tableName, idQuery);
            }
            catch (Exception ex)
            {
                _repository.RollbackTransaction();
                await MixLogService.LogExceptionAsync(ex);
                return default;
            }
        }

        public async Task<int> DeleteManyAsync(string tableName, List<MixQueryField> queries, string? modifiedBy = null, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                await LoadMixDb(tableName);
                _repository.BeginTransaction();
                return await _repository.DeleteAsync(tableName, ParseSearchQuery(queries), cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                _repository.RollbackTransaction();
                await MixLogService.LogExceptionAsync(ex);
                return default;
            }
        }

        public async Task DeleteDataRelationshipAsync(string tableName, int id, string? modifiedBy = null, CancellationToken cancellationToken = default)
        {
            await LoadMixDb(tableName);
            await _repository.DeleteAsync(GetRelationshipDbName(), new QueryField(_fieldNameService.Id, id), cancellationToken: cancellationToken);
        }

        #endregion

        #region HELPERS

        public string GetCacheFolder(string databaseName)
        {
            return $"{MixFolders.MixDbCacheFolder}/{databaseName}";
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

        #endregion

        #region private
        private List<QueryField> ParseSearchQuery(IEnumerable<MixQueryField> searchQueryFields)
        {
            List<QueryField> queries = new();
            foreach (var item in searchQueryFields)
            {
                queries.Add(ParseQueryField(item));
            }
            return queries;
        }

        private QueryField ParseQueryField(MixQueryField item)
        {
            Operation op = ParseMixOperator(item);
            if (op == Operation.In || op == Operation.NotIn)
            {
                return new QueryField(item.FieldName, op, item.Value);
            }
            else
            {
                return new QueryField(item.FieldName, op, item.Value);
            }
        }

        private Operation ParseMixOperator(MixQueryField field)
        {
            switch (field.CompareOperator)
            {
                case MixCompareOperator.InRange:
                    return Operation.In;
                case MixCompareOperator.ILike:
                case MixCompareOperator.Like:
                case MixCompareOperator.Contain:
                    return Operation.Like;
                case MixCompareOperator.NotEqual:
                    return Operation.NotEqual;
                case MixCompareOperator.NotContain:
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
                case MixCompareOperator.Equal:
                default:
                    return Operation.Equal;
            }
        }

        private string GetRelationshipDbName()
        {
            string relName = _mixDb.NamingConvention == MixDatabaseNamingConvention.SnakeCase
                            ? MixDatabaseNames.DATA_RELATIONSHIP_SNAKE_CASE
                            : MixDatabaseNames.DATA_RELATIONSHIP_TITLE_CASE;
            return _mixDb.MixDatabaseContextId.HasValue
                        ? $"{_mixDb.MixDatabaseContext.SystemName}_{relName}"
                        : MixDatabaseNames.SYSTEM_DATA_RELATIONSHIP;
        }
        public async Task<object> ExtractIdAsync(string tableName, JObject obj)
        {
            await LoadMixDb(tableName);
            return _mixDb.Type == MixDatabaseType.GuidService
                ? obj.GetJObjectProperty<Guid>(_fieldNameService.Id)
                : obj.GetJObjectProperty<int>(_fieldNameService.Id);
        }

        private object ParseId(object strId, MixDbDatabaseViewModel mixDb)
        {
            return mixDb.Type == MixDatabaseType.GuidService
                ? Guid.Parse(strId.ToString())
                : int.Parse(strId.ToString());
        }

        private List<OrderField>? ParseSortByFields(List<MixSortByColumn>? sortByFields)
        {
            if (sortByFields is null)
            {
                return null;
            }
            var orderFields = new List<OrderField>();
            foreach (var item in sortByFields)
            {
                orderFields.Add(new OrderField(item.FieldName, item.Direction == SortDirection.Asc ? Order.Ascending : Order.Descending));
            }
            return orderFields;
        }

        public Task<Dictionary<string, object>> ParseDtoToEntityAsync(JObject dto, string? username = null)
        {
            try
            {
                Dictionary<string, object> result = new();
                var encryptedColumnNames = _mixDb.Columns
                    .Where(m => m.ColumnConfigurations.IsEncrypt)
                    .Select(c => c.SystemName)
                    .ToList();
                foreach (var pr in dto.Properties())
                {
                    var colName = _fieldNameService.NamingConvention == MixDatabaseNamingConvention.TitleCase ? pr.Name.ToTitleCase() : pr.Name.ToHyphenCase('_', true);
                    var col = _mixDb.Columns.FirstOrDefault(c => c.SystemName.Equals(colName, StringComparison.InvariantCultureIgnoreCase));

                    if (encryptedColumnNames.Contains(colName))
                    {
                        result.Add(colName, AesEncryptionHelper.EncryptString(pr.Value.ToString(),
                                    _configuration.AesKey()));
                    }
                    else
                    {
                        if (col != null)
                        {
                            result.Add(colName, ParseObjectValueToDbType(col.DataType, pr.Value));
                        }
                        else
                        {
                            result.Add(colName, GetPropertyValue(pr));
                        }
                    }
                }

                if (!result.ContainsKey(_fieldNameService.Id))
                {
                    if (_mixDb.Type == MixDatabaseType.GuidService)
                    {
                        result.Add(_fieldNameService.Id, Guid.NewGuid());
                    }
                    else
                    {
                        result.Add(_fieldNameService.Id, string.Empty);
                    }
                    if (!result.ContainsKey(_fieldNameService.LastModified)
                        && _mixDb.Columns.Any(m => m.SystemName == _fieldNameService.LastModified))
                    {
                        result.Add(_fieldNameService.LastModified, DateTime.UtcNow);
                    }
                }
                else
                {
                    if (_mixDb.Columns.Any(m => m.SystemName == _fieldNameService.ModifiedBy))
                    {
                        result[_fieldNameService.ModifiedBy] = username ?? string.Empty;
                    }
                    if (_mixDb.Columns.Any(m => m.SystemName == _fieldNameService.LastModified))
                    {
                        result[_fieldNameService.LastModified] = DateTime.UtcNow;
                    }
                }
                if (!result.ContainsKey(_fieldNameService.CreatedBy))
                {
                    result.Add(_fieldNameService.CreatedBy, username ?? string.Empty);
                }
                if (!result.ContainsKey(_fieldNameService.CreatedDateTime)
                    && _mixDb.Columns.Any(m => m.SystemName == _fieldNameService.CreatedDateTime))
                {
                    result.Add(_fieldNameService.CreatedDateTime, DateTime.UtcNow);
                }
                
                if (!result.ContainsKey(_fieldNameService.Priority)
                    && _mixDb.Columns.Any(m => m.SystemName == _fieldNameService.Priority))
                {
                    result.Add(_fieldNameService.Priority, 0);
                }

                if (!result.ContainsKey(_fieldNameService.Status))
                {
                    result.Add(_fieldNameService.Status, MixContentStatus.Published.ToString());
                }

                if (!result.ContainsKey(_fieldNameService.IsDeleted))
                {
                    result.Add(_fieldNameService.IsDeleted, false);
                }
                return Task.FromResult(result);
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


        private object? GetPropertyValue(JProperty prop)
        {
            string strVal = prop.Value.ToString();
            return string.IsNullOrEmpty(strVal) || prop.Value?.Type == null
                ? default
                : prop.Value.Type switch
                {
                    JTokenType.Date => DateTime.Parse(strVal),
                    JTokenType.Array => JArray.Parse(strVal),
                    JTokenType.Object => JObject.Parse(strVal),
                    JTokenType.Integer => int.Parse(strVal),
                    JTokenType.Float => double.Parse(strVal),
                    JTokenType.Boolean => bool.Parse(strVal),
                    _ => prop.Value?.ToString()
                };
        }

        public object? ParseObjectValueToDbType(MixDataType? dataType, JToken value)
        {
            try
            {
                if (value != null)
                {
                    string strValue = value.ToString();
                    if (string.IsNullOrEmpty(strValue))
                    {
                        return default;
                    }
                    switch (dataType)
                    {
                        case MixDataType.Date:
                        case MixDataType.DateTime:
                            DateTime.TryParse(strValue, out var dateValue);
                            if (dateValue.Kind != DateTimeKind.Utc)
                            {
                                return dateValue.ToUniversalTime();
                            }
                            return dateValue;

                        case MixDataType.Boolean:
                            return bool.Parse(strValue);
                        case MixDataType.Array:
                        case MixDataType.ArrayMedia:
                            return value.Type != JTokenType.String
                                ? JArray.FromObject(value).ToString(Formatting.None)
                                : value.Value<string>();
                        case MixDataType.Json:
                        case MixDataType.ArrayRadio:
                            return value.Type != JTokenType.String
                                    ? JObject.FromObject(value).ToString(Formatting.None)
                                    : value.Value<string>(); ;
                        case MixDataType.Integer:
                        case MixDataType.Reference:
                            return int.Parse(strValue);
                        case MixDataType.Double:
                            return double.Parse(strValue);
                        case MixDataType.Guid:
                            Guid.TryParse(value.ToString(), out var guildResult);
                            return guildResult;
                        default:
                            return value.ToString();

                    }
                }
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.Badrequest, ex);
            }
            return null;
        }

        private async Task LoadMixDb(string tableName)
        {
            if (_mixDb != null && _mixDb.SystemName == tableName)
            {
                return;
            }
            string name = $"{typeof(MixDbDatabaseViewModel).FullName}_{tableName}";
            _mixDb = await GetMixDb(tableName);
            _fieldNameService = new FieldNameService(_mixDb.NamingConvention);
        }

        private async Task<MixDbDatabaseViewModel> GetMixDb(string tableName)
        {
            return await _memoryCache.TryGetValueAsync(
                tableName,
                cache =>
                {
                    cache.SlidingExpiration = TimeSpan.FromSeconds(20);
                    return MixDbDatabaseViewModel.GetRepository(_uow, _cacheSrv).GetSingleAsync(m => m.SystemName == tableName);
                }
                ) ?? throw new NullReferenceException(tableName);
        }

        #endregion

        public void Dispose()
        {
            _repository.Dispose();
        }

    }
}

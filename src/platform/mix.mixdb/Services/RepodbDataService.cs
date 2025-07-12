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
using Mix.Constant.Constants;
using Mix.Heart.Services;
using Mix.Heart.Exceptions;
using Mix.Mixdb.ViewModels;
using System.Data;
using Mix.RepoDb.Models;
using Mix.Mixdb.Interfaces;
using Mix.Heart.Helpers;
using NuGet.Protocol;
using Mix.RepoDb.ViewModels;
using Mix.Heart.Model;
using Mix.Mixdb.Dtos;
using Mix.Mixdb.Helpers;
using Mix.Database.Services.MixGlobalSettings;
using Microsoft.Extensions.Configuration;
using Mix.Service.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Storage;

namespace Mix.Mixdb.Services
{
    /// <summary>
    /// Service process data MixDb using RepoDb
    /// </summary>
    public class RepodbDataService : IMixDbDataService
    {
        #region Properties
        public MixDatabaseProvider DbProvider { get; }
        private RepoDbRepository _repository;
        private readonly MixCacheService _cacheSrv;
        private readonly IMixMemoryCacheService _memoryCache;
        private readonly IMixDbRelationshipService _relationshipService;
        private readonly IMixDbDataParser _dataParser;
        private readonly ILogger<RepodbDataService> _logger;
        private UnitOfWorkInfo<MixCmsContext> _uow;
        private readonly IMixDbInfoService _mixDbInfoService;
        private MixDbTableViewModel? _mixDb;
        private readonly IConfiguration _configuration;
        private FieldNameService? _fieldNameService;
        #endregion

        public RepodbDataService(
            IConfiguration configuration,
            ICache cache,
            DatabaseService databaseService,
            IMixMemoryCacheService memoryCache,
            MixCacheService cacheSrv,
            UnitOfWorkInfo<MixCmsContext> uow,
            IMixDbDataParser dataParser,
            IMixDbInfoService mixDbInfoService,
            ILogger<RepodbDataService> logger)
        {
            _configuration = configuration;
            DbProvider = databaseService.DatabaseProvider;
            _fieldNameService = new FieldNameService(MixDatabaseNamingConvention.SnakeCase);
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
            _dataParser = dataParser;
            _mixDbInfoService = mixDbInfoService;
            _logger = logger;
            _relationshipService = new MixDbRelationshipService(this, _fieldNameService, _logger);
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

        public void InitConnection(MixDatabaseProvider databaseProvider, string connectionString)
        {
            _repository = new(connectionString, databaseProvider, new AppSetting()
            {
                CacheItemExpiration = 10,
                CommandTimeout = 1000
            });
            _repository.CreateConnection(connectionString, true, true);
        }

        #region GET

        #region Get Single

        public async Task<JObject?> GetByIdAsync(string tableName, object objId, string? selectColumns, CancellationToken cancellationToken)
        {
            try
            {
                await LoadMixDb(tableName);
                return await GetSingleByAsync(tableName, new MixQueryField(_fieldNameService.Id, objId, MixCompareOperator.Equal), selectColumns, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error when getting data by ID from table {tableName}");
                throw;
            }
        }

        public async Task<T?> GetByIdAsync<T>(string tableName, object objId, string? selectColumns, CancellationToken cancellationToken)
            where T : class
        {
            try
            {
                await LoadMixDb(tableName);
                var obj = await GetSingleByAsync(tableName, new MixQueryField(_fieldNameService.Id, objId, MixCompareOperator.Equal), selectColumns, cancellationToken);
                return obj?.ToObject<T>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error when getting data by ID from table {tableName}");
                throw;
            }
        }

        public async Task<JObject?> GetSingleByAsync(string tableName, MixQueryField query, string? selectColumns, CancellationToken cancellationToken)
        {
            try
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
                    await _relationshipService.LoadNestedDataAsync(tableName, result, await _relationshipService.ParseRelatedDataRequests(selectColumns, tableName), cancellationToken);
                }
                _repository.CompleteTransaction();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error when getting single data from table {tableName}");
                _repository.RollbackTransaction();
                throw;
            }
        }

        public async Task<JObject?> GetSingleByAsync(string tableName, List<MixQueryField> queries, string selectColumns, CancellationToken cancellationToken)
        {
            try
            {
                await LoadMixDb(tableName);
                _repository.BeginTransaction();
                var result = await _repository.GetSingleAsync(tableName, ParseSearchQuery(queries), cancellationToken: cancellationToken);
                if (result != null)
                {
                    MixDbHelper.ParseRawDataToEntityAsync(result, _mixDb.Columns);
                    await _relationshipService.LoadNestedDataAsync(tableName, result, null, cancellationToken);
                }
                _repository.CompleteTransaction();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error when getting single data from table {tableName}");
                _repository.RollbackTransaction();
                throw;
            }
        }

        public async Task<JObject?> GetSingleByParentAsync(string tableName, MixContentType parentType, object parentId, string selectColumns, CancellationToken cancellationToken)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error when getting data by parent from table {tableName}");
                throw;
            }
        }

        public async Task<JObject?> GetSingleByColumnAsync(string tableName, string columnName, object columnValue, string selectColumns, CancellationToken cancellationToken)
        {
            try
            {
                await LoadMixDb(tableName);
                var queries = new List<MixQueryField>()
                    {
                        new MixQueryField(columnName, columnValue)
                    };
                return await GetSingleByAsync(
                    tableName,
                    queries,
                    selectColumns, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error when getting data by parent from table {tableName}");
                throw;
            }
        }
        #endregion

        #region Get List

        public async Task<List<JObject>?> GetListByAsync(SearchMixDbRequestModel request, CancellationToken cancellationToken = default)
        {
            try
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
                        await _relationshipService.LoadNestedDataAsync(request.TableName, item, null, cancellationToken);
                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error when getting list data from table {request.TableName}");
                throw;
            }
        }

        public async Task<List<JObject>?> GetListByAsync(
            string tableName,
            List<QueryField> queryFields,
            List<MixSortByColumn>? sortByFields = null,
            string? selectFields = null,
            bool loadNestedData = false,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _repository.BeginTransaction();
                await LoadMixDb(tableName);
                var data = await _repository.GetListByAsync(tableName, queryFields, selectFields, ParseSortByFields(sortByFields));
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        MixDbHelper.ParseRawDataToEntityAsync(item, _mixDb.Columns);
                        await _relationshipService.LoadNestedDataAsync(tableName, item, null, cancellationToken);
                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error when getting list data from table {tableName}");
                _repository.RollbackTransaction();
                throw;
            }
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
                        await _relationshipService.LoadNestedDataAsync(request.TableName, item, null, cancellationToken);
                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error when getting list data by parent from table {request.TableName}");
                _repository.RollbackTransaction();
                throw;
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
                        await _relationshipService.LoadNestedDataAsync(request.TableName, item, request.RelatedDataRequests, cancellationToken);
                    }
                }
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error when getting all data from table {request.TableName}");
                _repository.RollbackTransaction();
                throw;
            }
        }


        public async Task<PagingResponseModel<JObject>> GetPagingAsync(
            SearchMixDbRequestModel request,
            CancellationToken cancellationToken = default)
        {
            try
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
                    await _relationshipService.LoadNestedDataAsync(request.TableName, item, request.RelatedDataRequests, cancellationToken);
                }
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error when getting paging data from table {request.TableName}");
                throw;
            }
        }

        #region Load Nested Data
        private async Task LoadRelationShips(JObject? obj, List<SearchMixDbRequestModel>? relatedDataRequests, MixDbTableViewModel? mixDb, CancellationToken cancellationToken)
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
                    var rel = db.Relationships.FirstOrDefault(m => m.DestinateTableName == req.TableName);
                    if (rel == null)
                    {
                        continue;
                    }

                    if (rel.Type == MixDbTableRelationshipType.OneToMany)
                    {
                        await LoadOneToMany(tableName, item, rel, req.Clone(), cancellationToken);
                    }
                    if (rel.Type == MixDbTableRelationshipType.ManyToMany)
                    {
                        await LoadManyToMany(tableName, item, rel, req.Clone(), cancellationToken);
                    }
                }
                return;
            }
            throw new TaskCanceledException();
        }

        private async Task LoadOneToMany(string tableName, JObject item, MixDbTableRelationshipViewModel rel, SearchMixDbRequestModel req, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                req.Queries ??= new();
                var parentId = _fieldNameService.NamingConvention == MixDatabaseNamingConvention.SnakeCase
                    ? $"{rel.SourceTableName}_id"
                    : $"{rel.SourceTableName}Id";
                req.Queries.Add(new MixQueryField(parentId, await ExtractIdAsync(tableName, item)));
                var data = await GetPagingAsync(
                    req,
                    cancellationToken: cancellationToken);
                item.Add(new JProperty(rel.DisplayName, ReflectionHelper.ParseObject(data)));
                return;
            }
            throw new TaskCanceledException();
        }

        private async Task LoadManyToMany(string tableName, JObject item, MixDbTableRelationshipViewModel rel, SearchMixDbRequestModel req, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var relQuery = new List<QueryField>() {
                        new QueryField(_fieldNameService.ParentDatabaseName, rel.SourceTableName),
                        new QueryField(_fieldNameService.ChildDatabaseName, rel.DestinateTableName)
                };
                var parentDb = await GetMixDb(rel.SourceTableName);
                var childDb = await GetMixDb(rel.DestinateTableName);

                if (parentDb.Type == MixDbTableType.GuidService)
                {
                    relQuery.Add(new(_fieldNameService.GuidParentId, await ExtractIdAsync(rel.SourceTableName, item)));
                }
                else
                {
                    relQuery.Add(new(_fieldNameService.ParentId, await ExtractIdAsync(rel.DestinateTableName, item)));
                }

                var allowsRels = await GetListByAsync(GetRelationshipDbName(), relQuery);
                if (allowsRels != null)
                {

                    req.Queries ??= new List<MixQueryField>();
                    req.Queries.Add(
                        new(
                            _fieldNameService.Id,
                            childDb.Type == MixDbTableType.GuidService
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
                TableName = m.DestinateTableName,
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
                Dictionary<string, object> dicObj = await _dataParser.ParseDtoToEntityAsync(obj, createdBy);

                var fields = dicObj!.Keys.Select(m => new Field(m)).ToList();
                var result = await _repository.InsertAsync(tableName, dicObj, cancellationToken: cancellationToken);
                _repository.CompleteTransaction();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error when creating data in table {tableName}");
                _repository.RollbackTransaction();
                throw new MixException(MixErrorStatus.Badrequest, ex);
            }
        }


        public async Task CreateManyAsync(string tableName, List<JObject> entities, string? createdBy = null, CancellationToken cancellationToken = default)
        {
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
                    var dicObj = await _dataParser.ParseDtoToEntityAsync(entity, createdBy);
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
                _logger.LogError(ex, $"Error when creating multiple data in table {tableName}");
                _repository.RollbackTransaction();
                throw new MixException(MixErrorStatus.ServerError, ex);
            }
        }

        public async Task CreateDataRelationshipAsync(string tableName, CreateDataRelationshipDto obj, string? createdBy = null, CancellationToken cancellationToken = default)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error when creating data relationship in table {tableName}");
                throw;
            }
        }
        #endregion

        #region UPDATE

        public async Task<int?> UpdateManyAsync(string tableName, List<JObject> entities, string? modifiedBy = null, CancellationToken cancellationToken = default)
        {
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
                    var dicObj = await _dataParser.ParseDtoToEntityAsync(entity, modifiedBy);
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
                _logger.LogError(ex, $"Error when updating multiple data in table {tableName}");
                _repository.RollbackTransaction();
                throw new MixException(MixErrorStatus.ServerError, ex);
            }
        }

        public async Task<object?> UpdateAsync(string tableName, object id, JObject entity, string? modifiedBy = null, IEnumerable<string>? fieldNames = default,
            CancellationToken cancellationToken = default)
        {
            try
            {
                await LoadMixDb(tableName);
                _repository.BeginTransaction();
                QueryField idQuery = new QueryField(_fieldNameService.Id, id);
                entity[_fieldNameService.Id] = JToken.FromObject(id);
                var result = await _repository.UpdateAsync(tableName, idQuery,
                    await _dataParser.ParseDtoToEntityAsync(entity, modifiedBy), fieldNames: fieldNames, cancellationToken: cancellationToken
                    );
                var cacheFolder = GetCacheFolder(tableName);
                await _cacheSrv.RemoveCacheAsync(id, cacheFolder, cancellationToken);
                _repository.CompleteTransaction();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error when updating data in table {tableName}");
                _repository.RollbackTransaction();
                throw;
            }
        }
        #endregion

        #region DELETE
        public async Task<int> DeleteAsync(string tableName, object id, string? modifiedBy = null, CancellationToken cancellationToken = default)
        {
            try
            {
                await LoadMixDb(tableName);
                _repository.BeginTransaction();
                QueryField idQuery = new QueryField(_fieldNameService.Id, ParseId(id, _mixDb));
                return await _repository.DeleteAsync(tableName, idQuery);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error when deleting data from table {tableName}");
                _repository.RollbackTransaction();
                throw;
            }
        }

        public async Task<int> DeleteManyAsync(string tableName, List<MixQueryField> queries, string? modifiedBy = null, CancellationToken cancellationToken = default)
        {
            try
            {
                await LoadMixDb(tableName);
                _repository.BeginTransaction();
                return await _repository.DeleteAsync(tableName, ParseSearchQuery(queries), cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error when deleting multiple data from table {tableName}");
                _repository.RollbackTransaction();
                throw;
            }
        }

        public async Task DeleteDataRelationshipAsync(string tableName, int id, string? modifiedBy = null, CancellationToken cancellationToken = default)
        {
            try
            {
                await LoadMixDb(tableName);
                await _repository.DeleteAsync(GetRelationshipDbName(), new QueryField(_fieldNameService.Id, id), cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error when deleting data relationship from table {tableName}");
                throw;
            }
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
            return field.CompareOperator switch
            {
                MixCompareOperator.InRange => Operation.In,
                MixCompareOperator.ILike or MixCompareOperator.Like or MixCompareOperator.Contain => Operation.Like,
                MixCompareOperator.NotEqual => Operation.NotEqual,
                MixCompareOperator.NotContain or MixCompareOperator.NotInRange => Operation.NotIn,
                MixCompareOperator.GreaterThanOrEqual => Operation.GreaterThanOrEqual,
                MixCompareOperator.GreaterThan => Operation.GreaterThan,
                MixCompareOperator.LessThanOrEqual => Operation.LessThanOrEqual,
                MixCompareOperator.LessThan => Operation.LessThan,
                MixCompareOperator.Equal or _ => Operation.Equal
            };
        }

        private string GetRelationshipDbName()
        {
            string relName = _mixDb.NamingConvention == MixDatabaseNamingConvention.SnakeCase
                            ? MixDbTableNames.DATA_RELATIONSHIP_SNAKE_CASE
                            : MixDbTableNames.DATA_RELATIONSHIP_TITLE_CASE;
            return _mixDb.MixDbDatabaseId.HasValue
                        ? $"{_mixDb.MixDbDatabase.SystemName}_{relName}"
                        : MixDbTableNames.SYSTEM_DATA_RELATIONSHIP;
        }
        public async Task<object> ExtractIdAsync(string tableName, JObject obj)
        {
            await LoadMixDb(tableName);
            return _mixDb.Type == MixDbTableType.GuidService
                ? obj.GetJObjectProperty<Guid>(_fieldNameService.Id)
                : obj.GetJObjectProperty<int>(_fieldNameService.Id);
        }

        private object ParseId(object strId, MixDbTableViewModel mixDb)
        {
            return mixDb.Type == MixDbTableType.GuidService
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

        private async Task LoadMixDb(string tableName)
        {
            await _mixDbInfoService.LoadMixDb(tableName);
            _mixDb = await _mixDbInfoService.GetMixDb(tableName);
            _fieldNameService = new FieldNameService(_mixDb.NamingConvention);
            await _dataParser.LoadMixDb(tableName);
        }

        public async Task<MixDbTableViewModel?> GetMixDb(string tableName)
        {
            return await _mixDbInfoService.GetMixDb(tableName);
        }

        #endregion

        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}

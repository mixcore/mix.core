using Microsoft.AspNetCore.Http;
using Mix.Constant.Enums;
using Mix.Database.Entities.Cms;
using Mix.Heart.Models;
using Mix.Heart.UnitOfWork;
using Mix.Service.Services;
using Mix.Shared.Dtos;
using Newtonsoft.Json.Linq;
using RepoDb;
using Mix.Service.Interfaces;
using Mix.Heart.Services;
using Mix.Lib.Interfaces;
using Mix.Mixdb.Dtos;
using Mix.Mixdb.ViewModels;
using Mix.Mixdb.Interfaces;
using Mix.Database.Base;
using Mix.Scylladb.Repositories;
using Mix.Shared.Models;
using Mix.Heart.Model;
using Mix.Heart.Enums;
using Mix.RepoDb.ViewModels;
using Mix.Constant.Constants;
using NuGet.Protocol;
using Mix.Heart.Exceptions;
using Mix.Heart.Extensions;
using Mix.Heart.Helpers;
using Mix.Shared.Services;
using Newtonsoft.Json;
using Org.BouncyCastle.Security;
using Mix.Mixdb.Helpers;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Threading;
using MySqlX.XDevAPI.Common;
using Mix.Database.Services.MixGlobalSettings;
using Microsoft.Extensions.Configuration;
using Mix.Lib.Extensions;

namespace Mix.Mixdb.Services
{
    public class ScylladbDataService : TenantServiceBase, IMixDbDataService
    {
        #region Properties
        public MixDatabaseProvider DbProvider { get => MixDatabaseProvider.SCYLLADB; }
        private IDatabaseConstants _databaseConstant = new CassandraDatabaseConstants();
        private ScylladbRepository _repository;
        private readonly IMixMemoryCacheService _memoryCache;
        private MixDbDatabaseViewModel? _mixDb;
        private FieldNameService _fieldNameService;
        private readonly IConfiguration _configuration;
        private UnitOfWorkInfo<MixCmsContext> _cmsUow;
        private MixCacheService _cacheSrv;
        #endregion

        public ScylladbDataService(
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            UnitOfWorkInfo<MixCmsContext> uow,
            DatabaseService databaseService,
            IMixMemoryCacheService memoryCache,
            MixCacheService cacheService,
            IMixTenantService mixTenantService,
            MixCacheService cacheSrv,
            ScylladbRepository repository)
            : base(httpContextAccessor, cacheService, mixTenantService)
        {
            _configuration = configuration;
            _cmsUow = uow;
            _memoryCache = memoryCache;
            _cacheSrv = cacheSrv;
            _repository = repository;
        }

        #region Methods
        #endregion
        #region Implements

        #region CREATE
        public async Task<object> CreateAsync(string tableName, JObject obj, string? createdBy = null, CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await LoadMixDb(tableName);
                var entity = await ParseDtoToEntityAsync(obj, createdBy);
                await _repository.InsertAsync(tableName, entity, cancellationToken);
                return entity[_fieldNameService.Id];
            }
            throw new TaskCanceledException();
        }

        public async Task CreateManyAsync(string tableName, List<JObject> entities, string? createdBy = null, CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
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

                await _repository.InsertManyAsync(
                        _mixDb.SystemName,
                        dicObjs,
                        cancellationToken: cancellationToken
                        );
                return;
            }
            throw new TaskCanceledException();
        }

        public async Task CreateDataRelationshipAsync(string tableName, CreateDataRelationshipDto obj, string? createdBy = null, CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
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
                    { _fieldNameService.ChildId, obj.ChildId }
                };
                await _repository.InsertAsync(relDbName, rel, cancellationToken: cancellationToken);
                return;
            }
            throw new TaskCanceledException();
        }
        #endregion

        #region GET

        #region Get Single

        public async Task<JObject?> GetByIdAsync(string tableName, object objId, string? selectColumns, CancellationToken cancellationToken)
        {
            return await GetSingleByAsync(tableName, new MixQueryField(_fieldNameService.Id, objId, MixCompareOperator.Equal), selectColumns, cancellationToken);
        }

        public async Task<JObject?> GetSingleByAsync(string tableName, MixQueryField query, string? selectColumns, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await LoadMixDb(tableName);
                var result = await _repository.GetSingleByAsync(tableName, query, cancellationToken: cancellationToken);
                if (result != null)
                {
                    await ParseEntity(result, null, _mixDb, cancellationToken);
                }
                return result;
            }
            throw new TaskCanceledException();
        }

        public async Task<JObject?> GetSingleByAsync(string tableName, List<MixQueryField> queries, string? selectColumns, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await LoadMixDb(tableName);
                var result = await _repository.GetSingleByAsync(tableName, queries, cancellationToken: cancellationToken);
                await ParseEntity(result, null, _mixDb, cancellationToken);

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
        public async Task<List<JObject>?> GetAllAsync(
           SearchMixDbRequestModel request,
            CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await LoadMixDb(request.TableName);
                var data = await _repository.GetAllAsync(request.TableName,
                    await ParseFieldName(request.SelectColumns, request.TableName), 
                    cancellationToken: cancellationToken);
                if (data != null && request.RelatedDataRequests != null)
                {
                    foreach (var item in data)
                    {
                        await ParseEntity(item, request.RelatedDataRequests, _mixDb, cancellationToken);
                    }
                }
                return data;
            }
            throw new TaskCanceledException();
        }

        public async Task<PagingResponseModel<JObject>> GetMyDataAsync(string tableName, SearchMixDbRequestDto req, string username, CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var paging = new PagingRequestModel(req);

                req.Queries ??= new();
                req.Queries.Add(new(_fieldNameService.CreatedBy, username, MixCompareOperator.Equal));
                return await _repository.GetPagingAsync(
                                tableName,
                                req.Queries,
                                paging,
                                req.Conjunction);
            }
            throw new TaskCanceledException();
        }

        public async Task<List<JObject>?> GetListByAsync(
            SearchMixDbRequestModel request, CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await LoadMixDb(request.TableName);
                var data = await _repository.GetListAsync(request.TableName, request.Queries,
                    request.Conjunction,
                    request.Paging.SortByColumns,
                    await ParseFieldName(request.SelectColumns, request.TableName), cancellationToken: cancellationToken);
                if (data != null && request.RelatedDataRequests != null)
                {
                    foreach (var item in data)
                    {
                        await ParseEntity(item, null, _mixDb, cancellationToken);
                    }
                }
                return data;
            }
            throw new TaskCanceledException();
        }

        public async Task<PagingResponseModel<JObject>> GetPagingAsync(
            SearchMixDbRequestModel request,
            CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var data = await _repository.GetPagingAsync(
                request.TableName,
                request.Queries,
                request.Paging,
                request.Conjunction,
                await ParseFieldName(request.SelectColumns, request.TableName), cancellationToken: cancellationToken);
                if (request.RelatedDataRequests != null)
                {
                    foreach (var item in data.Items)
                    {
                        await ParseEntity(item, request.RelatedDataRequests, _mixDb, cancellationToken);
                    }
                }
                return data;
            }
            throw new TaskCanceledException();
        }

        private async Task ParseEntity(JObject? obj, List<SearchMixDbRequestModel>? relatedDataRequests, MixDbDatabaseViewModel? mixDb, CancellationToken cancellationToken)
        {
            if (mixDb != null && obj != null)
            {
                MixDbHelper.ParseRawDataToEntityAsync(obj, mixDb.Columns);
                if (relatedDataRequests != null)
                {
                    await LoadNestedDataAsync(mixDb.SystemName, obj, relatedDataRequests, cancellationToken);
                }
            }
        }

        private async Task<IEnumerable<string>?> ParseFieldName(string? selectFieldNames, string tableName)
        {
            await LoadMixDb(tableName);
            var fields = selectFieldNames?.Split(',') ?? _mixDb.Columns.Select(m => m.SystemName).ToArray();
            return fields;
        }

        public async Task<List<JObject>?> GetListByAsync(
            string tableName,
            List<MixQueryField> queryFields,
            List<MixSortByColumn>? sortByFields = null,
            MixConjunction conjunction = MixConjunction.And,
            string? selectFields = null,
            bool loadNestedData = false,
            CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await LoadMixDb(tableName);
                var data = await _repository.GetListAsync(tableName, queryFields, conjunction, sortByFields,
                    await ParseFieldName(selectFields, tableName), cancellationToken: cancellationToken);
                if (data != null && loadNestedData)
                {
                    foreach (var item in data)
                    {
                        await LoadNestedDataAsync(tableName, item, null, cancellationToken);
                    }
                }
                return data;
            }
            throw new TaskCanceledException();
        }

        public async Task<List<JObject>> GetListByParentAsync(SearchMixDbRequestModel request, MixContentType parentType, object parentId, CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await LoadMixDb(request.TableName);
                return await _repository.GetListAsync(request.TableName, new List<MixQueryField>() {
                    new MixQueryField(_fieldNameService.ParentType, parentType.ToString()),
                    new MixQueryField(_fieldNameService.ParentId, parentId)
                    }, MixConjunction.And,
                    cancellationToken: cancellationToken);
            }
            throw new TaskCanceledException();
        }

        #endregion

        #endregion

        #region UPDATE


        public string GetCacheFolder(string databaseName)
        {
            return $"{MixFolders.MixDbCacheFolder}/{databaseName}";
        }


        public async Task<object?> UpdateAsync(string tableName, object objId, JObject entity, string? modifiedBy = null, IEnumerable<string>? fieldNames = default, CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (!Guid.TryParse(objId.ToString(), out var id))
                {
                    throw new InvalidParameterException();
                }
                await LoadMixDb(tableName);
                await _repository.UpdateAsync(tableName, id, await ParseDtoToEntityAsync(entity, modifiedBy));
                var cacheFolder = GetCacheFolder(tableName);
                await _cacheSrv.RemoveCacheAsync(id, cacheFolder, cancellationToken);

                return entity;
            }
            throw new TaskCanceledException();
        }

        public Task<int?> UpdateManyAsync(string tableName, List<JObject> entities, string? modifiedBy = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteManyAsync(string tableName, List<MixQueryField> queries, string? modifiedBy = null, CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await LoadMixDb(tableName);
                var data = await _repository.GetListAsync(tableName, queries, MixConjunction.And, selectFields: new[] { _fieldNameService.Id });
                foreach (var item in data)
                {
                    await _repository.DeleteAsync(tableName, item.Value<Guid>(_fieldNameService.Id));
                }
                return 1;
            }
            throw new TaskCanceledException();
        }

        public async Task<int> DeleteAsync(string tableName, object objId, string? modifiedBy = null, CancellationToken cancellationToken = default)
        {
            if (!Guid.TryParse(objId.ToString(), out var id))
            {
                throw new InvalidParameterException();
            }
            while (!cancellationToken.IsCancellationRequested)
            {
                await LoadMixDb(tableName);
                await _repository.DeleteAsync(tableName, id);
                return 1;
            }
            throw new TaskCanceledException();
        }

        public Task DeleteDataRelationshipAsync(string tableName, int id, string? modifiedBy = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region PARSER

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
                            result.Add(colName, pr.Value);
                        }
                    }
                }

                if (!result.ContainsKey(_fieldNameService.Id))
                {
                    result.Add(_fieldNameService.Id, Guid.NewGuid());
                }
                //else
                //{
                //    obj[_fieldNameService.ModifiedBy] = username;
                //    obj[_fieldNameService.LastModified] = DateTime.UtcNow;
                //}
                if (!result.ContainsKey(_fieldNameService.CreatedBy) && !string.IsNullOrEmpty(username))
                {
                    result.Add(_fieldNameService.CreatedBy, username);
                }
                //if (!obj.ContainsKey(_fieldNameService.CreatedDateTime))
                //{
                //    obj.Add(new JProperty(_fieldNameService.CreatedDateTime, DateTime.UtcNow));
                //}
                //if (!obj.ContainsKey(_fieldNameService.CreatedBy))
                //{
                //    obj.Add(new JProperty(_fieldNameService.CreatedBy, username));
                //}

                //if (!obj.ContainsKey(_fieldNameService.Priority))
                //{
                //    obj.Add(new JProperty(_fieldNameService.Priority, 0));
                //}

                //if (!obj.ContainsKey(_fieldNameService.TenantId))
                //{
                //    obj.Add(new JProperty(_fieldNameService.TenantId, tenantId));
                //}

                //if (!obj.ContainsKey(_fieldNameService.Status))
                //{
                //    obj.Add(new JProperty(_fieldNameService.Status, MixContentStatus.Published.ToString()));
                //}

                //if (!obj.ContainsKey(_fieldNameService.IsDeleted))
                //{
                //    obj.Add(new JProperty(_fieldNameService.IsDeleted, false));
                //}
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

        public MixCompareOperator ParseMixCompareOperator(ExpressionMethod? searchMethod)
        {
            throw new NotImplementedException();
        }

        public MixCompareOperator ParseMixCompareOperator(string tableName, ExpressionMethod? searchMethod)
        {
            throw new NotImplementedException();
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

        #endregion

        #region Connection

        public void SetDbConnection(UnitOfWorkInfo<MixCmsContext> dbUow)
        {
            _cmsUow = dbUow;
        }

        public void Init(string connectionString)
        {
            _repository.Connect(connectionString);
        }
        public void Dispose()
        {
            _repository?.Disconnect();
        }
        #endregion

        #endregion

        #region Privates

        #region Load Nested Data

        public async Task LoadNestedDataAsync(string tableName, JObject item, List<SearchMixDbRequestModel> relatedDataRequests, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                foreach (var rel in _mixDb.Relationships)
                {
                    if (rel.Type == MixDatabaseRelationshipType.OneToMany)
                    {
                        await LoadOneToMany(tableName, item, rel, cancellationToken);
                    }
                    if (rel.Type == MixDatabaseRelationshipType.ManyToMany)
                    {
                        await LoadManyToMany(tableName, item, rel, cancellationToken);
                    }
                }
                return;
            }
            throw new TaskCanceledException();
        }

        private async Task LoadOneToMany(string tableName, JObject item, MixDatabaseRelationshipViewModel rel, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var queries = new List<MixQueryField>()
                    {
                        new MixQueryField(_fieldNameService.ParentDatabaseName, tableName),
                        new MixQueryField(_fieldNameService.ParentId, await ExtractIdAsync(tableName, item)),
                    };
                var data = await GetListByAsync(
                    GetRelationshipDbName(),
                    queries,
                    cancellationToken: cancellationToken);
                item.Add(new JProperty(rel.DisplayName, data));
                return;
            }
            throw new TaskCanceledException();
        }

        private async Task LoadManyToMany(string tableName, JObject item, MixDatabaseRelationshipViewModel rel, CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var relQuery = new List<MixQueryField>() {
                        new MixQueryField(_fieldNameService.ParentDatabaseName, rel.SourceDatabaseName),
                        new MixQueryField(_fieldNameService.ChildDatabaseName, rel.DestinateDatabaseName)
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

                    var queries = new List<MixQueryField>() {
                        new(
                            _fieldNameService.Id,
                            childDb.Type == MixDatabaseType.GuidService
                            ? allowsRels.Select(m => m.Value<int>(_fieldNameService.GuidChildId)).ToList()
                            : allowsRels.Select(m => m.Value<int>(_fieldNameService.ChildId)).ToList(),
                            MixCompareOperator.InRange
                    )};
                    var data = await GetListByAsync(
                        GetRelationshipDbName(),
                        queries,
                        cancellationToken: cancellationToken);
                    item.Add(new JProperty(rel.DisplayName, data));
                }
                return;
            }
            throw new TaskCanceledException();
        }
        #endregion

        private string GetRelationshipDbName()
        {
            string relName = _mixDb.NamingConvention == MixDatabaseNamingConvention.SnakeCase
                            ? MixDatabaseNames.DATA_RELATIONSHIP_SNAKE_CASE
                            : MixDatabaseNames.DATA_RELATIONSHIP_TITLE_CASE;
            return _mixDb.MixDatabaseContextId.HasValue
                        ? $"{_mixDb.MixDatabaseContext.SystemName}_{relName}"
                        : MixDatabaseNames.SYSTEM_DATA_RELATIONSHIP;
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
                    return MixDbDatabaseViewModel.GetRepository(_cmsUow, _cacheSrv).GetSingleAsync(m => m.SystemName == tableName);
                }
                ) ?? throw new NullReferenceException(tableName);
        }

        public async Task<object> ExtractIdAsync(string tableName, JObject obj)
        {
            await LoadMixDb(tableName);
            return _mixDb.Type == MixDatabaseType.GuidService
                ? obj.GetJObjectProperty<Guid>(_fieldNameService.Id)
                : obj.GetJObjectProperty<int>(_fieldNameService.Id);
        }

        public Task<List<JObject>?> GetListByAsync(string tableName, IEnumerable<MixQueryField> searchQueryFields, List<MixSortByColumn>? sortByFields = null, string? fields = null, bool loadNestedData = false, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<JObject>?> GetListByParentAsync(string tableName, MixContentType parentType, object parentId, List<MixSortByColumn>? sortByFields = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }


        #endregion
    }
}

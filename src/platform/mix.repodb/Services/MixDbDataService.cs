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

namespace Mix.RepoDb.Services
{
    public class MixDbDataService : TenantServiceBase, IMixDbDataService
    {
        private readonly MixRepoDbRepository _repository;
        private readonly MixRepoDbRepository _associationRepository;
        private readonly IMixMemoryCacheService _memoryCache;

        #region Properties

        private readonly UnitOfWorkInfo<MixCmsContext> _cmsUow;

        private const string CreatedDateFieldName = "CreatedDateTime";
        private const string CreatedByFieldName = "CreatedBy";
        private const string PriorityFieldName = "Priority";
        private const string IdFieldName = "Id";
        private const string ParentIdFieldName = "ParentId";
        private const string ChildIdFieldName = "ChildId";
        private const string TenantIdFieldName = "MixTenantId";
        private const string StatusFieldName = "Status";
        private const string IsDeletedFieldName = "IsDeleted";

        #endregion

        public MixDbDataService(
            IHttpContextAccessor httpContextAccessor,
            UnitOfWorkInfo<MixCmsContext> uow,
            DatabaseService databaseService,
            MixRepoDbRepository repository,
            ICache cache,
            IMixMemoryCacheService memoryCache)
            : base(httpContextAccessor)
        {
            _cmsUow = uow;
            _repository = repository;
            _associationRepository = new MixRepoDbRepository(cache, databaseService, uow);
            _associationRepository.InitTableName(nameof(MixDatabaseAssociation));
            _memoryCache = memoryCache;
        }

        #region Methods

        #region CRUD

        public async Task<JObject?> GetSingleByParent(string tableName, MixContentType parentType, object parentId, bool loadNestedData = false)
        {
            _repository.InitTableName(tableName);
            var data = await _repository.GetSingleByParentAsync(parentType, parentId);
            if (data != null)
            {
                JObject result = ReflectionHelper.ParseObject(data);
                if (loadNestedData)
                {
                    await LoadNestedData(tableName, result);
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
            queries.Add(new(CreatedByFieldName, Operation.Equal, username));
            return await GetResult(tableName, queries, paging, req.LoadNestedData);
        }

        public async Task<JObject> GetMyDataById(string tableName, string username, int id, bool loadNestedData)
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
                var database = await GetMixDatabase(tableName);
                foreach (var item in database.Relationships)
                {
                    if (loadNestedData)
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
                    }
                    else
                    {
                        data.Add(new JProperty($"{item.DisplayName}Url", $"{CurrentTenant.Configurations.Domain}/api/v2/rest/mix-portal/mix-db/{item.DestinateDatabaseName}?ParentId={id}&ParentName={item.SourceDatabaseName}"));
                    }
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
                    await LoadNestedData(tableName, data);
                }
                return data;
            }
            return default;
        }


        public async Task<JObject?> GetSingleBy(string tableName, List<QueryField> queries)
        {
            _repository.InitTableName(tableName);
            var obj = await _repository.GetSingleByAsync(queries);
            if (obj != null)
            {
                return ReflectionHelper.ParseObject(obj);
            }
            return default;
        }


        public async Task<int> CreateData(string tableName, JObject data)
        {
            _repository.InitTableName(tableName);
            JObject obj = new JObject();
            foreach (var pr in data.Properties())
            {
                obj.Add(new JProperty(pr.Name.ToTitleCase(), pr.Value));
            }
            if (!obj.ContainsKey(CreatedDateFieldName))
            {
                obj.Add(new JProperty(CreatedDateFieldName, DateTime.UtcNow));
            }
            if (!obj.ContainsKey(PriorityFieldName))
            {
                obj.Add(new JProperty(PriorityFieldName, 0));
            }
            if (!obj.ContainsKey(TenantIdFieldName))
            {
                obj.Add(new JProperty(TenantIdFieldName, CurrentTenant?.Id ?? 1));
            }

            if (!obj.ContainsKey(StatusFieldName))
            {
                obj.Add(new JProperty(StatusFieldName, MixContentStatus.Published.ToString()));
            }

            if (!obj.ContainsKey(IsDeletedFieldName))
            {
                obj.Add(new JProperty(IsDeletedFieldName, false));
            }
            return await _repository.InsertAsync(obj);
        }


        #endregion

        #region Helper

        public static string GetCacheFolder(string databaseName)
        {
            return $"{MixFolders.MixDbCacheFolder}/{databaseName}";
        }

        private async Task LoadNestedData(string tableName, JObject data)
        {
            var database = await GetMixDatabase(tableName);
            foreach (var item in database.Relationships)
            {
                List<QueryField> queries = GetAssociationQueries(item.SourceDatabaseName, item.DestinateDatabaseName, data.Value<int>("id"));
                var associations = await _associationRepository.GetListByAsync(queries);
                if (associations is { Count: > 0 })
                {
                    var nestedIds = JArray.FromObject(associations).Select(m => m.Value<int>(ChildIdFieldName)).ToList();
                    _repository.InitTableName(item.DestinateDatabaseName);
                    List<QueryField> query = new() { new(IdFieldName, Operation.In, nestedIds) };
                    var nestedData = await _repository.GetListByAsync(query);
                    data.Add(new JProperty(item.DisplayName, ReflectionHelper.ParseArray(nestedData)));
                }
            }
        }


        private async Task<PagingResponseModel<JObject>> GetResult(string tableName, IEnumerable<QueryField> queries, PagingRequestModel paging, bool loadNestedData)
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

        #endregion

        #region Private

        public void Dispose()
        {
            _repository.Dispose();
            _cmsUow.Dispose();
        }
        #endregion
    }
}

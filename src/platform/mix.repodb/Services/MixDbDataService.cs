using Microsoft.AspNetCore.Http;
using Mix.Constant.Enums;
using Mix.Database.Base;
using Mix.Database.Entities.Cms;
using Mix.Database.Services;
using Mix.Heart.Enums;
using Mix.Heart.Helpers;
using Mix.Heart.Models;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Base;
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

namespace Mix.RepoDb.Services
{
    public class MixDbDataService : TenantServiceBase
    {
        private readonly IDatabaseConstants _databaseConstant;
        private readonly MixRepoDbRepository _repository;
        private readonly MixRepoDbRepository _associationRepository;
        private readonly MixMemoryCacheService _memoryCache;

        #region Properties

        private readonly UnitOfWorkInfo<MixCmsContext> _cmsUOW;
        private readonly DatabaseService _databaseService;

        private const string createdDateFieldName = "CreatedDateTime";
        private const string createdByFieldName = "CreatedBy";
        private const string priorityFieldName = "Priority";
        private const string idFieldName = "Id";
        private const string parentIdFieldName = "ParentId";
        private const string childIdFieldName = "ChildId";
        private const string tenantIdFieldName = "MixTenantId";
        private const string statusFieldName = "Status";
        private const string isDeletedFieldName = "IsDeleted";

        #endregion

        public MixDbDataService(
            IHttpContextAccessor httpContextAccessor,
            UnitOfWorkInfo<MixCmsContext> uow,
            DatabaseService databaseService,
            MixRepoDbRepository repository,
            ICache cache,
            MixMemoryCacheService memoryCache)
            : base(httpContextAccessor)
        {
            _cmsUOW = uow;
            _databaseService = databaseService;
            _repository = repository;
            _associationRepository = new MixRepoDbRepository(cache, databaseService, uow);
            _associationRepository.InitTableName(nameof(MixDatabaseAssociation));
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

        public async Task<JObject?> GetSingleByParent(string tableName, MixContentType parentType, object parentId, bool loadNestedData = false)
        {
            _repository.InitTableName(tableName);
            var data = await _repository.GetSingleByParentAsync(parentType, parentId);
            if (data != null)
            {
                JObject result = ReflectionHelper.ParseObject(data);
                if (loadNestedData) {
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
            queries.Add(new(createdByFieldName, Operation.Equal, username));
            return await GetResult(tableName, queries, paging, req.LoadNestedData);
        }

        public async Task<JObject> GetMyDataById(string tableName, string username, int id, bool loadNestedData)
        {
            _repository.InitTableName(tableName);
            var queries = new List<QueryField>()
            {
                new QueryField(tenantIdFieldName, CurrentTenant.Id),
                new QueryField(idFieldName, id),
                new QueryField(createdByFieldName, username)
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

                        List<QueryField> associationQueries = GetAssociatoinQueries(item.SourceDatabaseName, item.DestinateDatabaseName, id);
                        var associations = await _associationRepository.GetListByAsync(associationQueries);
                        if (associations.Count > 0)
                        {
                            var nestedIds = JArray.FromObject(associations).Select(m => m.Value<int>(childIdFieldName)).ToList();
                            _repository.InitTableName(item.DestinateDatabaseName);
                            List<QueryField> query = new() { new(idFieldName, Operation.In, nestedIds) };
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
            if (!obj.ContainsKey(createdDateFieldName))
            {
                obj.Add(new JProperty(createdDateFieldName, DateTime.UtcNow));
            }
            if (!obj.ContainsKey(priorityFieldName))
            {
                obj.Add(new JProperty(priorityFieldName, 0));
            }
            if (!obj.ContainsKey(tenantIdFieldName))
            {
                obj.Add(new JProperty(tenantIdFieldName, CurrentTenant?.Id ?? 1));
            }

            if (!obj.ContainsKey(statusFieldName))
            {
                obj.Add(new JProperty(statusFieldName, MixContentStatus.Published.ToString()));
            }

            if (!obj.ContainsKey(isDeletedFieldName))
            {
                obj.Add(new JProperty(isDeletedFieldName, false));
            }
            return await _repository.InsertAsync(obj);
        }


        #endregion

        #region Helper
        private async Task LoadNestedData(string tableName, JObject data)
        {
            var database = await GetMixDatabase(tableName);
            foreach (var item in database.Relationships)
            {

                List<QueryField> queries = GetAssociatoinQueries(item.SourceDatabaseName, item.DestinateDatabaseName, data.Value<int>("id"));
                var associations = await _associationRepository.GetListByAsync(queries);
                if (associations.Count > 0)
                {
                    var nestedIds = JArray.FromObject(associations).Select(m => m.Value<int>(childIdFieldName)).ToList();
                    _repository.InitTableName(item.DestinateDatabaseName);
                    List<QueryField> query = new() { new(idFieldName, Operation.In, nestedIds) };
                    var nestedData = await _repository.GetListByAsync(query);
                    data.Add(new JProperty(item.DisplayName, ReflectionHelper.ParseArray(nestedData)));
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

                        List<QueryField> nestedQueries = GetAssociatoinQueries(rel.SourceDatabaseName, rel.DestinateDatabaseName, id);
                        var associations = await _associationRepository.GetListByAsync(nestedQueries);
                        if (associations.Count > 0)
                        {
                            var nestedIds = JArray.FromObject(associations).Select(m => m.Value<int>(childIdFieldName)).ToList();
                            _repository.InitTableName(rel.DestinateDatabaseName);
                            List<QueryField> query = new() { new(idFieldName, Operation.In, nestedIds) };
                            var nestedData = await _repository.GetListByAsync(query);
                            data.Add(new JProperty(rel.DisplayName, ReflectionHelper.ParseArray(nestedData)));
                        }
                    }
                }
                items.Add(data);
            }
            return new PagingResponseModel<JObject> { Items = items, PagingData = result.PagingData };
        }

        private List<QueryField> GetAssociatoinQueries(string parentDatabaseName = null, string childDatabaseName = null, int? parentId = null, int? childId = null)
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
                queries.Add(new QueryField(parentIdFieldName, parentId));
            }
            if (childId.HasValue)
            {
                queries.Add(new QueryField(childIdFieldName, parentId));
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
                    return MixDatabaseViewModel.GetRepository(_cmsUOW).GetSingleAsync(m => m.SystemName == tableName);
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
                    queries.Add(new(parentIdFieldName, request.ParentId));
                }
                else
                {
                    var allowsIds = _cmsUOW.DbContext.MixDatabaseAssociation
                            .Where(m => m.ParentDatabaseName == request.ParentName && m.ParentId == request.ParentId.Value && m.ChildDatabaseName == tableName)
                            .Select(m => m.ChildId).ToList();
                    queries.Add(new(idFieldName, Operation.In, allowsIds));
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
                new QueryField(tenantIdFieldName, CurrentTenant.Id)
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
            _cmsUOW.Dispose();
        }
        #endregion
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mix.RepoDb.Dtos;
using Mix.RepoDb.Repositories;
using Mix.Shared.Models;
using RepoDb;
using RepoDb.Enumerations;
using Mix.Heart.Extensions;
using System.Linq.Expressions;
using RepoDb.Interfaces;
using Mix.Database.Services;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-db/{name}")]
    [ApiController]
    public class MixDbController : MixApiControllerBase
    {
        UnitOfWorkInfo<MixCmsContext> _cmsUOW;
        private readonly MixRepoDbRepository _repository;
        private readonly MixMemoryCacheService _memoryCache;
        private readonly MixRepoDbRepository _associationRepository;
        private readonly MixCmsContext _context;
        private string _tableName;
        private static string _associationTableName = nameof(MixDatabaseAssociation);
        public MixDbController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCmsContext context,
            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixIdentityService mixIdentityService,
            IQueueService<MessageQueueModel> queueService,
            MixRepoDbRepository repository,
            MixMemoryCacheService memoryCache,
            UnitOfWorkInfo<MixCmsContext> cmsUOW,
            ICache cache,
            DatabaseService databaseService)
            : base(httpContextAccessor, configuration, mixService, translator, cultureRepository, mixIdentityService, queueService)
        {
            _context = context;
            _repository = repository;
            _associationRepository = new(cache, databaseService, cmsUOW);
            _associationRepository.Init(_associationTableName);
            _cmsUOW = cmsUOW;
            _memoryCache = memoryCache;
        }

        #region Overrides

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _tableName = RouteData?.Values["name"].ToString();
            _repository.Init(_tableName);

            base.OnActionExecuting(context);
        }

        #endregion

        [HttpGet]
        public async Task<ActionResult<PagingResponseModel<JObject>>> Get([FromQuery] SearchMixDbRequestDto req)
        {
            var result = await SearchHandler(req);

            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<JObject>> GetSingle(int id, [FromQuery] bool loadNestedData)
        {
            var obj = await _repository.GetAsync(id);
            if (obj != null)
            {
                var data = JObject.FromObject(obj);
                if (loadNestedData)
                {
                    var database = await GetMixDatabase();
                    foreach (var item in database.Relationships)
                    {
                        List<QueryField> queries = GetAssociatoinQueries(item.SourceDatabaseName, item.DestinateDatabaseName, id);
                        var associations = await _associationRepository.GetByAsync(queries);
                        if (associations.Count > 0)
                        {
                            var nestedIds = JArray.FromObject(associations).Select(m => m.Value<int>("ChildId")).ToList();
                            _repository.Init(item.DestinateDatabaseName);
                            List<QueryField> query = new() { new("id", Operation.In, nestedIds) };
                            var nestedData = await _repository.GetByAsync(query);
                            data.Add(new JProperty(item.DisplayName, JArray.FromObject(nestedData)));
                        }
                    }
                }
                return Ok(data);
            }
            throw new MixException(MixErrorStatus.NotFound, id);
        }


        [HttpPost]
        public async Task<ActionResult<object>> Create(JObject obj)
        {
            if (!obj.ContainsKey("createdDateTime"))
            {
                obj.Add(new JProperty("createdDateTime", DateTime.UtcNow));
            }
            var data = await _repository.InsertAsync(obj);

            return data > 0 ? Ok(await _repository.GetAsync(data)) : BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<object>> Update(int id, [FromBody] JObject obj)
        {
            var data = await _repository.UpdateAsync(obj);
            return data != null ? Ok(await _repository.GetAsync(id)) : BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<object>> Delete(int id)
        {
            var data = await _repository.DeleteAsync(id);
            //Expression<Func<MixDatabaseAssociation, bool>> associationPredicate = m => m.ParentDatabaseName == _tableName && m.ParentId == id;
            //associationPredicate = associationPredicate.Or(m => m.ChildDatabaseName == _tableName && m.ChildId == id);
            //await _associationRepository.DeleteManyAsync(associationPredicate);
            var childAssociationsQueries = GetAssociatoinQueries(parentDatabaseName: _tableName, parentId: id);
            var parentAssociationsQueries = GetAssociatoinQueries(childDatabaseName: _tableName, childId: id);
            _repository.Init(_associationTableName);
            await _repository.DeleteAsync(childAssociationsQueries);
            await _repository.DeleteAsync(parentAssociationsQueries);
            return data > 0 ? Ok() : NotFound();
        }


        #region Handler

        private async Task<PagingResponseModel<dynamic>> SearchHandler(SearchMixDbRequestDto request)
        {
            var queries = BuildSearchPredicate(request);
            return await _repository.GetPagingAsync(queries, new PagingRequestModel(Request));
        }

        private IEnumerable<QueryField> BuildSearchPredicate(SearchMixDbRequestDto req)
        {
            var queries = new List<QueryField>();
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
            if (req.ParentId.HasValue)
            {
                QueryField field = new QueryField($"{req.ParentName}Id", Operation.Equal, req.ParentId.Value);
                queries.Add(field);
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

        private ActionResult<PagingResponseModel<JObject>> ParseSearchResult(SearchRequestDto req, PagingResponseModel<JObject> result)
        {
            throw new NotImplementedException();
        }


        #endregion

        #region Private

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
                queries.Add(new QueryField("ParentId", parentId));
            }
            if (childId.HasValue)
            {
                queries.Add(new QueryField("ChildId", parentId));
            }
            return queries;
        }

        private async Task<MixDatabaseViewModel> GetMixDatabase()
        {
            return await _memoryCache.TryGetValueAsync(
                _tableName,
                cache =>
                {
                    cache.SlidingExpiration = TimeSpan.FromSeconds(20);
                    return MixDatabaseViewModel.GetRepository(_cmsUOW).GetSingleAsync(m => m.SystemName == _tableName);
                }
                );
        }


        #endregion
    }
}

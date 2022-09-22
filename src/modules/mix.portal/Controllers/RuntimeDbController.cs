using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mix.Shared.Models;
using Mix.Heart.Extensions;
using System.Linq.Expressions;
using Mix.Database.Services;
using Mix.Shared.Services;
using Microsoft.EntityFrameworkCore;
using Mix.Database.Repositories;
using Mix.Heart.Helpers;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/runtime-db/{name}")]
    [ApiController]
    public class RuntimeDbController : MixApiControllerBase
    {
        private UnitOfWorkInfo<MixCmsContext> _cmsUOW;
        private readonly RuntimeDbContextService _runtimeDbContextService;
        private RuntimeDbRepository _repository;
        private readonly MixMemoryCacheService _memoryCache;
        private readonly MixCmsContext _context;
        private string _tableName;
        public RuntimeDbController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCmsContext context,
            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixIdentityService mixIdentityService,
            IQueueService<MessageQueueModel> queueService,
            MixMemoryCacheService memoryCache,
            UnitOfWorkInfo<MixCmsContext> cmsUOW,
            DatabaseService databaseService,
            RuntimeDbContextService runtimeDbContextService)
            : base(httpContextAccessor, configuration, mixService, translator, cultureRepository, mixIdentityService, queueService)
        {
            _context = context;
            _cmsUOW = cmsUOW;
            _memoryCache = memoryCache;
            _runtimeDbContextService = runtimeDbContextService;

        }

        #region Overrides

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _tableName = RouteData?.Values["name"].ToString();
            _repository = new(_runtimeDbContextService.GetMixDatabaseDbContext(), _tableName);
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
            var obj = await _repository.GetSingleAsync(id);
            if (obj != null)
            {
                var data = JObject.FromObject(obj);
                var database = await GetMixDatabase();
                foreach (var item in database.Relationships)
                {
                    if (loadNestedData)
                    {

                        //List<SearchQueryField> queries = GetAssociatoinQueries(item.SourceDatabaseName, item.DestinateDatabaseName, id);
                        //var associations = _cmsUOW.DbContext.MixDatabaseAssociation
                        //    .Where(m => m.ParentDatabaseName == item.SourceDatabaseName
                        //        && m.ChildDatabaseName == item.DestinateDatabaseName && m.ParentId == id);
                        //if (associations.Count() > 0)
                        //{
                        //    var nestedIds = associations.Select(m => m.ChildId).ToList();
                        //    List<SearchQueryField> query = new() { new("id", Operation.In, nestedIds) };
                        //    var nestedData = _repository.GetListQuery(predicate);
                        //    data.Add(new JProperty(item.DisplayName, JArray.FromObject(nestedData)));
                        //}
                    }
                    else
                    {
                        data.Add(new JProperty($"{item.DisplayName}Url", $"{CurrentTenant.Configurations.Domain}/api/v2/rest/mix-portal/mix-db/{item.DestinateDatabaseName}?ParentId={id}&ParentName={item.SourceDatabaseName}"));
                    }
                }
                return Ok(data);
            }
            throw new MixException(MixErrorStatus.NotFound, id);
        }

        //[HttpGet("get-by-parent/{parentId}")]
        //public async Task<ActionResult<JObject>> GetSingleByParent(int parentId, [FromQuery] bool loadNestedData)
        //{
        //    dynamic obj = await _repository.GetSingleByParentAsync(parentId);
        //    if (obj != null)
        //    {
        //        var data = JObject.FromObject(obj);
        //        var database = await GetMixDatabase();
        //        foreach (var item in database.Relationships)
        //        {
        //            if (loadNestedData)
        //            {

        //                List<SearchQueryField> queries = GetAssociatoinQueries(item.SourceDatabaseName, item.DestinateDatabaseName, data.id);
        //                var associations = await _associationRepository.GetListByAsync(queries);
        //                if (associations.Count > 0)
        //                {
        //                    var nestedIds = JArray.FromObject(associations).Select(m => m.Value<int>("ChildId")).ToList();
        //                    _repository.Init(item.DestinateDatabaseName);
        //                    List<SearchQueryField> query = new() { new("id", Operation.In, nestedIds) };
        //                    var nestedData = await _repository.GetListByAsync(query);
        //                    data.Add(new JProperty(item.DisplayName, JArray.FromObject(nestedData)));
        //                }
        //            }
        //            else
        //            {
        //                data.Add(new JProperty($"{item.DisplayName}Url", $"{GlobalConfigService.Instance.Domain}/api/v2/rest/mix-portal/mix-db/{item.DestinateDatabaseName}?ParentId={data.id}&ParentName={item.SourceDatabaseName}"));
        //            }
        //        }
        //        return Ok(data);
        //    }
        //    return NotFound();
        //    //throw new MixException(MixErrorStatus.NotFound, id);
        //}


        [HttpPost]
        public async Task<ActionResult<object>> Create(JObject obj)
        {
            if (!obj.ContainsKey("createdDateTime"))
            {
                obj.Add(new JProperty("createdDateTime", DateTime.UtcNow));
            }
            var result = await _repository.CreateAsync(obj);

            return result != null ? Ok(result) : BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<object>> Update(int id, [FromBody] JObject obj)
        {
            var data = await _repository.UpdateAsync(obj);
            return data != null ? Ok(data) : BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<object>> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            var associations = _cmsUOW.DbContext.MixDatabaseAssociation
                .Where(m => (m.ParentDatabaseName == _tableName && m.ParentId == id)
                        || (m.ChildDatabaseName == _tableName && m.ChildId == id));
            _cmsUOW.DbContext.MixDatabaseAssociation.RemoveRange(associations);
            await _cmsUOW.CompleteAsync();
            return Ok();
        }


        #region Handler

        private async Task<PagingResponseModel<JObject>> SearchHandler(SearchMixDbRequestDto request)
        {
            var queries = BuildSearchPredicate(request).ToList();
            if (request.ParentId.HasValue)
            {
                var associations = _cmsUOW.DbContext.MixDatabaseAssociation
                    .Where(m => (m.ParentDatabaseName == request.ParentName && m.ParentId == request.ParentId.Value));
                if (associations.Count() > 0)
                {
                    var nestedIds = associations.Select(m => m.ChildId).ToArray();
                    queries.Add(new("id", string.Join(',', nestedIds), MixCompareOperator.Contain));
                }
            }
            return await _repository.GetPagingEntitiesAsync(queries, new PagingRequestModel(Request));
        }

        private IEnumerable<SearchQueryField> BuildSearchPredicate(SearchMixDbRequestDto req)
        {
            var queries = new List<SearchQueryField>();
            if (!string.IsNullOrEmpty(req.SearchColumns) && !string.IsNullOrEmpty(req.Keyword))
            {
                var searchColumns = req.SearchColumns.Replace(" ", string.Empty).Split(',');
                var operation = ParseSearchOperation(req.SearchMethod);

                foreach (var item in searchColumns)
                {
                    SearchQueryField field = new SearchQueryField(item, req.Keyword, operation);
                    queries.Add(field);
                }
            }
            return queries;
        }

        private MixCompareOperator ParseSearchOperation(ExpressionMethod? searchMethod)
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
                ExpressionMethod.In => MixCompareOperator.Contain,
                _ => MixCompareOperator.Equal
            };
        }

        private ActionResult<PagingResponseModel<JObject>> ParseSearchResult(SearchRequestDto req, PagingResponseModel<JObject> result)
        {
            throw new NotImplementedException();
        }


        #endregion

        #region Private

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

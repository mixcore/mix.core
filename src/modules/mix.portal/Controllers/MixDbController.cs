using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mix.RepoDb.Repositories;
using Mix.Shared.Models;
using Mix.Shared.Services;
using RepoDb;
using RepoDb.Enumerations;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-db/{name}")]
    [ApiController]
    public class MixDbController : MixApiControllerBase
    {
        private readonly MixRepoDbRepository _repository;
        private readonly MixCmsContext _context;
        public MixDbController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCmsContext context,
            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixIdentityService mixIdentityService,
            IQueueService<MessageQueueModel> queueService, MixRepoDbRepository repository)
            : base(httpContextAccessor, configuration, mixService, translator, cultureRepository, mixIdentityService, queueService)
        {
            _context = context;
            _repository = repository;
        }

        #region Overrides

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _repository.Init(RouteData?.Values["name"].ToString());
            base.OnActionExecuting(context);
        }

        #endregion

        [HttpGet]
        public async Task<ActionResult<PagingResponseModel<JObject>>> Get([FromQuery] SearchRequestDto req)
        {
            var result = await SearchHandler(req);

            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<JObject>> GetSingle(int id)
        {
            var data = await _repository.GetAsync(id);
            return data != null ? Ok(data) : throw new MixException(MixErrorStatus.NotFound, id);
        }

        [HttpPost]
        public async Task<ActionResult<object>> Create(JObject obj)
        {
            var data = await _repository.InsertAsync(obj);
            return data != null ? Ok() : BadRequest();
        }

        [HttpPut]
        public async Task<ActionResult<object>> Update(JObject obj)
        {
            var data = await _repository.UpdateAsync(obj);
            return data != null ? Ok() : BadRequest();
        }

        [HttpDelete]
        public async Task<ActionResult<object>> Delete(int id)
        {
            var data = await _repository.DeleteAsync(id);
            return data > 0 ? Ok() : NotFound();
        }


        #region Handler

        private async Task<PagingResponseModel<dynamic>> SearchHandler(SearchRequestDto request)
        {
            var queries = BuildSearchPredicate(request);
            return await _repository.GetPagingAsync(queries, new PagingRequestModel(Request));
        }

        private IEnumerable<QueryField> BuildSearchPredicate(SearchRequestDto req)
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
    }
}

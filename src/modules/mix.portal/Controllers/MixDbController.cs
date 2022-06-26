using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mix.RepoDb.Repositories;
using Mix.Shared.Models;
using Mix.Shared.Services;

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


        #region Handler

        private async Task<PagingResponseModel<dynamic>> SearchHandler(SearchRequestDto request)
        {
            //var searchRequest = BuildSearchRequest(req);
            //return await _repository.GetPagingAsync(searchRequest.Predicate, searchRequest.PagingData);
            return await _repository.GetPagingAsync(null, new PagingRequestModel()
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                SortBy = request.OrderBy,
                SortDirection = request.Direction
            });
        }

        private object BuildSearchRequest(SearchRequestDto req)
        {
            return null;
        }

        private ActionResult<PagingResponseModel<JObject>> ParseSearchResult(SearchRequestDto req, PagingResponseModel<JObject> result)
        {
            throw new NotImplementedException();
        }


        #endregion
    }
}

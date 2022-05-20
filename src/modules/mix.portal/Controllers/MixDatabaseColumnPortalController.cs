using Microsoft.AspNetCore.Mvc;
using Mix.Shared.Services;
using System.Linq.Expressions;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-database-column")]
    [ApiController]
    [MixAuthorize($"{MixRoles.SuperAdmin}, {MixRoles.Owner}")]
    public class MixDatabaseColumnPortalController
        : MixRestApiControllerBase<MixDatabaseColumnViewModel, MixCmsContext, MixDatabaseColumn, int>
    {
        private readonly MixDataService _mixDataService;
        private readonly MixEndpointService _endpointService;

        public MixDatabaseColumnPortalController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixDataService mixDataService,
            MixIdentityService mixIdentityService,
            MixEndpointService endpointService,
            MixCacheDbContext cacheDbContext,
            MixCmsContext context,
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor, configuration, mixService, translator, cultureRepository, mixIdentityService, cacheDbContext, context, queueService)
        {
            _mixDataService = mixDataService;
            _endpointService = endpointService;
        }

        [HttpGet("init/{mixDatabase}")]
        public async Task<ActionResult<List<MixDatabaseColumnViewModel>>> Init(string mixDatabase)
        {
            int.TryParse(mixDatabase, out int mixDatabaseId);
            var getData = await _repository.GetListAsync(
                f => f.MixDatabaseName == mixDatabase || f.MixDatabaseId == mixDatabaseId);
            return Ok(getData);
        }

        protected override SearchQueryModel<MixDatabaseColumn, int> BuildSearchRequest(SearchRequestDto req)
        {
            var searchReq = base.BuildSearchRequest(req);
            if (!string.IsNullOrEmpty(req.Keyword))
            {
                Expression<Func<MixDatabaseColumn, bool>> keywordPred = model =>
                 model.MixDatabaseName.Contains(req.Keyword)
                 || model.SystemName.Contains(req.Keyword)
                 || model.DisplayName.Contains(req.Keyword)
                 || model.DefaultValue.Contains(req.Keyword);
                searchReq.Predicate = searchReq.Predicate.AndAlso(keywordPred);
            }
            return searchReq;
        }
    }
}

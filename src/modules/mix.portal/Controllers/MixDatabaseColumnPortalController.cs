using Microsoft.AspNetCore.Mvc;
using Mix.Heart.Helpers;
using Mix.Lib.Interfaces;
using Mix.Mq.Lib.Models;
using Mix.RepoDb.Dtos;
using Mix.RepoDb.Interfaces;
using Mix.RepoDb.ViewModels;
using Mix.SignalR.Interfaces;
using System.Linq.Expressions;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-database-column")]
    [ApiController]
    [MixAuthorize(MixRoles.Owner)]
    public class MixDatabaseColumnPortalController
        : MixRestfulApiControllerBase<MixDatabaseColumnViewModel, MixCmsContext, MixDatabaseColumn, int>
    {
        private readonly IMixDbService _mixDbService;
        public MixDatabaseColumnPortalController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> uow,
            IMemoryQueueService<MessageQueueModel> queueService,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService,
            IMixDbService mixDbService)
            : base(httpContextAccessor, configuration,
                  cacheService, translator, mixIdentityService, uow, queueService, portalHub, mixTenantService)
        {
            _mixDbService = mixDbService;
        }

        [HttpGet("init/{mixDatabase}")]
        public async Task<ActionResult<List<MixDatabaseColumnViewModel>>> Init(string mixDatabase)
        {
            int.TryParse(mixDatabase, out int mixDatabaseId);
            var getData = await Repository.GetListAsync(
                f => f.MixDatabaseName == mixDatabase || f.MixDatabaseId == mixDatabaseId);
            return Ok(getData);
        }

        [MixAuthorize(MixRoles.Owner)]
        [HttpPost("alter-column")]
        public async Task<ActionResult> AlterColumn([FromBody] AlterColumnDto colDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _mixDbService.AlterColumn(colDto);
                return result ? Ok() : BadRequest();
            }
            return BadRequest();
        }

        protected override async Task<int> CreateHandlerAsync(MixDatabaseColumnViewModel data, CancellationToken cancellationToken = default)
        {
            var result = await base.CreateHandlerAsync(data, cancellationToken);
            var repoCol = new RepoDbMixDatabaseColumnViewModel();
            ReflectionHelper.Map(data, repoCol);
            await _mixDbService.AddColumn(repoCol);
            return result;
        }
        protected override async Task DeleteHandler(MixDatabaseColumnViewModel data, CancellationToken cancellationToken = default)
        {
            await base.DeleteHandler(data, cancellationToken);
            var repoCol = new RepoDbMixDatabaseColumnViewModel();
            ReflectionHelper.Map(data, repoCol);
            await _mixDbService.DropColumn(repoCol);
        }
        protected override SearchQueryModel<MixDatabaseColumn, int> BuildSearchRequest(SearchRequestDto req)
        {
            var searchReq = base.BuildSearchRequest(req);
            if (!string.IsNullOrEmpty(req.Keyword))
            {
                Expression<Func<MixDatabaseColumn, bool>> keywordPred =
                    model =>
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

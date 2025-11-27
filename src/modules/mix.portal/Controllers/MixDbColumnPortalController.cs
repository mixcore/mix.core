using Microsoft.AspNetCore.Mvc;
using Mix.Heart.Helpers;
using Mix.Lib.Interfaces;
using Mix.Mixdb.Dtos;
using Mix.Mixdb.Interfaces;
using Mix.Mixdb.ViewModels;
using Mix.Mq.Lib.Models;
using Mix.SignalR.Interfaces;
using System.Linq.Expressions;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-db-column")]
    [ApiController]
    [MixAuthorize(MixRoles.Owner)]
    public class MixDbColumnPortalController
        : MixRestfulApiControllerBase<MixDbColumnViewModel, MixCmsContext, MixDbColumn, int>
    {
        private readonly IMixdbStructure _mixDbStructure;
        public MixDbColumnPortalController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> uow,
            IMemoryQueueService<MessageQueueModel> queueService,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService,
            IMixdbStructure mixDbService)
            : base(httpContextAccessor, configuration,
                  cacheService, mixIdentityService, uow, queueService, portalHub, mixTenantService)
        {
            _mixDbStructure = mixDbService;
        }

        [HttpGet("init/{mixDatabase}")]
        public async Task<ActionResult<List<MixDbColumnViewModel>>> Init(string mixDatabase)
        {
            int.TryParse(mixDatabase, out int mixDatabaseId);
            var getData = await Repository.GetListAsync(
                f => f.MixDbTableName == mixDatabase || f.MixDbTableId == mixDatabaseId);
            return Ok(getData);
        }

        [MixAuthorize(MixRoles.Owner)]
        [HttpPost("alter-column")]
        public async Task<ActionResult> AlterColumn([FromBody] AlterColumnDto colDto, CancellationToken cancellationToken = default)
        {
            if (ModelState.IsValid)
            {
                var repoCol = new MixdbColumnViewModel(colDto);
                await _mixDbStructure.AlterColumn(repoCol, colDto.IsDrop, cancellationToken);
                return Ok();
            }
            return BadRequest();
        }

        protected override async Task<int> CreateHandlerAsync(MixDbColumnViewModel data, CancellationToken cancellationToken = default)
        {
            var result = await base.CreateHandlerAsync(data, cancellationToken);
            var repoCol = new MixdbColumnViewModel();
            ReflectionHelper.Map(data, repoCol);
            await _mixDbStructure.AddColumn(repoCol);
            return result;
        }

        protected override async Task DeleteHandler(MixDbColumnViewModel data, CancellationToken cancellationToken = default)
        {
            await base.DeleteHandler(data, cancellationToken);
            //var repoCol = new MixdbColumnViewModel();
            //ReflectionHelper.Map(data, repoCol);
            //await _mixDbStructure.DropColumn(repoCol, cancellationToken);
        }
        protected override SearchQueryModel<MixDbColumn, int> BuildSearchRequest(SearchRequestDto req)
        {
            var searchReq = base.BuildSearchRequest(req);
            if (!string.IsNullOrEmpty(req.Keyword))
            {
                Expression<Func<MixDbColumn, bool>> keywordPred =
                    model =>
                     model.MixDbTableName.Contains(req.Keyword)
                     || model.SystemName.Contains(req.Keyword)
                     || model.DisplayName.Contains(req.Keyword)
                     || model.DefaultValue.Contains(req.Keyword);
                searchReq.Predicate = searchReq.Predicate.AndAlso(keywordPred);
            }
            return searchReq;
        }
    }
}

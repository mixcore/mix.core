using Microsoft.AspNetCore.Mvc;
using Mix.Lib.Interfaces;
using Mix.Mixdb.Interfaces;
using Mix.Mq.Lib.Models;
using Mix.RepoDb.Interfaces;
using Mix.SignalR.Interfaces;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-module-data")]
    [ApiController]
    public class MixModuleDataController
        : MixRestfulApiControllerBase<MixModuleDataViewModel, MixCmsContext, MixModuleData, int>
    {
        public MixModuleDataController(
           IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> cmsUow,
            IMemoryQueueService<MessageQueueModel> queueService,
            IMixdbStructure mixDbService,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService)
            : base(httpContextAccessor, configuration, 
                  cacheService, mixIdentityService, cmsUow, queueService, portalHub, mixTenantService)
        {

        }

        #region Overrides


        protected override SearchQueryModel<MixModuleData, int> BuildSearchRequest(SearchRequestDto req)
        {
            var request = new SearchModuleDataDto(req, Request);
            var searchRequest = base.BuildSearchRequest(request);

            searchRequest.Predicate = searchRequest.Predicate.AndAlsoIf(
                request.ModuleId.HasValue,
                m => m.ParentId == request.ModuleId);

            return searchRequest;
        }

        protected override Task<int> CreateHandlerAsync(MixModuleDataViewModel data, CancellationToken cancellationToken = default)
        {
            data.Specificulture = Culture.Specificulture;
            data.MixCultureId = Culture.Id;
            return base.CreateHandlerAsync(data, cancellationToken);
        }
        #endregion

        [HttpGet]
        [Route("init-form/{moduleId}")]
        public async Task<ActionResult<MixModuleDataViewModel>> InitByIdAsync(int moduleId, CancellationToken cancellationToken = default)
        {
            var getModule = await MixModuleContentViewModel.GetRepository(Uow, CacheService).GetSingleAsync(m => m.Id == moduleId, cancellationToken)
                .ConfigureAwait(false);
            if (getModule != null)
            {
                var moduleData = new MixModuleDataViewModel()
                {
                    ParentId = getModule.Id,
                    SimpleDataColumns = getModule.SimpleDataColumns
                };
                await moduleData.ExpandView(cancellationToken);
                return Ok(moduleData);
            }

            return BadRequest();
        }
    }
}

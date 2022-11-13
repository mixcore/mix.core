using Microsoft.AspNetCore.Mvc;

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
            MixService mixService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> cmsUOW,
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, cmsUOW, queueService)
        {

        }

        #region Overrides


        protected override SearchQueryModel<MixModuleData, int> BuildSearchRequest(SearchRequestDto req)
        {
            var request = new SearchModuleDataDto(req, Request);
            var searchRequest = base.BuildSearchRequest(request);

            searchRequest.Predicate = searchRequest.Predicate.AndAlsoIf(
                request.ModuleContentId.HasValue,
                m => m.ParentId == request.ModuleContentId);

            return searchRequest;
        }
        #endregion

        [HttpGet]
        [Route("init-form/{moduleId}")]
        public async Task<ActionResult<MixModuleDataViewModel>> InitByIdAsync(int moduleId)
        {
            var getModule = await MixModuleContentViewModel.GetRepository(_uow).GetSingleAsync(m => m.Id == moduleId)
                .ConfigureAwait(false);
            if (getModule != null)
            {
                var moduleData = new MixModuleDataViewModel()
                {
                    ParentId = getModule.Id,
                    SimpleDataColumns = getModule.SimpleDataColumns
                };
                await moduleData.ExpandView();
                return Ok(moduleData);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/configuration")]
    [ApiController]
    [MixAuthorize(MixRoles.Owner)]
    public class MixConfigurationController
        : MixRestfulApiControllerBase<MixConfigurationContentViewModel, MixCmsContext, MixConfigurationContent, int>
    {
        private readonly MixConfigurationService _configService;
        private readonly UnitOfWorkInfo<MixCmsContext> _cmsUOW;
        public MixConfigurationController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> cmsUOW,
            IQueueService<MessageQueueModel> queueService,
            MixConfigurationService configService)
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, cmsUOW, queueService)
        {
            _cmsUOW = cmsUOW;
            _configService = configService;
        }

        #region Overrides

        protected override async Task<int> CreateHandlerAsync(MixConfigurationContentViewModel data)
        {
            var result = await base.CreateHandlerAsync(data);
            await _configService.Reload(_cmsUOW);
            return result;
        }
        protected override async Task UpdateHandler(int id, MixConfigurationContentViewModel data)
        {
            await base.UpdateHandler(id, data);
            await _configService.Reload(_cmsUOW);
        }

        protected override async Task DeleteHandler(MixConfigurationContentViewModel data)
        {
            await base.DeleteHandler(data);
            await _configService.Reload(_cmsUOW);
        }
        #endregion


    }
}

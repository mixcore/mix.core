using Microsoft.AspNetCore.Mvc;
using Mix.Common.Domain.Helpers;
using Mix.Common.Domain.ViewModels;
using Mix.Common.Models;
using Mix.Lib.Interfaces;
using Mix.Lib.Services;
using Mix.Mq.Lib.Models;
using Mix.Queue.Interfaces;
using Mix.Shared.Models.Configurations;
using Mix.Shared.Services;

namespace Mix.Common.Controllers
{
    [Route("api/v2/rest/shared")]
    [ApiController]
    public class SharedTenantApiController : MixTenantApiControllerBase
    {
        protected UnitOfWorkInfo Uow;
        protected readonly MixCmsContext Context;
        private readonly ViewQueryRepository<MixCmsContext, MixConfigurationContent, int, MixConfigurationContentViewModel> _configRepo;
        private readonly ViewQueryRepository<MixCmsContext, MixLanguageContent, int, MixLanguageContentViewModel> _langRepo;
        private readonly MixAuthenticationConfigurations _authConfigurations;
        private readonly MixEndpointService _endpointService;
        public SharedTenantApiController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            AuthConfigService authConfigService,
            MixCmsContext context,
            IMemoryQueueService<MessageQueueModel> queueService,
            IMixTenantService mixTenantService,
            MixEndpointService endpointService)
            : base(httpContextAccessor, configuration,
                  cacheService, translator, mixIdentityService, queueService, mixTenantService)
        {
            _authConfigurations = authConfigService.AppSettings;
            Context = context;
            Uow = new(Context);
            _configRepo = MixConfigurationContentViewModel.GetRepository(Uow, CacheService);
            _langRepo = MixLanguageContentViewModel.GetRepository(Uow, CacheService);
            _endpointService = endpointService;
        }

        #region Routes

        [HttpGet]
        [Route("get-global-settings")]
        public ActionResult<Models.GlobalSettings> GetSharedSettings()
        {
            var settings = CommonHelper.GetAppSettings(_authConfigurations, CurrentTenant);
            return Ok(settings);
        }

        // GET api/v1/portal/check-config
        [HttpGet]
        [Route("check-config/{lastSync}")]
        public ActionResult<JObject> CheckConfig(DateTime lastSync)
        {
            var lastUpdate = CurrentTenant.Configurations.LastUpdateConfiguration;
            if (lastSync.ToUniversalTime() < lastUpdate)
            {
                return Ok(GetAllSettingsAsync());
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("get-all-settings")]
        public async Task<ActionResult<AllSettingModel>> GetAllSettingsAsync()
        {
            var settings = await GetSettingsAsync();
            return Ok(settings);
        }

        [HttpGet]
        [Route("get-shared-settings/{culture}")]
        public async Task<ActionResult<AllSettingModel>> GetSharedSettingsAsync(string culture)
        {
            var settings = await GetSettingsAsync(culture);
            return Ok(settings);
        }

        #endregion

        private async Task<AllSettingModel> GetSettingsAsync(string lang = null, CancellationToken cancellationToken = default)
        {
            return new AllSettingModel()
            {
                GlobalSettings = CommonHelper.GetAppSettings(_authConfigurations, CurrentTenant),
                MixConfigurations = await _configRepo.GetListAsync(m => m.Specificulture == lang, cancellationToken),
                Translator = _langRepo.GetListQuery(m => m.Specificulture == lang).ToList(),
                Endpoints = _endpointService.AppSettings
            };
        }
    }
}

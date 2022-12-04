using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Mix.Common.Domain.Helpers;
using Mix.Common.Domain.ViewModels;
using Mix.Common.Models;
using Mix.Lib.Services;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Shared.Models.Configurations;
using Mix.Shared.Services;
using ApplicationLifetime = Microsoft.Extensions.Hosting.IHostApplicationLifetime;

namespace Mix.Common.Controllers
{
    [Route("api/v2/rest/shared")]
    [ApiController]
    public class SharedTenantApiController : MixTenantApiControllerBase
    {
        private readonly ApplicationLifetime _applicationLifetime;
        protected UnitOfWorkInfo _uow;
        protected readonly MixCmsContext _context;
        private readonly ViewQueryRepository<MixCmsContext, MixConfigurationContent, int, MixConfigurationContentViewModel> _configRepo;
        private readonly ViewQueryRepository<MixCmsContext, MixLanguageContent, int, MixLanguageContentViewModel> _langRepo;
        private readonly AuthConfigService _authConfigService;
        private readonly MixAuthenticationConfigurations _authConfigurations;
        private readonly IActionDescriptorCollectionProvider _routeProvider;
        public SharedTenantApiController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            IActionDescriptorCollectionProvider routeProvider,
            MixIdentityService mixIdentityService, AuthConfigService authConfigService,
            MixCmsContext context, IQueueService<MessageQueueModel> queueService, ApplicationLifetime applicationLifetime)
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, queueService)
        {
            _authConfigurations = authConfigService.AppSettings;
            _context = context;
            _uow = new(_context);
            _configRepo = MixConfigurationContentViewModel.GetRepository(_uow);
            _langRepo = MixLanguageContentViewModel.GetRepository(_uow);
            _routeProvider = routeProvider;
            _authConfigService = authConfigService;
            _applicationLifetime = applicationLifetime;
        }

        #region Routes

        [HttpGet]
        [Route("get-global-settings")]
        public ActionResult<GlobalSettings> GetSharedSettings()
        {
            var settings = CommonHelper.GetAppSettings(_authConfigurations, CurrentTenant);
            return Ok(settings);
        }

        // GET api/v1/portal/check-config
        [HttpGet]
        [Route("check-config/{lastSync}")]
        public ActionResult<JObject> checkConfig(DateTime lastSync)
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
                Translator = _langRepo.GetListQuery(m => m.Specificulture == lang).ToList()
            };
        }
    }
}

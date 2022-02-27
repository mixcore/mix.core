using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Mix.Common.Domain.Dtos;
using Mix.Common.Domain.Helpers;
using Mix.Common.Domain.Models;
using Mix.Common.Domain.ViewModels;
using Mix.Common.Models;
using Mix.Lib.Services;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Shared.Models;
using Mix.Shared.Services;

namespace Mix.Common.Controllers
{
    [Route("api/v2/rest/shared")]
    [ApiController]
    public class SharedApiController : MixApiControllerBase
    {
        protected UnitOfWorkInfo _uow;
        protected readonly MixCmsContext _context;
        private readonly ViewQueryRepository<MixCmsContext, MixConfigurationContent, int, MixConfigurationContentViewModel> _configRepo;
        private readonly ViewQueryRepository<MixCmsContext, MixLanguageContent, int, MixLanguageContentViewModel> _langRepo;
        protected readonly CultureService _cultureService;
        private readonly AuthConfigService _authConfigService;
        private readonly MixAuthenticationConfigurations _authConfigurations;
        private readonly IActionDescriptorCollectionProvider _routeProvider;
        public SharedApiController(
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            IActionDescriptorCollectionProvider routeProvider,
            MixIdentityService mixIdentityService, AuthConfigService authConfigService,
            CultureService cultureService,
            MixCmsContext context, IQueueService<MessageQueueModel> queueService)
            : base(configuration, mixService, translator, cultureRepository, mixIdentityService, queueService)
        {
            _authConfigurations = authConfigService.AppSettings;
            _context = context;
            _uow = new(_context);
            _configRepo = MixConfigurationContentViewModel.GetRepository(_uow);
            _langRepo = MixLanguageContentViewModel.GetRepository(_uow);
            _routeProvider = routeProvider;
            _authConfigService = authConfigService;
            _cultureService = cultureService;
        }

        #region Routes

        [HttpPost]
        [Route("encrypt-message")]
        public ActionResult<string> EncryptMessage(CryptoMessageDto encryptMessage)
        {
            string key = encryptMessage.Key
                        ?? GlobalConfigService.Instance.AppSettings.ApiEncryptKey;
            string msg = encryptMessage.ObjectData != null
                    ? encryptMessage.ObjectData.ToString(Newtonsoft.Json.Formatting.None)
                    : encryptMessage.StringData;
            var result = AesEncryptionHelper.EncryptString(msg, key);
            return Ok(result);
        }

        [HttpPost]
        [Route("decrypt-message")]
        public ActionResult<string> DecryptMessage(CryptoMessageDto encryptMessage)
        {
            string key = encryptMessage.Key ?? GlobalConfigService.Instance.AppSettings.ApiEncryptKey;
            string msg = AesEncryptionHelper.DecryptString(encryptMessage.StringData, key);
            return Ok(msg);
        }

        [HttpGet]
        [Route("routes")]
        public ActionResult GetRoutes()
        {
            var routes = _routeProvider.ActionDescriptors.Items.Where(
            ad => ad.AttributeRouteInfo != null).Select(ad => new RouteModel
            {
                Name = ad.AttributeRouteInfo.Name,
                Template = ad.AttributeRouteInfo.Template,
                Method = ad.ActionConstraints?.OfType<HttpMethodActionConstraint>().FirstOrDefault()?.HttpMethods.First(),
            }).ToList();

            var res = new RootResultModel
            {
                Routes = routes,
                Total = routes.Count
            };

            return Ok(res);
        }

        [HttpGet]
        [Route("mix-configuration/{lang}")]
        public ActionResult GetMixConfigurations(string lang)
        {
            return Ok(_configRepo.GetListQuery(c => c.Specificulture == lang).ToList());
        }

        [HttpGet]
        [Route("mix-translation/{lang}")]
        public ActionResult GetMixTranslation(string lang)
        {
            return Ok(_langRepo.GetListQuery(c => c.Specificulture == lang).ToList());
        }

        [HttpGet]
        [Route("ping")]
        public ActionResult Ping()
        {
            return Ok(DateTime.UtcNow);
        }

        [HttpGet]
        [Route("get-global-settings")]
        public ActionResult<GlobalSettings> GetSharedSettings()
        {
            var settings = CommonHelper.GetAppSettings(_authConfigurations);
            return Ok(settings);
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

        // GET api/v1/portal/check-config
        [HttpGet]
        [Route("check-config/{lastSync}")]
        public ActionResult<JObject> checkConfig(DateTime lastSync)
        {
            var lastUpdate = GlobalConfigService.Instance.AppSettings.LastUpdateConfiguration;
            if (lastSync.ToUniversalTime() < lastUpdate)
            {
                return Ok(GetAllSettingsAsync());
            }
            else
            {
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("json-data/{name}")]
        public ActionResult<JObject> LoadData(string name)
        {
            try
            {
                var data = MixFileHelper.GetFile(
                    name, MixFileExtensions.Json, MixFolders.JsonDataFolder, false);
                var obj = JObject.Parse(data.Content);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }

        #endregion

        private async Task<AllSettingModel> GetSettingsAsync(string lang = null)
        {
            return new AllSettingModel()
            {
                GlobalSettings = CommonHelper.GetAppSettings(_authConfigurations),
                MixConfigurations = await _configRepo.GetListAsync(m => m.Specificulture == lang),
                Translator = _langRepo.GetListQuery(m => m.Specificulture == lang).ToList()
            };
        }
    }
}

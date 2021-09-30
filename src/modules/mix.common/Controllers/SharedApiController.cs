using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mix.Shared.Constants;
using Mix.Shared.Enums;
using Mix.Lib.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using Mix.Shared.Services;
using Mix.Heart.Enums;
using Mix.Heart.Helpers;
using Mix.Shared.Models;
using Mix.Heart.Repository;
using Mix.Database.Entities.Cms;
using Mix.Common.Models;
using Mix.Common.Domain.ViewModels;
using System.Threading.Tasks;
using Mix.Lib.Abstracts;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Mix.Common.Domain.Models;
using Mix.Identity.Services;
using Mix.Common.Domain.Dtos;

namespace Mix.Common.Controllers
{
    [Route("api/v2/shared")]
    [ApiController]
    public class SharedApiController : MixApiControllerBase
    {
        private readonly QueryRepository<MixCmsContext, MixConfigurationContent, int> _configRepo;
        private readonly QueryRepository<MixCmsContext, MixLanguageContent, int> _langRepo;
        private readonly MixFileService _fileService;
        protected readonly CultureService _cultureService;
        private readonly AuthConfigService authConfigService;
        private readonly MixAuthenticationConfigurations _authConfigurations;
        private readonly IActionDescriptorCollectionProvider _routeProvider;

        public SharedApiController(
            ILogger<MixApiControllerBase> logger,
            GlobalConfigService globalConfigService,
            MixService mixService,
            TranslatorService translator,
            Repository<MixCmsContext, MixCulture, int> cultureRepository,
            MixFileService fileService,
            QueryRepository<MixCmsContext, MixConfigurationContent, int> configRepo,
            QueryRepository<MixCmsContext, MixLanguageContent, int> langRepo,
            IActionDescriptorCollectionProvider routeProvider,
            MixIdentityService mixIdentityService, AuthConfigService authConfigService,
            CultureService cultureService)
            : base(logger, globalConfigService, mixService, translator, cultureRepository, mixIdentityService)
        {
            _fileService = fileService;
            _authConfigurations = authConfigService.AuthConfigurations;
            _configRepo = configRepo;
            _langRepo = langRepo;
            _routeProvider = routeProvider;
            this.authConfigService = authConfigService;
            _cultureService = cultureService;
        }

        #region Routes

        [HttpPost]
        [Route("encrypt-message")] 
        public ActionResult EncryptMessage(CryptoMessageDto encryptMessage)
        {
            string key = encryptMessage.Key ?? _globalConfigService.GetConfig<string>(MixAppSettingKeywords.ApiEncryptKey);
            string msg = AesEncryptionHelper.EncryptString(encryptMessage.Message, key);
            return Ok(msg);
        }
        
        [HttpPost]
        [Route("decrypt-message")] 
        public ActionResult DecryptMessage(CryptoMessageDto encryptMessage)
        {
            string key = encryptMessage.Key ?? _globalConfigService.GetConfig<string>(MixAppSettingKeywords.ApiEncryptKey);
            string msg = AesEncryptionHelper.DecryptString(encryptMessage.Message, key);
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
        [Route("get-shared-settings")]
        public async Task<ActionResult<AllSettingModel>> GetSharedSettingsAsync()
        {
            var settings = await GetAllSettingsAsync();
            return Ok(settings);
        }

        [HttpGet]
        [Route("get-shared-settings/{culture}")]
        public async Task<ActionResult<AllSettingModel>> GetSharedSettingsAsync(string culture)
        {
            var settings = await GetAllSettingsAsync(culture);
            return Ok(settings);
        }

        // GET api/v1/portal/check-config
        [HttpGet]
        [Route("check-config/{lastSync}")]
        public ActionResult<JObject> checkConfig(DateTime lastSync)
        {
            var lastUpdate = _globalConfigService.GetConfig<DateTime>(MixAppSettingKeywords.LastUpdateConfiguration);
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
                var data = _fileService.GetFile(
                    name, MixFileExtensions.Json, MixFolders.JsonDataFolder, false, "[]");
                var obj = JObject.Parse(data.Content);
                return Ok(obj);
            }
            catch
            {
                return NotFound();
            }
        }

        #endregion

        private async Task<AllSettingModel> GetAllSettingsAsync(string lang = null)
        {
            lang ??= _globalConfigService.GetConfig<string>(MixAppSettingKeywords.DefaultCulture);
            var cultures = _cultureService.Cultures;
            var culture = _cultureService.LoadCulture(lang);
            // Get Settings
            AppSettingModel globalSettings = new()
            {
                Domain = _globalConfigService.GetConfig<string>(MixAppSettingKeywords.Domain),
                Lang = lang,
                PortalThemeSettings = _globalConfigService.GetConfig<JObject>(MixAppSettingKeywords.PortalThemeSettings),
                ApiEncryptKey = _globalConfigService.GetConfig<string>(MixAppSettingKeywords.ApiEncryptKey),
                IsEncryptApi = _globalConfigService.GetConfig<bool>(MixAppSettingKeywords.IsEncryptApi),
                Cultures = cultures,
                PageTypes = Enum.GetNames(typeof(MixPageType)),
                ModuleTypes = Enum.GetNames(typeof(MixModuleType)),
                MixDatabaseTypes = Enum.GetNames(typeof(MixDatabaseType)),
                DataTypes = Enum.GetNames(typeof(MixDataType)),
                Statuses = Enum.GetNames(typeof(MixContentStatus)),
                RSAKeys = RSAEncryptionHelper.GenerateKeys(),
                ExternalLoginProviders = new JObject()
                {
                    new JProperty("Facebook", _authConfigurations.Facebook?.AppId),
                    new JProperty("Google", _authConfigurations.Google?.AppId),
                    new JProperty("Twitter", _authConfigurations.Twitter?.AppId),
                    new JProperty("Microsoft", _authConfigurations.Microsoft?.AppId),
                },
                LastUpdateConfiguration = _globalConfigService.GetConfig<DateTime?>(MixAppSettingKeywords.LastUpdateConfiguration)

            };

            return new AllSettingModel()
            {
                AppSettings = globalSettings,
                MixConfigurations = await _configRepo.GetListViewAsync<MixConfigurationContentViewModel>(m => m.Specificulture == lang),
                Translator = _langRepo.GetListQuery(m => m.Specificulture == lang).ToList()
            };
        }


    }
}

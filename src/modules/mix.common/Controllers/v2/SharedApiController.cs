using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mix.Shared.Constants;
using Mix.Lib.Controllers;
using Mix.Shared.Enums;
using Mix.Lib.Models.Common;
using Mix.Lib.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using Mix.Shared.Services;
using Mix.Heart.Enums;
using Mix.Heart.Helpers;
using Mix.Shared.Models;
using Mix.Heart.Repository;
using Mix.Database.Entities.Cms.v2;
using Mix.Common.Models;
using Mix.Common.Domain.ViewModels;
using System.Threading.Tasks;
using Mix.Lib.Abstracts;

namespace Mix.Common.Controllers.v2
{
    [Route("api/v2/shared")]
    public class SharedApiController : MixApiControllerBase
    {
        private readonly QueryRepository<MixCmsContext, MixConfigurationContent, int> _configRepo;
        private readonly QueryRepository<MixCmsContext, MixLanguageContent, int> _langRepo;
        private readonly MixFileService _fileService;
        private readonly MixAuthenticationConfigurations _authConfigurations;

        public SharedApiController(ILogger<MixApiControllerBase> logger,
            MixFileService fileService,
            MixAppSettingService appSettingService,
            MixService mixService,
            TranslatorService translator,
            QueryRepository<MixCmsContext, MixConfigurationContent, int> configRepo, QueryRepository<MixCmsContext, MixLanguageContent, int> langRepo) : base(logger, appSettingService, mixService, translator)
        {
            _fileService = fileService;
            _authConfigurations = _appSettingService.LoadSection<MixAuthenticationConfigurations>(MixAppSettingsSection.Authentication);
            _configRepo = configRepo;
            _langRepo = langRepo;
        }

        #region Routes

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
            var lastUpdate = _appSettingService.GetConfig<DateTime>(
                                MixAppSettingsSection.GlobalSettings, "LastUpdateConfiguration");
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
            lang ??= _appSettingService.GetConfig<string>(
                        MixAppSettingsSection.GlobalSettings, MixAppSettingKeywords.DefaultCulture);
            var cultures = _appSettingService.Cultures;
            var culture = cultures?.FirstOrDefault(c => c == lang);
            // Get Settings
            AppSettingModel globalSettings = new()
            {
                Domain = _appSettingService.GetConfig<string>(
                    MixAppSettingsSection.GlobalSettings, MixAppSettingKeywords.Domain),
                Lang = lang,
                PortalThemeSettings = _appSettingService.GetConfig<JObject>(
                    MixAppSettingsSection.GlobalSettings, MixAppSettingKeywords.PortalThemeSettings),
                ApiEncryptKey = _appSettingService.GetConfig<string>(
                    MixAppSettingsSection.GlobalSettings, MixAppSettingKeywords.ApiEncryptKey),
                IsEncryptApi = _appSettingService.GetConfig<bool>(
                    MixAppSettingsSection.GlobalSettings, MixAppSettingKeywords.IsEncryptApi),
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
                LastUpdateConfiguration = _appSettingService.GetConfig<DateTime?>(
                    MixAppSettingsSection.GlobalSettings, MixAppSettingKeywords.LastUpdateConfiguration)

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

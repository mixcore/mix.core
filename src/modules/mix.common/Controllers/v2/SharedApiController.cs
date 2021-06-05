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
using Mix.Lib.Helpers;
using Mix.Heart.Helpers;

namespace Mix.Common.Controllers.v2
{
    [Route("api/v2/shared")]
    public class SharedApiController : MixApiControllerBase
    {
        private readonly MixFileService _fileService;

        public SharedApiController(ILogger<MixApiControllerBase> logger,
            MixFileService fileService, 
            MixAppSettingService appSettingService, 
            MixService mixService, 
            TranslatorService translator) : base(logger, appSettingService, mixService, translator)
        {
            _fileService = fileService;
        }

        [HttpGet]
        [Route("ping")]
        public ActionResult Ping()
        {
            return Ok(DateTime.UtcNow);
        }

        [HttpGet]
        [Route("get-shared-settings")]
        public ActionResult<JObject> GetSharedSettingsAsync()
        {
            return Ok(GetAllSettings());
        }
        
        [HttpGet]
        [Route("{culture}/get-shared-settings")]
        public ActionResult<JObject> GetSharedSettingsAsync(string culture)
        {
            return Ok(GetAllSettings(culture));
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
                return Ok(GetAllSettings());
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
                    name, MixFileExtensions.Json, MixFolders.JsonDataFolder,  false, "[]");
                var obj = JObject.Parse(data.Content);
                return Ok(obj);
            }
            catch
            {
                return NotFound();
            }
        }
        private JObject GetAllSettings(string lang = null)
        {
            lang ??= _appSettingService.GetConfig<string>(
                        MixAppSettingsSection.GlobalSettings, MixAppSettingKeywords.DefaultCulture);
            var cultures = _appSettingService.Cultures;
            var culture = cultures?.FirstOrDefault(c => c == lang);
            // Get Settings
            GlobalSettingModel configurations = new()
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
                PageTypes = MixHelper.ParseEnumToObject(typeof(MixPageType)),
                ModuleTypes = MixHelper.ParseEnumToObject(typeof(MixModuleType)),
                MixDatabaseTypes = MixHelper.ParseEnumToObject(typeof(MixDatabaseType)),
                DataTypes = MixHelper.ParseEnumToObject(typeof(MixDataType)),
                Statuses = MixHelper.ParseEnumToObject(typeof(MixContentStatus)),
                RSAKeys = RSAEncryptionHelper.GenerateKeys(),
                ExternalLoginProviders = new JObject()
                {
                    new JProperty("Facebook", _appSettingService.MixAuthentications.Facebook?.AppId),
                    new JProperty("Google", _appSettingService.MixAuthentications.Google?.AppId),
                    new JProperty("Twitter", _appSettingService.MixAuthentications.Twitter?.AppId),
                    new JProperty("Microsoft", _appSettingService.MixAuthentications.Microsoft?.AppId),
                },
                LastUpdateConfiguration = _appSettingService.GetConfig<DateTime?>(
                    MixAppSettingsSection.GlobalSettings, MixAppSettingKeywords.LastUpdateConfiguration)

            };

            return new JObject()
            {
                new JProperty("globalSettings", JObject.FromObject(configurations)),
            };
        }

        
    }
}

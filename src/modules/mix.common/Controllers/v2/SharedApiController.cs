using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mix.Common.Helper;
using Mix.Heart.Helpers;
using Mix.Heart.Models;
using Mix.Infrastructure.Repositories;
using Mix.Shared.Constants;
using Mix.Lib.Controllers;
using Mix.Shared.Enums;
using Mix.Lib.Models.Common;
using Mix.Lib.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using Mix.Shared.Services;

namespace Mix.Common.Controllers.v2
{
    [Route("api/v2/shared")]
    public class SharedApiController : MixApiControllerBase
    {
        public SharedApiController(ILogger<MixApiControllerBase> logger, MixService mixService, TranslatorService translator) : base(logger, mixService, translator)
        {
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
            return Ok(GetAllSettings().Data);
        }
        
        [HttpGet]
        [Route("{culture}/get-shared-settings")]
        public ActionResult<JObject> GetSharedSettingsAsync(string culture)
        {
            return Ok(GetAllSettings(culture).Data);
        }

        // GET api/v1/portal/check-config
        [HttpGet]
        [Route("check-config/{lastSync}")]
        public ActionResult<RepositoryResponse<JObject>> checkConfig(DateTime lastSync)
        {
            var lastUpdate = MixAppSettingService.GetConfig<DateTime>("LastUpdateConfiguration");
            if (lastSync.ToUniversalTime() < lastUpdate)
            {
                return Ok(GetAllSettings());
            }
            else
            {
                return new RepositoryResponse<JObject>()
                {
                    IsSucceed = true,
                };
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("jarray-data/{name}")]
        public ActionResult<JArray> LoadData(string name)
        {
            try
            {
                var cultures = MixFileRepository.Instance.GetFile(name, MixFolders.JsonDataFolder, true, "[]");
                var obj = JObject.Parse(cultures.Content);
                return Ok(obj["data"] as JArray);
            }
            catch
            {
                return NotFound();
            }
        }
        private RepositoryResponse<JObject> GetAllSettings(string lang = null)
        {
            lang ??= MixAppSettingService.GetConfig<string>(MixAppSettingKeywords.DefaultCulture);
            var cultures = MixAppSettingService.Instance.Cultures;
            var culture = cultures?.FirstOrDefault(c => c == lang);
            // Get Settings
            GlobalSettingModel configurations = new()
            {
                Domain = MixAppSettingService.GetConfig<string>(MixAppSettingKeywords.Domain),
                Lang = lang,
                PortalThemeSettings = MixAppSettingService.GetConfig<JObject>(MixAppSettingKeywords.PortalThemeSettings),
                ApiEncryptKey = MixAppSettingService.GetConfig<string>(MixAppSettingKeywords.ApiEncryptKey),
                IsEncryptApi = MixAppSettingService.GetConfig<bool>(MixAppSettingKeywords.IsEncryptApi),
                Cultures = cultures,
                PageTypes = MixCommonHelper.ParseEnumToObject(typeof(MixPageType)),
                ModuleTypes = MixCommonHelper.ParseEnumToObject(typeof(MixModuleType)),
                MixDatabaseTypes = MixCommonHelper.ParseEnumToObject(typeof(MixDatabaseType)),
                DataTypes = MixCommonHelper.ParseEnumToObject(typeof(MixDataType)),
                Statuses = MixCommonHelper.ParseEnumToObject(typeof(MixContentStatus)),
                RSAKeys = RSAEncryptionHelper.GenerateKeys(),
                ExternalLoginProviders = new JObject()
                {
                    new JProperty("Facebook", ""),
                    new JProperty("Google", MixAppSettingService.MixAuthentications.Google?.AppId),
                    new JProperty("Twitter", MixAppSettingService.MixAuthentications.Twitter?.AppId),
                    new JProperty("Microsoft", MixAppSettingService.MixAuthentications.Microsoft?.AppId),
                },
                LastUpdateConfiguration = MixAppSettingService.GetConfig<DateTime?>(MixAppSettingKeywords.LastUpdateConfiguration)

            };

            // Get translator
            var translator = new JObject()
            {
                new JProperty("lang",lang),
                //new JProperty("data", MixAppSettingService.GetTranslator(lang))
            };

            // Get Configurations
            var localizeSettings = new JObject()
            {
                new JProperty("lang",lang),
                new JProperty("langIcon",configurations.LangIcon),

                //new JProperty("data", MixAppSettingService.GetLocalizeSettings(lang))
            };


            JObject result = new JObject()
            {
                new JProperty("globalSettings", JObject.FromObject(configurations)),
                new JProperty("translator", translator),
                new JProperty("localizeSettings", JObject.FromObject(localizeSettings))
            };



            return new RepositoryResponse<JObject>()
            {
                IsSucceed = true,
                Data = result
            };
        }

        
    }
}

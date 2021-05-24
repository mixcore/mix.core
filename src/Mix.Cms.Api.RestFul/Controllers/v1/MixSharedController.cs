using Microsoft.AspNetCore.Mvc;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Helpers;
using Mix.Cms.Lib.Repositories;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.ViewModels;
using Mix.Common.Helper;
using Mix.Heart.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace Mix.Cms.Api.RestFul.Controllers.v1
{
    [Route("api/v1/rest/shared")]
    [ApiController]
    public class MixSharedController : ControllerBase
    {
        [HttpGet]
        [Route("ping")]
        public ActionResult Ping()
        {
            return Ok(DateTime.UtcNow);
        }

        [HttpGet]
        [Route("get-shared-settings")]
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
            var lastUpdate = MixService.GetConfig<DateTime>("LastUpdateConfiguration");
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

        private RepositoryResponse<JObject> GetAllSettings(string lang = null)
        {
            lang ??= MixService.GetConfig<string>(MixAppSettingKeywords.DefaultCulture);
            var cultures = CommonRepository.Instance.LoadCultures();
            var culture = cultures.FirstOrDefault(c => c.Specificulture == lang);

            // Get Settings
            GlobalSettingsViewModel configurations = new GlobalSettingsViewModel()
            {
                Domain = MixService.GetConfig<string>(MixAppSettingKeywords.Domain),
                Lang = lang,
                PortalThemeSettings = MixService.GetConfig<JObject>(MixAppSettingKeywords.PortalThemeSettings),
                ThemeId = MixService.GetConfig<int>(MixAppSettingKeywords.ThemeId, lang),
                ApiEncryptKey = MixService.GetConfig<string>(MixAppSettingKeywords.ApiEncryptKey),
                IsEncryptApi = MixService.GetConfig<bool>(MixAppSettingKeywords.IsEncryptApi),
                Cultures = cultures,
                PageTypes = MixCommonHelper.ParseEnumToObject(typeof(MixPageType)),
                ModuleTypes = MixCommonHelper.ParseEnumToObject(typeof(MixModuleType)),
                MixDatabaseTypes = MixCommonHelper.ParseEnumToObject(typeof(MixDatabaseType)),
                DataTypes = MixCommonHelper.ParseEnumToObject(typeof(MixDataType)),
                Statuses = MixCommonHelper.ParseEnumToObject(typeof(MixContentStatus)),
                RSAKeys = RSAEncryptionHelper.GenerateKeys(),
                ExternalLoginProviders = new JObject()
                {
                    new JProperty("Facebook", MixService.Instance.MixAuthentications.Facebook?.AppId),
                    new JProperty("Google", MixService.Instance.MixAuthentications.Google?.AppId),
                    new JProperty("Twitter", MixService.Instance.MixAuthentications.Twitter?.AppId),
                    new JProperty("Microsoft", MixService.Instance.MixAuthentications.Microsoft?.AppId),
                },
                LastUpdateConfiguration = MixService.GetConfig<DateTime?>(MixAppSettingKeywords.LastUpdateConfiguration)

            };

            configurations.LangIcon = culture?.Icon ?? MixService.GetConfig<string>(MixAppSettingKeywords.Language);

            // Get translator
            var translator = new JObject()
            {
                new JProperty("lang",lang),
                new JProperty("data", MixService.GetTranslator(lang))
            };

            // Get Configurations
            var localizeSettings = new JObject()
            {
                new JProperty("lang",lang),
                new JProperty("langIcon",configurations.LangIcon),

                new JProperty("data", MixService.GetLocalizeSettings(lang))
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

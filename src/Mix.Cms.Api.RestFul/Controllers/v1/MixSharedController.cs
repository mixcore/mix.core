using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Helpers;
using Mix.Cms.Lib.Repositories;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.ViewModels;
using Mix.Common.Helper;
using Mix.Heart.Models;
using Mix.Identity.Constants;
using Mix.Infrastructure.Repositories;
using Mix.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MixRoles = Mix.Cms.Lib.ViewModels.Account.MixRoles;

namespace Mix.Cms.Api.RestFul.Controllers.v1
{
    [Route("api/v1/rest/shared")]
    [ApiController]
    public class MixSharedController : ControllerBase
    {
        private readonly IApplicationLifetime applicationLifetime;
        protected readonly RoleManager<IdentityRole> _roleManager;
        public MixSharedController(IApplicationLifetime applicationLifetime, RoleManager<IdentityRole> roleManager)
        {
            this.applicationLifetime = applicationLifetime;
            _roleManager = roleManager;
        }

        [HttpGet]
        [Route("ping")]
        public ActionResult Ping()
        {
            return Ok(DateTime.UtcNow);
        }

        [HttpGet]
        [Route("stop-application")]
        public ActionResult StopApplication()
        {
            applicationLifetime.StopApplication();
            return Ok(DateTime.UtcNow);
        }

        [HttpGet]
        [Route("clear-cache")]
        public async Task<ActionResult> ClearCacheAsync()
        {
            await MixCacheService.RemoveCacheAsync();
            return Ok(DateTime.UtcNow);
        }

        [HttpGet]
        [Route("json-data/{name}/{isArray}")]
        public ActionResult loadJsonData(string name, bool isArray)
        {
            var data = MixFileRepository.Instance.GetFile(name, MixFolders.DataFolder, true, "{}");
            var obj = JObject.Parse(data.Content);
            return isArray
                    ? Ok(obj["data"] as JArray)
                    : Ok(obj);
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
            var lastUpdate = MixService.GetAppSetting<DateTime>("LastUpdateConfiguration");
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

        [HttpGet]
        [Route("permissions")]
        public async Task<JObject> GetPermissions()
        {
            RepositoryResponse<List<MixRoles.ReadViewModel>> permissions = new RepositoryResponse<List<MixRoles.ReadViewModel>>()
            {
                IsSucceed = true,
                Data = new List<MixRoles.ReadViewModel>()
            };
            var roles = User.Claims.Where(c => c.Type == MixClaims.Role).ToList();
            if (!roles.Any(role => role.Value.ToUpper() == MixDefaultRoles.SuperAdmin))
            {
                foreach (var item in roles)
                {
                    var role = await _roleManager.FindByNameAsync(item.Value);
                    var temp = await MixRoles.ReadViewModel.Repository.GetSingleModelAsync(r => r.Id == role.Id);
                    if (temp.IsSucceed)
                    {
                        await temp.Data.LoadPermissions();
                        permissions.Data.Add(temp.Data);
                    }
                }
            }
            return JObject.FromObject(permissions);
        }

        private RepositoryResponse<JObject> GetAllSettings(string lang = null)
        {
            lang ??= MixService.GetAppSetting<string>(MixAppSettingKeywords.DefaultCulture);
            var cultures = CommonRepository.Instance.LoadCultures();
            var culture = cultures.FirstOrDefault(c => c.Specificulture == lang);

            // Get Settings
            GlobalSettingsViewModel configurations = new GlobalSettingsViewModel()
            {
                Domain = MixService.GetAppSetting<string>(MixAppSettingKeywords.Domain),
                Lang = lang,
                PortalThemeSettings = MixService.GetAppSetting<JObject>(MixAppSettingKeywords.PortalThemeSettings),
                ThemeId = MixService.GetConfig<int>(MixAppSettingKeywords.ThemeId, lang),
                ApiEncryptKey = MixService.GetAppSetting<string>(MixAppSettingKeywords.ApiEncryptKey),
                IsEncryptApi = MixService.GetAppSetting<bool>(MixAppSettingKeywords.IsEncryptApi),
                Cultures = cultures,
                PageTypes = Enum.GetNames(typeof(MixPageType)),
                ModuleTypes = Enum.GetNames(typeof(MixModuleType)),
                MixDatabaseTypes = Enum.GetNames(typeof(MixDatabaseType)),
                DataTypes = Enum.GetNames(typeof(MixDataType)),
                Statuses = Enum.GetNames(typeof(MixContentStatus)),
                RSAKeys = RSAEncryptionHelper.GenerateKeys(),
                ExternalLoginProviders = new JObject()
                {
                    new JProperty("Facebook", MixService.Instance.MixAuthentications.Facebook?.AppId),
                    new JProperty("Google", MixService.Instance.MixAuthentications.Google?.AppId),
                    new JProperty("Twitter", MixService.Instance.MixAuthentications.Twitter?.AppId),
                    new JProperty("Microsoft", MixService.Instance.MixAuthentications.Microsoft?.AppId),
                },
                LastUpdateConfiguration = MixService.GetAppSetting<DateTime?>(MixAppSettingKeywords.LastUpdateConfiguration)

            };

            configurations.LangIcon = culture?.Icon ?? MixService.GetAppSetting<string>(MixAppSettingKeywords.Language);

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

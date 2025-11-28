using Microsoft.AspNetCore.Mvc;
using Mix.Auth.Constants;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Exceptions;
using Mix.Lib.Extensions;
using Mix.Lib.Interfaces;
using Mix.Lib.Services;
using Mix.Mq.Lib.Models;
using Mix.Queue.Interfaces;
using Mix.Service.Models;

namespace Mix.Common.Controllers
{
    // TODO: NEED TO ENHANCE SECURITY FOR THESE APIs
    [Route("api/v2/rest/settings")]
    [ApiController]
    [MixAuthorize(roles: MixRoles.Owner)]
    public class SettingApiController : MixTenantApiControllerBase
    {
        private ConfigurationServiceBase<JObject> _settingService;
        public SettingApiController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            MixIdentityService mixIdentityService,
            IMemoryQueueService<MessageQueueModel> queueService,
            IMixTenantService mixTenantService)
            : base(httpContextAccessor, configuration,
                  cacheService, mixIdentityService, queueService, mixTenantService)
        {
        }

        #region Routes

        [HttpGet]
        [Route("get-tenant-settings")]
        public ActionResult<JObject> GetTenantSettings()
        {
            try
            {
                return Ok(CurrentTenant.Configurations);
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.NotFound, ex);
            }
        }


        [HttpGet]
        [Route("{settingType}")]
        public ActionResult<JObject> LoadData(MixAppConfigEnums settingType)
        {
            try
            {
                string filePath = GetSettingFilePath(settingType);
                if (!string.IsNullOrEmpty(filePath))
                {
                    _settingService = new(filePath, aesKey: Configuration.AesKey());

                    return Ok(_settingService.RawSettings);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.NotFound, ex);
            }
        }

        [HttpPost]
        [Route("{settingType}")]
        public ActionResult<JObject> SaveData(MixAppConfigEnums settingType, JObject appSettings)
        {
            try
            {
                string filePath = GetSettingFilePath(settingType);
                if (!string.IsNullOrEmpty(filePath))
                {
                    _settingService = new(filePath, aesKey: Configuration.AesKey());
                    _settingService.RawSettings = appSettings;
                    _settingService.SaveSettings();
                    return Ok(_settingService.AppSettings);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.NotFound, ex);
            }
        }



        #endregion

        #region Helpers

        private string GetSettingFilePath(MixAppConfigEnums settingType)
        {
            return settingType switch
            {
                MixAppConfigEnums.Portal => MixAppConfigFilePaths.Portal,
                MixAppConfigEnums.Authentication => MixAppConfigFilePaths.Authentication,
                MixAppConfigEnums.Quartz => MixAppConfigFilePaths.Quartz,
                MixAppConfigEnums.IPSecurity => MixAppConfigFilePaths.IPSecurity,
                MixAppConfigEnums.EPPlus => MixAppConfigFilePaths.EPPlus,
                MixAppConfigEnums.MixHeart => MixAppConfigFilePaths.MixHeart,
                MixAppConfigEnums.Smtp => MixAppConfigFilePaths.Smtp,
                MixAppConfigEnums.Endpoint => MixAppConfigFilePaths.Endpoint,
                MixAppConfigEnums.Global => MixAppConfigFilePaths.Global,
                MixAppConfigEnums.Azure => MixAppConfigFilePaths.Azure,
                MixAppConfigEnums.Ocelot => MixAppConfigFilePaths.Ocelot,
                MixAppConfigEnums.Queue => MixAppConfigFilePaths.Queue,
                MixAppConfigEnums.Storage => MixAppConfigFilePaths.Storage,
                MixAppConfigEnums.Payments => MixAppConfigFilePaths.Payments,
                MixAppConfigEnums.Redis => MixAppConfigFilePaths.Redis,
                MixAppConfigEnums.Log => MixAppConfigFilePaths.Log,
                MixAppConfigEnums.RateLimit => MixAppConfigFilePaths.RateLimit,
                MixAppConfigEnums.Google => MixAppConfigFilePaths.Google,
                MixAppConfigEnums.GoogleCredential => MixAppConfigFilePaths.GoogleCredential,
                _ => string.Empty
            };
        }

        #endregion

    }
}

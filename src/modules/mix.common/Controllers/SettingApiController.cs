using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Mix.Common.Domain.Dtos;
using Mix.Common.Domain.Helpers;
using Mix.Common.Domain.Models;
using Mix.Common.Domain.ViewModels;
using Mix.Common.Models;
using Mix.Identity.Constants;
using Mix.Lib.Models.Configurations;
using Mix.Lib.Services;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Shared.Models.Configurations;
using Mix.Shared.Services;
using ApplicationLifetime = Microsoft.Extensions.Hosting.IHostApplicationLifetime;

namespace Mix.Common.Controllers
{
    // TODO: NEED TO ENHANCE SECURITY FOR THESE APIs
    [Route("api/v2/rest/settings")]
    [ApiController]
    [MixAuthorize(roles: MixRoles.Owner)]
    public class SettingApiController : MixTenantApiControllerBase
    {
        private ConfigurationServiceBase<JObject> _settingService;
        private readonly ApplicationLifetime _applicationLifetime;
        public SettingApiController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            IQueueService<MessageQueueModel> queueService,
            ApplicationLifetime applicationLifetime)
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, queueService)
        {
            _applicationLifetime = applicationLifetime;
        }

        #region Routes

        [HttpGet]
        [Route("get-tenant-settings")]
        public ActionResult<JObject> GetTenantSettings()
        {
            try
            {
                TenantConfigService service = new(CurrentTenant.SystemName);
                if (service != null)
                {
                    return Ok(service.AppSettings);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
            }
        }
        
        [HttpPost]
        [Route("save-tenant-settings")]
        public ActionResult<JObject> SaveTenantSettings(TenantConfigurationModel appSettings)
        {
            try
            {
                TenantConfigService service = new(CurrentTenant.SystemName);
                if (service != null)
                {
                    service.AppSettings = appSettings;
                    service.SaveSettings();
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
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
                    _settingService = new(filePath);
                    return Ok(_settingService.AppSettings);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
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
                    _settingService = new(filePath);
                    _settingService.AppSettings = appSettings;
                    _settingService.SaveSettings();
                    return Ok(_settingService.AppSettings);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return NotFound();
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
                _ => string.Empty
            };
        }

        #endregion

    }
}

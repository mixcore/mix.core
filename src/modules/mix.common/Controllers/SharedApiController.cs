using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mix.Shared.Constants;
using Mix.Lib.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using Mix.Shared.Services;
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
using Mix.Common.Domain.Dtos;
using Mix.Common.Domain.Helpers;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Heart.Services;

namespace Mix.Common.Controllers
{
    [Route("api/v2/rest/shared")]
    [ApiController]
    public class SharedApiController : MixApiControllerBase
    {
        private readonly ViewQueryRepository<MixCmsContext, MixConfigurationContent, int, MixConfigurationContentViewModel> _configRepo;
        private readonly ViewQueryRepository<MixCmsContext, MixLanguageContent, int, MixLanguageContentViewModel> _langRepo;
        private readonly MixFileService _fileService;
        protected readonly CultureService _cultureService;
        private readonly AuthConfigService _authConfigService;
        private readonly MixAuthenticationConfigurations _authConfigurations;
        private readonly IActionDescriptorCollectionProvider _routeProvider;
        private IQueueService<MessageQueueModel> _queueService;
        public SharedApiController(
            ILogger<MixApiControllerBase> logger,
            GlobalConfigService globalConfigService,
            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixFileService fileService,
            IActionDescriptorCollectionProvider routeProvider,
            MixIdentityService mixIdentityService, AuthConfigService authConfigService,
            CultureService cultureService,
            MixCmsContext context, IQueueService<MessageQueueModel> queueService)
            : base(logger, globalConfigService, mixService, translator, cultureRepository, mixIdentityService)
        {
            _fileService = fileService;
            _authConfigurations = authConfigService.AuthConfigurations;
            _configRepo = MixConfigurationContentViewModel.GetRootRepository(context);
            _langRepo = MixLanguageContentViewModel.GetRootRepository(context);
            _routeProvider = routeProvider;
            _authConfigService = authConfigService;
            _cultureService = cultureService;
            _queueService = queueService;
        }

        #region Routes

        [HttpPost]
        [Route("encrypt-message")] 
        public ActionResult<string> EncryptMessage(CryptoMessageDto encryptMessage)
        {
            string key = encryptMessage.Key 
                        ?? _globalConfigService.GetConfig<string>(MixAppSettingKeywords.ApiEncryptKey);
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
            string key = encryptMessage.Key ?? _globalConfigService.GetConfig<string>(MixAppSettingKeywords.ApiEncryptKey);
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
            return new AllSettingModel()
            {
                GlobalSettings = CommonHelper.GetAppSettings(lang, _authConfigurations, _globalConfigService, _cultureService),
                MixConfigurations = await _configRepo.GetListAsync(m => m.Specificulture == lang),
                Translator = _langRepo.GetListQuery(m => m.Specificulture == lang).ToList()
            };
        }
    }
}

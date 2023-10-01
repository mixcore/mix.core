using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Mix.Common.Domain.Dtos;
using Mix.Common.Domain.Models;
using Mix.Common.Domain.ViewModels;
using Mix.Database.Entities.MixDb;
using Mix.Heart.Exceptions;
using Mix.Identity.Constants;
using Mix.Lib.Interfaces;
using Mix.Lib.Services;
using Mix.Mixdb.ViewModels;
using Mix.Queue.Engines.MixQueue;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Shared.Models.Configurations;
using Mix.Shared.Services;
using System.Diagnostics;
using ApplicationLifetime = Microsoft.Extensions.Hosting.IHostApplicationLifetime;

namespace Mix.Common.Controllers
{
    [Route("api/v2/rest/shared")]
    [ApiController]
    public sealed class SharedApiController : MixTenantApiControllerBase
    {
        private readonly MixQueueMessages<MessageQueueModel> _mixMemoryMessageQueue;
        private readonly MixCacheService _cacheService;
        private readonly ApplicationLifetime _applicationLifetime;
        private readonly UnitOfWorkInfo<MixCmsContext> _uow;
        private readonly IMixCmsService _mixCmsService;
        private readonly ViewQueryRepository<MixCmsContext, MixConfigurationContent, int, MixConfigurationContentViewModel> _configRepo;
        private readonly ViewQueryRepository<MixCmsContext, MixLanguageContent, int, MixLanguageContentViewModel> _langRepo;
        private readonly IActionDescriptorCollectionProvider _routeProvider;

        public SharedApiController(
            IConfiguration configuration,
            TranslatorService translator,
            IActionDescriptorCollectionProvider routeProvider,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> uow,
            IQueueService<MessageQueueModel> queueService,
            ApplicationLifetime applicationLifetime,
            MixCacheService cacheService,
            IHttpContextAccessor httpContextAccessor,
            MixQueueMessages<MessageQueueModel> mixMemoryMessageQueue,
            IMixTenantService mixTenantService,
            IMixCmsService mixCmsService)
            : base(httpContextAccessor, configuration,
                  cacheService, translator, mixIdentityService, queueService, mixTenantService)
        {
            _uow = uow;
            _cacheService = cacheService;
            _configRepo = MixConfigurationContentViewModel.GetRepository(_uow, _cacheService);
            _langRepo = MixLanguageContentViewModel.GetRepository(_uow, _cacheService);
            _routeProvider = routeProvider;
            _applicationLifetime = applicationLifetime;
            _mixMemoryMessageQueue = mixMemoryMessageQueue;
            _mixCmsService = mixCmsService;
        }

        #region Routes

        [HttpPost]
        [Route("encrypt-message")]
        public ActionResult<string> EncryptMessage(CryptoMessageDto encryptMessage)
        {
            string key = encryptMessage.Key
                        ?? GlobalConfigService.Instance.AppSettings.ApiEncryptKey;
            string msg = encryptMessage.ObjectData != null
                    ? encryptMessage.ObjectData.ToString(Formatting.None)
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
        [MixAuthorize(roles: $"{MixRoles.SuperAdmin},{MixRoles.Owner}")]
        [Route("stop-application")]
        public async Task<ActionResult> StopApplication()
        {
            _applicationLifetime.StopApplication();
            string _currentProcess = Path.GetFullPath(Process.GetCurrentProcess().MainModule.FileName);

            Process.Start(_currentProcess);

            await Task.FromResult(0);
            return Ok(DateTime.UtcNow);
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
        [Route("queues")]
        public ActionResult GetQueues()
        {
            return Ok(ReflectionHelper.ParseArray(_mixMemoryMessageQueue.GetAllTopic()));
        }

        [MixAuthorize(roles: MixRoles.SuperAdmin)]
        [HttpGet]
        [Route("clear-cache")]
        public async Task<ActionResult> ClearCacheAsync(CancellationToken cancellationToken = default)
        {
            await _cacheService.ClearAllCacheAsync(cancellationToken);
            return Ok();
        }

        [HttpGet("sitemap")]
        public async Task<ActionResult> Sitemap(CancellationToken cancellationToken = default)
        {
            var file = await _mixCmsService.ParseSitemapAsync(cancellationToken);
            return Ok(file);
        }

        [HttpGet]
        [Route("mix-configuration/{lang}")]
        public ActionResult GetMixConfigurations(string lang)
        {
            return Ok(_configRepo.GetListQuery(c => c.Specificulture == lang).ToList());
        }

        [MixAuthorize(roles: $"{MixRoles.SuperAdmin},{MixRoles.Owner}")]
        [HttpGet]
        [Route("settings/{name}")]
        public ActionResult GetSettings(string name)
        {
            try
            {
                var data = MixFileHelper.GetFile(name, MixFileExtensions.Json, MixFolders.AppConfigFolder);
                var obj = JObject.Parse(data.Content);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.NotFound, ex);
            }
        }

        [MixAuthorize(roles: $"{MixRoles.SuperAdmin},{MixRoles.Owner}")]
        [HttpPost]
        [Route("settings/{name}")]
        public ActionResult SaveSettings(string name, [FromBody] JObject settings)
        {
            try
            {
                var data = MixFileHelper.GetFile(
                    name, MixFileExtensions.Json, MixFolders.AppConfigFolder);
                if (data != null)
                {
                    data.Content = settings.ToString();
                }
                MixFileHelper.SaveFile(data);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.NotFound, ex);
            }
        }

        [HttpGet]
        [Route("global-settings")]
        public ActionResult GetGlobalSettings()
        {
            return Ok(GlobalConfigService.Instance.AppSettings);
        }

        [HttpPost]
        [Route("global-settings")]
        public ActionResult GetSettings([FromBody] GlobalSettingsModel settings)
        {
            GlobalConfigService.Instance.AppSettings = settings;
            GlobalConfigService.Instance.SaveSettings();
            return Ok(GlobalConfigService.Instance.AppSettings);
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


        [AllowAnonymous]
        [HttpGet]
        [Route("json-data/{name}")]
        public ActionResult<JObject> LoadData(string name)
        {
            try
            {
                var data = MixFileHelper.GetFile(
                    name, MixFileExtensions.Json, MixFolders.JsonDataFolder);
                var obj = JObject.Parse(data.Content);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.NotFound, ex);
            }
        }

        #endregion
    }
}

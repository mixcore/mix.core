using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Mix.Auth.Constants;
using Mix.Common.Domain.Dtos;
using Mix.Common.Domain.Models;
using Mix.Common.Domain.ViewModels;
using Mix.Database.Entities.MixDb;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Exceptions;
using Mix.Lib.Extensions;
using Mix.Lib.Interfaces;
using Mix.Lib.Services;
using Mix.Mixdb.ViewModels;
using Mix.Mq.Lib.Models;
using Mix.Queue.Engines.MixQueue;
using Mix.Queue.Interfaces;
using Mix.Shared.Models.Configurations;
using Mix.SignalR.Enums;
using Mix.SignalR.Hubs;
using Mix.SignalR.Interfaces;
using Mix.SignalR.Models;
using System.Diagnostics;
using ApplicationLifetime = Microsoft.Extensions.Hosting.IHostApplicationLifetime;

namespace Mix.Common.Controllers
{
    [Route("api/v2/rest/shared")]
    [ApiController]
    public sealed class SharedApiController : MixTenantApiControllerBase
    {
        private readonly IPortalHubClientService _portalHub;
        private readonly MixCacheService _cacheService;
        private readonly GlobalSettingsService _globalSettingSrv;
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
            IMemoryQueueService<MessageQueueModel> queueService,
            ApplicationLifetime applicationLifetime,
            MixCacheService cacheService,
            IHttpContextAccessor httpContextAccessor,
            IMixTenantService mixTenantService,
            IMixCmsService mixCmsService,
            IPortalHubClientService portalHub,
            GlobalSettingsService globalSettingSrv)
            : base(httpContextAccessor, configuration,
                  cacheService, translator, mixIdentityService, queueService, mixTenantService)
        {
            _uow = uow;
            _cacheService = cacheService;
            _configRepo = MixConfigurationContentViewModel.GetRepository(_uow, _cacheService);
            _langRepo = MixLanguageContentViewModel.GetRepository(_uow, _cacheService);
            _routeProvider = routeProvider;
            _applicationLifetime = applicationLifetime;
            _mixCmsService = mixCmsService;
            _portalHub = portalHub;
            _globalSettingSrv = globalSettingSrv;
        }

        #region Routes

        [HttpPost]
        [Route("encrypt-message")]
        public ActionResult<string> EncryptMessage(CryptoMessageDto encryptMessage)
        {
            string msg = encryptMessage.ObjectData != null
                    ? encryptMessage.ObjectData.ToString(Formatting.None)
                    : encryptMessage.StringData;
            var result = AesEncryptionHelper.EncryptString(msg, Configuration.AesKey()
                , encryptMessage.GetEncoding());
            return Ok(result);
        }

        [HttpPost]
        [Route("decrypt-message")]
        public ActionResult<string> DecryptMessage(CryptoMessageDto encryptMessage)
        {
            string msg = AesEncryptionHelper.DecryptString(
                encryptMessage.StringData, 
                Configuration.AesKey(), encryptMessage.GetEncoding());
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

        [MixAuthorize(roles: MixRoles.SuperAdmin)]
        [HttpGet]
        [Route("clear-cache")]
        public async Task<ActionResult> ClearCacheAsync(CancellationToken cancellationToken = default)
        {
            await _cacheService.ClearAllCacheAsync(cancellationToken);
            await _portalHub.SendMessageAsync(new SignalRMessageModel()
            {
                Action = MessageAction.NewQueueMessage,
                Title = MixQueueTopics.MixViewModelChanged,
                Message = MixQueueActions.ClearCache,
                Type = MessageType.Success,
            });
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
            return Ok(_globalSettingSrv.RawSettings);
        }

        [HttpPost]
        [Route("global-settings")]
        public ActionResult GetSettings([FromBody] JObject settings)
        {
            _globalSettingSrv.RawSettings = settings;
            _globalSettingSrv.SaveSettings();
            return Ok(_globalSettingSrv.RawSettings);
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

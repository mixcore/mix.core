 using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Mix.Common.Domain.Dtos;
using Mix.Common.Domain.Models;
using Mix.Common.Domain.ViewModels;
using Mix.Heart.Exceptions;
using Mix.Identity.Constants;
using Mix.Lib.Services;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Shared.Models.Configurations;
using Mix.Shared.Services;
using ApplicationLifetime = Microsoft.Extensions.Hosting.IHostApplicationLifetime;

namespace Mix.Common.Controllers
{
    [Route("api/v2/rest/shared")]
    [ApiController]
    public class SharedApiController : MixApiControllerBase
    {
        private readonly MixCacheService _cacheService;
        private readonly ApplicationLifetime _applicationLifetime;
        protected UnitOfWorkInfo Uow;
        protected readonly MixCmsContext Context;
        private readonly ViewQueryRepository<MixCmsContext, MixConfigurationContent, int, MixConfigurationContentViewModel> _configRepo;
        private readonly ViewQueryRepository<MixCmsContext, MixLanguageContent, int, MixLanguageContentViewModel> _langRepo;
        private readonly IActionDescriptorCollectionProvider _routeProvider;

        public SharedApiController(
            IConfiguration configuration,
            TranslatorService translator,
            IActionDescriptorCollectionProvider routeProvider,
            MixIdentityService mixIdentityService,
            MixCmsContext context, IQueueService<MessageQueueModel> queueService, ApplicationLifetime applicationLifetime, MixCacheService cacheService)
            : base(configuration, translator, mixIdentityService, queueService)
        {
            Context = context;
            _cacheService = cacheService;
            Uow = new(Context);
            _configRepo = MixConfigurationContentViewModel.GetRepository(Uow, _cacheService);
            _langRepo = MixLanguageContentViewModel.GetRepository(Uow, _cacheService);
            _routeProvider = routeProvider;
            _applicationLifetime = applicationLifetime;
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
        public ActionResult StopApplication()
        {
            _applicationLifetime.StopApplication();
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
            return Ok();
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
        public ActionResult GetSettings([FromBody] GlobalConfigurations settings)
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

using Microsoft.AspNetCore.Mvc;
using Mix.Database.Entities.Settings;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Lib.Extensions;
using Mix.Lib.Interfaces;
using Mix.Mq.Lib.Models;
using Mix.SignalR.Interfaces;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/global-settings")]
    [ApiController]
    [MixAuthorize("Owner")]
    public class MixGlobalSettingController
        : MixRestfulApiControllerBase<MixGlobalSettingViewModel, GlobalSettingContext, MixGlobalSetting, int>
    {
        public MixGlobalSettingController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<GlobalSettingContext> uow,
            IMemoryQueueService<MessageQueueModel> queueService,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService)
            : base(httpContextAccessor, configuration,
                  cacheService, translator, mixIdentityService, uow, queueService, portalHub, mixTenantService)
        {
        }

        #region Routes

        #endregion

        #region Overrides
        protected override async Task UpdateHandler(int id, MixGlobalSettingViewModel data, CancellationToken cancellationToken = default)
        {
            if (data.IsEncrypt && !data.Settings.IsBase64())
            {
                data.Settings = data.Settings.Encrypt(Configuration.AesKey());
            }

            await base.UpdateHandler(id, data, cancellationToken);

        }

        protected override Task<int> CreateHandlerAsync(MixGlobalSettingViewModel data, CancellationToken cancellationToken = default)
        {
            if (data.IsEncrypt && !data.Settings.IsBase64())
            {
                data.Settings = data.Settings.Encrypt(Configuration.AesKey());
            }
            return base.CreateHandlerAsync(data, cancellationToken);
        }

        #endregion
    }
}

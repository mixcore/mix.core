using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mix.Database.Entities.Account;
using Mix.Identity.Interfaces;
using Mix.Identity.ViewModels;
using Mix.Lib.Interfaces;
using Mix.Lib.Services;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.SignalR.Interfaces;

namespace Mix.Account.Controllers
{
    [MixAuthorize(roles: MixRoles.Owner)]
    [Route("api/auth/oauth-client")]
    [ApiController]
    public class OAuthClientController : MixRestfulApiControllerBase<OAuthClientViewModel, MixCmsAccountContext, OAuthClient, Guid>
    {
        private readonly IOAuthClientService _oauthClientService;
        public OAuthClientController(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, MixCacheService cacheService, TranslatorService translator, MixIdentityService mixIdentityService, UnitOfWorkInfo<MixCmsAccountContext> uow, IQueueService<MessageQueueModel> queueService, IPortalHubClientService portalHub, IMixTenantService mixTenantService, IOAuthClientService oauthClientService) : base(httpContextAccessor, configuration, cacheService, translator, mixIdentityService, uow, queueService, portalHub, mixTenantService)
        {
            _oauthClientService = oauthClientService;
        }

        protected override async Task<Guid> CreateHandlerAsync(OAuthClientViewModel data, CancellationToken cancellationToken = default)
        {
            var result = await base.CreateHandlerAsync(data, cancellationToken);
            _oauthClientService.LoadClients(true);
            return result;
        }
        protected override async Task UpdateHandler(Guid id, OAuthClientViewModel data, CancellationToken cancellationToken = default)
        {
            await base.UpdateHandler(id, data, cancellationToken);
            _oauthClientService.LoadClients(true);
        }
        protected override async Task DeleteHandler(OAuthClientViewModel data, CancellationToken cancellationToken = default)
        {
            await base.DeleteHandler(data, cancellationToken);
            _oauthClientService.LoadClients(true);
        }
    }
}

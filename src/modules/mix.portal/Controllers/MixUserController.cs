using Microsoft.AspNetCore.Mvc;
using Mix.Lib.Interfaces;

namespace Mix.Portal.Controllers;

[Route("api/v2/rest/mix-portal/mix-users")]
[ApiController]
[MixAuthorize(MixRoles.Owner)]
public class MixUserController : MixTenantApiControllerBase
{
    public MixUserController(
        IHttpContextAccessor httpContextAccessor,
        IConfiguration configuration,
        MixCacheService cacheService,
        TranslatorService translator,
        MixIdentityService mixIdentityService,
        IQueueService<MessageQueueModel> queueService,
        IMixTenantService mixTenantService)
        : base(
            httpContextAccessor,
            configuration,
            cacheService,
            translator,
            mixIdentityService,
            queueService,
            mixTenantService)
    {
    }

    [MixAuthorize]
    [Route("")]
    [HttpGet]
    public Task<ActionResult> MyTenants()
    {
        return Task.FromResult<ActionResult>(Ok());
    }
}
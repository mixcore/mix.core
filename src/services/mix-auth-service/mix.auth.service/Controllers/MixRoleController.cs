using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mix.Database.Entities.Account;
using Mix.Heart.Entities.Cache;
using Mix.Lib.Interfaces;
using Mix.Lib.Services;
using Mix.Mq.Lib.Models;
using Mix.Queue.Interfaces;

namespace mix.auth.service.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Owner")]
    [Route("api/v2/rest/auth/role")]
    [ApiController]
    public class MixRoleController : MixRestEntityApiControllerBase<MixCmsAccountContext, MixRole, Guid>
    {
        public MixRoleController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            MixCacheDbContext cacheDbContext,
            MixCmsAccountContext context,
            IQueueService<MessageQueueModel> queueService,
            IMixTenantService mixTenantService)
            : base(httpContextAccessor, configuration,
                  cacheService, translator, mixIdentityService, cacheDbContext, context, queueService, mixTenantService)
        {

        }

    }
}

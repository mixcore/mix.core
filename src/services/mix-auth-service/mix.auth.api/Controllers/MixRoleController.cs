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
        private readonly TenantRoleManager _roleManager;
        public MixRoleController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            MixCacheDbContext cacheDbContext,
            MixCmsAccountContext context,
            IMemoryQueueService<MessageQueueModel> queueService,
            IMixTenantService mixTenantService,
            TenantRoleManager roleManager)
            : base(httpContextAccessor, configuration,
                  cacheService, translator, mixIdentityService, cacheDbContext, context, queueService, mixTenantService)
        {
            _roleManager = roleManager;
        }

        protected override async Task<Guid> CreateHandlerAsync(MixRole data)
        {
            data.Id = Guid.NewGuid();
            var result = await _roleManager.CreateAsync(data);
            if (result.Succeeded)
            {
                return data.Id;
            }
            else
            {
                throw new MixException(MixErrorStatus.Badrequest, "Invalid Role", result.Errors.Select(m=>m.Description).ToArray());
            }
        }
        protected override async Task DeleteHandler(MixRole data)
        {
            await _roleManager.DeleteAsync(data);
            await base.DeleteHandler(data);
        }
    }
}

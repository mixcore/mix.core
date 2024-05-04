using Microsoft.AspNetCore.Mvc;
using Mix.Database.Entities.Account;
using Mix.Heart.Entities.Cache;
using Mix.Heart.Models;
using Mix.Identity.Models;
using Mix.Lib.Interfaces;
using Mix.Lib.Services;
using Mix.Mq.Lib.Models;
using Mix.Queue.Interfaces;
using Mix.Services.Databases.Lib.Interfaces;

namespace mix.auth.service.Controllers
{
    [MixAuthorize(roles: MixRoles.Owner)]
    [Route("api/v2/rest/auth/role")]
    [ApiController]
    public class MixRoleController : MixRestEntityApiControllerBase<MixCmsAccountContext, MixRole, Guid>
    {
        private readonly TenantRoleManager _roleManager;
        private readonly IMixPermissionService _permissionService;
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
            TenantRoleManager roleManager,
            IMixPermissionService permissionService)
            : base(httpContextAccessor, configuration,
                  cacheService, translator, mixIdentityService, cacheDbContext, context, queueService, mixTenantService)
        {
            _roleManager = roleManager;
            _permissionService = permissionService;
        }
        public override async Task<ActionResult<PagingResponseModel<MixRole>>> Get([FromQuery] SearchRequestDto req)
        {
            var roles = await base.GetHandler(req);
            var result = new PagingResponseModel<RoleBOModel>()
            {
                PagingData = roles.PagingData
            };
            var models = new List<RoleBOModel>();
            foreach (var item in roles.Items)
            {
                models.Add(new RoleBOModel()
                {
                    ConcurrencyStamp = item.ConcurrencyStamp,
                    Id = item.Id,
                    Name = item.Name,
                    NormalizedName = item.NormalizedName,
                    Permissions = await _permissionService.GetPermissionByRoleId(item.Id)
                });
            }
            result.Items = models;
            return Ok(result);
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
                throw new MixException(MixErrorStatus.Badrequest, "Invalid Role", result.Errors.Select(m => m.Description).ToArray());
            }
        }
        protected override async Task DeleteHandler(MixRole data)
        {
            await _roleManager.DeleteAsync(data);
        }
    }
}

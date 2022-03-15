using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Mix.Database.Entities.Account;
using Mix.Identity.Enums;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-tenant")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MixTenantController
        : MixRestApiControllerBase<MixTenantViewModel, MixCmsContext, MixTenant, int>
    {
        private readonly TenantUserManager _userManager;
        private readonly RoleManager<MixRole> _roleManager;
        public MixTenantController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixIdentityService mixIdentityService,
            MixCmsContext context,
            IQueueService<MessageQueueModel> queueService,
            RoleManager<MixRole> roleManager, TenantUserManager userManager)
            : base(httpContextAccessor, configuration, mixService, translator, cultureRepository, mixIdentityService, context, queueService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        #region Overrides
        protected override async Task<int> CreateHandlerAsync(MixTenantViewModel data)
        {
            var tenantId = await base.CreateHandlerAsync(data);
            await _uow.CompleteAsync();
            var roles = MixHelper.LoadEnumValues(typeof(MixRoles));
            foreach (var role in roles)
            {
                await _roleManager.CreateAsync(new MixRole()
                {
                    Id = Guid.NewGuid(),
                    Name = role.ToString(),
                    MixTenantId = tenantId
                }
                );
            }
            var user = await _userManager.FindByIdAsync(_mixIdentityService.GetClaim(User, MixClaims.Id));
            await _userManager.AddToRoleAsync(user, MixRoles.Owner.ToString(), tenantId);
            await _userManager.AddToTenant(user, tenantId);
            return tenantId;
        }
        #endregion


    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Account;
using Mix.Identity.Enums;
using Mix.Lib.Repositories;

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
        private readonly MixCmsAccountContext _accContext;
        public MixTenantController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixIdentityService mixIdentityService,
            MixCmsContext context,
            IQueueService<MessageQueueModel> queueService,
            RoleManager<MixRole> roleManager, TenantUserManager userManager, MixCmsAccountContext accContext)
            : base(httpContextAccessor, configuration, mixService, translator, cultureRepository, mixIdentityService, context, queueService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _accContext = accContext;
        }

        #region Overrides
        protected override async Task<int> CreateHandlerAsync(MixTenantViewModel data)
        {
            var tenantId = await base.CreateHandlerAsync(data);
            MixTenantRepository.Instance.AllTenants = await _context.MixTenant.ToListAsync();
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
            await _userManager.AddToRoleAsync(user, MixRoles.Owner, tenantId);
            await _userManager.AddToTenant(user, tenantId);
            return tenantId;
        }

        protected override async Task UpdateHandler(string id, MixTenantViewModel data)
        {
            await base.UpdateHandler(id, data);
            MixTenantRepository.Instance.AllTenants = await _context.MixTenant.ToListAsync();
        }

        protected override async Task DeleteHandler(MixTenantViewModel data)
        {
            await base.DeleteHandler(data);
            MixTenantRepository.Instance.AllTenants = await _context.MixTenant.ToListAsync();
            await _uow.CompleteAsync();
            foreach (var item in _accContext.MixRoles.Where(m => m.MixTenantId == data.Id))
            {
                _accContext.Entry(item).State = EntityState.Deleted;
            }
            foreach (var item in _accContext.MixUserTenants.Where(m => m.TenantId == data.Id))
            {
                _accContext.Entry(item).State = EntityState.Deleted;
            }
            await _accContext.SaveChangesAsync();
        }
        #endregion


    }
}

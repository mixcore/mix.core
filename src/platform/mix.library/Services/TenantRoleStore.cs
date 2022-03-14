using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Mix.Database.Entities.Account;

namespace Mix.Lib.Services
{
    public class TenantRoleStore : RoleStore<MixRole, MixCmsAccountContext, Guid, AspNetUserRoles, AspNetRoleClaims>
    {
        public TenantRoleStore(
            MixCmsAccountContext context,
            IdentityErrorDescriber describer = null)
            : base(context, describer)
        {
        }

        public override IQueryable<MixRole> Roles => base.Roles.Where(r => r.MixTenantId == MixTenantRepository.Instance.CurrentTenant.Id);
    }
}

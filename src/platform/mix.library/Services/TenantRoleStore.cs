using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Mix.Database.Entities.Account;

namespace Mix.Lib.Services
{
    public class TenantRoleStore : RoleStore<MixRole, MixCmsAccountContext, Guid, AspNetUserRoles, AspNetRoleClaims>
    {
        public readonly int tenantId;
        public TenantRoleStore(
            IHttpContextAccessor httpContext,
            MixCmsAccountContext accContext,
            IdentityErrorDescriber describer = null)

            : base(accContext, describer)
        {
            if (httpContext.HttpContext.Session.GetInt32(MixRequestQueryKeywords.MixTenantId).HasValue)
            {
                tenantId = httpContext.HttpContext.Session.GetInt32(MixRequestQueryKeywords.MixTenantId).Value;
            }
        }

        public override IQueryable<MixRole> Roles => base.Roles.Where(r => r.MixTenantId == tenantId);
    }
}

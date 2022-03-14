using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Mix.Database.Entities.Account;

namespace Mix.Lib.Services
{
    public class TenantUserStore : 
        UserStore<MixUser, MixRole, MixCmsAccountContext, Guid, AspNetUserClaims, AspNetUserRoles, AspNetUserLogins, AspNetUserTokens, AspNetRoleClaims>
    {
        public TenantUserStore(MixCmsAccountContext context, IdentityErrorDescriber describer = null) : base(context, describer)
        {
        }
    }
}

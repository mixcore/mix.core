using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Mix.Database.Entities.Account;
using Mix.Lib.Extensions;

namespace Mix.Lib.Services
{
    public class TenantRoleStore : RoleStore<MixRole, MixCmsAccountContext, Guid, AspNetUserRoles, AspNetRoleClaims>
    {
        protected IHttpContextAccessor _httpContextAccessor;
        private MixTenantSystemViewModel _currentTenant;
        protected MixTenantSystemViewModel CurrentTenant
        {
            get
            {
                if (_currentTenant == null)
                {
                    _currentTenant = _httpContextAccessor.HttpContext.Session.Get<MixTenantSystemViewModel>(MixRequestQueryKeywords.Tenant);
                }
                return _currentTenant;
            }
        }
        public TenantRoleStore(
            IHttpContextAccessor httpContextAccessor,
            MixCmsAccountContext accContext,
            IdentityErrorDescriber describer = null)

            : base(accContext, describer)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}

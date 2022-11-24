using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Mix.Database.Entities.Account;
using Mix.Lib.Extensions;
using Mix.Lib.Models;

namespace Mix.Lib.Services
{
    public class TenantRoleStore : RoleStore<MixRole, MixCmsAccountContext, Guid, AspNetUserRoles, AspNetRoleClaims>
    {
        protected IHttpContextAccessor _httpContextAccessor;
        private MixTenantSystemModel _currentTenant;
        protected MixTenantSystemModel CurrentTenant
        {
            get
            {
                if (_currentTenant == null)
                {
                    _currentTenant = _httpContextAccessor.HttpContext.Session.Get<MixTenantSystemModel>(MixRequestQueryKeywords.Tenant);
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

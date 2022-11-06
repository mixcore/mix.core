using Microsoft.AspNetCore.Http;
using Mix.Lib.Extensions;

namespace Mix.Lib.Base
{
    public abstract class TenantServiceBase
    {
        protected IHttpContextAccessor _httpContextAccessor;

        protected TenantServiceBase(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

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
    }
}

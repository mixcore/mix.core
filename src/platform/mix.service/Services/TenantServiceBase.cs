using Microsoft.AspNetCore.Http;
using Mix.Constant.Constants;
using Mix.Heart.Services;
using Mix.Service.Models;
using Mix.Shared.Extensions;

namespace Mix.Service.Services
{
    public abstract class TenantServiceBase
    {
        protected IHttpContextAccessor HttpContextAccessor;
        protected readonly MixCacheService CacheService;
        protected TenantServiceBase(IHttpContextAccessor httpContextAccessor, MixCacheService cacheService)
        {
            HttpContextAccessor = httpContextAccessor;
            CacheService = cacheService;
        }

        private MixTenantSystemModel _currentTenant;

        protected MixTenantSystemModel CurrentTenant
        {
            get
            {
                if (_currentTenant != null)
                {
                    return _currentTenant;
                }

                var httpContext = HttpContextAccessor.HttpContext;
                if (httpContext is not null)
                {
                    _currentTenant = httpContext.Session.Get<MixTenantSystemModel>(MixRequestQueryKeywords.Tenant);
                }

                return _currentTenant;
            }
        }
    }
}

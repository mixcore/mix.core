using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mix.Constant.Constants;
using Mix.Heart.Services;
using Mix.Lib.Interfaces;
using Mix.Service.Models;
using Mix.Shared.Extensions;
namespace Mix.Service.Services
{
    public abstract class TenantServiceBase
    {
        protected IHttpContextAccessor HttpContextAccessor;
        protected IMixTenantService MixTenantService;
        protected readonly MixCacheService CacheService;
        private readonly MixDitributedCache _cache;

        protected TenantServiceBase(IConfiguration configuration, MixDitributedCache cache)
        {
            _cache = cache;
            CacheService = new MixCacheService(configuration, cache);
        }

        protected TenantServiceBase(IHttpContextAccessor httpContextAccessor, MixCacheService cacheService, IMixTenantService mixTenantService)
        {
            HttpContextAccessor = httpContextAccessor;
            CacheService = cacheService;
            MixTenantService = mixTenantService;
        }

        protected MixTenantSystemModel? _currentTenant;

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
                    _currentTenant = httpContext.Session.Get<MixTenantSystemModel?>(MixRequestQueryKeywords.Tenant);
                }
                _currentTenant ??= MixTenantService.GetDefaultTenant().GetAwaiter().GetResult();

                return _currentTenant;
            }
        }
        
        public void SetTenant(MixTenantSystemModel tenant)
        {
            _currentTenant = tenant;
        }
        
        public void SetTenantId(int tenantId)
        {
            _currentTenant = new()
            {
                Id = tenantId
            };
        }
        protected bool IsValidCulture(string culture)
        {
            return CurrentTenant != null && !string.IsNullOrEmpty(culture)
                && CurrentTenant.Cultures.Any(m => m.Specificulture == culture);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Mix.Portal.Domain.Interfaces;

namespace Mix.Portal.Domain.Services
{
    public sealed class ThemeService : TenantServiceBase, IThemeService
    {
        private readonly UnitOfWorkInfo<MixCmsContext> _cmsUow;
        public ThemeService(IHttpContextAccessor httpContextAccessor, UnitOfWorkInfo<MixCmsContext> cmsUow, MixCacheService cacheService) : base(httpContextAccessor, cacheService)
        {
            _cmsUow = cmsUow;
        }

        public Task<MixTheme> GetActiveTheme()
        {
            return _cmsUow.DbContext.MixTheme.FirstOrDefaultAsync(m => m.MixTenantId == CurrentTenant.Id);
        }
    }
}

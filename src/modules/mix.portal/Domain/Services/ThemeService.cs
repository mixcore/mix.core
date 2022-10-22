using Microsoft.EntityFrameworkCore;

namespace Mix.Portal.Domain.Services
{
    public sealed class ThemeService : TenantServiceBase
    {
        public ThemeService(IHttpContextAccessor httpContextAccessor, UnitOfWorkInfo<MixCmsContext> cmsUOW) : base(httpContextAccessor)
        {
        }
        public Task<MixTheme> GetActiveTheme()
        {
            return _cmsUOW.DbContext.MixTheme.FirstOrDefaultAsync(m => m.MixTenantId == CurrentTenant.Id);
        }
    }
}

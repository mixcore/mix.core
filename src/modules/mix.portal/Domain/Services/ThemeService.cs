using Microsoft.EntityFrameworkCore;

namespace Mix.Portal.Domain.Services
{
    public class ThemeService : TenantServiceBase
    {
        public ThemeService(IHttpContextAccessor httpContextAccessor, UnitOfWorkInfo<MixCmsContext> cmsUOW) : base(httpContextAccessor, cmsUOW)
        {
        }
        public Task<MixTheme> GetActiveTheme()
        {
            return _cmsUOW.DbContext.MixTheme.FirstOrDefaultAsync(m => m.MixTenantId == CurrentTenant.Id);
        }
    }
}

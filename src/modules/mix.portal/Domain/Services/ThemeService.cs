using Microsoft.EntityFrameworkCore;

namespace Mix.Portal.Domain.Services
{
    public sealed class ThemeService : TenantServiceBase
    {
        private readonly UnitOfWorkInfo<MixCmsContext> _cmsUOW;
        public ThemeService(IHttpContextAccessor httpContextAccessor, UnitOfWorkInfo<MixCmsContext> cmsUOW) : base(httpContextAccessor)
        {
            _cmsUOW = cmsUOW;
        }
        public Task<MixTheme> GetActiveTheme()
        {
            return _cmsUOW.DbContext.MixTheme.FirstOrDefaultAsync(m => m.MixTenantId == CurrentTenant.Id);
        }
    }
}

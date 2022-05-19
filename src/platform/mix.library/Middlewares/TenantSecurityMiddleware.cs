using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Mix.Lib.Middlewares
{
    public class TenantSecurityMiddleware
    {
        private readonly RequestDelegate next;

        public TenantSecurityMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(
            HttpContext context,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            MixCmsContext cmsContext)
        {
            if (GlobalConfigService.Instance.InitStatus == InitStep.Blank)
            {
                await next.Invoke(context);
            }
            else
            {
                if (MixTenantRepository.Instance.AllTenants == null)
                {
                    var uow = new UnitOfWorkInfo(cmsContext);
                    MixTenantRepository.Instance.AllTenants = await MixTenantViewModel.GetRepository(uow).GetAllAsync(m => true);
                }
                if (!context.Session.GetInt32(MixRequestQueryKeywords.MixTenantId).HasValue)
                {
                    context.Session.SetInt32(
                        MixRequestQueryKeywords.MixTenantId,
                        MixTenantRepository.Instance.GetCurrentTenant(context.Request.Headers.Host).Id);
                }
                await next.Invoke(context);
            }
        }
    }
}

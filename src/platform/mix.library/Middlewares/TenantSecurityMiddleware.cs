using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mix.Lib.Extensions;

namespace Mix.Lib.Middlewares
{
    public sealed class TenantSecurityMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IQueueService<MessageQueueModel> _queueService;
        public TenantSecurityMiddleware(RequestDelegate next, IQueueService<MessageQueueModel> queueService)
        {
            this.next = next;
            _queueService = queueService;
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
                    await MixTenantRepository.Instance.Reload(uow);
                }
                var currentTenant = context.Session.Get<MixTenantSystemViewModel>(MixRequestQueryKeywords.Tenant);
                if (currentTenant == null || !currentTenant.Domains.Any(m => m.Host == context.Request.Headers.Host))
                {
                    currentTenant = MixTenantRepository.Instance.GetCurrentTenant(context.Request.Headers.Host);
                    context.Session.Put(MixRequestQueryKeywords.Tenant, currentTenant);
                }
                await next.Invoke(context);
            }
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mix.Lib.Extensions;

namespace Mix.Lib.Middlewares
{
    public class TenantSecurityMiddleware
    {
        private readonly RequestDelegate next;
        protected readonly IQueueService<MessageQueueModel> _queueService;
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
                    MixTenantRepository.Instance.AllTenants = await MixTenantSystemViewModel.GetRepository(uow).GetAllAsync(m => true);
                }
                if (!context.Session.GetInt32(MixRequestQueryKeywords.TenantId).HasValue)
                {
                    var currentTenant = MixTenantRepository.Instance.GetCurrentTenant(context.Request.Headers.Host);
                    context.Session.SetInt32(
                        MixRequestQueryKeywords.TenantId,
                        currentTenant.Id);
                    context.Session.Put(MixRequestQueryKeywords.TenantName, currentTenant.SystemName);
                    context.Session.Put(MixRequestQueryKeywords.Tenant, currentTenant);
                }
                await next.Invoke(context);
            }
        }
    }
}

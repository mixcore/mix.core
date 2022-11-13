using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mix.Lib.Extensions;
using Mix.Lib.Services;

namespace Mix.Lib.Middlewares
{
    public sealed class TenantSecurityMiddleware
    {
        private readonly MixEndpointService _mixEndpointService;
        private readonly MixConfigurationService _configService;
        private readonly RequestDelegate next;
        private readonly IQueueService<MessageQueueModel> _queueService;
        private readonly MixTenantService _mixTenantService;
        public TenantSecurityMiddleware(
            RequestDelegate next, 
            IQueueService<MessageQueueModel> queueService, 
            MixTenantService mixTenantService, 
            MixEndpointService mixEndpointService, 
            MixConfigurationService configService)
        {
            this.next = next;
            _queueService = queueService;
            _mixTenantService = mixTenantService;
            _mixEndpointService = mixEndpointService;
            _configService = configService;
        }

        public async Task Invoke(HttpContext context)
        {
            if (GlobalConfigService.Instance.InitStatus == InitStep.Blank)
            {
                await next.Invoke(context);
            }
            else
            {
                
                if (_mixTenantService.AllTenants == null)
                {
                    await _mixTenantService.Reload();
                }
                var currentTenant = context.Session.Get<MixTenantSystemViewModel>(MixRequestQueryKeywords.Tenant);
                if (currentTenant == null || !currentTenant.Domains.Any(m => m.Host == context.Request.Headers.Host))
                {
                    currentTenant = _mixTenantService.GetCurrentTenant(context.Request.Headers.Host);
                    context.Session.Put(MixRequestQueryKeywords.Tenant, currentTenant);
                    _mixEndpointService.SetDefaultDomain($"https://{currentTenant.PrimaryDomain}");
                }
                if (_configService.Configs == null)
                {
                    await _configService.Reload();
                }
                await next.Invoke(context);
            }
        }
    }
}

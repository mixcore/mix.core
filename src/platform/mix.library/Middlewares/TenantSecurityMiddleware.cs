using Microsoft.AspNetCore.Http;
using Mix.Lib.Services;

namespace Mix.Lib.Middlewares
{
    public sealed class TenantSecurityMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantSecurityMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(
            HttpContext context,
            MixTenantService mixTenantService,
            MixConfigurationService configService,
            MixEndpointService mixEndpointService)
        {
            if (GlobalConfigService.Instance.InitStatus == InitStep.Blank)
            {
                await _next.Invoke(context);
            }
            else
            {

                if (mixTenantService.AllTenants == null)
                {
                    await mixTenantService.Reload();
                }

                var currentTenant = context.Session.Get<MixTenantSystemModel>(MixRequestQueryKeywords.Tenant);
                if (currentTenant == null || currentTenant.Domains.All(m => m.Host != context.Request.Headers.Host))
                {
                    currentTenant = mixTenantService.GetTenant(context.Request.Headers.Host);
                    context.Session.Put(MixRequestQueryKeywords.Tenant, currentTenant);
                    mixEndpointService.SetDefaultDomain($"https://{currentTenant.PrimaryDomain}");
                }
                if (configService.Configs == null)
                {
                    await configService.Reload();
                }

                await _next.Invoke(context);
            }
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Lib.Interfaces;
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
            IConfiguration configuration,
            IMixTenantService mixTenantService,
            MixConfigurationService configService,
            MixPermissionService permissionService,
            MixEndpointService mixEndpointService)
        {
            if (configuration.GetValue<InitStep>("InitStatus") == InitStep.Blank)
            {
                await _next.Invoke(context);
            }
            else
            {
                if (MixCmsHelper.CheckStaticFileRequest(context.Request.Path))
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
                        context.Session.SetInt32(MixRequestQueryKeywords.TenantId, currentTenant.Id);
                        context.Session.Put(MixRequestQueryKeywords.Tenant, currentTenant);
                        mixEndpointService.SetDefaultDomain($"https://{currentTenant.PrimaryDomain}");
                    }

                    if (configService.Configs == null)
                    {
                        await configService.Reload(currentTenant.Id);
                    }

                    //if (permissionService.RoleEndpoints == null)
                    //{
                    //    await permissionService.Reload();
                    //}

                    await _next.Invoke(context);
                }
            }
        }
    }
}
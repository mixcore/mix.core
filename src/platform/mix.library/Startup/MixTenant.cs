using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Lib.Interfaces;
using Mix.Lib.Middlewares;
using Mix.Lib.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMixTenant(this IServiceCollection services)
        {
            services.TryAddSingleton<IMixTenantService, MixTenantService>();
            return services;
        }
        public static IApplicationBuilder UseMixTenant(this IApplicationBuilder app)
        {
            app.UseSession();
            app.UseMiddleware<TenantSecurityMiddleware>();
            return app;
        }
    }
}

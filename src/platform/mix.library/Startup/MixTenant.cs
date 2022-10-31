using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Lib.Middlewares;
using Mix.Lib.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMixTenant(this IServiceCollection services)
        {
            services.TryAddSingleton<MixTenantService>();
            return services;
        }
        public static IApplicationBuilder UseMixTenant(this IApplicationBuilder app)
        {
            app.UseMiddleware<TenantSecurityMiddleware>();
            return app;
        }
    }
}

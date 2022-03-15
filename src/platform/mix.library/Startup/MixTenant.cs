using Microsoft.AspNetCore.Builder;
using Mix.Lib.Middlewares;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMixTenant(this IServiceCollection services)
        {
            services.AddSingleton<MixTenantRepository>();
            return services;
        }
        public static IApplicationBuilder UseMixTenant(this IApplicationBuilder app)
        {
            app.UseMiddleware<TenantSecurityMiddleware>();
            return app;
        }
    }
}

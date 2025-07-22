using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Lib.Interfaces;
using Mix.Lib.Middlewares;
using Mix.Lib.Services;
using Mix.Shared.Models.Configurations;
using RepoDb;
using RepoDb.Interfaces;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMixTenant(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMixCache(configuration);
            services.TryAddSingleton<IMixTenantService, MixTenantService>();
            return services;
        }
        public static IApplicationBuilder UseMixTenant(this IApplicationBuilder app)
        {
            app.UseCookiePolicy();
            app.UseSession();
            app.UseMiddleware<TenantSecurityMiddleware>();
            return app;
        }
    }
}

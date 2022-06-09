using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Database.Entities.Account;
using Mix.Lib.Middlewares;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUOWs(this IServiceCollection services)
        {
            services.TryAddScoped<UnitOfWorkInfo<MixCmsContext>>();
            services.TryAddScoped<UnitOfWorkInfo<MixCacheDbContext>>();
            services.TryAddScoped<UnitOfWorkInfo<MixCmsAccountContext>>();
            return services;
        }

        public static IApplicationBuilder UseUOWs(this IApplicationBuilder app)
        {
            app.UseMiddleware<UnitOfWorkMiddleware>();
            return app;
        }
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Database.Entities.Account;
using Mix.Database.Entities.AuditLog;
using Mix.Lib.Middlewares;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUoWs(this IServiceCollection services)
        {
            services.TryAddScoped<UnitOfWorkInfo<MixCmsContext>>();
            services.TryAddScoped<UnitOfWorkInfo<MixCacheDbContext>>();
            services.TryAddScoped<UnitOfWorkInfo<MixCmsAccountContext>>();
            services.TryAddScoped<UnitOfWorkInfo<AuditLogDbContext>>();

            UnitOfWorkMiddleware.AddUnitOfWork<UnitOfWorkInfo<MixCmsContext>>();
            UnitOfWorkMiddleware.AddUnitOfWork<UnitOfWorkInfo<MixCacheDbContext>>();
            UnitOfWorkMiddleware.AddUnitOfWork<UnitOfWorkInfo<MixCmsAccountContext>>();
            UnitOfWorkMiddleware.AddUnitOfWork<UnitOfWorkInfo<AuditLogDbContext>>();

            return services;
        }

        public static IApplicationBuilder UseUoWs(this IApplicationBuilder app)
        {
            app.UseMiddleware<UnitOfWorkMiddleware>();
            return app;
        }
    }
}

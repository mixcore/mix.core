using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Database.Entities.Account;
using Mix.Database.Entities.AuditLog;
using Mix.Database.Entities.QueueLog;
using Mix.Database.Entities.Settings;
using Mix.Lib.Middlewares;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUoWs(this IServiceCollection services)
        {
            services.AddDbContext<AuditLogDbContext>();
            services.AddDbContext<QueueLogDbContext>();
            services.TryAddScoped<UnitOfWorkInfo<MixCmsContext>>();
            services.TryAddScoped<UnitOfWorkInfo<GlobalSettingContext>>();
            services.TryAddScoped<UnitOfWorkInfo<MixCacheDbContext>>();
            services.TryAddScoped<UnitOfWorkInfo<MixCmsAccountContext>>();
            services.TryAddScoped<UnitOfWorkInfo<AuditLogDbContext>>();
            services.TryAddScoped<UnitOfWorkInfo<QueueLogDbContext>>();

            UnitOfWorkMiddleware.AddUnitOfWork<UnitOfWorkInfo<MixCmsContext>>();
            UnitOfWorkMiddleware.AddUnitOfWork<UnitOfWorkInfo<GlobalSettingContext>>();
            UnitOfWorkMiddleware.AddUnitOfWork<UnitOfWorkInfo<MixCacheDbContext>>();
            UnitOfWorkMiddleware.AddUnitOfWork<UnitOfWorkInfo<MixCmsAccountContext>>();
            UnitOfWorkMiddleware.AddUnitOfWork<UnitOfWorkInfo<AuditLogDbContext>>();
            UnitOfWorkMiddleware.AddUnitOfWork<UnitOfWorkInfo<QueueLogDbContext>>();

            return services;
        }

        public static IApplicationBuilder UseUoWs(this IApplicationBuilder app)
        {
            app.UseMiddleware<UnitOfWorkMiddleware>();
            return app;
        }
    }
}

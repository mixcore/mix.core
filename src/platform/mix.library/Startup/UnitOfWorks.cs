using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Lib.Middlewares;

namespace Microsoft.Extensions.DependencyInjection
{
    public static partial class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUOWs(this IServiceCollection services)
        {
            services.TryAddScoped<GenericUnitOfWorkInfo<MixCmsContext>>();
            services.TryAddScoped<GenericUnitOfWorkInfo<MixCacheDbContext>>();
            return services;
        }

        public static IApplicationBuilder UseUOWs(this IApplicationBuilder app)
        {
            app.UseMiddleware<UnitOfWorkMiddleware>();
            return app;
        }
    }
}

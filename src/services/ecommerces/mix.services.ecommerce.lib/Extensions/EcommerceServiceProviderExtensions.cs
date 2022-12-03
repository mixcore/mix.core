using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Middlewares;
using Mix.Services.Ecommerce.Lib.Entities;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceProviderExtensions
    {
        public static void AddMixEcommerce(this IServiceCollection services)
        {
            services.TryAddScoped<EcommerceDbContext>();
            services.TryAddScoped<UnitOfWorkInfo<EcommerceDbContext>>();
            UnitOfWorkMiddleware.AddUnitOfWork<UnitOfWorkInfo<EcommerceDbContext>>();
        }
    }
}
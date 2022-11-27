using Microsoft.Extensions.DependencyInjection.Extensions;
using mix.services.ecommerce.Domain.Entities;
using mix.services.ecommerce.Domain.Services;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Middlewares;
using Mix.Shared.Interfaces;

namespace mix.services.ecommerce.Domain
{
    public class StartupService : IStartupService
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddScoped<EcommerceDbContext>();
            services.TryAddScoped<UnitOfWorkInfo<EcommerceDbContext>>();
            services.TryAddScoped<EcommerceService>();
            UnitOfWorkMiddleware.AddUnitOfWork<UnitOfWorkInfo<EcommerceDbContext>>();
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
        }
    }
}

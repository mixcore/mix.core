using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Middlewares;
using Mix.Services.Payments.Onepay.Domain.Entities;
using Mix.Services.Payments.Onepay.Domain.Services;
using Mix.Shared.Interfaces;

namespace Mix.Services.Payments.Onepay.Domain
{
    public class StartupServices : IStartupService
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddScoped<OnepayDbContext>();
            services.TryAddScoped<UnitOfWorkInfo<OnepayDbContext>>();
            services.TryAddScoped<OnepayService>();
            UnitOfWorkMiddleware.AddUnitOfWork<UnitOfWorkInfo<OnepayDbContext>>();
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
        }
    }
}

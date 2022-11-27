using Microsoft.Extensions.DependencyInjection.Extensions;
using Mix.Services.Payments.Onepay.Domain.Services;
using Mix.Shared.Interfaces;

namespace Mix.Services.Payments.Onepay.Domain
{
    public class StartupServices : IStartupService
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddScoped<OnepayService>();
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
        }
    }
}

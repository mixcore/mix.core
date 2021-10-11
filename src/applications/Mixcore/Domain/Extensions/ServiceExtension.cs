using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace Mixcore.Domain.Extensions
{
    public static class ServiceExtension
    {
        public static void AddMixOcelot(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOcelot(configuration);
        }

        public static void UseMixOcelot(this IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
            app.UseOcelot().Wait();
        }
    }
}

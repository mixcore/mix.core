namespace Mixcore.Domain
{
    public class StartupServices : IStartupService
    {
        public void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMixRoutes();
        }

        public void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop)
        {
        }

        public void UseEndpoints(IEndpointRouteBuilder endpoints, IConfiguration configuration, bool isDevelop)
        {
            endpoints.UseMixMVCEndpoints();
        }
    }
}

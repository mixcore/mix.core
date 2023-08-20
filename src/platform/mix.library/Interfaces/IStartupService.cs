using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mix.Lib.Interfaces
{
    public interface IStartupService
    {
        void AddServices(IServiceCollection services, IConfiguration configuration);
        void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop);
        void UseEndpoints(IEndpointRouteBuilder endpoints, IConfiguration configuration, bool isDevelop);
    }
}

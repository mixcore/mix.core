using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Mix.Shared.Interfaces
{
    public interface IStartupService
    {
        void AddServices(IHostApplicationBuilder builder);
        void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop);
        void UseEndpoints(IEndpointRouteBuilder endpoints, IConfiguration configuration, bool isDevelop);
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mix.Lib.Interfaces
{
    public interface IStartupService
    {
        void AddServices(IServiceCollection services, IConfiguration configuration);
        void UseApps(IApplicationBuilder app, IConfiguration configuration, bool isDevelop);
    }
}

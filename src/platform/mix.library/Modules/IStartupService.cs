using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Mix.Lib.Modules
{
    public interface IStartupService
    {
        void AddServices(IServiceCollection services);
        void UseApps(IApplicationBuilder app, bool isDevelop);
    }
}

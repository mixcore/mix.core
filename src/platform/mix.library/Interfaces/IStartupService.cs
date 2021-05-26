using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Mix.Lib.Interfaces
{
    public interface IStartupService
    {
        void AddServices(IServiceCollection services);
        void UseApps(IApplicationBuilder app, bool isDevelop);
    }
}

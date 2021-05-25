using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace mix.library.Modules
{
    public interface IStartupService
    {
        void AddServices(IServiceCollection services);
        void UseApps(IApplicationBuilder app);
    }
}

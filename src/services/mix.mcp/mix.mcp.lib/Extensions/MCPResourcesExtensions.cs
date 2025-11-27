using Microsoft.Extensions.DependencyInjection;
using Mix.MCP.Lib.Resources;

namespace Mix.MCP.Lib.Extensions
{
    /// <summary>
    /// Extensions to register MCP resource-related services
    /// </summary>
    public static class MCPResourcesExtensions
    {
        /// <summary>
        /// Register ResourceLoader to DI container
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <returns>Updated service collection</returns>
        public static IServiceCollection AddMCPResources(this IServiceCollection services)
        {
            // Register ResourceLoader as singleton
            services.AddSingleton<ResourceLoader>();

            return services;
        }
    }
}
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Mix.MCP.Lib.Agents;
using Mix.MCP.Lib.Messenger;
using Mix.MCP.Lib.Resources;

namespace Mix.MCP.Lib.Extensions
{
    /// <summary>
    /// Extensions to register MCP resource-related services
    /// </summary>
    public static class AgentExtensions
    {
        /// <summary>
        /// Register ResourceLoader to DI container
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <returns>Updated service collection</returns>
        public static IHostApplicationBuilder AddAgents(this IHostApplicationBuilder builder)
        {
            // Register ResourceLoader as singleton
            builder.Services.TryAddSingleton<DatabaseAgent>();
            builder.Services.TryAddSingleton<TaskAgent>();
            builder.Services.TryAddSingleton<ChatAgent>();
            builder.Services.TryAddSingleton<RoutingAgent>();
            builder.Services.TryAddSingleton<PlanningAgent>();
            //builder.Services.AddHostedService<LLMChatHostedService>();
            return builder;
        }
    }
}
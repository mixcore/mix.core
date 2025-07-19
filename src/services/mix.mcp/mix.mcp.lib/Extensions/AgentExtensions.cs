using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Mix.MCP.Lib.Agents;
using Mix.MCP.Lib.Messenger;
using Mix.MCP.Lib.Resources;
using Mix.MCP.Lib.Services.Knowledge;
using Mix.MCP.Lib.Services.Search;
using Mix.MCP.Lib.Services.Cache;
using Microsoft.Extensions.Configuration;
using Mix.Lib.Extensions;

namespace Mix.MCP.Lib.Extensions
{
    /// <summary>
    /// Extensions to register MCP resource-related services
    /// </summary>
    public static class AgentExtensions
    {
        /// <summary>
        /// Register agents and supporting services to DI container
        /// </summary>
        /// <param name="builder">Host application builder</param>
        /// <returns>Updated host application builder</returns>
        public static IHostApplicationBuilder AddAgents(this IHostApplicationBuilder builder)
        {
            // Register knowledge and search services
            builder.Services.TryAddSingleton<IKnowledgeBaseService, KnowledgeBaseService>();
            builder.Services.TryAddSingleton<ISemanticSearchService, SemanticSearchService>();
            builder.Services.TryAddSingleton<IResourceCacheService, ResourceCacheService>();

            // Register agents with knowledge service injection
            builder.Services.TryAddSingleton<DatabaseAgent>();
            builder.Services.TryAddSingleton<TaskAgent>();
            builder.Services.TryAddSingleton<ChatAgent>();
            builder.Services.TryAddSingleton<RoutingAgent>();
            builder.Services.TryAddSingleton<PlanningAgent>();

            // Register hosted services
            if (!string.IsNullOrEmpty(builder.Configuration.BaseUrl()))
            {
                builder.Services.AddHostedService<LLMChatHostedService>();
            }

            return builder;
        }
    }
}
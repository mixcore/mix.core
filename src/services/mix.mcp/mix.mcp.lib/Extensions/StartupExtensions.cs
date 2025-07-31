using Google.Apis.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Mix.Heart.Services;
using Mix.MCP.Lib.Agents;
using Mix.MCP.Lib.Hubs;
using Mix.MCP.Lib.Prompts;
using Mix.MCP.Lib.Resources;
using Mix.MCP.Lib.Services;
using Mix.MCP.Lib.Services.Knowledge;
using Mix.MCP.Lib.Services.LLM;
using Mix.MCP.Lib.Tools;
using Mix.SignalR.Constants;
using Mix.MCP.Lib.Services.Search;

namespace Mix.MCP.Lib.Extensions
{
    public static class StartupExtensions
    {
        public static IHostApplicationBuilder AddMCPServices(this IHostApplicationBuilder builder)
        {
            builder.Services.TryAddScoped<MixCacheService>();
            builder.Services.AddMixDbContexts();
            // Register LLM service options
            builder.Services.Configure<LlmServiceOptions>(options =>
            {
                // Configure from appsettings if available
                var config = builder.Configuration.GetSection("LlmServices");
                if (config.Exists())
                {
                    config.Bind(options);
                }

                // Ensure timeout is at least 60 seconds
                options.DefaultTimeoutSeconds = Math.Max(60, options.DefaultTimeoutSeconds);
            });

            // Register MySQL services
            builder.Services.AddSingleton(provider =>
            {
                var factory = new DatabaseServiceFactory(
                    builder.Configuration,
                    provider.GetRequiredService<ILoggerFactory>());
                return factory.CreateService();
            });

            builder.AddAgents();

            // Register MCP resources
            builder.Services.AddMCPResources();
            builder.Services.TryAddSingleton<QdrantService>();
            builder.Services.AddSingleton<ISemanticSearchService, SemanticSearchService>(provider =>
            {
                var cache = provider.GetRequiredService<IMemoryCache>();
                var llm = provider.GetRequiredService<ILlmServiceFactory>();
                var qdrantService = provider.GetRequiredService<QdrantService>();
                var logger = provider.GetRequiredService<ILogger<SemanticSearchService>>();
                var configuration = provider.GetService<IConfiguration>();
                return new SemanticSearchService(cache, logger, qdrantService, llm);
            });

            // Register KnowledgeBaseService and ensure it loads resources
            builder.Services.AddSingleton<IKnowledgeBaseService, KnowledgeBaseService>(provider =>
            {
                var cache = provider.GetRequiredService<IMemoryCache>();
                var logger = provider.GetRequiredService<ILogger<KnowledgeBaseService>>();
                var semanticSearchService = provider.GetService<ISemanticSearchService>();
                var service = new KnowledgeBaseService(cache, logger, semanticSearchService);
                // KnowledgeBaseService will load knowledge into resources in its constructor
                return service;
            });

            // Register MCP services
            builder.Services
                .AddMcpServer(options =>
                {
                    options.ServerInfo = new ModelContextProtocol.Protocol.Types.Implementation
                    {
                        Name = MCPResources.ServerDefaults.ServerName,
                        Version = MCPResources.ServerDefaults.ServerVersion
                    };
                    options.ServerInstructions = "This MCP server provides tools and prompts for Mixcore applications with enhanced knowledge and semantic search capabilities.";
                })
                .WithHttpTransport()
                .WithStdioServerTransport()
                .WithToolsFromAssembly();

            // Register other services

            builder.Services.AddSingleton<ILlmServiceFactory, LlmServiceFactory>();

            return builder;
        }

        public static IEndpointRouteBuilder MapMCPEndpoints(this IEndpointRouteBuilder endpoints, bool isDevelop)
        {
            endpoints.MapHub<LLMHub>(HubEndpoints.LLMHub);
            endpoints.MapMcp("/mcp");
            endpoints.MapMcp("/sse");
            Console.WriteLine("Mapped Mcp endpoint to /mcp");
            return endpoints;
        }
    }
}

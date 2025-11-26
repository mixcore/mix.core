using Google.Apis.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Mix.Heart.Services;
using Mix.MCP.Lib.Agents;
using Mix.MCP.Lib.Prompts;
using Mix.MCP.Lib.Resources;
using Mix.MCP.Lib.Services;
using Mix.MCP.Lib.Services.LLM;
using Mix.MCP.Lib.Tools;
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

            // Register MCP services
            builder.Services
                .AddMcpServer(options =>
                {
                    options.ServerInfo = new ModelContextProtocol.Protocol.Types.Implementation
                    {
                        Name = MCPResources.ServerDefaults.ServerName,
                        Version = MCPResources.ServerDefaults.ServerVersion
                    };
                    options.ServerInstructions = "This MCP server provides tools and prompts for Mixcore applications.";
                })
                .WithHttpTransport()
                .WithStdioServerTransport()
                .WithTools<EchoTool>()
                .WithPrompts<GeneratePrompt>()
                .WithPrompts<ResourcePrompts>()
                .WithPrompts<MixDbPrompts>()
                .WithTools<MixDbPromptTool>()
                .WithTools<DatabaseAgent>()
                //.WithTools<LLMTools>()
                //.WithTools<ResourceTool>()
                //.WithTools<MixDbDataTool>()
                .WithToolsFromAssembly();

            // Register other services

            builder.Services.AddSingleton<ILlmServiceFactory, LlmServiceFactory>();

            return builder;
        }

        public static IEndpointRouteBuilder MapMCPEndpoints(this IEndpointRouteBuilder endpoints, bool isDevelop)
        {
            endpoints.MapMcp("/mcp");
            endpoints.MapMcp("/sse");
            endpoints.MapMcp("/LLMMessage");
            Console.WriteLine("Mapped Mcp endpoint to /mcp");
            return endpoints;
        }
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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

            // Register HTTP clients
            builder.Services.AddHttpClient("LmStudio", client =>
            {
                // Use default LM Studio URL but allow override from configuration
                var baseUrl = builder.Configuration.GetValue<string>("LlmServices:LmStudioBaseUrl") 
                    ?? "http://localhost:1234/v1";
                client.BaseAddress = new Uri(baseUrl);
                client.Timeout = TimeSpan.FromSeconds(
                    builder.Configuration.GetValue<int>("LlmServices:DefaultTimeoutSeconds", 120));
            });
            
            builder.Services.AddHttpClient("OpenAI", client =>
            {
                // OpenAI API configuration
                var baseUrl = builder.Configuration.GetValue<string>("LlmServices:OpenAIBaseUrl") 
                    ?? "https://api.openai.com/v1";
                client.BaseAddress = new Uri(baseUrl);
                client.Timeout = TimeSpan.FromSeconds(
                    builder.Configuration.GetValue<int>("LlmServices:DefaultTimeoutSeconds", 120));
            });
            
            builder.Services.AddHttpClient("DeepSeek", client =>
            {
                // DeepSeek API configuration
                var baseUrl = builder.Configuration.GetValue<string>("LlmServices:DeepSeekBaseUrl") 
                    ?? "https://api.deepseek.com/v1";
                client.BaseAddress = new Uri(baseUrl);
                client.Timeout = TimeSpan.FromSeconds(
                    builder.Configuration.GetValue<int>("LlmServices:DefaultTimeoutSeconds", 120));
            });

            // Register MySQL services
            builder.Services.AddSingleton(provider =>
            {
                var factory = new DatabaseServiceFactory(
                    builder.Configuration,
                    provider.GetRequiredService<ILoggerFactory>());
                return factory.CreateService();
            });

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
                .WithPrompts<MixDatabasePrompts>()
                .WithTools<LLMTools>()
                .WithTools<ResourceTool>()
                .WithTools<MixDatabasePromptTool>()
                .WithToolsFromAssembly();

            // Register other services
            builder.Services.AddSingleton<ILlmServiceFactory, LlmServiceFactory>();

            return builder;
        }

        public static IEndpointRouteBuilder MapMCPEndpoints(this IEndpointRouteBuilder endpoints, bool isDevelop)
        {
            endpoints.MapMcp("/mcp");
            endpoints.MapMcp("/sse");
            endpoints.MapMcp("/message");
            Console.WriteLine("Mapped Mcp endpoint to /mcp");
            return endpoints;
        }
    }
}

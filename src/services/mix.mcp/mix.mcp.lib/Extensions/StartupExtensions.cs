using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
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
            // Register HTTP clients
            builder.Services.AddHttpClient("Deepseek", client =>
            {
                // Thay thế bằng URL API base của Deepseek
                client.BaseAddress = new Uri("https://api.deepseek.com"); // Ví dụ URL, hãy kiểm tra lại URL chính xác
                                                                          // Có thể thêm các cấu hình mặc định khác ở đây nếu cần
                                                                          // client.DefaultRequestHeaders.Add("Accept", "application/json");
            });
            // Trong phương thức ConfigureServices hoặc tương đương
            builder.Services.AddHttpClient("ChatGPT", client =>
            {
                // URL API base của OpenAI
                client.BaseAddress = new Uri("https://api.openai.com/v1/");
                // Có thể thêm các cấu hình mặc định khác ở đây nếu cần
                // client.DefaultRequestHeaders.Add("Accept", "application/json");
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
                //.WithPrompts<GeneratePrompt>()
                //.WithPrompts<MixCoreDatabaseTools>()
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

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mix.MCP.Lib.Prompts;
using Mix.MCP.Lib.Services;
using Mix.MCP.Lib.Tools;
using ModelContextProtocol.Server;
namespace Mix.MCP.Lib.Extensions
{
    public static class StartupExtensions
    {
        public static IHostApplicationBuilder AddMCPServices(this IHostApplicationBuilder builder)
        {
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
            builder.Services
                .AddMcpServer()
                .WithHttpTransport()
                .WithStdioServerTransport()
                .WithPrompts<DeepseekPrompts>()
                .WithPrompts<GeneratePrompt>()
                .WithPrompts<IoTHealthDataPrompt>()
                .WithTools<EchoTool>()
                .WithTools<LlmTools>()
                .WithToolsFromAssembly();

            builder.Services.AddSingleton<ILlmService, LlmService>();
            return builder;
        }

        public static IEndpointRouteBuilder MapMCPEndpoints(this IEndpointRouteBuilder endpoints, bool isDevelop)
        {
            endpoints.MapMcp("/mcp");
            Console.WriteLine("Mapped Mcp endpoint to /mcp");
            return endpoints;
        }
    }
}

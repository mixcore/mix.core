using Microsoft.Extensions.Logging;
using Mix.MCP.Lib.Services;
using ModelContextProtocol.Server;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Mix.MCP.Lib.Tools
{
    /// <summary>
    /// Tools for working with large language models
    /// </summary>
    [McpServerToolType]
    public class LlmTools
    {
        private readonly ILlmService _llmService;
        private readonly ILogger<LlmTools> _logger;

        public LlmTools(ILlmService llmService, ILogger<LlmTools> logger)
        {
            _llmService = llmService;
            _logger = logger;
        }

        /// <summary>
        /// Send a message to ChatGPT and get response
        /// </summary>
        [McpServerTool, Description("Send message to ChatGPT and get response")]
        public async Task<string> ChatWithOpenAI(
            [Description("OpenAI API key")] string apiKey,
            [Description("Message to send")] string message,
            [Description("ChatGPT model (default: gpt-4o)")] string model = "gpt-4o",
            [Description("Creativity level (0.0-2.0)")] float temperature = 0.7f,
            CancellationToken cancellationToken = default)
        {
            return await _llmService.ChatWithOpenAIAsync(apiKey, message, model, temperature, cancellationToken);
        }

        /// <summary>
        /// Send a message to Deepseek and get response
        /// </summary>
        [McpServerTool, Description("Send message to Deepseek and get response")]
        public async Task<string> ChatWithDeepseek(
            [Description("Deepseek API key")] string apiKey,
            [Description("Message to send")] string message,
            [Description("Deepseek model (default: deepseek-chat)")] string model = "deepseek-chat",
            [Description("Creativity level (0.0-1.0)")] float temperature = 0.7f,
            CancellationToken cancellationToken = default)
        {
            return await _llmService.ChatWithDeepseekAsync(apiKey, message, model, temperature, cancellationToken);
        }
    }
}

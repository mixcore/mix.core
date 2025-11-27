using Microsoft.Extensions.Logging;
using Mix.MCP.Lib.Services;
using Mix.MCP.Lib.Services.LLM;
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
    public class LLMTools
    {
        private readonly ILlmServiceFactory _llmServiceFactory;
        private readonly ILogger<LLMTools> _logger;

        public LLMTools(ILlmServiceFactory llmServiceFactory, ILogger<LLMTools> logger)
        {
            _llmServiceFactory = llmServiceFactory;
            _logger = logger;
        }

        /// <summary>
        /// Send a LLMMessage to LLM service and get response
        /// </summary>
        [McpServerTool, Description("Send LLMMessage to LLM service and get response")]
        public async Task<LLMChatResponse> ChatWithLLM(
            [Description("LLM service type (OpenAI, DeepSeek, etc.)")] LLMServiceType serviceType,
            [Description("LLMMessage to send")] string message,
            [Description("Model name")] string model,
            [Description("API key (optional)")] string? apiKey = null,
            [Description("Creativity level (0.0-2.0)")] float temperature = 0.7f)
        {
            var llmService = _llmServiceFactory.CreateService(serviceType);
            return await llmService.ChatAsync(message, model, temperature, -1);
        }
    }
}

using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Mix.Database.Services;
using Mix.MCP.Lib.Hubs;
using Mix.MCP.Lib.Models;
using Mix.MCP.Lib.Services.Knowledge;
using Mix.MCP.Lib.Services.LLM;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Mix.MCP.Lib.Agents
{
    public class RoutingAgent : BaseAgent
    {
        private readonly ChatAgent _chatAgent;
        private readonly PlanningAgent _planningAgent;

        public RoutingAgent(
            AppSettingsService appSettingsService,
            ILlmServiceFactory llmServiceFactory,
            ILogger<RoutingAgent> logger,
            IHubContext<LLMHub> hubContext,
            ChatAgent chatAgent,
            PlanningAgent planningAgent,
            IKnowledgeBaseService? knowledgeBaseService = null,
            TimeSpan? defaultTimeout = null)
            : base(appSettingsService, llmServiceFactory, hubContext, logger, knowledgeBaseService, defaultTimeout)
        {
            _chatAgent = chatAgent;
            _planningAgent = planningAgent;
        }

        public override async Task<AgentProcessResult> ProcessInputAsync(
            string userInput,
            string deviceId,
            string sessionId = "default",
            LLMServiceType serviceType = LLMServiceType.DeepSeek,
            CancellationToken cancellationToken = default)
        {
            // Ensure knowledge is loaded and system instructions are set before processing
            await EnsureKnowledgeLoadedAsync(userInput, sessionId, "general", cancellationToken);

            var intent = await ClassifyIntentAsync(userInput, serviceType, cancellationToken);

            switch (intent)
            {
                case "chat":
                    return await _chatAgent.ProcessInputAsync(userInput, deviceId, sessionId, serviceType, cancellationToken);
                case "plan":
                    return await _planningAgent.ProcessInputAsync(userInput, deviceId, sessionId, serviceType, cancellationToken);
                default:
                    return new AgentProcessResult(false, "Sorry, I could not route your request.");
            }
        }

        private async Task<string> ClassifyIntentAsync(
            string userInput,
            LLMServiceType serviceType,
            CancellationToken cancellationToken)
        {
            var prompt = $@"
You are an AI assistant. Classify the following user request as either a normal conversation or a planning/multi-step request.
Respond in this JSON format:
{{ ""type"": ""chat"" | ""plan"" }}
User request: ""{userInput}""";
            var response = await AskAIAsync(prompt, "routing", serviceType, "deepseek-chat", 0.2, -1, cancellationToken, "routing");
            var content = response?.choices?[0]?.Message?.Content;

            if (string.IsNullOrWhiteSpace(content))
                return "chat";

            try
            {
                var jsonStart = content.IndexOf('{');
                var jsonEnd = content.LastIndexOf('}');
                if (jsonStart < 0 || jsonEnd < 0 || jsonEnd < jsonStart)
                    return "chat";

                var json = content.Substring(jsonStart, jsonEnd - jsonStart + 1);
                using var doc = JsonDocument.Parse(json);
                if (doc.RootElement.TryGetProperty("type", out var typeProp))
                {
                    var type = typeProp.GetString();
                    if (type == "plan")
                        return "plan";
                }
                return "chat";
            }
            catch
            {
                return "chat";
            }
        }
    }
}
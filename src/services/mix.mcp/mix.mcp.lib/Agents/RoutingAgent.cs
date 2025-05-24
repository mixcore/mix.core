using Microsoft.Extensions.Logging;
using Mix.MCP.Lib.Services.LLM;
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
            ILlmServiceFactory llmServiceFactory,
            ILogger<RoutingAgent> logger,
            ChatAgent chatAgent,
            PlanningAgent planningAgent,
            TimeSpan? defaultTimeout = null)
            : base(llmServiceFactory, logger, defaultTimeout)
        {
            _chatAgent = chatAgent;
            _planningAgent = planningAgent;
        }

        public override async Task<string> ProcessInputAsync(
            string userInput,
            string deviceId,
            string sessionId = "default",
            LLMServiceType serviceType = LLMServiceType.DeepSeek,
            CancellationToken cancellationToken = default)
        {
            var intent = await ClassifyIntentAsync(userInput, serviceType, cancellationToken);

            switch (intent)
            {
                case "chat":
                    return await _chatAgent.ProcessInputAsync(userInput, deviceId, sessionId, serviceType, cancellationToken);
                case "plan":
                    return await _planningAgent.ProcessInputAsync(userInput, deviceId, sessionId, serviceType, cancellationToken);
                default:
                    return "Sorry, I could not route your request.";
            }
        }

        private async Task<string> ClassifyIntentAsync(
            string userInput,
            LLMServiceType serviceType,
            CancellationToken cancellationToken)
        {
            var llmService = _llmServiceFactory.CreateService(serviceType);
            var prompt = $@"
You are an AI assistant. Classify the following user request as either a normal conversation or a planning/multi-step request.
Respond in this JSON format:
{{ ""type"": ""chat"" | ""plan"" }}
User request: ""{userInput}""
";
            var response = await llmService.ChatAsync(prompt, "deepseek-chat", 0.2, -1, cancellationToken);
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
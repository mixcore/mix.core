using Microsoft.Extensions.Logging;
using Mix.MCP.Lib.Services.LLM;
using Mix.MCP.Lib.Messenger;
using Mix.MCP.Lib.Models;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Mix.MCP.Lib.Agents
{
    public class PlanningAgent : BaseAgent
    {
        private readonly TaskAgent _taskAgent;
        private readonly IMqttMessageService _mqttMessageService;

        public PlanningAgent(
            IConfiguration configuration,
            ILlmServiceFactory llmServiceFactory,
            ILogger<PlanningAgent> logger,
            TaskAgent taskAgent,
            TimeSpan? defaultTimeout = null)
            : base(llmServiceFactory, logger, defaultTimeout)
        {
            _taskAgent = taskAgent;
            _mqttMessageService = new MqttMessageService(configuration);
        }

        public override async Task<string> ProcessInputAsync(
            string userInput,
            string deviceId,
            string sessionId = "default",
            LLMServiceType serviceType = LLMServiceType.DeepSeek,
            CancellationToken cancellationToken = default)
        {
            try
            {
                ValidateInput(userInput, sessionId);
                _logger.LogInformation("PlanningAgent analyzing input: {UserInput}", userInput);

                // 1. Use LLM to break down user input into prompts
                var prompts = await AnalyzeAndExtractPromptsAsync(userInput, serviceType, cancellationToken);

                // Publish prompts analysis to device
                var promptsContent = prompts.Count == 0
                    ? "No actionable prompts were found in your request."
                    : $"Prompts extracted: {string.Join('\n', prompts)}";
                await SendMqttMessageAsync(deviceId, sessionId, serviceType, promptsContent, cancellationToken);

                if (prompts.Count == 0)
                    return "No actionable prompts were found in your request.";

                // 2. Execute each prompt using TaskAgent, passing previous result as context
                var results = new List<string>();
                string previousResult = null;
                for (int i = 0; i < prompts.Count; i++)
                {
                    string prompt = prompts[i];
                    if (i > 0 && !string.IsNullOrWhiteSpace(previousResult))
                    {
                        // Add previous result as context to the next prompt
                        prompt = $"Previous result:\n{previousResult}\n\nNext task:\n{prompt}";
                    }

                    string resultMsg;
                    try
                    {
                        var result = await _taskAgent.ProcessInputAsync(prompt, sessionId, deviceId, serviceType, cancellationToken);
                        resultMsg = $"[Success] {prompts[i]}: {result}";
                        previousResult = result; // Pass this to the next prompt
                        results.Add(resultMsg);
                    }
                    catch (Exception ex)
                    {
                        resultMsg = $"[Failed] {prompts[i]}: {ex.Message}";
                        previousResult = ex.Message; // Still pass error as context
                        results.Add(resultMsg);
                    }

                    // Publish each task result to device
                    await SendMqttMessageAsync(deviceId, sessionId, serviceType, resultMsg, cancellationToken);
                }

                // 3. Return a summary
                return BuildSummary(prompts, results);
            }
            catch (Exception ex)
            {
                return HandleException(ex, userInput);
            }
        }

        private async Task SendMqttMessageAsync(
            string deviceId,
            string sessionId,
            LLMServiceType serviceType,
            string content,
            CancellationToken cancellationToken)
        {
            await _mqttMessageService.PublishAsync(deviceId, content, cancellationToken);
        }

        private async Task<List<string>> AnalyzeAndExtractPromptsAsync(
            string userInput,
            LLMServiceType serviceType,
            CancellationToken cancellationToken)
        {
            var llmService = _llmServiceFactory.CreateService(serviceType);
            var prompt = $@"
You are a planning assistant. Analyze the following user request and break it down into a list of actionable prompts.
Respond in this JSON array format:
[
  ""First prompt as a string."",
  ""Second prompt as a string.""
]
User request: ""{userInput}""
";
            var response = await llmService.ChatAsync(prompt, "deepseek-chat", 0.2, -1, cancellationToken);
            var content = response?.choices?[0]?.Message?.Content;

            if (string.IsNullOrWhiteSpace(content))
                return new List<string>();

            // Extract JSON array from LLM response
            var jsonStart = content.IndexOf('[');
            var jsonEnd = content.LastIndexOf(']');
            if (jsonStart < 0 || jsonEnd < 0 || jsonEnd < jsonStart)
                return new List<string>();

            var json = content.Substring(jsonStart, jsonEnd - jsonStart + 1);

            try
            {
                var prompts = JsonSerializer.Deserialize<List<string>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<string>();

                return prompts;
            }
            catch
            {
                return new List<string>();
            }
        }

        private string BuildSummary(List<string> prompts, List<string> results)
        {
            var summary = new System.Text.StringBuilder();
            summary.AppendLine("Plan execution summary:");
            for (int i = 0; i < prompts.Count; i++)
            {
                var result = results.Count > i ? results[i] : "[No result]";
                summary.AppendLine($"- {result}");
            }
            return summary.ToString();
        }
    }

    public class PlannedTask
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public TaskStatus Status { get; set; } = TaskStatus.Pending;
    }

    public class TaskResult
    {
        public string TaskId { get; set; }
        public bool Success { get; set; }
        public string Output { get; set; }
        public string ErrorMessage { get; set; }
    }
}
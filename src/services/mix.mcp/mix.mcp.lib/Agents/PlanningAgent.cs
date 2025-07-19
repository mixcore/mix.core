using Microsoft.Extensions.Logging;
using Mix.MCP.Lib.Services.LLM;
using Mix.MCP.Lib.Messenger;
using Mix.MCP.Lib.Models;
using Mix.MCP.Lib.Services.Knowledge;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Mix.Shared.Services;
using Mix.Database.Services;
using System.Linq;

namespace Mix.MCP.Lib.Agents
{
    public class PlanningAgent : BaseAgent
    {
        private readonly TaskAgent _taskAgent;
        private readonly IMqttMessageService _mqttMessageService;

        public PlanningAgent(
            IConfiguration configuration,
            AppSettingsService appSettingsService,
            ILlmServiceFactory llmServiceFactory,
            ILogger<PlanningAgent> logger,
            TaskAgent taskAgent,
            IKnowledgeBaseService? knowledgeBaseService = null,
            TimeSpan? defaultTimeout = null)
            : base(appSettingsService, llmServiceFactory, logger, knowledgeBaseService, defaultTimeout)
        {
            _taskAgent = taskAgent;
            _mqttMessageService = new MqttMessageService(configuration);
        }

        public override async Task<AgentProcessResult> ProcessInputAsync(
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
                    return new AgentProcessResult(true, "No actionable prompts were found in your request.");

                // 2. Execute each prompt using TaskAgent, passing previous result as context
                // FAIL-FAST: Stop execution immediately when any task fails
                var results = new List<TaskExecutionResult>();
                string previousResult = null;
                bool executionFailed = false;
                Exception? firstFailureException = null;

                for (int i = 0; i < prompts.Count; i++)
                {
                    string prompt = prompts[i];
                    if (i > 0 && !string.IsNullOrWhiteSpace(previousResult))
                    {
                        // Add previous result as context to the next prompt
                        prompt = $"Previous result:\n{previousResult}\n\nNext task:\n{prompt}";
                    }

                    var executionResult = new TaskExecutionResult
                    {
                        TaskIndex = i,
                        OriginalPrompt = prompts[i],
                        ExecutedPrompt = prompt,
                        StartTime = DateTime.UtcNow
                    };

                    try
                    {
                        _logger.LogInformation("Executing task {Index}/{Total}: {Prompt}", i + 1, prompts.Count, prompts[i]);

                        var result = await _taskAgent.ProcessInputAsync(prompt, sessionId, deviceId, serviceType, cancellationToken);

                        executionResult.EndTime = DateTime.UtcNow;
                        executionResult.Success = result.IsSuccess;
                        executionResult.Result = result.Response;
                        executionResult.Message = $"[{result.Result}] {prompts[i]}: {result}";
                        previousResult = result.Response; // Pass successful result to next prompt

                        _logger.LogInformation("Task {Index}/{Total} completed successfully in {Duration}ms: {Prompt}",
                            i + 1, prompts.Count, executionResult.Duration.TotalMilliseconds, prompts[i]);

                        if (!result.IsSuccess)
                        {
                            break; // Stop execution if any task fails
                        }
                    }
                    catch (Exception ex)
                    {
                        executionResult.EndTime = DateTime.UtcNow;
                        executionResult.Success = false;
                        executionResult.Exception = ex;
                        executionResult.Result = ex.Message;
                        executionResult.Message = $"[Failed] {prompts[i]}: {ex.Message}";

                        executionFailed = true;
                        firstFailureException = ex;

                        _logger.LogError(ex, "Task {Index}/{Total} failed after {Duration}ms: {Prompt}",
                            i + 1, prompts.Count, executionResult.Duration.TotalMilliseconds, prompts[i]);

                        // Add this failed result
                        results.Add(executionResult);

                        // Publish the failure message
                        await SendMqttMessageAsync(deviceId, sessionId, serviceType, executionResult.Message, cancellationToken);

                        // FAIL-FAST: Stop execution immediately when any task fails
                        _logger.LogWarning("Stopping execution due to task failure. {RemainingTasks} remaining tasks will not be executed",
                            prompts.Count - i - 1);
                        break;
                    }

                    results.Add(executionResult);
                    // Publish each successful task result to device
                    await SendMqttMessageAsync(deviceId, sessionId, serviceType, executionResult.Message, cancellationToken);
                }

                // 3. Build and return summary with proper status indication
                var summary = BuildSummary(prompts, results, executionFailed, firstFailureException);

                // If execution failed, log it as an error
                if (executionFailed)
                {
                    _logger.LogError("Plan execution failed. {ExecutedTasks}/{TotalTasks} tasks executed before failure",
                        results.Count, prompts.Count);

                    // Publish final summary to device
                    await SendMqttMessageAsync(deviceId, sessionId, serviceType,
                        $"? Plan execution stopped due to failure. {results.Count}/{prompts.Count} tasks completed.",
                        cancellationToken);
                }
                else
                {
                    _logger.LogInformation("Plan execution completed successfully. All {TotalTasks} tasks executed",
                        prompts.Count);

                    // Publish success summary to device
                    await SendMqttMessageAsync(deviceId, sessionId, serviceType,
                        $"? Plan execution completed successfully. All {prompts.Count} tasks completed.",
                        cancellationToken);
                }

                return new AgentProcessResult(true, summary);
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
            try
            {
                await _mqttMessageService.PublishAsync(deviceId, content, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to send MQTT message to device {DeviceId}", deviceId);
                // Don't let MQTT failures affect the main execution flow
            }
        }

        private async Task<List<string>> AnalyzeAndExtractPromptsAsync(
            string userInput,
            LLMServiceType serviceType,
            CancellationToken cancellationToken)
        {
            var llmService = _llmServiceFactory.CreateService(serviceType);

            // Get contextual knowledge before planning
            var knowledgeContext = await GetKnowledgeContextAsync(userInput, "planning", cancellationToken);

            // Get supported MCP tools and format for prompt
            var supportedActions = Mix.MCP.Lib.Tools.ToolDiscovery.SupportedPromptToolActions;
            var toolList = string.Join("\n", supportedActions.Select(a => $"- {a.MethodName}: {a.Description}"));

            var promptBuilder = new System.Text.StringBuilder();
            promptBuilder.AppendLine("You are a planning assistant. Analyze the following user request and break it down into a list of actionable prompts.");
            promptBuilder.AppendLine();

            // Add knowledge context if available
            if (!string.IsNullOrWhiteSpace(knowledgeContext))
            {
                promptBuilder.AppendLine("Relevant context and guidance:");
                promptBuilder.AppendLine(knowledgeContext);
                promptBuilder.AppendLine();
            }

            promptBuilder.AppendLine("Here is a list of supported MCP tools you can use:");
            promptBuilder.AppendLine(toolList);
            promptBuilder.AppendLine();
            promptBuilder.AppendLine("IMPORTANT: Each prompt should be independent but can use results from previous steps.");
            promptBuilder.AppendLine("If any step fails, the entire plan execution will stop.");
            promptBuilder.AppendLine();
            promptBuilder.AppendLine("Respond in this JSON array format:");
            promptBuilder.AppendLine("[");
            promptBuilder.AppendLine("  \"First prompt as a string.\",");
            promptBuilder.AppendLine("  \"Second prompt as a string.\"");
            promptBuilder.AppendLine("]");
            promptBuilder.AppendLine($"User request: \"{userInput}\"");

            var prompt = promptBuilder.ToString();

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

                _logger.LogInformation("Extracted {Count} prompts with knowledge context for user input", prompts.Count);
                return prompts;
            }
            catch
            {
                return new List<string>();
            }
        }

        private string BuildSummary(List<string> prompts, List<TaskExecutionResult> results, bool executionFailed, Exception? firstFailureException)
        {
            var summary = new System.Text.StringBuilder();

            // Add overall status
            if (executionFailed)
            {
                summary.AppendLine("? Plan execution FAILED and was stopped:");
                if (firstFailureException != null)
                {
                    summary.AppendLine($"   Error: {firstFailureException.Message}");
                }
            }
            else
            {
                summary.AppendLine("? Plan execution completed successfully:");
            }

            summary.AppendLine();

            // Add detailed results for executed tasks
            for (int i = 0; i < prompts.Count; i++)
            {
                var result = results.FirstOrDefault(r => r.TaskIndex == i);
                if (result != null)
                {
                    var durationText = result.Duration.TotalMilliseconds > 0 ? $" ({result.Duration.TotalMilliseconds:F0}ms)" : "";
                    summary.AppendLine($"- {result.Message}{durationText}");
                }
                else
                {
                    summary.AppendLine($"- [Not executed] {prompts[i]}");
                }
            }

            // Add execution statistics
            var successCount = results.Count(r => r.Success);
            var failureCount = results.Count(r => !r.Success);
            var notExecutedCount = prompts.Count - results.Count;
            var totalDuration = results.Where(r => r.Duration.TotalMilliseconds > 0).Sum(r => r.Duration.TotalMilliseconds);

            summary.AppendLine();
            summary.AppendLine($"?? Execution Summary:");
            summary.AppendLine($"   • {successCount} succeeded");
            summary.AppendLine($"   • {failureCount} failed");
            summary.AppendLine($"   • {notExecutedCount} not executed");
            if (totalDuration > 0)
            {
                summary.AppendLine($"   • Total time: {totalDuration:F0}ms");
            }

            return summary.ToString();
        }
    }

    /// <summary>
    /// Represents the result of a task execution within a plan
    /// </summary>
    public class TaskExecutionResult
    {
        public int TaskIndex { get; set; }
        public string OriginalPrompt { get; set; } = string.Empty;
        public string ExecutedPrompt { get; set; } = string.Empty;
        public bool Success { get; set; }
        public string Result { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public Exception? Exception { get; set; }
        public DateTime StartTime { get; set; } = DateTime.UtcNow;
        public DateTime? EndTime { get; set; }
        public TimeSpan Duration => EndTime?.Subtract(StartTime) ?? TimeSpan.Zero;
    }

    public class PlannedTask
    {
        public string Id { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskStatus Status { get; set; } = TaskStatus.Pending;
    }

    public class TaskResult
    {
        public string TaskId { get; set; } = string.Empty;
        public bool Success { get; set; }
        public string Output { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
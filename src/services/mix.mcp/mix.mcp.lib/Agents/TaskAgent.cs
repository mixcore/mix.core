using Microsoft.Extensions.Logging;
using Mix.MCP.Lib.Services.LLM;
using Mix.MCP.Lib.Tools;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol.Transport;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mix.MCP.Lib.Agents
{
    /// <summary>
    /// A task-oriented agent that can handle specific commands and maintain task state
    /// </summary>
    public class TaskAgent : BaseAgent
    {
        private const string TaskStateKey = "task_state";
        private const string TaskHistoryKey = "task_history";
        private const int MaxTaskHistory = 20;

        private readonly Dictionary<string, Func<TaskState, string, Task<string>>> _commandHandlers;
        private IMcpClient _mcpClient;

        /// <summary>
        /// Initializes a new instance of the TaskAgent class
        /// </summary>
        public TaskAgent(
            ILlmServiceFactory llmServiceFactory,
            ILogger<TaskAgent> logger,
            TimeSpan? defaultTimeout = null)
            : base(llmServiceFactory, logger, defaultTimeout)
        {
            _commandHandlers = new Dictionary<string, Func<TaskState, string, Task<string>>>(StringComparer.OrdinalIgnoreCase)
            {
                { "start", HandleStartTaskAsync },
                { "status", HandleTaskStatusAsync },
                { "complete", HandleCompleteTaskAsync },
                { "cancel", HandleCancelTaskAsync },
                { "list", HandleListTasksAsync }
            };
        }

        /// <inheritdoc />
        public override async Task<string> ProcessInputAsync(
            string userInput,
            string deviceId,
            string sessionId = "default",
            LLMServiceType serviceType = LLMServiceType.DeepSeek,
            CancellationToken cancellationToken = default)
        {
            try
            {
                EnsureMcpClientInitialized();

                ValidateInput(userInput, sessionId);
                _logger.LogInformation("Processing task input for session {SessionId}: {UserInput}", sessionId, userInput);

                var memory = GetOrCreateMemory(sessionId);
                var taskState = GetTaskState(memory);
                var taskHistory = GetTaskHistory(memory);

                var (isTool, toolName, toolParams) = await AnalyzeInputForToolAsync(userInput, serviceType, cancellationToken);

                if (isTool && !string.IsNullOrEmpty(toolName))
                {
                    var toolResult = await CallMcpToolAsync(toolName, toolParams, cancellationToken);
                    AddToTaskHistory(taskHistory, $"tool:{toolName}", FormatToolArguments(toolParams), toolResult);
                    memory.SetValue(TaskHistoryKey, taskHistory);
                    return toolResult;
                }

                var (command, args) = ParseCommand(userInput);

                if (_commandHandlers.TryGetValue(command, out var handler))
                {
                    var response = await handler(taskState, args);
                    AddToTaskHistory(taskHistory, command, args, response);
                    TrimTaskHistory(taskHistory);
                    memory.SetValue(TaskStateKey, taskState);
                    memory.SetValue(TaskHistoryKey, taskHistory);
                    return response;
                }

                return await ProcessWithLlmAsync(userInput, taskState, serviceType, cancellationToken);
            }
            catch (Exception ex)
            {
                return HandleException(ex, userInput);
            }
        }

        private void EnsureMcpClientInitialized()
        {
            if (_mcpClient != null) return;

            var clientTransport = new SseClientTransport(new SseClientTransportOptions
            {
                Endpoint = new Uri("http://localhost:5011/mcp/sse"),
                Name = "MixTaskAgentClient"
            });
            _mcpClient = McpClientFactory.CreateAsync(clientTransport).GetAwaiter().GetResult();
        }

        private static void AddToTaskHistory(List<TaskHistoryEntry> history, string command, string arguments, string response)
        {
            history.Add(new TaskHistoryEntry
            {
                Timestamp = DateTime.UtcNow,
                Command = command,
                Arguments = arguments,
                Response = response
            });
        }

        private static void TrimTaskHistory(List<TaskHistoryEntry> history)
        {
            if (history.Count > MaxTaskHistory)
            {
                history.RemoveRange(0, history.Count - MaxTaskHistory);
            }
        }

        private static string FormatToolArguments(Dictionary<string, object> toolParams)
        {
            return string.Join(", ", toolParams.Select(kv => $"{kv.Key}={kv.Value}"));
        }

        private static (string Command, string Args) ParseCommand(string input)
        {
            var parts = input.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
            return (parts[0].ToLowerInvariant(), parts.Length > 1 ? parts[1] : string.Empty);
        }

        private TaskState GetTaskState(AgentMemory memory)
        {
            var state = memory.GetValue<TaskState>(TaskStateKey);
            if (state == null)
            {
                state = new TaskState();
                memory.SetValue(TaskStateKey, state);
            }
            return state;
        }

        private List<TaskHistoryEntry> GetTaskHistory(AgentMemory memory)
        {
            var history = memory.GetValue<List<TaskHistoryEntry>>(TaskHistoryKey);
            if (history == null)
            {
                history = new List<TaskHistoryEntry>();
                memory.SetValue(TaskHistoryKey, history);
            }
            return history;
        }

        private async Task<(bool IsTool, string ToolName, Dictionary<string, object> ToolParams)> AnalyzeInputForToolAsync(
            string userInput,
            LLMServiceType serviceType,
            CancellationToken cancellationToken)
        {
            // Gather context from session memory (e.g., last task result, task state, or history)
            var memory = GetOrCreateMemory("default");
            var taskState = GetTaskState(memory);
            var taskHistory = GetTaskHistory(memory);

            // You can customize what context to include. Here, we include the last task result and current state.
            string lastResult = taskHistory.LastOrDefault()?.Response ?? "";
            string stateSummary = $"Current task: {taskState.CurrentTask}, Status: {taskState.Status}";

            var supportedActions = ToolDiscovery.SupportedPromptToolActions;
            var toolList = string.Join("\n", supportedActions.Select(a => $"- {a.MethodName}: {a.Description}"));

            // Compose a prompt that includes the context
            var prompt = string.Format(
                """
                You are an AI assistant for a database platform.
                Read the following context before deciding how to handle the user's request.

                Context:
                - { 0}
            -Last result: { 1}

            Classify the user's request into one of these supported mcp tools:
                { 2}
            User message: "{3}"
                Respond in this JSON format:
                {
                {
                    "type": "tool" | "chat",
                  "toolName": "...", // Only if type is tool
                  "parameters": { { ... } } // Only if type is tool
                }
            }
            """,
                stateSummary,
                lastResult,
                toolList,
                userInput
            );

            var llmService = _llmServiceFactory.CreateService(serviceType);
            var response = await llmService.ChatAsync(prompt, "deepseek-chat", 0.2, -1, cancellationToken);
            var content = response?.choices?.FirstOrDefault()?.Message?.Content;
            if (string.IsNullOrWhiteSpace(content))
                return (false, null, null);

            try
            {
                var jsonStart = content.IndexOf('{');
                var jsonEnd = content.LastIndexOf('}');
                if (jsonStart < 0 || jsonEnd < 0 || jsonEnd < jsonStart)
                    return (false, null, null);

                var json = content.Substring(jsonStart, jsonEnd - jsonStart + 1);
                var doc = JObject.Parse(json);
                var type = doc.Value<string>("type");
                if (type == "tool")
                {
                    var toolName = doc.Value<string>("toolName");
                    var parameters = doc["parameters"]?.ToObject<Dictionary<string, object>>() ?? new Dictionary<string, object>();
                    return (true, toolName, parameters);
                }
            }
            catch
            {
                // Ignore parse errors, fallback to normal flow
            }
            return (false, null, null);
        }

        private async Task<string> CallMcpToolAsync(string toolName, Dictionary<string, object> parameters, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _mcpClient.CallToolAsync(toolName, parameters, cancellationToken: cancellationToken);
                return result.Content.FirstOrDefault(c => c.Type == "text")?.Text ?? "No response received from tool.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling MCP tool {ToolName}: {ErrorMessage}", toolName, ex.Message);
                return $"Error calling MCP tool {toolName}: {ex.Message}";
            }
        }

        /// <summary>
        /// Helper to get all supported [McpServerTool] methods and their descriptions from all [McpServerToolType] classes in the assembly
        /// </summary>
        private static List<(string MethodName, string Description)> GetSupportedPromptToolActions()
        {
            var actions = new List<(string MethodName, string Description)>();
            var assembly = typeof(TaskAgent).Assembly;

            foreach (var type in assembly.GetTypes())
            {
                // Only consider classes with [McpServerToolType] attribute
                var hasToolType = type.GetCustomAttributes(false)
                    .Any(attr => attr.GetType().Name == "McpServerToolTypeAttribute");
                if (!hasToolType)
                    continue;

                foreach (var method in type.GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public))
                {
                    // Only consider methods with [McpServerTool] attribute
                    var hasToolAttr = method.GetCustomAttributes(false)
                        .Any(attr => attr.GetType().Name == "McpServerToolAttribute");
                    if (!hasToolAttr)
                        continue;

                    // Try to get a [Description] attribute if present
                    var descAttr = method.GetCustomAttributes(false)
                        .FirstOrDefault(attr => attr.GetType().Name == "DescriptionAttribute");
                    string description = descAttr != null
                        ? (string)descAttr.GetType().GetProperty("Description")?.GetValue(descAttr)
                        : string.Join(", ", method.GetParameters().Where(p => !p.HasDefaultValue).Select(p => $"{p.Name} ({p.ParameterType.Name})"));

                    actions.Add(($"{type.Name}.{method.Name}", description));
                }
            }
            return actions;
        }

        private async Task<string> ProcessWithLlmAsync(string input, TaskState taskState, LLMServiceType serviceType, CancellationToken cancellationToken)
        {
            var llmService = _llmServiceFactory.CreateService(serviceType);
            var prompt = $"Current task state: {taskState.Status}\nUser input: {input}\nPlease provide a helpful response.";

            var response = await llmService.ChatAsync(
                prompt,
                "deepseek-chat",
                0.7,
                -1,
                cancellationToken);

            return response?.choices?.FirstOrDefault()?.Message?.Content
                ?? "I apologize, but I couldn't process your request.";
        }

        #region Command Handlers

        private static Task<string> HandleStartTaskAsync(TaskState state, string args)
        {
            if (state.Status == TaskStatus.InProgress)
            {
                return Task.FromResult("A task is already in progress. Please complete or cancel it first.");
            }

            state.Status = TaskStatus.InProgress;
            state.CurrentTask = args;
            state.StartTime = DateTime.UtcNow;
            return Task.FromResult($"Started new task: {args}");
        }

        private static Task<string> HandleTaskStatusAsync(TaskState state, string args)
        {
            if (state.Status == TaskStatus.NotStarted)
            {
                return Task.FromResult("No task is currently in progress.");
            }

            var duration = DateTime.UtcNow - state.StartTime;
            return Task.FromResult($"Current task: {state.CurrentTask}\nStatus: {state.Status}\nDuration: {duration.TotalMinutes:F1} minutes");
        }

        private static Task<string> HandleCompleteTaskAsync(TaskState state, string args)
        {
            if (state.Status != TaskStatus.InProgress)
            {
                return Task.FromResult("No task is currently in progress.");
            }

            state.Status = TaskStatus.Completed;
            state.CompletionTime = DateTime.UtcNow;
            return Task.FromResult($"Completed task: {state.CurrentTask}");
        }

        private static Task<string> HandleCancelTaskAsync(TaskState state, string args)
        {
            if (state.Status != TaskStatus.InProgress)
            {
                return Task.FromResult("No task is currently in progress.");
            }

            state.Status = TaskStatus.Cancelled;
            state.CompletionTime = DateTime.UtcNow;
            return Task.FromResult($"Cancelled task: {state.CurrentTask}");
        }

        private Task<string> HandleListTasksAsync(TaskState state, string args)
        {
            var memory = GetOrCreateMemory("default");
            var history = GetTaskHistory(memory);

            if (!history.Any())
            {
                return Task.FromResult("No task history available.");
            }

            var response = new System.Text.StringBuilder("Recent tasks:\n");
            foreach (var entry in history.TakeLast(5))
            {
                response.AppendLine($"- {entry.Timestamp:g}: {entry.Command} {entry.Arguments}");
            }

            return Task.FromResult(response.ToString());
        }

        #endregion
    }

    /// <summary>
    /// Represents the current state of a task
    /// </summary>
    public class TaskState
    {
        public TaskStatus Status { get; set; } = TaskStatus.NotStarted;
        public string CurrentTask { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? CompletionTime { get; set; }
    }

    /// <summary>
    /// Represents the status of a task
    /// </summary>
    public enum TaskStatus
    {
        NotStarted,
        InProgress,
        Pending,
        Failed,
        Completed,
        Cancelled
    }

    /// <summary>
    /// Represents an entry in the task history
    /// </summary>
    public class TaskHistoryEntry
    {
        public DateTime Timestamp { get; set; }
        public string Command { get; set; }
        public string Arguments { get; set; }
        public string Response { get; set; }
    }
}
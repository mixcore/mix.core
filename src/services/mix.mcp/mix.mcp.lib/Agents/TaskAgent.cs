using Microsoft.Extensions.Logging;
using Mix.MCP.Lib.Services.LLM;
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
        private const string TASK_STATE_KEY = "task_state";
        private const string TASK_HISTORY_KEY = "task_history";
        private const int MAX_TASK_HISTORY = 20;

        private readonly Dictionary<string, Func<TaskState, string, Task<string>>> _commandHandlers;

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
                { "start", HandleStartTask },
                { "status", HandleTaskStatus },
                { "complete", HandleCompleteTask },
                { "cancel", HandleCancelTask },
                { "list", HandleListTasks }
            };
        }

        /// <summary>
        /// Processes user input and handles task-related commands
        /// </summary>
        public override async Task<string> ProcessInputAsync(
            string userInput,
            string sessionId = "default",
            LLMServiceType serviceType = LLMServiceType.DeepSeek,
            CancellationToken cancellationToken = default)
        {
            try
            {
                ValidateInput(userInput, sessionId);
                _logger.LogInformation("Processing task input for session {SessionId}: {UserInput}", sessionId, userInput);

                var memory = GetOrCreateMemory(sessionId);
                var taskState = GetTaskState(memory);
                var taskHistory = GetTaskHistory(memory);

                // Parse command and arguments
                var (command, args) = ParseCommand(userInput);
                
                if (_commandHandlers.TryGetValue(command, out var handler))
                {
                    var response = await handler(taskState, args);
                    
                    // Update task history
                    taskHistory.Add(new TaskHistoryEntry
                    {
                        Timestamp = DateTime.UtcNow,
                        Command = command,
                        Arguments = args,
                        Response = response
                    });

                    // Trim history if needed
                    if (taskHistory.Count > MAX_TASK_HISTORY)
                    {
                        taskHistory.RemoveRange(0, taskHistory.Count - MAX_TASK_HISTORY);
                    }

                    // Update memory
                    memory.SetValue(TASK_STATE_KEY, taskState);
                    memory.SetValue(TASK_HISTORY_KEY, taskHistory);

                    return response;
                }

                // If no command handler found, use LLM to process the input
                return await ProcessWithLLM(userInput, taskState, serviceType, cancellationToken);
            }
            catch (Exception ex)
            {
                return HandleException(ex, userInput);
            }
        }

        /// <summary>
        /// Gets the current task state from memory or creates a new one
        /// </summary>
        private TaskState GetTaskState(AgentMemory memory)
        {
            var state = memory.GetValue<TaskState>(TASK_STATE_KEY);
            if (state == null)
            {
                state = new TaskState();
                memory.SetValue(TASK_STATE_KEY, state);
            }
            return state;
        }

        /// <summary>
        /// Gets the task history from memory or creates a new one
        /// </summary>
        private List<TaskHistoryEntry> GetTaskHistory(AgentMemory memory)
        {
            var history = memory.GetValue<List<TaskHistoryEntry>>(TASK_HISTORY_KEY);
            if (history == null)
            {
                history = new List<TaskHistoryEntry>();
                memory.SetValue(TASK_HISTORY_KEY, history);
            }
            return history;
        }

        /// <summary>
        /// Parses the user input into a command and arguments
        /// </summary>
        private (string command, string args) ParseCommand(string input)
        {
            var parts = input.Split(new[] { ' ' }, 2);
            return (parts[0].ToLower(), parts.Length > 1 ? parts[1] : string.Empty);
        }

        /// <summary>
        /// Processes input using LLM when no specific command is recognized
        /// </summary>
        private async Task<string> ProcessWithLLM(string input, TaskState taskState, LLMServiceType serviceType, CancellationToken cancellationToken)
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

        private async Task<string> HandleStartTask(TaskState state, string args)
        {
            if (state.Status == TaskStatus.InProgress)
            {
                return "A task is already in progress. Please complete or cancel it first.";
            }

            state.Status = TaskStatus.InProgress;
            state.CurrentTask = args;
            state.StartTime = DateTime.UtcNow;
            return $"Started new task: {args}";
        }

        private async Task<string> HandleTaskStatus(TaskState state, string args)
        {
            if (state.Status == TaskStatus.NotStarted)
            {
                return "No task is currently in progress.";
            }

            var duration = DateTime.UtcNow - state.StartTime;
            return $"Current task: {state.CurrentTask}\nStatus: {state.Status}\nDuration: {duration.TotalMinutes:F1} minutes";
        }

        private async Task<string> HandleCompleteTask(TaskState state, string args)
        {
            if (state.Status != TaskStatus.InProgress)
            {
                return "No task is currently in progress.";
            }

            state.Status = TaskStatus.Completed;
            state.CompletionTime = DateTime.UtcNow;
            return $"Completed task: {state.CurrentTask}";
        }

        private async Task<string> HandleCancelTask(TaskState state, string args)
        {
            if (state.Status != TaskStatus.InProgress)
            {
                return "No task is currently in progress.";
            }

            state.Status = TaskStatus.Cancelled;
            state.CompletionTime = DateTime.UtcNow;
            return $"Cancelled task: {state.CurrentTask}";
        }

        private async Task<string> HandleListTasks(TaskState state, string args)
        {
            var memory = GetOrCreateMemory("default");
            var history = GetTaskHistory(memory);
            
            if (!history.Any())
            {
                return "No task history available.";
            }

            var response = new System.Text.StringBuilder("Recent tasks:\n");
            foreach (var entry in history.TakeLast(5))
            {
                response.AppendLine($"- {entry.Timestamp:g}: {entry.Command} {entry.Arguments}");
            }

            return response.ToString();
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
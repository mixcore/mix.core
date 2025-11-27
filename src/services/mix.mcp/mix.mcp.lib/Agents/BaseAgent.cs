using Microsoft.Extensions.Logging;
using Mix.MCP.Lib.Services.LLM;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Mix.MCP.Lib.Agents
{
    /// <summary>
    /// Base class for AI agents providing core functionality and best practices
    /// </summary>
    public abstract class BaseAgent
    {
        protected readonly ILlmServiceFactory _llmServiceFactory;
        protected readonly ILogger _logger;
        protected readonly ConcurrentDictionary<string, AgentMemory> _sessionMemories;
        protected readonly TimeSpan _defaultTimeout;

        /// <summary>
        /// Initializes a new instance of the BaseAgent class
        /// </summary>
        protected BaseAgent(
            ILlmServiceFactory llmServiceFactory,
            ILogger logger,
            TimeSpan? defaultTimeout = null)
        {
            _llmServiceFactory = llmServiceFactory ?? throw new ArgumentNullException(nameof(llmServiceFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _sessionMemories = new ConcurrentDictionary<string, AgentMemory>();
            _defaultTimeout = defaultTimeout ?? TimeSpan.FromSeconds(120);
        }

        /// <summary>
        /// Processes a user input and returns a response
        /// </summary>
        public abstract Task<string> ProcessInputAsync(
            string userInput,
            string deviceId,
            string sessionId = "default",
            LLMServiceType serviceType = LLMServiceType.DeepSeek,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets or creates a memory store for the specified session
        /// </summary>
        protected AgentMemory GetOrCreateMemory(string sessionId)
        {
            return _sessionMemories.GetOrAdd(sessionId, _ => new AgentMemory());
        }

        /// <summary>
        /// Clears the memory for a specific session
        /// </summary>
        public void ClearMemory(string sessionId)
        {
            if (_sessionMemories.TryRemove(sessionId, out _))
            {
                _logger.LogInformation("Memory cleared for session {SessionId}", sessionId);
            }
        }

        /// <summary>
        /// Clears all session memories
        /// </summary>
        public void ClearAllMemories()
        {
            _sessionMemories.Clear();
            _logger.LogInformation("All memories cleared");
        }

        /// <summary>
        /// Gets all active session IDs
        /// </summary>
        public string[] GetActiveSessionIds()
        {
            return _sessionMemories.Keys.ToArray();
        }

        /// <summary>
        /// Validates the input parameters
        /// </summary>
        protected virtual void ValidateInput(string userInput, string sessionId)
        {
            if (string.IsNullOrWhiteSpace(userInput))
            {
                throw new ArgumentException("User input cannot be empty", nameof(userInput));
            }

            if (string.IsNullOrWhiteSpace(sessionId))
            {
                throw new ArgumentException("Session ID cannot be empty", nameof(sessionId));
            }
        }

        /// <summary>
        /// Handles exceptions that occur during agent processing
        /// </summary>
        protected virtual string HandleException(Exception ex, string userInput)
        {
            _logger.LogError(ex, "Error processing input: {UserInput}", userInput);
            return "I apologize, but I encountered an error while processing your request. Please try again.";
        }
    }

    /// <summary>
    /// Represents the memory state of an agent session
    /// </summary>
    public class AgentMemory
    {
        private readonly ConcurrentDictionary<string, object> _memory;

        public AgentMemory()
        {
            _memory = new ConcurrentDictionary<string, object>();
        }

        /// <summary>
        /// Gets a value from memory
        /// </summary>
        public T GetValue<T>(string key)
        {
            if (_memory.TryGetValue(key, out var value) && value is T typedValue)
            {
                return typedValue;
            }
            return default;
        }

        /// <summary>
        /// Sets a value in memory
        /// </summary>
        public void SetValue<T>(string key, T value)
        {
            _memory.AddOrUpdate(key, value, (_, __) => value);
        }

        /// <summary>
        /// Removes a value from memory
        /// </summary>
        public bool RemoveValue(string key)
        {
            return _memory.TryRemove(key, out _);
        }

        /// <summary>
        /// Checks if a key exists in memory
        /// </summary>
        public bool HasKey(string key)
        {
            return _memory.ContainsKey(key);
        }

        /// <summary>
        /// Gets all keys in memory
        /// </summary>
        public string[] GetAllKeys()
        {
            return _memory.Keys.ToArray();
        }

        /// <summary>
        /// Clears all values from memory
        /// </summary>
        public void Clear()
        {
            _memory.Clear();
        }
    }
}
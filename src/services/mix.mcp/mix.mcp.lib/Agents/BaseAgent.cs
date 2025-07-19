using Microsoft.Extensions.Logging;
using Mix.Database.Services;
using Mix.MCP.Lib.Models;
using Mix.MCP.Lib.Services.LLM;
using Mix.MCP.Lib.Services.Knowledge;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol.Transport;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
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
        protected readonly AppSettingsService _appSettingsService;
        protected readonly ConcurrentDictionary<string, AgentMemory> _sessionMemories;
        protected readonly TimeSpan _defaultTimeout;
        protected IMcpClient _mcpClient
        {
            get
            {
                if (_mcpClient == null)
                {
                    string mcpServer = _appSettingsService.AppSettings.McpSettings.BaseUrl ?? $"{_appSettingsService.AppSettings.BaseUrl}/mcp/sse";
                    // Initialize MCP client
                    var clientTransport = new SseClientTransport(new SseClientTransportOptions()
                    {
                        Endpoint = new Uri(mcpServer),
                        Name = "MixDatabaseAgentClient"
                    });
                    _mcpClient = McpClientFactory.CreateAsync(clientTransport).GetAwaiter().GetResult();
                }
                return _mcpClient;
            }
            set { _mcpClient = value; }
        }
        protected readonly IKnowledgeBaseService? _knowledgeBaseService;

        /// <summary>
        /// Initializes a new instance of the BaseAgent class
        /// </summary>
        protected BaseAgent(
            AppSettingsService appSettingsService,
            ILlmServiceFactory llmServiceFactory,
            ILogger logger,
            IKnowledgeBaseService? knowledgeBaseService = null,
            TimeSpan? defaultTimeout = null)
        {
            _llmServiceFactory = llmServiceFactory ?? throw new ArgumentNullException(nameof(llmServiceFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _sessionMemories = new ConcurrentDictionary<string, AgentMemory>();
            _defaultTimeout = defaultTimeout ?? TimeSpan.FromSeconds(120);
            _knowledgeBaseService = knowledgeBaseService;
            _appSettingsService = appSettingsService ?? throw new ArgumentNullException(nameof(appSettingsService));
        }

        /// <summary>
        /// Processes a user input and returns a response
        /// </summary>
        public abstract Task<AgentProcessResult> ProcessInputAsync(
            string userInput,
            string deviceId,
            string sessionId = "default",
            LLMServiceType serviceType = LLMServiceType.DeepSeek,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Helper method to call database tools through MCP client
        /// </summary>
        protected async Task<string> CallDatabaseToolAsync(string toolName, Dictionary<string, object> parameters, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _mcpClient.CallToolAsync(toolName, parameters, cancellationToken: cancellationToken);
                return result.Content.FirstOrDefault(c => c.Type == "text")?.Text ?? "No response received from tool.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling database tool {ToolName}: {ErrorMessage}", toolName, ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets contextual knowledge for the current request
        /// </summary>
        /// <param name="userInput">User input to get context for</param>
        /// <param name="agentType">Type of agent requesting context</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Contextual information or empty string if no knowledge service available</returns>
        protected async Task<string> GetKnowledgeContextAsync(
            string userInput,
            string agentType = "general",
            CancellationToken cancellationToken = default)
        {
            if (_knowledgeBaseService == null)
            {
                _logger.LogDebug("Knowledge base service not available for context retrieval");
                return string.Empty;
            }

            try
            {
                return await _knowledgeBaseService.GetContextForPlanningAsync(userInput, agentType, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to retrieve knowledge context for input: {UserInput}", userInput);
                return string.Empty;
            }
        }

        /// <summary>
        /// Searches the knowledge base for relevant information
        /// </summary>
        /// <param name="query">Search query</param>
        /// <param name="maxResults">Maximum number of results</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Knowledge entries or empty collection if no service available</returns>
        protected async Task<IEnumerable<KnowledgeEntry>> SearchKnowledgeAsync(
            string query,
            int maxResults = 5,
            CancellationToken cancellationToken = default)
        {
            if (_knowledgeBaseService == null)
            {
                return Enumerable.Empty<KnowledgeEntry>();
            }

            try
            {
                return await _knowledgeBaseService.SearchAsync(query, maxResults, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to search knowledge base for query: {Query}", query);
                return Enumerable.Empty<KnowledgeEntry>();
            }
        }

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
        protected virtual AgentProcessResult HandleException(Exception ex, string userInput)
        {
            _logger.LogError(ex, "Error processing input: {UserInput}", userInput);
            return new AgentProcessResult(false, "I apologize, but I encountered an error while processing your request. Please try again.");
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
        public T? GetValue<T>(string key)
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
            _memory.AddOrUpdate(key, value!, (_, __) => value!);
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
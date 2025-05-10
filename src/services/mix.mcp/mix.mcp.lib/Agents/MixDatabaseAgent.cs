using Microsoft.Extensions.Logging;
using Mix.Constant.Enums;
using Mix.MCP.Lib.Services.LLM;
using Mix.MCP.Lib.Tools;
using ModelContextProtocol.Server;
using System.Collections.Concurrent;
using System.Text.Json;

namespace Mix.MCP.Lib.Agents
{
    /// <summary>
    /// Agent for handling database operations using MixDatabasePromptTool
    /// </summary>
    public class MixDatabaseAgent
    {
        private readonly MixDatabasePromptTool _databaseTool;
        private readonly ILlmServiceFactory _llmServiceFactory;
        private readonly ILogger<MixDatabaseAgent> _logger;
        private readonly ConcurrentDictionary<string, Dictionary<string, object>> _sessionMemory;
        private const int DEFAULT_TIMEOUT_SECONDS = 120;
        private const string DEFAULT_SESSION_ID = "default";

        /// <summary>
        /// Initializes a new instance of the MixDatabaseAgent class
        /// </summary>
        public MixDatabaseAgent(
            MixDatabasePromptTool databaseTool,
            ILlmServiceFactory llmServiceFactory,
            ILogger<MixDatabaseAgent> logger)
        {
            _databaseTool = databaseTool ?? throw new ArgumentNullException(nameof(databaseTool));
            _llmServiceFactory = llmServiceFactory ?? throw new ArgumentNullException(nameof(llmServiceFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _sessionMemory = new ConcurrentDictionary<string, Dictionary<string, object>>();
        }

        /// <summary>
        /// Creates a new database based on a natural language description
        /// </summary>
        public async Task<string> CreateDatabaseAsync(
            string displayName,
            string schemaDescription,
            string sessionId = DEFAULT_SESSION_ID,
            int mixDatabaseContextId = 1,
            LLMServiceType llmServiceType = LLMServiceType.LmStudio,
            string llmModel = "mathstral-7b-v0.1",
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Creating database {DisplayName} with schema: {SchemaDescription} for session {SessionId}", 
                    displayName, schemaDescription, sessionId);

                // Get or create session memory
                var sessionMemory = _sessionMemory.GetOrAdd(sessionId, _ => new Dictionary<string, object>());

                // Store context in memory
                sessionMemory["current_operation"] = "create_database";
                sessionMemory["display_name"] = displayName;
                sessionMemory["schema_description"] = schemaDescription;

                // Execute the database creation
                var result = await _databaseTool.CreateDatabaseFromPrompt(
                    displayName,
                    schemaDescription,
                    mixDatabaseContextId,
                    llmServiceType,
                    llmModel,
                    cancellationToken);

                // Parse and store the result
                var resultObj = JsonSerializer.Deserialize<JsonElement>(result);
                if (resultObj.GetProperty("Success").GetBoolean())
                {
                    sessionMemory["last_created_database"] = resultObj.GetProperty("SystemName").GetString();
                    _logger.LogInformation("Successfully created database {DatabaseName} for session {SessionId}", 
                        displayName, sessionId);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating database {DisplayName} for session {SessionId}", 
                    displayName, sessionId);
                throw;
            }
        }

        /// <summary>
        /// Adds columns to an existing database based on a natural language description
        /// </summary>
        public async Task<string> AddColumnsAsync(
            string databaseSystemName,
            string schemaText,
            string sessionId = DEFAULT_SESSION_ID,
            LLMServiceType llmServiceType = LLMServiceType.LmStudio,
            string llmModel = "mathstral-7b-v0.1",
            int timeoutSeconds = DEFAULT_TIMEOUT_SECONDS,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Adding columns to database {DatabaseName} with schema: {SchemaText} for session {SessionId}", 
                    databaseSystemName, schemaText, sessionId);

                // Get or create session memory
                var sessionMemory = _sessionMemory.GetOrAdd(sessionId, _ => new Dictionary<string, object>());

                // Store context in memory
                sessionMemory["current_operation"] = "add_columns";
                sessionMemory["database_name"] = databaseSystemName;
                sessionMemory["schema_text"] = schemaText;

                // Execute the column addition
                var result = await _databaseTool.AddColumnToDatabase(
                    databaseSystemName,
                    schemaText,
                    llmServiceType,
                    llmModel,
                    timeoutSeconds,
                    cancellationToken);

                // Parse and store the result
                var resultObj = JsonSerializer.Deserialize<JsonElement>(result);
                if (resultObj.GetProperty("Success").GetBoolean())
                {
                    sessionMemory["last_modified_database"] = databaseSystemName;
                    _logger.LogInformation("Successfully added columns to database {DatabaseName} for session {SessionId}", 
                        databaseSystemName, sessionId);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding columns to database {DatabaseName} for session {SessionId}", 
                    databaseSystemName, sessionId);
                throw;
            }
        }

        /// <summary>
        /// Updates columns in an existing database based on a natural language description
        /// </summary>
        public async Task<string> UpdateColumnsAsync(
            string databaseSystemName,
            string schemaText,
            string sessionId = DEFAULT_SESSION_ID,
            LLMServiceType llmServiceType = LLMServiceType.LmStudio,
            string llmModel = "mathstral-7b-v0.1",
            int timeoutSeconds = DEFAULT_TIMEOUT_SECONDS,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Updating columns in database {DatabaseName} with schema: {SchemaText} for session {SessionId}", 
                    databaseSystemName, schemaText, sessionId);

                // Get or create session memory
                var sessionMemory = _sessionMemory.GetOrAdd(sessionId, _ => new Dictionary<string, object>());

                // Store context in memory
                sessionMemory["current_operation"] = "update_columns";
                sessionMemory["database_name"] = databaseSystemName;
                sessionMemory["schema_text"] = schemaText;

                // Execute the column update
                var result = await _databaseTool.UpdateDatabaseColumn(
                    databaseSystemName,
                    schemaText,
                    llmServiceType,
                    llmModel,
                    timeoutSeconds,
                    cancellationToken);

                // Parse and store the result
                var resultObj = JsonSerializer.Deserialize<JsonElement>(result);
                if (resultObj.GetProperty("Success").GetBoolean())
                {
                    sessionMemory["last_modified_database"] = databaseSystemName;
                    _logger.LogInformation("Successfully updated columns in database {DatabaseName} for session {SessionId}", 
                        databaseSystemName, sessionId);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating columns in database {DatabaseName} for session {SessionId}", 
                    databaseSystemName, sessionId);
                throw;
            }
        }

        /// <summary>
        /// Deletes columns from an existing database based on a natural language description
        /// </summary>
        public async Task<string> DeleteColumnsAsync(
            string databaseSystemName,
            string schemaText,
            string sessionId = DEFAULT_SESSION_ID,
            LLMServiceType llmServiceType = LLMServiceType.LmStudio,
            string llmModel = "mathstral-7b-v0.1",
            int timeoutSeconds = DEFAULT_TIMEOUT_SECONDS,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Deleting columns from database {DatabaseName} with schema: {SchemaText} for session {SessionId}", 
                    databaseSystemName, schemaText, sessionId);

                // Get or create session memory
                var sessionMemory = _sessionMemory.GetOrAdd(sessionId, _ => new Dictionary<string, object>());

                // Store context in memory
                sessionMemory["current_operation"] = "delete_columns";
                sessionMemory["database_name"] = databaseSystemName;
                sessionMemory["schema_text"] = schemaText;

                // Execute the column deletion
                var result = await _databaseTool.DeleteDatabaseColumn(
                    databaseSystemName,
                    schemaText,
                    llmServiceType,
                    llmModel,
                    timeoutSeconds,
                    "YES",
                    cancellationToken);

                // Parse and store the result
                var resultObj = JsonSerializer.Deserialize<JsonElement>(result);
                if (resultObj.GetProperty("Success").GetBoolean())
                {
                    sessionMemory["last_modified_database"] = databaseSystemName;
                    _logger.LogInformation("Successfully deleted columns from database {DatabaseName} for session {SessionId}", 
                        databaseSystemName, sessionId);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting columns from database {DatabaseName} for session {SessionId}", 
                    databaseSystemName, sessionId);
                throw;
            }
        }

        /// <summary>
        /// Gets the current state of the agent's memory for a specific session
        /// </summary>
        public Dictionary<string, object> GetMemoryState(string sessionId = DEFAULT_SESSION_ID)
        {
            return _sessionMemory.TryGetValue(sessionId, out var memory) 
                ? new Dictionary<string, object>(memory) 
                : new Dictionary<string, object>();
        }

        /// <summary>
        /// Clears the agent's memory for a specific session
        /// </summary>
        public void ClearMemory(string sessionId = DEFAULT_SESSION_ID)
        {
            if (_sessionMemory.TryRemove(sessionId, out _))
            {
                _logger.LogInformation("Agent memory cleared for session {SessionId}", sessionId);
            }
        }

        /// <summary>
        /// Gets all active session IDs
        /// </summary>
        public IEnumerable<string> GetActiveSessionIds()
        {
            return _sessionMemory.Keys;
        }

        /// <summary>
        /// Clears all session memories
        /// </summary>
        public void ClearAllMemories()
        {
            _sessionMemory.Clear();
            _logger.LogInformation("All agent memories cleared");
        }
    }
} 
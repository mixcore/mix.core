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

        // Enum for database intent classification
        private enum DatabaseIntent
        {
            CreateDatabase,
            AddColumns,
            UpdateColumns,
            DeleteColumns,
            CreateRecord,
            UpdateRecord,
            DeleteRecord,
            QueryRecord,
            Unknown
        }

        /// <summary>
        /// Helper to get all supported [McpServerTool] methods and their descriptions from MixDatabasePromptTool
        /// </summary>
        private static List<(string MethodName, string Description)> GetSupportedPromptToolActions()
        {
            var toolType = typeof(MixDatabasePromptTool);
            var actions = new List<(string, string)>();
            foreach (var method in toolType.GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public))
            {
                var toolAttr = method.GetCustomAttributes(typeof(ModelContextProtocol.Server.McpServerToolAttribute), false).FirstOrDefault();
                if (toolAttr != null)
                {
                    var descAttr = method.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false)
                        .Cast<System.ComponentModel.DescriptionAttribute>()
                        .FirstOrDefault();
                    actions.Add((method.Name, descAttr?.Description ?? method.Name));
                }
            }
            return actions;
        }

        /// <summary>
        /// Classifies the user's natural language input into a database intent and extracts parameters using LLM
        /// </summary>
        private async Task<(DatabaseIntent intent, Dictionary<string, string> parameters)> ClassifyIntentAsync(
            string userInput,
            LLMServiceType llmServiceType,
            string llmModel,
            CancellationToken cancellationToken)
        {
            var supportedActions = GetSupportedPromptToolActions();
            var toolList = string.Join("\n", supportedActions.Select(a => $"- {a.MethodName}: {a.Description}"));
            string prompt = """
You are an AI assistant for a database platform. The following actions are supported by the system:
{toolList}

Classify the user's request into one of the supported actions (by method name), and extract any relevant parameters (e.g., database name, column names, values, etc.) as a JSON object.

User request: \"{userInput}\"

Respond in this JSON format:
{{
  \"action\": \"...\",
  \"parameters\": {{ ... }}
}}
""";
            var llmService = _llmServiceFactory.CreateService(llmServiceType);
            var response = await llmService.ChatAsync(prompt, llmModel, 0.2, -1, cancellationToken);
            var content = response?.choices?.FirstOrDefault()?.Message?.Content;
            if (string.IsNullOrWhiteSpace(content))
                return (DatabaseIntent.Unknown, new Dictionary<string, string>());

            try
            {
                var doc = JsonDocument.Parse(content);
                var actionStr = doc.RootElement.GetProperty("action").GetString();
                var parameters = new Dictionary<string, string>();
                if (doc.RootElement.TryGetProperty("parameters", out var paramElement) && paramElement.ValueKind == JsonValueKind.Object)
                {
                    foreach (var prop in paramElement.EnumerateObject())
                    {
                        parameters[prop.Name] = prop.Value.ToString();
                    }
                }
                // Map action string to DatabaseIntent (fallback to Unknown if not mapped)
                DatabaseIntent intent = actionStr switch
                {
                    "CreateDatabaseFromPrompt" => DatabaseIntent.CreateDatabase,
                    "AddColumnToDatabase" => DatabaseIntent.AddColumns,
                    "UpdateDatabaseColumn" => DatabaseIntent.UpdateColumns,
                    "DeleteDatabaseColumn" => DatabaseIntent.DeleteColumns,
                    _ => DatabaseIntent.Unknown
                };
                return (intent, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to parse LLM intent classification response: {Content}", content);
                return (DatabaseIntent.Unknown, new Dictionary<string, string>());
            }
        }

        /// <summary>
        /// Handles a natural language request by classifying intent and routing to the appropriate tool
        /// </summary>
        public async Task<string> HandleNaturalLanguageRequestAsync(
            string userInput,
            string sessionId = DEFAULT_SESSION_ID,
            LLMServiceType llmServiceType = LLMServiceType.LmStudio,
            string llmModel = "mathstral-7b-v0.1",
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Received user input: {UserInput} for session {SessionId}", userInput, sessionId);
                var (intent, parameters) = await ClassifyIntentAsync(userInput, llmServiceType, llmModel, cancellationToken);
                switch (intent)
                {
                    case DatabaseIntent.CreateDatabase:
                        // Validate required parameters
                        if (!parameters.TryGetValue("displayName", out var displayName) || string.IsNullOrWhiteSpace(displayName) ||
                            !parameters.TryGetValue("schemaDescription", out var schemaDescription) || string.IsNullOrWhiteSpace(schemaDescription))
                        {
                            _logger.LogWarning("Missing required parameters for CreateDatabase: {Params}", parameters);
                            return "Missing required parameters for creating a database.";
                        }
                        return await _databaseTool.CreateDatabaseFromPrompt(
                            displayName,
                            schemaDescription,
                            1, // mixDatabaseContextId
                            llmServiceType,
                            llmModel,
                            cancellationToken);
                    case DatabaseIntent.AddColumns:
                        if (!parameters.TryGetValue("databaseSystemName", out var dbNameAdd) || string.IsNullOrWhiteSpace(dbNameAdd) ||
                            !parameters.TryGetValue("schemaText", out var schemaTextAdd) || string.IsNullOrWhiteSpace(schemaTextAdd))
                        {
                            _logger.LogWarning("Missing required parameters for AddColumns: {Params}", parameters);
                            return "Missing required parameters for adding columns.";
                        }
                        return await _databaseTool.AddColumnToDatabase(
                            dbNameAdd,
                            schemaTextAdd,
                            llmServiceType,
                            llmModel,
                            DEFAULT_TIMEOUT_SECONDS,
                            cancellationToken);
                    case DatabaseIntent.UpdateColumns:
                        if (!parameters.TryGetValue("databaseSystemName", out var dbNameUpdate) || string.IsNullOrWhiteSpace(dbNameUpdate) ||
                            !parameters.TryGetValue("schemaText", out var schemaTextUpdate) || string.IsNullOrWhiteSpace(schemaTextUpdate))
                        {
                            _logger.LogWarning("Missing required parameters for UpdateColumns: {Params}", parameters);
                            return "Missing required parameters for updating columns.";
                        }
                        return await _databaseTool.UpdateDatabaseColumn(
                            dbNameUpdate,
                            schemaTextUpdate,
                            llmServiceType,
                            llmModel,
                            DEFAULT_TIMEOUT_SECONDS,
                            cancellationToken);
                    case DatabaseIntent.DeleteColumns:
                        if (!parameters.TryGetValue("databaseSystemName", out var dbNameDelete) || string.IsNullOrWhiteSpace(dbNameDelete) ||
                            !parameters.TryGetValue("schemaText", out var schemaTextDelete) || string.IsNullOrWhiteSpace(schemaTextDelete))
                        {
                            _logger.LogWarning("Missing required parameters for DeleteColumns: {Params}", parameters);
                            return "Missing required parameters for deleting columns.";
                        }
                        return await _databaseTool.DeleteDatabaseColumn(
                            dbNameDelete,
                            schemaTextDelete,
                            llmServiceType,
                            llmModel,
                            DEFAULT_TIMEOUT_SECONDS,
                            "YES",
                            cancellationToken);
                    // Data CRUD operations (requires MixDatabaseDataTool)
                    case DatabaseIntent.CreateRecord:
                        _logger.LogWarning("CreateRecord intent detected, but data tool integration is not implemented in this agent.");
                        return "CreateRecord intent detected, but data tool integration is not implemented.";
                    case DatabaseIntent.UpdateRecord:
                        _logger.LogWarning("UpdateRecord intent detected, but data tool integration is not implemented in this agent.");
                        return "UpdateRecord intent detected, but data tool integration is not implemented.";
                    case DatabaseIntent.DeleteRecord:
                        _logger.LogWarning("DeleteRecord intent detected, but data tool integration is not implemented in this agent.");
                        return "DeleteRecord intent detected, but data tool integration is not implemented.";
                    case DatabaseIntent.QueryRecord:
                        _logger.LogWarning("QueryRecord intent detected, but data tool integration is not implemented in this agent.");
                        return "QueryRecord intent detected, but data tool integration is not implemented.";
                    default:
                        _logger.LogWarning("Could not classify user intent for input: {UserInput}", userInput);
                        return "Sorry, I could not understand your request. Please rephrase.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling natural language request: {UserInput}", userInput);
                throw;
            }
        }
    }
} 
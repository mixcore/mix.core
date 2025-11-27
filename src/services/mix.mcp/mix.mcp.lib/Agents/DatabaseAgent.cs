using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mix.Constant.Enums;
using Mix.Database.Services;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Extensions;
using Mix.MCP.Lib.Helpers;
using Mix.MCP.Lib.Services.LLM;
using Mix.MCP.Lib.Tools;
using Mix.Shared.Models.Configurations;
using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol.Transport;
using ModelContextProtocol.Server;
using Newtonsoft.Json.Linq;
using NuGet.ProjectModel;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Text.Json;

namespace Mix.MCP.Lib.Agents
{
    /// <summary>
    /// Options for configuring MCP client
    /// </summary>
    public class McpOptions
    {
        public string Endpoint { get; set; }
        public string ClientName { get; set; }
    }

    /// <summary>
    /// Agent for handling database operations using MixDbPromptTool
    /// </summary>
    public class DatabaseAgent
    {
        private readonly ILlmServiceFactory _llmServiceFactory;
        private readonly ILogger<DatabaseAgent> _logger;
        private readonly ConcurrentDictionary<string, Dictionary<string, object>> _sessionMemory;
        private readonly IMcpClient _mcpClient;
        private readonly ChatAgent _chatAgent;
        private const int DEFAULT_TIMEOUT_SECONDS = 120;
        private const string DEFAULT_SESSION_ID = "default";

        /// <summary>
        /// Initializes a new instance of the DatabaseAgent class
        /// </summary>
        public DatabaseAgent(
            ILlmServiceFactory llmServiceFactory,
            ILogger<DatabaseAgent> logger,
            ChatAgent chatAgent,
            IConfiguration configuration,
            AppSettingsService appSettingsService)
        {
            _llmServiceFactory = llmServiceFactory ?? throw new ArgumentNullException(nameof(llmServiceFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _sessionMemory = new ConcurrentDictionary<string, Dictionary<string, object>>();
            
            // Initialize MCP client
            var clientTransport = new SseClientTransport(new SseClientTransportOptions()
            {
                Endpoint = new Uri(appSettingsService.AppSettings.McpSettings.BaseUrl),
                Name = "MixDatabaseAgentClient"
            });
            _mcpClient = McpClientFactory.CreateAsync(clientTransport).GetAwaiter().GetResult();
            _chatAgent = chatAgent;
        }
        /// <summary>
        /// Handles a natural language request by classifying intent and routing to the appropriate tool
        /// </summary>
        public async Task<string> DatabaseOperation(
            string userInput,
            string sessionId = DEFAULT_SESSION_ID,
            LLMServiceType llmServiceType = LLMServiceType.DeepSeek,
            string llmModel = "deepseek-chat",
            CancellationToken cancellationToken = default)
        {
            try
            {
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
                        return await CallDatabaseToolAsync("CreateDatabaseFromPrompt", new Dictionary<string, object>
                        {
                            ["displayName"] = displayName,
                            ["schemaDescription"] = schemaDescription,
                            ["mixDatabaseContextId"] = 1,
                            ["llmServiceType"] = llmServiceType,
                            ["llmModel"] = llmModel
                        }, cancellationToken);
                    case DatabaseIntent.AddColumns:
                        if (!parameters.TryGetValue("databaseSystemName", out var dbNameAdd) || string.IsNullOrWhiteSpace(dbNameAdd) ||
                            !parameters.TryGetValue("schemaText", out var schemaTextAdd) || string.IsNullOrWhiteSpace(schemaTextAdd))
                        {
                            _logger.LogWarning("Missing required parameters for AddColumns: {Params}", parameters);
                            return "Missing required parameters for adding columns.";
                        }
                        return await CallDatabaseToolAsync("AddColumnToDatabase", new Dictionary<string, object>
                        {
                            ["databaseSystemName"] = dbNameAdd,
                            ["schemaText"] = schemaTextAdd,
                            ["llmServiceType"] = llmServiceType,
                            ["llmModel"] = llmModel,
                            ["timeoutSeconds"] = DEFAULT_TIMEOUT_SECONDS
                        }, cancellationToken);
                    case DatabaseIntent.UpdateColumns:
                        if (!parameters.TryGetValue("databaseSystemName", out var dbNameUpdate) || string.IsNullOrWhiteSpace(dbNameUpdate) ||
                            !parameters.TryGetValue("schemaText", out var schemaTextUpdate) || string.IsNullOrWhiteSpace(schemaTextUpdate))
                        {
                            _logger.LogWarning("Missing required parameters for UpdateColumns: {Params}", parameters);
                            return "Missing required parameters for updating columns.";
                        }
                        return await CallDatabaseToolAsync("UpdateDatabaseColumn", new Dictionary<string, object>
                        {
                            ["databaseSystemName"] = dbNameUpdate,
                            ["schemaText"] = schemaTextUpdate,
                            ["llmServiceType"] = llmServiceType,
                            ["llmModel"] = llmModel,
                            ["timeoutSeconds"] = DEFAULT_TIMEOUT_SECONDS
                        }, cancellationToken);
                    case DatabaseIntent.DeleteColumns:
                        if (!parameters.TryGetValue("databaseSystemName", out var dbNameDelete) || string.IsNullOrWhiteSpace(dbNameDelete) ||
                            !parameters.TryGetValue("schemaText", out var schemaTextDelete) || string.IsNullOrWhiteSpace(schemaTextDelete))
                        {
                            _logger.LogWarning("Missing required parameters for DeleteColumns: {Params}", parameters);
                            return "Missing required parameters for deleting columns.";
                        }
                        return await CallDatabaseToolAsync("DeleteDatabaseColumn", new Dictionary<string, object>
                        {
                            ["databaseSystemName"] = dbNameDelete,
                            ["schemaText"] = schemaTextDelete,
                            ["llmServiceType"] = llmServiceType,
                            ["llmModel"] = llmModel,
                            ["timeoutSeconds"] = DEFAULT_TIMEOUT_SECONDS,
                            ["confirmation"] = "YES"
                        }, cancellationToken);
                    // Data CRUD operations (requires MixDbDataTool)
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
                    case DatabaseIntent.Unknown:
                        return parameters["response"];
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

        /// <summary>
        /// Helper method to call database tools through MCP client
        /// </summary>
        private async Task<string> CallDatabaseToolAsync(string toolName, Dictionary<string, object> parameters, CancellationToken cancellationToken)
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
        /// Helper to get all supported [McpServerTool] methods and their descriptions from MixDbPromptTool
        /// </summary>
        private static List<(string MethodName, string Description)> GetSupportedPromptToolActions()
        {
            var toolType = typeof(MixDbPromptTool);
            var actions = new List<(string, string)>();
            foreach (var method in toolType.GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public))
            {
                var toolAttr = method.GetCustomAttributes(typeof(ModelContextProtocol.Server.McpServerToolAttribute), false).FirstOrDefault();
                if (toolAttr != null)
                {
                    //var descAttr = method.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false)
                    //    .Cast<System.ComponentModel.DescriptionAttribute>()
                    //    .FirstOrDefault();
                    var parameters = method.GetParameters().Where(m => !m.HasDefaultValue).Select(m => $"{m.Name} ({m.ParameterType})").ToList();
                    actions.Add((method.Name, string.Join(", ", parameters)));
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
            var parameters = new Dictionary<string, string>();
            string prompt = $$"""
                You are an AI assistant.When a user sends a message, decide if you should:
                -Respond directly as a chatbot(for general questions, greetings, small talk, etc.)
                -Or, if the user is asking for a database/ tool operation, classify the request and extract parameters with selected tool parameters.

               You are an AI assistant for a database platform.Classify the user's request into one of these intents:
                { { toolList} }
            User message: "{{userInput}}"

                Respond in this JSON format:
                {
                "type": "chatbot" | "tool",
                    "response": "...", // Only if type is chatbot
                    "action": "...", // Only if type is tool
                    "parameters": { ...} // Only if type is tool
            }
            """;
            var llmService = _llmServiceFactory.CreateService(llmServiceType);
            var response = await llmService.ChatAsync(prompt, llmModel, 0.2, -1, cancellationToken);
            if (string.IsNullOrWhiteSpace(response?.choices?.FirstOrDefault()?.Message?.Content))
                return (DatabaseIntent.Unknown, new Dictionary<string, string>());

            try
            {
                var jsonContent = JsonHelper.ExtractJsonObjectFromText(response?.choices?.FirstOrDefault()?.Message?.Content!);
                if (!jsonContent.IsJsonString())
                {
                    parameters.Add("response", jsonContent);
                    return (DatabaseIntent.Unknown, parameters);
                }
                var doc = JObject.Parse(jsonContent);
                if (!doc.ContainsKey("action"))
                {
                    return (DatabaseIntent.Unknown, doc.ToObject<Dictionary<string, string>>());
                }
                var actionStr = doc.GetValue<string>("action");
                if (doc.TryGetValue("parameters", out var paramElement) && paramElement.Type == JTokenType.Object)
                {
                    foreach (var prop in paramElement.ToObject<JObject>().Properties())
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
                _logger.LogError(ex, "Failed to parse LLM intent classification response: {Data}", response?.choices?.FirstOrDefault()?.Message?.Content);
                return (DatabaseIntent.Unknown, new Dictionary<string, string>());
            }
        }


    }
}
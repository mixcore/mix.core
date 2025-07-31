using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Mix.Database.Services;
using Mix.MCP.Lib.Hubs;
using Mix.MCP.Lib.Models;
using Mix.MCP.Lib.Services.Knowledge;
using Mix.MCP.Lib.Services.LLM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mix.MCP.Lib.Agents
{
    /// <summary>
    /// A chat agent that can maintain conversation context and handle user interactions
    /// </summary>
    public class ChatAgent : BaseAgent
    {
        private const string CONVERSATION_HISTORY_KEY = "conversation_history";
        private const int MAX_HISTORY_LENGTH = 10;

        /// <summary>
        /// Initializes a new instance of the ChatAgent class
        /// </summary>
        public ChatAgent(
            AppSettingsService appSettingsService,
            ILlmServiceFactory llmServiceFactory,
            IHubContext<LLMHub> hubContext,
            ILogger<ChatAgent> logger,
            IKnowledgeBaseService? knowledgeBaseService = null,
            TimeSpan? defaultTimeout = null)
            : base(appSettingsService, llmServiceFactory, hubContext, logger, knowledgeBaseService, defaultTimeout)
        {
        }

        /// <summary>
        /// Processes user input and generates a response while maintaining conversation context
        /// </summary>
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
                _logger.LogInformation("Processing input for session {SessionId}: {UserInput}", sessionId, userInput);

                var memory = GetOrCreateMemory(sessionId);
                var conversationHistory = GetConversationHistory(memory);

                // Add user input to history
                conversationHistory.Add(new LLMMessage { SessionId = sessionId, Data = { Role = "user", Content = userInput } });

                // Prepare the prompt with conversation history (system instructions will be appended by AskAIAsync)
                var prompt = BuildPrompt(conversationHistory, memory, includeSystemInstructions: false);

                // Use AskAIAsync to get response from LLM (system prompts appended automatically)
                var response = await AskAIAsync(
                    prompt,
                    sessionId,
                    serviceType,
                    "deepseek-chat",
                    0.7,
                    -1,
                    cancellationToken);

                if (response?.choices?.FirstOrDefault()?.Message?.Content == null)
                {
                    throw new InvalidOperationException("No valid response received from LLM service");
                }

                var assistantResponse = response.choices.First().Message.Content;

                await NotifyResult(deviceId, new AgentProcessResult(true, assistantResponse));

                // Add assistant response to history
                conversationHistory.Add(new LLMMessage { SessionId = sessionId, Data = { Role = "assistant", Content = assistantResponse } });

                // Update memory with new history
                memory.SetValue(CONVERSATION_HISTORY_KEY, conversationHistory);

                return new AgentProcessResult(true, assistantResponse);
            }
            catch (Exception ex)
            {
                return HandleException(ex, userInput);
            }
        }

        /// <summary>
        /// Gets the conversation history from memory or creates a new one
        /// </summary>
        private List<LLMMessage> GetConversationHistory(AgentMemory memory)
        {
            var history = memory.GetValue<List<LLMMessage>>(CONVERSATION_HISTORY_KEY);
            if (history == null)
            {
                history = new List<LLMMessage>();
                memory.SetValue(CONVERSATION_HISTORY_KEY, history);
            }
            return history;
        }

        /// <summary>
        /// Builds a prompt from the conversation history and appends system instructions if present
        /// </summary>
        private string BuildPrompt(List<LLMMessage> conversationHistory, AgentMemory memory, bool includeSystemInstructions = true)
        {
            var prompt = new System.Text.StringBuilder();

            // Add system message
            prompt.AppendLine("You are a helpful AI assistant. Please respond to the user's message based on the conversation history:");
            prompt.AppendLine();

            // Optionally append system custom instructions
            if (includeSystemInstructions)
            {
                var systemInstructions = memory.GetValue<string>("system_custom_instructions");
                if (!string.IsNullOrWhiteSpace(systemInstructions))
                {
                    prompt.AppendLine("System Instructions:");
                    prompt.AppendLine(systemInstructions);
                    prompt.AppendLine();
                }
            }

            // Add conversation history
            foreach (var message in conversationHistory.TakeLast(MAX_HISTORY_LENGTH))
            {
                prompt.AppendLine($"{message.Data.Role}: {message.Data.Content}");
            }

            return prompt.ToString();
        }
    }
}
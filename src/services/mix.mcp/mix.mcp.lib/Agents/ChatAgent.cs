using Microsoft.Extensions.Logging;
using Mix.MCP.Lib.Models;
using Mix.MCP.Lib.Services.LLM;
using System;
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
            ILlmServiceFactory llmServiceFactory,
            ILogger<ChatAgent> logger,
            TimeSpan? defaultTimeout = null)
            : base(llmServiceFactory, logger, defaultTimeout)
        {
        }

        /// <summary>
        /// Processes user input and generates a response while maintaining conversation context
        /// </summary>
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
                _logger.LogInformation("Processing input for session {SessionId}: {UserInput}", sessionId, userInput);

                var memory = GetOrCreateMemory(sessionId);
                var conversationHistory = GetConversationHistory(memory);

                // Add user input to history
                conversationHistory.Add(new LLMMessage { SessionId = sessionId, Data = { Role = "user", Content = userInput } });

                // Prepare the prompt with conversation history
                var prompt = BuildPrompt(conversationHistory);

                // Get response from LLM
                var llmService = _llmServiceFactory.CreateService(serviceType);
                var response = await llmService.ChatAsync(
                    prompt,
                    "deepseek-chat",
                    0.7,
                    -1,
                    cancellationToken);

                if (response?.choices?.FirstOrDefault()?.Message?.Content == null)
                {
                    throw new InvalidOperationException("No valid response received from LLM service");
                }

                var assistantResponse = response.choices.First().Message.Content;

                // Add assistant response to history
                conversationHistory.Add(new LLMMessage { SessionId = sessionId, Data = { Role = "user", Content = userInput } });

                // Update memory with new history
                memory.SetValue(CONVERSATION_HISTORY_KEY, conversationHistory);

                return assistantResponse;
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
        /// Builds a prompt from the conversation history
        /// </summary>
        private string BuildPrompt(List<LLMMessage> conversationHistory)
        {
            var prompt = new System.Text.StringBuilder();

            // Add system message
            prompt.AppendLine("You are a helpful AI assistant. Please respond to the user's message based on the conversation history:");
            prompt.AppendLine();

            // Add conversation history
            foreach (var message in conversationHistory.TakeLast(MAX_HISTORY_LENGTH))
            {
                prompt.AppendLine($"{message.Data.Role}: {message.Data.Content}");
            }

            return prompt.ToString();
        }
    }


}
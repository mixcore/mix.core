using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Mix.MCP.Lib.Services.LLM
{
    /// <summary>
    /// Service for interacting with OpenAI API
    /// </summary>
    public class OpenAIService : BaseLlmService
    {
        private const string HttpClientName = "OpenAI";

        /// <summary>
        /// Initialize a new instance of OpenAIService
        /// </summary>
        public OpenAIService(
            IHttpClientFactory httpClientFactory,
            ILogger<OpenAIService> logger,
            string apiKey,
            string baseUrl = "https://api.openai.com/v1")
            : base(httpClientFactory, logger, baseUrl, apiKey)
        {
        }

        /// <summary>
        /// Send a chat LLMMessage to OpenAI API
        /// </summary>
        public override async Task<LLMChatResponse> ChatAsync(
            string message,
            string model = "gpt-3.5-turbo",
            double temperature = 0.7,
            int maxTokens = 8000,
            CancellationToken cancellationToken = default)
        {
            var request = new
            {
                model = model,
                messages = new[]
                {
                    new { role = "user", content = message }
                },
                temperature = temperature,
                max_tokens = maxTokens > 0 ? maxTokens : 8000
            };

            var client = CreateHttpClient(HttpClientName);
            return await SendPostRequestAsync<LLMChatResponse>(
                client,
                "chat/completions",
                request,
                cancellationToken);
        }

        /// <summary>
        /// Send a completion request to OpenAI API
        /// </summary>
        public override async Task<LLMCompletionResponse> CompleteAsync(
            string prompt,
            string model = "text-davinci-003",
            double temperature = 0.7,
            int maxTokens = 8000,
            CancellationToken cancellationToken = default)
        {
            var request = new
            {
                model = model,
                prompt = prompt,
                temperature = temperature,
                max_tokens = maxTokens > 0 ? maxTokens : 8000
            };

            var client = CreateHttpClient(HttpClientName);
            return await SendPostRequestAsync<LLMCompletionResponse>(
                client,
                "completions",
                request,
                cancellationToken);
        }

        /// <summary>
        /// CreateMixDbData embeddings using OpenAI API
        /// </summary>
        public override async Task<LLMEmbeddingResponse> CreateEmbeddingsAsync(
            string input,
            string model = "text-embedding-ada-002",
            CancellationToken cancellationToken = default)
        {
            var request = new
            {
                model = model,
                input = input
            };

            var client = CreateHttpClient(HttpClientName);
            return await SendPostRequestAsync<LLMEmbeddingResponse>(
                client,
                "embeddings",
                request,
                cancellationToken);
        }
    }
}
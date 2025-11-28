using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Mix.MCP.Lib.Services.LLM
{
    /// <summary>
    /// Service for interacting with LM Studio API
    /// </summary>
    public class LlmStudioService : BaseLlmService
    {
        private const string HttpClientName = "LmStudio";

        /// <summary>
        /// Initialize a new instance of LmStudioService
        /// </summary>
        public LlmStudioService(
            IHttpClientFactory httpClientFactory,
            ILogger<LlmStudioService> logger,
            string baseUrl = "http://localhost:1234/v1")
            : base(httpClientFactory, logger, baseUrl)
        {
        }

        /// <summary>
        /// Send a chat LLMMessage to LM Studio API
        /// </summary>
        public override async Task<LLMChatResponse> ChatAsync(
            string message,
            string model = "mathstral-7b-v0.1",
            double temperature = 0.7,
            int maxTokens = -1,
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
                max_tokens = maxTokens > 0 ? maxTokens : -1, // Only include if positive
                stream = false
            };

            var client = CreateHttpClient(HttpClientName);
            return await SendPostRequestAsync<LLMChatResponse>(
                client,
                "chat/completions",
                request,
                cancellationToken);
        }

        /// <summary>
        /// Send a completion request to LM Studio API
        /// </summary>
        public override async Task<LLMCompletionResponse> CompleteAsync(
            string prompt,
            string model = "mathstral-7b-v0.1",
            double temperature = 0.7,
            int maxTokens = -1,
            CancellationToken cancellationToken = default)
        {
            var request = new
            {
                model = model,
                prompt = prompt,
                temperature = temperature,
                max_tokens = maxTokens > 0 ? maxTokens : -1
            };

            var client = CreateHttpClient(HttpClientName);
            return await SendPostRequestAsync<LLMCompletionResponse>(
                client,
                "completions",
                request,
                cancellationToken);
        }

        /// <summary>
        /// CreateMixDbData embeddings using LM Studio API
        /// </summary>
        public override async Task<LLMEmbeddingResponse> CreateEmbeddingsAsync(
            string input,
            string model = "text-embedding-nomic-embed-text-v1.5",
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
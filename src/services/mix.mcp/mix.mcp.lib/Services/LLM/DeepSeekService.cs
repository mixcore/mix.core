using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Mix.MCP.Lib.Services.LLM
{
    /// <summary>
    /// Service for interacting with DeepSeek API
    /// </summary>
    public class DeepSeekService : BaseLlmService
    {
        private const string HttpClientName = "DeepSeek";

        /// <summary>
        /// Initialize a new instance of DeepSeekService
        /// </summary>
        public DeepSeekService(
            IHttpClientFactory httpClientFactory,
            ILogger<DeepSeekService> logger,
            string apiKey,
            string baseUrl = "https://api.deepseek.com/v1")
            : base(httpClientFactory, logger, baseUrl, apiKey)
        {
        }

        /// <summary>
        /// Send a chat LLMMessage to DeepSeek API
        /// </summary>
        public override async Task<LLMChatResponse> ChatAsync(
            string message,
            string model = "deepseek-chat",
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
        /// Send a completion request to DeepSeek API
        /// </summary>
        public override async Task<LLMCompletionResponse> CompleteAsync(
            string prompt,
            string model = "deepseek-chat",
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
        /// CreateMixDbData embeddings using DeepSeek API
        /// </summary>
        public override async Task<LLMEmbeddingResponse> CreateEmbeddingsAsync(
            string input,
            string model = "deepseek-embedding",
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
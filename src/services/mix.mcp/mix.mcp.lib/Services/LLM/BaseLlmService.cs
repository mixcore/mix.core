using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;

namespace Mix.MCP.Lib.Services.LLM
{
    /// <summary>
    /// Base class for all LLM services providing common functionality
    /// </summary>
    public abstract class BaseLlmService : ILlmService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        protected readonly ILogger _logger;
        protected readonly string _baseUrl;
        protected readonly string _apiKey;
        protected TimeSpan _timeout = TimeSpan.FromSeconds(3000); // 2 minutes timeout

        protected BaseLlmService(
            IHttpClientFactory httpClientFactory,
            ILogger logger,
            string baseUrl,
            string apiKey = null)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _baseUrl = baseUrl;
            _apiKey = apiKey;
        }

        /// <summary>
        /// CreateMixDbData and configure an HttpClient with the proper timeout and headers
        /// </summary>
        protected HttpClient CreateHttpClient(string clientName)
        {
            var client = _httpClientFactory.CreateClient(clientName);
            client.Timeout = _timeout;
            client.BaseAddress = new Uri(_baseUrl);
            if (!string.IsNullOrEmpty(_apiKey))
            {
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
            }

            return client;
        }

        /// <summary>
        /// Send LLMMessage to chat API
        /// </summary>
        public abstract Task<LLMChatResponse> ChatAsync(
            string message,
            string model,
            double temperature = 0.7,
            int maxTokens = -1,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Send prompt to completion API
        /// </summary>
        public abstract Task<LLMCompletionResponse> CompleteAsync(
            string prompt,
            string model,
            double temperature = 0.7,
            int maxTokens = -1,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// CreateMixDbData embeddings for the input
        /// </summary>
        public abstract Task<LLMEmbeddingResponse> CreateEmbeddingsAsync(
            string input,
            string model,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Send a POST request to the specified endpoint
        /// </summary>
        protected async Task<T> SendPostRequestAsync<T>(
            HttpClient client,
            string endpoint,
            object data,
            CancellationToken cancellationToken)
        {
            try
            {
                var content = new StringContent(
                    JsonSerializer.Serialize(data),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await client.PostAsync(
                    $"{_baseUrl.TrimEnd('/')}/{endpoint.TrimStart('/')}",
                    content,
                    cancellationToken
                );

                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                return JsonSerializer.Deserialize<T>(responseContent, options);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending request to {Endpoint}: {LLMMessage}", endpoint, ex.Message);
                throw;
            }
        }

        public void SetTimeout(TimeSpan timeSpan)
        {
            this._timeout = timeSpan;
        }
    }

    /// <summary>
    /// Chat response from LLM service
    /// </summary>
    public class LLMChatResponse
    {
        [JsonPropertyName("choices")]
        public LLMChatChoice[] choices { get; set; } = Array.Empty<LLMChatChoice>();
    }

    /// <summary>
    /// Single chat choice
    /// </summary>
    public class LLMChatChoice
    {
        [JsonPropertyName("message")]
        public LLMChatMessage Message { get; set; }
    }

    public class LLMChatMessage
    {
        [JsonPropertyName("role")]
        public string Role { get; set; }
        [JsonPropertyName("content")]
        public string Content { get; set; }
    }

    /// <summary>
    /// Completion response from LLM service
    /// </summary>
    public class LLMCompletionResponse
    {
        // TODO: Check response confidence

        [JsonPropertyName("choices")]
        public CompletionChoice[] choices { get; set; } = Array.Empty<CompletionChoice>();
    }

    /// <summary>
    /// Single completion choice
    /// </summary>
    public class CompletionChoice
    {
        [JsonPropertyName("text")]
        public string text { get; set; }
    }

    /// <summary>
    /// Embedding response from LLM service
    /// </summary>
    public class LLMEmbeddingResponse
    {
        [JsonPropertyName("data")]
        public EmbeddingData[] data { get; set; } = Array.Empty<EmbeddingData>();
    }

    /// <summary>
    /// Single embedding data
    /// </summary>
    public class EmbeddingData
    {
        [JsonPropertyName("embedding")]
        public float[] embedding { get; set; }
    }
}
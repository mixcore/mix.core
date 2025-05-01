using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;

namespace Mix.MCP.Lib.Services.LLM
{
    public class LlmStudioService : ILlmService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly ILogger<LlmStudioService> _logger;

        public LlmStudioService(ILogger<LlmStudioService> logger, string baseUrl = "http://localhost:1234/v1")
        {
            _httpClient = new HttpClient();
            _baseUrl = baseUrl;
            _logger = logger;
        }

        public async Task<LLMChatResponse> ChatAsync(string message, string model = "mathstral-7b-v0.1", double temperature = 0.7, int maxTokens = -1, CancellationToken cancellationToken = default)
        {
            try
            {
                var request = new
                {
                    model = model,
                    messages = new[]
                    {
                        new { role = "user", content = message }
                    },
                    temperature = temperature,
                    max_tokens = maxTokens,
                    stream = false
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(request),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync($"{_baseUrl}/chat/completions", content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<LLMChatResponse>(responseContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ChatAsync");
                throw;
            }
        }

        public async Task<LLMCompletionResponse> CompleteAsync(string prompt, string model = "mathstral-7b-v0.1", double temperature = 0.7, int maxTokens = -1, CancellationToken cancellationToken = default)
        {
            try
            {
                var request = new
                {
                    model = model,
                    prompt = prompt,
                    temperature = temperature,
                    max_tokens = maxTokens
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(request),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync($"{_baseUrl}/completions", content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<LLMCompletionResponse>(responseContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CompleteAsync");
                throw;
            }
        }

        public async Task<LLMEmbeddingResponse> CreateEmbeddingsAsync(string input, string model = "text-embedding-nomic-embed-text-v1.5", CancellationToken cancellationToken = default)
        {
            try
            {
                var request = new
                {
                    model = model,
                    input = input
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(request),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PostAsync($"{_baseUrl}/embeddings", content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<LLMEmbeddingResponse>(responseContent);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in CreateEmbeddingsAsync");
                throw;
            }
        }
    }

    public class LLMChatResponse
    {
        public ChatResponse[] choices { get; set; }
    }

    public class ChatChoice
    {
        public ChatMessage message { get; set; }
    }

    public class LLMCompletionResponse
    {
        public CompletionChoice[] choices { get; set; }
    }

    public class CompletionChoice
    {
        public string text { get; set; }
    }

    public class LLMEmbeddingResponse
    {
        public EmbeddingData[] data { get; set; }
    }

    public class EmbeddingData
    {
        public float[] embedding { get; set; }
    }
} 
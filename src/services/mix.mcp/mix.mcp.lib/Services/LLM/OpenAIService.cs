using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Mix.MCP.Lib.Services.LLM
{
    public class OpenAIService : ILlmService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl;
        private readonly ILogger<OpenAIService> _logger;

        public OpenAIService(ILogger<OpenAIService> logger, string apiKey, string baseUrl = "https://api.openai.com/v1")
        {
            _httpClient = new HttpClient();
            _apiKey = apiKey;
            _baseUrl = baseUrl;
            _logger = logger;

            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");
        }

        public async Task<LLMChatResponse> ChatAsync(string message, string model = "gpt-3.5-turbo", double temperature = 0.7, int maxTokens = -1, CancellationToken cancellationToken = default)
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
                    max_tokens = maxTokens
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

        public async Task<LLMCompletionResponse> CompleteAsync(string prompt, string model = "text-davinci-003", double temperature = 0.7, int maxTokens = -1, CancellationToken cancellationToken = default)
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

        public async Task<LLMEmbeddingResponse> CreateEmbeddingsAsync(string input, string model = "text-embedding-ada-002", CancellationToken cancellationToken = default)
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
} 
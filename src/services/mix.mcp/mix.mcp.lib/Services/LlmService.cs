using Microsoft.Extensions.Logging;
using Mix.MCP.Lib.Models.LLM;
using Mix.Shared.Models;
using Mix.Shared.Services;
using ModelContextProtocol.Server;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mix.MCP.Lib.Services
{
    /// <summary>
    /// Implementation of the service for communicating with large language models
    /// </summary>
    public class LlmService : ILlmService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<LlmService> _logger;
        private readonly IMcpServer _server;
        private readonly HttpService _httpService;
        public LlmService(IHttpClientFactory httpClientFactory, IMcpServer server, ILogger<LlmService> logger, HttpService httpService)
        {
            _server = server;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _httpService = httpService;
        }

        /// <inheritdoc />
        public async Task<string> ChatWithOpenAIAsync(
            string apiKey,
            string message,
            string model = "gpt-4o",
            float temperature = 0.7f,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ChatGPT");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                var request = new
                {
                    model,
                    messages = new[]
                    {
                    new { role = "user", content = message }
                },
                    temperature
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(request),
                    Encoding.UTF8,
                    "application/json");

                var response = await client.PostAsync("chat/completions", content, cancellationToken);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
                var result = JsonSerializer.Deserialize<OpenAIResponse>(responseBody);

                return result?.Choices?[0]?.Message?.Content ?? "No response";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling OpenAI API: {Message}", ex.Message);
                throw new Exception($"Error calling OpenAI API: {ex.Message}", ex);
            }
        }

        /// <inheritdoc />
        public async Task<string> ChatWithDeepseekAsync(
            string apiKey,
            string message,
            string model = "deepseek-chat",
            float temperature = 0.7f,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var request = new HttpRequestModel()
                {
                    RequestUrl = $"https://api.deepseek.com/chat/completions",
                    Method = "POST",
                    BearerToken = $"Bearer {apiKey}",
                    Body = JObject.FromObject(new
                    {
                        model,
                        messages = new[]
                        {
                            new { role = "user", content = message }
                        },
                        temperature
                    }),
                };

                var response = await _httpService.SendHttpRequestModel(request, cancellationToken);
                return response?.ToObject<DeepseekResponse>()?.Choices?[0]?.Message?.Content ?? "No response";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling Deepseek API: {Message}", ex.Message);
                throw new Exception($"Error calling Deepseek API: {ex.Message}", ex);
            }
        }
    }

}

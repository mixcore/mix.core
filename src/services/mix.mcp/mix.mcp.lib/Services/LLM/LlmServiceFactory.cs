using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;
using Mix.Shared.Services;
using Microsoft.Extensions.Configuration;

namespace Mix.MCP.Lib.Services.LLM
{
    /// <summary>
    /// Factory interface for creating LLM services
    /// </summary>
    public interface ILlmServiceFactory
    {
        /// <summary>
        /// CreateMixDbData an LLM service for the specified service type
        /// </summary>
        ILlmService CreateService(LLMServiceType serviceType);
    }

    /// <summary>
    /// Factory implementation for creating LLM services
    /// </summary>
    public class LlmServiceFactory : ILlmServiceFactory
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly LlmServiceOptions? _options;
        private readonly IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// Initialize a new instance of LlmServiceFactory
        /// </summary>
        public LlmServiceFactory(
            IConfiguration configuration,
            ILoggerFactory loggerFactory,
            IHttpClientFactory httpClientFactory)
        {
            _loggerFactory = loggerFactory;
            _httpClientFactory = httpClientFactory;
            _options = configuration.GetSection("LlmServiceOptions").Get<LlmServiceOptions>()
                ?? new();
        }

        /// <summary>
        /// CreateMixDbData an LLM service for the specified service type
        /// </summary>
        public ILlmService CreateService(LLMServiceType serviceType)
        {
            return serviceType switch
            {
                LLMServiceType.LmStudio => new LlmStudioService(
                    _httpClientFactory,
                    _loggerFactory.CreateLogger<LlmStudioService>(),
                    _options.LmStudio.BaseUrl),

                LLMServiceType.OpenAI => new OpenAIService(
                    _httpClientFactory,
                    _loggerFactory.CreateLogger<OpenAIService>(),
                    _options.OpenAI.ApiKey,
                    _options.OpenAI.BaseUrl),

                LLMServiceType.DeepSeek => new DeepSeekService(
                    _httpClientFactory,
                    _loggerFactory.CreateLogger<DeepSeekService>(),
                    _options.DeepSeek.ApiKey,
                    _options.DeepSeek.BaseUrl),

                _ => throw new ArgumentException($"Unsupported service type: {serviceType}")
            };
        }
    }

    /// <summary>
    /// Options for configuring LLM services
    /// </summary>
    public class LlmServiceOptions
    {
        /// <summary>
        /// Base URL for LM Studio API
        /// </summary>
        public LmStudioServiceOptions LmStudio { get; set; }

        /// <summary>
        /// API key for OpenAI
        /// </summary>
        public OpenAIServiceOptions OpenAI { get; set; }

        /// <summary>
        /// API key for DeepSeek
        /// </summary>
        public DeepSeekServiceOptions DeepSeek { get; set; }

        public int DefaultTimeoutSeconds { get; set; } = 120;
    }

    public class LmStudioServiceOptions : ILLMServiceOptions
    {
        public string Model { get; set; } = "mathstral-7b-v0.1";
        public double Temperature { get; set; } = 0.7;
        public int MaxTokens { get; set; } = -1;
        public string BaseUrl { get; set; }
        public string ApiKey { get; set; }
        public int TimeoutSeconds { get; set; } = 300;
    }

    public class OpenAIServiceOptions : ILLMServiceOptions
    {
        public string Model { get; set; } = "gpt-3.5-turbo";
        public double Temperature { get; set; } = 0.7;
        public int MaxTokens { get; set; } = 8000;
        public string BaseUrl { get; set; }
        public string ApiKey { get; set; }
        public int TimeoutSeconds { get; set; } = 300;
    }

    public class DeepSeekServiceOptions : ILLMServiceOptions
    {
        public string Model { get; set; } = "deepseek-chat";
        public double Temperature { get; set; } = 0.7;
        public int MaxTokens { get; set; } = 8000;
        public string BaseUrl { get; set; }
        public string ApiKey { get; set; }
        public int TimeoutSeconds { get; set; } = 300;
    }

    public interface ILLMServiceOptions
    {
        public string BaseUrl { get; set; }
        public string ApiKey { get; set; }
        public string Model { get; set; }
        public double Temperature { get; set; }
        public int MaxTokens { get; set; }
        public int TimeoutSeconds { get; set; }
    }
}
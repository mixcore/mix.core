using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace Mix.MCP.Lib.Services.LLM
{
    /// <summary>
    /// Factory interface for creating LLM services
    /// </summary>
    public interface ILlmServiceFactory
    {
        /// <summary>
        /// Create an LLM service for the specified service type
        /// </summary>
        ILlmService CreateService(LLMServiceType serviceType);
    }

    /// <summary>
    /// Factory implementation for creating LLM services
    /// </summary>
    public class LlmServiceFactory : ILlmServiceFactory
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly LlmServiceOptions _options;
        private readonly IHttpClientFactory _httpClientFactory;

        /// <summary>
        /// Initialize a new instance of LlmServiceFactory
        /// </summary>
        public LlmServiceFactory(
            ILoggerFactory loggerFactory,
            IHttpClientFactory httpClientFactory,
            IOptions<LlmServiceOptions> options)
        {
            _loggerFactory = loggerFactory;
            _httpClientFactory = httpClientFactory;
            _options = options.Value;
        }

        /// <summary>
        /// Create an LLM service for the specified service type
        /// </summary>
        public ILlmService CreateService(LLMServiceType serviceType)
        {
            return serviceType switch
            {
                LLMServiceType.LmStudio => new LlmStudioService(
                    _httpClientFactory,
                    _loggerFactory.CreateLogger<LlmStudioService>(),
                    _options.LmStudioBaseUrl),

                LLMServiceType.OpenAI => new OpenAIService(
                    _httpClientFactory,
                    _loggerFactory.CreateLogger<OpenAIService>(),
                    _options.OpenAIApiKey,
                    _options.OpenAIBaseUrl),

                LLMServiceType.DeepSeek => new DeepSeekService(
                    _httpClientFactory,
                    _loggerFactory.CreateLogger<DeepSeekService>(),
                    _options.DeepSeekApiKey,
                    _options.DeepSeekBaseUrl),

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
        public string LmStudioBaseUrl { get; set; } = "http://localhost:1234/v1";
        
        /// <summary>
        /// API key for OpenAI
        /// </summary>
        public string OpenAIApiKey { get; set; }
        
        /// <summary>
        /// Base URL for OpenAI API
        /// </summary>
        public string OpenAIBaseUrl { get; set; } = "https://api.openai.com/v1";
        
        /// <summary>
        /// API key for DeepSeek
        /// </summary>
        public string DeepSeekApiKey { get; set; }
        
        /// <summary>
        /// Base URL for DeepSeek API
        /// </summary>
        public string DeepSeekBaseUrl { get; set; } = "https://api.deepseek.com/v1";
        
        /// <summary>
        /// Default timeout for LLM requests in seconds
        /// </summary>
        public int DefaultTimeoutSeconds { get; set; } = 120;
    }
} 
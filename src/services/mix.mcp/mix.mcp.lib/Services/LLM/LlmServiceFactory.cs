using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Mix.MCP.Lib.Services.LLM
{
    public interface ILlmServiceFactory
    {
        ILlmService CreateService(LLMServiceType serviceType);
    }

    public class LlmServiceFactory : ILlmServiceFactory
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly LlmServiceOptions _options;

        public LlmServiceFactory(ILoggerFactory loggerFactory, IOptions<LlmServiceOptions> options)
        {
            _loggerFactory = loggerFactory;
            _options = options.Value;
        }

        public ILlmService CreateService(LLMServiceType serviceType)
        {
            return serviceType switch
            {
                LLMServiceType.LmStudio => new LlmStudioService(
                    _loggerFactory.CreateLogger<LlmStudioService>(),
                    _options.LmStudioBaseUrl),

                LLMServiceType.OpenAI => new OpenAIService(
                    _loggerFactory.CreateLogger<OpenAIService>(),
                    _options.OpenAIApiKey,
                    _options.OpenAIBaseUrl),

                LLMServiceType.DeepSeek => new DeepSeekService(
                    _loggerFactory.CreateLogger<DeepSeekService>(),
                    _options.DeepSeekApiKey,
                    _options.DeepSeekBaseUrl),

                _ => throw new ArgumentException($"Unsupported service type: {serviceType}")
            };
        }
    }

    public class LlmServiceOptions
    {
        public string LmStudioBaseUrl { get; set; } = "http://localhost:1234/v1";
        public string OpenAIApiKey { get; set; }
        public string OpenAIBaseUrl { get; set; } = "https://api.openai.com/v1";
        public string DeepSeekApiKey { get; set; }
        public string DeepSeekBaseUrl { get; set; } = "https://api.deepseek.com/v1";
    }
} 
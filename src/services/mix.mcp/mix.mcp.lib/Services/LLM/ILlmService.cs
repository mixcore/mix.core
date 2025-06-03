using System.Threading.Tasks;

namespace Mix.MCP.Lib.Services.LLM
{
    /// <summary>
    /// Service for communicating with large language models
    /// </summary>
    public interface ILlmService
    {
        Task<LLMChatResponse> ChatAsync(string message, string model, double temperature = 0.7, int maxTokens = -1, CancellationToken cancellationToken = default);
        Task<LLMCompletionResponse> CompleteAsync(string prompt, string model, double temperature = 0.7, int maxTokens = -1, CancellationToken cancellationToken = default);
        Task<LLMEmbeddingResponse> CreateEmbeddingsAsync(string input, string model, CancellationToken cancellationToken = default);
        void SetTimeout(TimeSpan timeSpan);
    }
}
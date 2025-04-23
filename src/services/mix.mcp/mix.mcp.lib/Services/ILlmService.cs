using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.MCP.Lib.Services
{
    /// <summary>
    /// Service for communicating with large language models
    /// </summary>
    public interface ILlmService
    {
        /// <summary>
        /// Send a message to ChatGPT and receive a response
        /// </summary>
        /// <param name="apiKey">OpenAI API key</param>
        /// <param name="model">Model name (default: gpt-4o)</param>
        /// <param name="message">Message content</param>
        /// <param name="temperature">Creativity level (0.0-2.0)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response from ChatGPT</returns>
        Task<string> ChatWithOpenAIAsync(
            string apiKey,
            string message,
            string model = "gpt-4o",
            float temperature = 0.7f,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Send a message to Deepseek and receive a response
        /// </summary>
        /// <param name="apiKey">Deepseek API key</param>
        /// <param name="model">Model name (default: deepseek-chat)</param>
        /// <param name="message">Message content</param>
        /// <param name="temperature">Creativity level (0.0-1.0)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response from Deepseek</returns>
        Task<string> ChatWithDeepseekAsync(
            string apiKey,
            string message,
            string model = "deepseek-chat",
            float temperature = 0.7f,
            CancellationToken cancellationToken = default);
    }
}

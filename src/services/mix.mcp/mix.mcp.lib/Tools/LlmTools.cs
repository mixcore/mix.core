using Microsoft.Extensions.Logging;
using Mix.MCP.Lib.Services;
using ModelContextProtocol.Server;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Mix.MCP.Lib.Tools
{
    /// <summary>
    /// Tools for working with large language models
    /// </summary>
    [McpServerToolType]
    public class LlmTools
    {
        private readonly ILlmService _llmService;
        private readonly ILogger<LlmTools> _logger;

        public LlmTools(ILlmService llmService, ILogger<LlmTools> logger)
        {
            _llmService = llmService;
            _logger = logger;
        }

        /// <summary>
        /// Generate code based on requirements
        /// </summary>
        [McpServerTool, Description("Generate code based on requirements")]
        public async Task<string> GenerateCodeAsync(
            [Description("Requirements for code generation")] string requirements,
            [Description("Programming language to use")] string programmingLanguage = "C#",
            CancellationToken cancellationToken = default)
        {
            try
            {
                var prompt = $"""
                Please generate code in {programmingLanguage} based on the following requirements:
                {requirements}

                Requirements for the generated code:
                1. Must be complete and executable
                2. Must include necessary import statements
                3. Must include error handling
                4. Must follow best practices for {programmingLanguage}
                5. Must include comments explaining the code
                6. Must be well-structured and easy to read
                """;

                return await _llmService.ChatWithOpenAIAsync(
                    Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? throw new InvalidOperationException("OPENAI_API_KEY not found"),
                    prompt,
                    cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating code: {Message}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Generate documentation for code
        /// </summary>
        [McpServerTool, Description("Generate documentation for code")]
        public async Task<string> GenerateDocumentationAsync(
            [Description("Code to document")] string code,
            [Description("Type of documentation to generate")] string documentationType = "XML",
            CancellationToken cancellationToken = default)
        {
            try
            {
                var prompt = $"""
                Please generate {documentationType} documentation for the following code:
                {code}

                Requirements for the documentation:
                1. Must be complete and detailed
                2. Must include descriptions for all classes, methods, and properties
                3. Must include examples if necessary
                4. Must follow the {documentationType} documentation standard
                5. Must be well-structured and easy to read
                """;

                return await _llmService.ChatWithOpenAIAsync(
                    Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? throw new InvalidOperationException("OPENAI_API_KEY not found"),
                    prompt,
                    cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating documentation: {Message}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Generate unit tests for code
        /// </summary>
        [McpServerTool, Description("Generate unit tests for code")]
        public async Task<string> GenerateUnitTestsAsync(
            [Description("Code to generate tests for")] string code,
            [Description("Testing framework to use")] string testingFramework = "xUnit",
            CancellationToken cancellationToken = default)
        {
            try
            {
                var prompt = $"""
                Please generate unit tests using {testingFramework} for the following code:
                {code}

                Requirements for the tests:
                1. Must cover all important test cases
                2. Must include necessary setup and teardown
                3. Must include assertions
                4. Must follow best practices for {testingFramework}
                5. Must be well-structured and easy to read
                6. Must include comments explaining the tests
                """;

                return await _llmService.ChatWithOpenAIAsync(
                    Environment.GetEnvironmentVariable("OPENAI_API_KEY") ?? throw new InvalidOperationException("OPENAI_API_KEY not found"),
                    prompt,
                    cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating unit tests: {Message}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Send a message to ChatGPT and get response
        /// </summary>
        [McpServerTool, Description("Send message to ChatGPT and get response")]
        public async Task<string> ChatWithOpenAI(
            [Description("OpenAI API key")] string apiKey,
            [Description("Message to send")] string message,
            [Description("ChatGPT model (default: gpt-4o)")] string model = "gpt-4o",
            [Description("Creativity level (0.0-2.0)")] float temperature = 0.7f,
            CancellationToken cancellationToken = default)
        {
            return await _llmService.ChatWithOpenAIAsync(apiKey, message, model, temperature, cancellationToken);
        }

        /// <summary>
        /// Send a message to Deepseek and get response
        /// </summary>
        [McpServerTool, Description("Send message to Deepseek and get response")]
        public async Task<string> ChatWithDeepseek(
            [Description("Deepseek API key")] string apiKey,
            [Description("Message to send")] string message,
            [Description("Deepseek model (default: deepseek-chat)")] string model = "deepseek-chat",
            [Description("Creativity level (0.0-1.0)")] float temperature = 0.7f,
            CancellationToken cancellationToken = default)
        {
            return await _llmService.ChatWithDeepseekAsync(apiKey, message, model, temperature, cancellationToken);
        }
    }
}

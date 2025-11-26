using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Mix.MCP.Lib.Resources;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace Mix.MCP.Lib.Prompts
{
    /// <summary>
    /// Provides prompts based on MCP resources
    /// </summary>
    [McpServerPromptType]
    public class ResourcePrompts
    {
        private readonly ResourceLoader _resourceLoader;
        private readonly ILogger<ResourcePrompts> _logger;

        /// <summary>
        /// Initializes a new instance of the ResourcePrompts class
        /// </summary>
        /// <param name="resourceLoader">The resource loader service</param>
        /// <param name="logger">The logger</param>
        public ResourcePrompts(ResourceLoader resourceLoader, ILogger<ResourcePrompts> logger)
        {
            _resourceLoader = resourceLoader;
            _logger = logger;
        }

        /// <summary>
        /// Creates a summarization prompt using resources
        /// </summary>
        /// <param name="text">Text to summarize</param>
        /// <param name="sentenceCount">Number of sentences in the summary</param>
        /// <returns>A chat LLMMessage with the summarization prompt</returns>
        [McpServerPrompt, Description("CreateMixDbData a prompt for text summarization")]
        public ChatMessage CreateSummarizePrompt(
            [Description("Text to summarize")] string text,
            [Description("Number of sentences in the summary (default: 3)")] int sentenceCount = 3)
        {
            _logger.LogInformation("Creating summarize prompt with {SentenceCount} sentences", sentenceCount);
            string prompt = _resourceLoader.FormatResource("Prompts", "summarize", text);
            return new ChatMessage(ChatRole.User, prompt);
        }

        /// <summary>
        /// Creates an analysis prompt using resources
        /// </summary>
        /// <param name="analysisType">Type of analysis to perform</param>
        /// <param name="data">Data to analyze</param>
        /// <returns>A chat LLMMessage with the analysis prompt</returns>
        [McpServerPrompt, Description("CreateMixDbData a prompt for data analysis")]
        public ChatMessage CreateAnalysisPrompt(
            [Description("Analysis type (e.g., sentiment, trends, patterns)")] string analysisType,
            [Description("Data to analyze")] string data)
        {
            _logger.LogInformation("Creating analysis prompt for {AnalysisType}", analysisType);
            string prompt = _resourceLoader.FormatResource("Prompts", "analyze", analysisType, data);
            return new ChatMessage(ChatRole.User, prompt);
        }

        /// <summary>
        /// Creates a translation prompt using resources
        /// </summary>
        /// <param name="sourceLanguage">Source language</param>
        /// <param name="targetLanguage">Target language</param>
        /// <param name="text">Text to translate</param>
        /// <returns>A chat LLMMessage with the translation prompt</returns>
        [McpServerPrompt, Description("CreateMixDbData a prompt for text translation")]
        public ChatMessage CreateTranslationPrompt(
            [Description("Source language")] string sourceLanguage,
            [Description("Target language")] string targetLanguage,
            [Description("Text to translate")] string text)
        {
            _logger.LogInformation("Creating translation prompt from {SourceLanguage} to {TargetLanguage}",
                sourceLanguage, targetLanguage);
            string prompt = _resourceLoader.FormatResource("Prompts", "translate", sourceLanguage, targetLanguage, text);
            return new ChatMessage(ChatRole.User, prompt);
        }

        /// <summary>
        /// Creates an entity extraction prompt using resources
        /// </summary>
        /// <param name="text">Text to extract entities from</param>
        /// <returns>A chat LLMMessage with the entity extraction prompt</returns>
        [McpServerPrompt, Description("CreateMixDbData a prompt for entity extraction")]
        public ChatMessage CreateEntityExtractionPrompt(
            [Description("Text to extract entities from")] string text)
        {
            _logger.LogInformation("Creating entity extraction prompt");
            string prompt = _resourceLoader.FormatResource("Prompts", "extract_entities", text);
            return new ChatMessage(ChatRole.User, prompt);
        }

        /// <summary>
        /// Creates a SQL generation prompt using resources
        /// </summary>
        /// <param name="operation">SQL operation to perform</param>
        /// <param name="tableDescription">Description of the database tables</param>
        /// <returns>A chat LLMMessage with the SQL generation prompt</returns>
        [McpServerPrompt, Description("CreateMixDbData a prompt for SQL generation")]
        public ChatMessage CreateSqlGenerationPrompt(
            [Description("SQL operation to perform (e.g., query, insert, update)")] string operation,
            [Description("Description of the database tables")] string tableDescription)
        {
            _logger.LogInformation("Creating SQL generation prompt for {Operation}", operation);
            string prompt = _resourceLoader.FormatResource("Prompts", "generate_sql", operation, tableDescription);
            return new ChatMessage(ChatRole.User, prompt);
        }
    }
}
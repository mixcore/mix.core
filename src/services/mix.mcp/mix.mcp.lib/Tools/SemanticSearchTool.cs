using Microsoft.Extensions.Logging;
using Mix.MCP.Lib.Services.Search;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Mix.MCP.Lib.Tools
{
    /// <summary>
    /// MCP tool for semantic search operations
    /// </summary>
    [McpServerToolType]
    public class SemanticSearchTool
    {
        private readonly ISemanticSearchService _searchService;
        private readonly ILogger<SemanticSearchTool> _logger;

        public SemanticSearchTool(ISemanticSearchService searchService, ILogger<SemanticSearchTool> logger)
        {
            _searchService = searchService ?? throw new ArgumentNullException(nameof(searchService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Performs semantic search for documents and content
        /// </summary>
        /// <param name="query">Search query</param>
        /// <param name="maxResults">Maximum number of results to return</param>
        /// <param name="threshold">Similarity threshold (0.0 to 1.0)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>JSON formatted search results</returns>
        [McpServerTool]
        [Description("Performs semantic search for documents and content based on meaning and context")]
        public async Task<string> SearchAsync(
            [Description("The search query or question")] string query,
            [Description("Maximum number of results to return (default: 5)")] int maxResults = 5,
            [Description("Similarity threshold from 0.0 to 1.0 (default: 0.7)")] double threshold = 0.7,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                {
                    return JsonSerializer.Serialize(new { error = "Query cannot be empty" });
                }

                if (maxResults <= 0 || maxResults > 50)
                {
                    maxResults = 5;
                }

                if (threshold < 0.0 || threshold > 1.0)
                {
                    threshold = 0.7;
                }

                _logger.LogInformation("Performing semantic search with query: {Query}, maxResults: {MaxResults}, threshold: {Threshold}",
                    query, maxResults, threshold);

                var results = await _searchService.SearchAsync(query, maxResults, threshold, cancellationToken);

                var response = new
                {
                    query = query,
                    totalResults = results.Count(),
                    threshold = threshold,
                    results = results.Select(r => new
                    {
                        id = r.Id,
                        title = r.Title,
                        snippet = r.Snippet,
                        category = r.Category,
                        source = r.Source,
                        score = Math.Round(r.Score, 3),
                        metadata = r.Metadata
                    }).ToArray()
                };

                return JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error performing semantic search for query: {Query}", query);
                return JsonSerializer.Serialize(new { error = $"Search failed: {ex.Message}" });
            }
        }

        /// <summary>
        /// Finds documents similar to the provided text
        /// </summary>
        /// <param name="text">Text to find similar documents for</param>
        /// <param name="maxResults">Maximum number of similar documents to return</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>JSON formatted similar documents</returns>
        [McpServerTool]
        [Description("Finds documents similar to the provided text content")]
        public async Task<string> FindSimilarAsync(
            [Description("Text content to find similar documents for")] string text,
            [Description("Maximum number of similar documents to return (default: 3)")] int maxResults = 3,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(text))
                {
                    return JsonSerializer.Serialize(new { error = "Text cannot be empty" });
                }

                if (maxResults <= 0 || maxResults > 20)
                {
                    maxResults = 3;
                }

                _logger.LogInformation("Finding similar documents for text (length: {Length}), maxResults: {MaxResults}",
                    text.Length, maxResults);

                var results = await _searchService.FindSimilarAsync(text, maxResults, cancellationToken);

                var response = new
                {
                    inputTextLength = text.Length,
                    totalResults = results.Count(),
                    results = results.Select(r => new
                    {
                        id = r.Id,
                        title = r.Title,
                        snippet = r.Snippet,
                        category = r.Category,
                        source = r.Source,
                        similarity = Math.Round(r.Score, 3)
                    }).ToArray()
                };

                return JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error finding similar documents");
                return JsonSerializer.Serialize(new { error = $"Similar search failed: {ex.Message}" });
            }
        }

        /// <summary>
        /// Indexes a new document for semantic search
        /// </summary>
        /// <param name="title">Document title</param>
        /// <param name="content">Document content</param>
        /// <param name="category">Document category</param>
        /// <param name="source">Document source</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>JSON formatted indexing result</returns>
        [McpServerTool]
        [Description("Indexes a new document for semantic search")]
        public async Task<string> IndexDocumentAsync(
            [Description("Document title")] string title,
            [Description("Document content")] string content,
            [Description("Document category (optional)")] string category = "",
            [Description("Document source (optional)")] string source = "user",
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(content))
                {
                    return JsonSerializer.Serialize(new { error = "Title and content are required" });
                }

                var document = new SearchDocument
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = title,
                    Content = content,
                    Category = category ?? "",
                    Source = source ?? "user",
                    CreatedAt = DateTime.UtcNow
                };

                _logger.LogInformation("Indexing document: {Title} (category: {Category})", title, category);

                await _searchService.IndexDocumentAsync(document, cancellationToken);

                var response = new
                {
                    success = true,
                    documentId = document.Id,
                    title = document.Title,
                    category = document.Category,
                    indexedAt = document.CreatedAt
                };

                return JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error indexing document: {Title}", title);
                return JsonSerializer.Serialize(new { error = $"Indexing failed: {ex.Message}" });
            }
        }
    }
}
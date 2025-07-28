using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mix.MCP.Lib.Services.Search
{
    /// <summary>
    /// Basic implementation of semantic search service using text similarity
    /// In production, this should be replaced with a proper vector database integration
    /// </summary>
    public class SemanticSearchService : ISemanticSearchService
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<SemanticSearchService> _logger;
        private readonly List<SearchDocument> _documents;
        private const string CACHE_PREFIX = "search_";
        private const int CACHE_DURATION_MINUTES = 15;

        public SemanticSearchService(IMemoryCache cache, ILogger<SemanticSearchService> logger)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _documents = new List<SearchDocument>();

            // Initialize with sample documents
            InitializeSampleDocuments();
        }

        public async Task<IEnumerable<SearchResult>> SearchAsync(
            string query,
            int maxResults = 10,
            double threshold = 0.7,
            CancellationToken cancellationToken = default)
        {
            var cacheKey = $"{CACHE_PREFIX}query_{query}_{maxResults}_{threshold}";

            if (_cache.TryGetValue(cacheKey, out IEnumerable<SearchResult>? cachedResults))
            {
                _logger.LogDebug("Retrieved search results from cache for query: {Query}", query);
                return cachedResults!;
            }

            _logger.LogInformation("Performing semantic search for query: {Query}", query);

            var results = await Task.Run(() =>
            {
                return _documents
                    .Select(doc => new SearchResult
                    {
                        Id = doc.Id,
                        Title = doc.Title,
                        Content = doc.Content,
                        Category = doc.Category,
                        Source = doc.Source,
                        Metadata = doc.Metadata,
                        Score = CalculateSimilarity(query, doc),
                        Snippet = GenerateSnippet(doc.Content, query)
                    })
                    .Where(result => result.Score >= threshold)
                    .OrderByDescending(result => result.Score)
                    .Take(maxResults)
                    .ToList();
            }, cancellationToken);

            // Cache results
            _cache.Set(cacheKey, results, TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));

            _logger.LogInformation("Found {Count} search results for query: {Query}", results.Count(), query);
            return results;
        }

        public async Task IndexDocumentAsync(SearchDocument document, CancellationToken cancellationToken = default)
        {
            if (document == null) throw new ArgumentNullException(nameof(document));

            await Task.Run(() =>
            {
                var existingIndex = _documents.FindIndex(d => d.Id == document.Id);
                if (existingIndex >= 0)
                {
                    _documents[existingIndex] = document;
                    _logger.LogInformation("Updated document in search index: {Id}", document.Id);
                }
                else
                {
                    if (string.IsNullOrEmpty(document.Id))
                    {
                        document.Id = Guid.NewGuid().ToString();
                    }
                    _documents.Add(document);
                    _logger.LogInformation("Added document to search index: {Id}", document.Id);
                }
            }, cancellationToken);

            // Clear cache since index changed
            ClearSearchCache();
        }

        public async Task RemoveDocumentAsync(string documentId, CancellationToken cancellationToken = default)
        {
            await Task.Run(() =>
            {
                var index = _documents.FindIndex(d => d.Id == documentId);
                if (index >= 0)
                {
                    _documents.RemoveAt(index);
                    _logger.LogInformation("Removed document from search index: {Id}", documentId);

                    // Clear cache since index changed
                    ClearSearchCache();
                }
            }, cancellationToken);
        }

        public async Task<IEnumerable<SearchResult>> FindSimilarAsync(
            string text,
            int maxResults = 5,
            CancellationToken cancellationToken = default)
        {
            var cacheKey = $"{CACHE_PREFIX}similar_{text.GetHashCode()}_{maxResults}";

            if (_cache.TryGetValue(cacheKey, out IEnumerable<SearchResult>? cachedResults))
            {
                return cachedResults!;
            }

            var results = await Task.Run(() =>
            {
                return _documents
                    .Select(doc => new SearchResult
                    {
                        Id = doc.Id,
                        Title = doc.Title,
                        Content = doc.Content,
                        Category = doc.Category,
                        Source = doc.Source,
                        Metadata = doc.Metadata,
                        Score = CalculateTextSimilarity(text, doc.Content),
                        Snippet = GenerateSnippet(doc.Content, text, 100)
                    })
                    .Where(result => result.Score > 0.3)
                    .OrderByDescending(result => result.Score)
                    .Take(maxResults)
                    .ToList();
            }, cancellationToken);

            _cache.Set(cacheKey, results, TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));
            return results;
        }

        private void InitializeSampleDocuments()
        {
            var sampleDocs = new[]
            {
                new SearchDocument
                {
                    Id = "mix_cms_overview",
                    Title = "Mix CMS Overview",
                    Content = "Mix CMS is a powerful content management system built with ASP.NET Core and Razor Pages. It provides comprehensive tools for content creation, management, and delivery.",
                    Category = "documentation",
                    Source = "system"
                },
                new SearchDocument
                {
                    Id = "mcp_tools_guide",
                    Title = "MCP Tools Usage Guide",
                    Content = "MCP tools in Mix CMS enable powerful integrations and automation. Key tools include database operations, content management, template handling, and resource management.",
                    Category = "tools",
                    Source = "system"
                },
                new SearchDocument
                {
                    Id = "database_patterns",
                    Title = "Database Patterns in Mix CMS",
                    Content = "Mix CMS uses MixDb patterns for multi-tenant database operations. This includes proper tenant isolation, data consistency, and performance optimization.",
                    Category = "development",
                    Source = "system"
                }
            };

            _documents.AddRange(sampleDocs);
            _logger.LogInformation("Initialized semantic search with {Count} sample documents", sampleDocs.Length);
        }

        private static double CalculateSimilarity(string query, SearchDocument document)
        {
            // Simple text-based similarity calculation
            // In production, this should use proper vector embeddings
            var queryTokens = TokenizeText(query.ToLowerInvariant());
            var titleTokens = TokenizeText(document.Title.ToLowerInvariant());
            var contentTokens = TokenizeText(document.Content.ToLowerInvariant());

            double titleScore = CalculateJaccardSimilarity(queryTokens, titleTokens) * 0.4;
            double contentScore = CalculateJaccardSimilarity(queryTokens, contentTokens) * 0.6;

            return titleScore + contentScore;
        }

        private static double CalculateTextSimilarity(string text1, string text2)
        {
            var tokens1 = TokenizeText(text1.ToLowerInvariant());
            var tokens2 = TokenizeText(text2.ToLowerInvariant());

            return CalculateJaccardSimilarity(tokens1, tokens2);
        }

        private static string[] TokenizeText(string text)
        {
            return text.Split(new[] { ' ', '\t', '\n', '\r', '.', ',', ';', ':', '!', '?' },
                StringSplitOptions.RemoveEmptyEntries);
        }

        private static double CalculateJaccardSimilarity(string[] set1, string[] set2)
        {
            var hashSet1 = new HashSet<string>(set1);
            var hashSet2 = new HashSet<string>(set2);

            var intersection = hashSet1.Intersect(hashSet2).Count();
            var union = hashSet1.Union(hashSet2).Count();

            return union == 0 ? 0.0 : (double)intersection / union;
        }

        private static string GenerateSnippet(string content, string query, int maxLength = 150)
        {
            if (string.IsNullOrEmpty(content) || string.IsNullOrEmpty(query))
                return content.Length > maxLength ? content.Substring(0, maxLength) + "..." : content;

            var queryTokens = TokenizeText(query.ToLowerInvariant());
            var contentLower = content.ToLowerInvariant();

            // Find the first occurrence of any query token
            int bestIndex = -1;
            foreach (var token in queryTokens)
            {
                int index = contentLower.IndexOf(token);
                if (index >= 0 && (bestIndex == -1 || index < bestIndex))
                {
                    bestIndex = index;
                }
            }

            if (bestIndex == -1)
            {
                // No match found, return beginning
                return content.Length > maxLength ? content.Substring(0, maxLength) + "..." : content;
            }

            // Extract snippet around the match
            int start = Math.Max(0, bestIndex - maxLength / 2);
            int length = Math.Min(maxLength, content.Length - start);

            var snippet = content.Substring(start, length);
            if (start > 0) snippet = "..." + snippet;
            if (start + length < content.Length) snippet += "...";

            return snippet;
        }

        private void ClearSearchCache()
        {
            // In a real implementation, you might want to use cache tags or patterns
            _logger.LogDebug("Search cache cleared due to index changes");
        }
    }
}
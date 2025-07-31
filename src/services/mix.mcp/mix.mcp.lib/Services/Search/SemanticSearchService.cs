using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mix.MCP.Lib.Services.LLM;
using System;
using System.Collections.Generic;
using System.IO;
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
        protected readonly ILlmServiceFactory _llmServiceFactory;
        private readonly ILogger<SemanticSearchService> _logger;
        private readonly QdrantService _qdrantService;
        private List<SearchDocument> _documents;
        private const string CACHE_PREFIX = "search_";
        private const int CACHE_DURATION_MINUTES = 15;

        public SemanticSearchService(
            IMemoryCache cache,
            ILogger<SemanticSearchService> logger,
            QdrantService qdrantService,
            ILlmServiceFactory llmServiceFactory)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _llmServiceFactory = llmServiceFactory;
            _qdrantService = qdrantService ?? throw new ArgumentNullException(nameof(qdrantService));
            _documents = new List<SearchDocument>();
            IndexInstructionsOnLoad();
            LoadDocumentsFromVectorDb();
        }

        private void LoadDocumentsFromVectorDb()
        {
            try
            {
                _documents = _qdrantService.GetAllDocumentsAsync().GetAwaiter().GetResult();
                _logger.LogInformation("[SemanticSearchService] Loaded {Count} documents from Qdrant vector DB", _documents.Count);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to load documents from Qdrant vector DB. No documents loaded.");
            }
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
            // TODO: You must provide a vector for the document to upsert
            float[] vector = GetVectorForDocument(document); // Implement this method as needed
            await _qdrantService.UpsertDocumentAsync(document, vector);
            _documents = await _qdrantService.GetAllDocumentsAsync();
            ClearSearchCache();
        }

        public async Task RemoveDocumentAsync(string documentId, CancellationToken cancellationToken = default)
        {
            await _qdrantService.RemoveDocumentAsync(documentId);
            _documents = await _qdrantService.GetAllDocumentsAsync();
            ClearSearchCache();
        }

        public async Task<IEnumerable<SearchResult>> FindSimilarAsync(
            string text,
            ulong maxResults = 5,
            CancellationToken cancellationToken = default)
        {
            var cacheKey = $"{CACHE_PREFIX}similar_{text.GetHashCode()}_{maxResults}";

            if (_cache.TryGetValue(cacheKey, out IEnumerable<SearchResult>? cachedResults))
            {
                return cachedResults!;
            }

            // TODO: You must provide a vector for the text to search
            float[] queryVector = GetVectorForText(text); // Implement this method as needed
            var results = await _qdrantService.SearchAsync(queryVector, limit: maxResults);
            _cache.Set(cacheKey, results, TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));
            return results;
        }

        public async Task RebuildIndexAsync(CancellationToken cancellationToken = default)
        {
            _documents = await _qdrantService.GetAllDocumentsAsync();
            _logger.LogInformation("Cleared and reloaded search index documents from Qdrant.");
            await Task.CompletedTask;
        }

        private static double CalculateSimilarity(string query, SearchDocument document)
        {
            var queryTokens = TokenizeText(query.ToLowerInvariant());
            var titleTokens = TokenizeText(document.Title.ToLowerInvariant());
            var contentTokens = TokenizeText(document.Content.ToLowerInvariant());
            double titleScore = CalculateJaccardSimilarity(queryTokens, titleTokens) * 0.4;
            double contentScore = CalculateJaccardSimilarity(queryTokens, contentTokens) * 0.6;
            return titleScore + contentScore;
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
                return content.Length > maxLength ? content.Substring(0, maxLength) + "..." : content;
            }
            int start = Math.Max(0, bestIndex - maxLength / 2);
            int length = Math.Min(maxLength, content.Length - start);
            var snippet = content.Substring(start, length);
            if (start > 0) snippet = "..." + snippet;
            if (start + length < content.Length) snippet += "...";
            return snippet;
        }

        private void ClearSearchCache()
        {
            _logger.LogDebug("Search cache cleared due to index changes");
        }

        // Placeholder: You must implement vectorization logic for your use case
        private float[] GetVectorForDocument(SearchDocument document)
        {
            var llmService = _llmServiceFactory.CreateService(LLMServiceType.DeepSeek);
            // Simple deterministic vectorization using hash codes of content and title
            return GetVectorForText(document.Content + " " + document.Title);
        }

        private float[] GetVectorForText(string text)
        {
            var llmService = _llmServiceFactory.CreateService(LLMServiceType.DeepSeek);
            // Simple deterministic vectorization: hash each char, fill vector
            const int vectorSize = 100;
            var vector = new float[vectorSize];
            if (string.IsNullOrEmpty(text)) return vector;
            int hash = text.GetHashCode();
            for (int i = 0; i < vectorSize; i++)
            {
                int charIndex = i % text.Length;
                vector[i] = ((text[charIndex] + hash + i) % 1000) / 1000f;
            }
            return vector;
        }


        private void IndexInstructionsOnLoad()
        {
            try
            {
                var instructionsDir = Path.Combine(AppContext.BaseDirectory, "instructions");
                if (!Directory.Exists(instructionsDir))
                {
                    _logger.LogWarning("Instructions directory not found: {Dir}", instructionsDir);
                    return;
                }
                _qdrantService.UpsertCollectionAsync("mixcore").GetAwaiter().GetResult();
                var mdFiles = Directory.GetFiles(instructionsDir, "*.md", SearchOption.AllDirectories);
                foreach (var file in mdFiles)
                {
                    var content = File.ReadAllText(file);
                    var doc = new SearchDocument
                    {
                        Id = Guid.NewGuid().ToString("N"),
                        Collection = "mixcore",
                        Title = Path.GetFileNameWithoutExtension(file),
                        Content = content,
                        Category = "instructions",
                        Source = "instructions",
                        CreatedAt = File.GetCreationTimeUtc(file),
                        Metadata = new Dictionary<string, object> { { "path", file } }
                    };
                    // Fire and forget, or you can await if you want to block
                    IndexDocumentAsync(doc).GetAwaiter().GetResult();
                }
                _logger.LogInformation("Indexed {Count} markdown documents from instructions folder", mdFiles.Length);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to index instructions markdown documents on load");
            }
        }

    }
}
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mix.MCP.Lib.Services.Knowledge
{
    /// <summary>
    /// Default implementation of knowledge base service with in-memory storage and caching
    /// </summary>
    public class KnowledgeBaseService : IKnowledgeBaseService
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<KnowledgeBaseService> _logger;
        private readonly List<KnowledgeEntry> _knowledgeBase;
        private const string CACHE_PREFIX = "knowledge_";
        private const int CACHE_DURATION_MINUTES = 30;

        public KnowledgeBaseService(IMemoryCache cache, ILogger<KnowledgeBaseService> logger)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _knowledgeBase = new List<KnowledgeEntry>();
            
            // Initialize with default knowledge entries
            InitializeDefaultKnowledge();
        }

        public async Task<IEnumerable<KnowledgeEntry>> SearchAsync(
            string query, 
            int maxResults = 5, 
            CancellationToken cancellationToken = default)
        {
            var cacheKey = $"{CACHE_PREFIX}search_{query}_{maxResults}";
            
            if (_cache.TryGetValue(cacheKey, out IEnumerable<KnowledgeEntry>? cachedResults))
            {
                _logger.LogDebug("Retrieved search results from cache for query: {Query}", query);
                return cachedResults!;
            }

            _logger.LogInformation("Searching knowledge base for query: {Query}", query);

            var results = await Task.Run(() =>
            {
                var queryLower = query.ToLowerInvariant();
                return _knowledgeBase
                    .Where(entry => 
                        entry.Title.ToLowerInvariant().Contains(queryLower) ||
                        entry.Content.ToLowerInvariant().Contains(queryLower) ||
                        entry.Category.ToLowerInvariant().Contains(queryLower))
                    .Select(entry => new KnowledgeEntry
                    {
                        Id = entry.Id,
                        Title = entry.Title,
                        Content = entry.Content,
                        Category = entry.Category,
                        Source = entry.Source,
                        LastUpdated = entry.LastUpdated,
                        Metadata = entry.Metadata,
                        Relevance = CalculateRelevance(entry, queryLower)
                    })
                    .OrderByDescending(entry => entry.Relevance)
                    .Take(maxResults)
                    .ToList();
            }, cancellationToken);

            // Cache results for future use
            _cache.Set(cacheKey, results, TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));

            _logger.LogInformation("Found {Count} knowledge entries for query: {Query}", results.Count(), query);
            return results;
        }

        public async Task<IEnumerable<KnowledgeEntry>> GetByCategoryAsync(
            string category, 
            CancellationToken cancellationToken = default)
        {
            var cacheKey = $"{CACHE_PREFIX}category_{category}";
            
            if (_cache.TryGetValue(cacheKey, out IEnumerable<KnowledgeEntry>? cachedResults))
            {
                _logger.LogDebug("Retrieved category results from cache for: {Category}", category);
                return cachedResults!;
            }

            var results = await Task.Run(() =>
            {
                return _knowledgeBase
                    .Where(entry => string.Equals(entry.Category, category, StringComparison.OrdinalIgnoreCase))
                    .OrderByDescending(entry => entry.LastUpdated)
                    .ToList();
            }, cancellationToken);

            _cache.Set(cacheKey, results, TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));

            _logger.LogInformation("Found {Count} knowledge entries in category: {Category}", results.Count(), category);
            return results;
        }

        public async Task AddOrUpdateAsync(KnowledgeEntry entry, CancellationToken cancellationToken = default)
        {
            if (entry == null) throw new ArgumentNullException(nameof(entry));

            await Task.Run(() =>
            {
                var existingIndex = _knowledgeBase.FindIndex(e => e.Id == entry.Id);
                if (existingIndex >= 0)
                {
                    _knowledgeBase[existingIndex] = entry;
                    _logger.LogInformation("Updated knowledge entry: {Id}", entry.Id);
                }
                else
                {
                    if (string.IsNullOrEmpty(entry.Id))
                    {
                        entry.Id = Guid.NewGuid().ToString();
                    }
                    _knowledgeBase.Add(entry);
                    _logger.LogInformation("Added new knowledge entry: {Id}", entry.Id);
                }

                entry.LastUpdated = DateTime.UtcNow;
            }, cancellationToken);

            // Clear relevant cache entries
            ClearCacheForCategory(entry.Category);
        }

        public async Task<string> GetContextForPlanningAsync(
            string userInput, 
            string agentType = "planning", 
            CancellationToken cancellationToken = default)
        {
            var cacheKey = $"{CACHE_PREFIX}context_{agentType}_{userInput.GetHashCode()}";
            
            if (_cache.TryGetValue(cacheKey, out string? cachedContext))
            {
                return cachedContext!;
            }

            _logger.LogInformation("Getting planning context for agent type: {AgentType}, input: {UserInput}", agentType, userInput);

            // Search for relevant knowledge
            var relevantEntries = await SearchAsync(userInput, 3, cancellationToken);
            
            // Also get entries specific to the agent type
            var agentSpecificEntries = await GetByCategoryAsync($"agent_{agentType}", cancellationToken);

            var contextBuilder = new List<string>();
            
            if (relevantEntries.Any())
            {
                contextBuilder.Add("Relevant documentation:");
                foreach (var entry in relevantEntries.Take(2))
                {
                    contextBuilder.Add($"- {entry.Title}: {TruncateContent(entry.Content, 200)}");
                }
            }

            if (agentSpecificEntries.Any())
            {
                contextBuilder.Add($"\nAgent-specific guidance for {agentType}:");
                foreach (var entry in agentSpecificEntries.Take(2))
                {
                    contextBuilder.Add($"- {entry.Title}: {TruncateContent(entry.Content, 150)}");
                }
            }

            var context = string.Join("\n", contextBuilder);
            
            // Cache the context
            _cache.Set(cacheKey, context, TimeSpan.FromMinutes(CACHE_DURATION_MINUTES));

            return context;
        }

        private void InitializeDefaultKnowledge()
        {
            var defaultEntries = new[]
            {
                new KnowledgeEntry
                {
                    Id = "mcp_tools_overview",
                    Title = "MCP Tools Overview",
                    Content = "MCP tools in Mix CMS include database operations, content management, template handling, and resource management. Common tools: MixDbDataTool, MixPageContentTool, MixTemplateTool, MixModuleContentTool.",
                    Category = "tools",
                    Source = "system"
                },
                new KnowledgeEntry
                {
                    Id = "planning_best_practices",
                    Title = "Planning Agent Best Practices",
                    Content = "When decomposing user requests: 1) Break into atomic tasks, 2) Consider dependencies between tasks, 3) Use available MCP tools efficiently, 4) Provide clear context between steps.",
                    Category = "agent_planning",
                    Source = "system"
                },
                new KnowledgeEntry
                {
                    Id = "database_operations",
                    Title = "Database Operations Guide",
                    Content = "Mix CMS supports multi-tenant database operations. Use MixDbDataTool for CRUD operations, respect tenant isolation, and follow MixDb patterns for consistency.",
                    Category = "database",
                    Source = "system"
                },
                new KnowledgeEntry
                {
                    Id = "content_management",
                    Title = "Content Management Patterns",
                    Content = "Content in Mix CMS includes pages, posts, modules, and templates. Each content type has specific workflows and validation rules. Use appropriate content tools for each type.",
                    Category = "content",
                    Source = "system"
                }
            };

            _knowledgeBase.AddRange(defaultEntries);
            _logger.LogInformation("Initialized knowledge base with {Count} default entries", defaultEntries.Length);
        }

        private static double CalculateRelevance(KnowledgeEntry entry, string queryLower)
        {
            double relevance = 0.0;

            // Title matches have higher weight
            if (entry.Title.ToLowerInvariant().Contains(queryLower))
                relevance += 0.5;

            // Content matches
            if (entry.Content.ToLowerInvariant().Contains(queryLower))
                relevance += 0.3;

            // Category matches
            if (entry.Category.ToLowerInvariant().Contains(queryLower))
                relevance += 0.2;

            // Boost recent entries slightly
            var daysSinceUpdate = (DateTime.UtcNow - entry.LastUpdated).TotalDays;
            if (daysSinceUpdate < 30)
                relevance += 0.1;

            return relevance;
        }

        private static string TruncateContent(string content, int maxLength)
        {
            if (string.IsNullOrEmpty(content) || content.Length <= maxLength)
                return content;

            return content.Substring(0, maxLength) + "...";
        }

        private void ClearCacheForCategory(string category)
        {
            // In a real implementation, you might want to use a more sophisticated cache invalidation strategy
            _logger.LogDebug("Cache invalidated for category: {Category}", category);
        }
    }
}
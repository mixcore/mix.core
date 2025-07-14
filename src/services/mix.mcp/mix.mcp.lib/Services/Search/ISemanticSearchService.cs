using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mix.MCP.Lib.Services.Search
{
    /// <summary>
    /// Interface for semantic search service
    /// </summary>
    public interface ISemanticSearchService
    {
        /// <summary>
        /// Performs semantic search for documents/content
        /// </summary>
        /// <param name="query">Search query</param>
        /// <param name="maxResults">Maximum number of results</param>
        /// <param name="threshold">Similarity threshold (0.0 to 1.0)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of search results</returns>
        Task<IEnumerable<SearchResult>> SearchAsync(
            string query, 
            int maxResults = 10, 
            double threshold = 0.7,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Indexes a document for semantic search
        /// </summary>
        /// <param name="document">Document to index</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task IndexDocumentAsync(SearchDocument document, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes a document from the index
        /// </summary>
        /// <param name="documentId">Document ID to remove</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task RemoveDocumentAsync(string documentId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets similar documents to the provided text
        /// </summary>
        /// <param name="text">Text to find similar documents for</param>
        /// <param name="maxResults">Maximum number of results</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of similar documents</returns>
        Task<IEnumerable<SearchResult>> FindSimilarAsync(
            string text, 
            int maxResults = 5, 
            CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Represents a document for semantic search
    /// </summary>
    public class SearchDocument
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Dictionary<string, object> Metadata { get; set; } = new();
    }

    /// <summary>
    /// Represents a search result with relevance score
    /// </summary>
    public class SearchResult
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public double Score { get; set; } = 0.0;
        public string Snippet { get; set; } = string.Empty;
        public Dictionary<string, object> Metadata { get; set; } = new();
    }
}
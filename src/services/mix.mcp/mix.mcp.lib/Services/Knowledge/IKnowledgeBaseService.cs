using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mix.MCP.Lib.Services.Knowledge
{
    /// <summary>
    /// Interface for knowledge base service that provides context and documentation
    /// </summary>
    public interface IKnowledgeBaseService
    {
        /// <summary>
        /// Searches for relevant knowledge based on user input
        /// </summary>
        /// <param name="query">The search query</param>
        /// <param name="maxResults">Maximum number of results to return</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of relevant knowledge entries</returns>
        Task<IEnumerable<KnowledgeEntry>> SearchAsync(
            string query,
            int maxResults = 5,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets knowledge entries by category
        /// </summary>
        /// <param name="category">Knowledge category (e.g., "documentation", "faq", "tools")</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of knowledge entries in the category</returns>
        Task<IEnumerable<KnowledgeEntry>> GetByCategoryAsync(
            string category,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds or updates a knowledge entry
        /// </summary>
        /// <param name="entry">Knowledge entry to add or update</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task AddOrUpdateAsync(KnowledgeEntry entry, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets contextual information for agent planning
        /// </summary>
        /// <param name="userInput">User input to analyze</param>
        /// <param name="agentType">Type of agent requesting context</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Contextual information for the agent</returns>
        Task<string> GetContextForPlanningAsync(
            string userInput,
            string agentType = "planning",
            CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Represents a knowledge base entry
    /// </summary>
    public class KnowledgeEntry
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public double Relevance { get; set; } = 0.0;
        public Dictionary<string, object> Metadata { get; set; } = new();
    }
}
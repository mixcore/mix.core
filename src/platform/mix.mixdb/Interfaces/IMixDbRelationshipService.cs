using Mix.Mixdb.ViewModels;
using Mix.RepoDb.ViewModels;
using Mix.Shared.Models;
using Newtonsoft.Json.Linq;

namespace Mix.Mixdb.Interfaces
{
    /// <summary>
    /// Interface defining methods for handling data relationships in MixDb
    /// </summary>
    public interface IMixDbRelationshipService
    {
        /// <summary>
        /// Loads related data for an object
        /// </summary>
        Task LoadNestedDataAsync(string tableName, JObject item, List<SearchMixDbRequestModel> relatedDataRequests, CancellationToken cancellationToken);

        /// <summary>
        /// Loads one-to-many relationship data
        /// </summary>
        Task LoadOneToMany(string tableName, JObject item, MixDbTableRelationshipViewModel rel, SearchMixDbRequestModel req, CancellationToken cancellationToken);

        /// <summary>
        /// Loads many-to-many relationship data
        /// </summary>
        Task LoadManyToMany(string tableName, JObject item, MixDbTableRelationshipViewModel rel, SearchMixDbRequestModel req, CancellationToken cancellationToken);

        /// <summary>
        /// Parses related data requests
        /// </summary>
        Task<List<SearchMixDbRequestModel>?> ParseRelatedDataRequests(string? selectFieldNames, string tableName);
    }
}
using Mix.Constant.Enums;
using Mix.Database.Entities.Cms;
using Mix.Database.Entities.MixDb;
using Mix.Heart.Enums;
using Mix.Heart.Model;
using Mix.Heart.Models;
using Mix.Heart.UnitOfWork;
using Mix.Mixdb.Dtos;
using Mix.Mixdb.ViewModels;
using Mix.Service.Services;
using Mix.Shared.Dtos;
using Mix.Shared.Models;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;
using RepoDb;
using System.Data;

namespace Mix.Mixdb.Interfaces
{
    /// <summary>
    /// Interface defining basic methods for manipulating MixDb data
    /// </summary>
    public interface IMixDbDataService : IDisposable
    {
        #region Properties
        MixDatabaseProvider DbProvider { get; }
        #endregion

        #region Methods
        void SetDbConnection(UnitOfWorkInfo<MixCmsContext> uow);
        void Init(string connectionString);

        #region GET
        Task<JObject?> GetByIdAsync(string tableName, object objId, string? selectColumns, CancellationToken cancellationToken);
        Task<T?> GetByIdAsync<T>(string tableName, object objId, string? selectColumns, CancellationToken cancellationToken) where T : class;
        Task<JObject?> GetSingleByAsync(string tableName, MixQueryField query, string? selectColumns, CancellationToken cancellationToken);
        Task<JObject?> GetSingleByAsync(string tableName, List<MixQueryField> queries, string selectColumns, CancellationToken cancellationToken);
        Task<JObject?> GetSingleByParentAsync(string tableName, MixContentType parentType, object parentId, string selectColumns, CancellationToken cancellationToken);
        Task<JObject?> GetSingleByColumnAsync(string tableName, string columnName, object columnValue, string selectColumns, CancellationToken cancellationToken);
        Task<List<JObject>?> GetListByAsync(SearchMixDbRequestModel request, CancellationToken cancellationToken = default);
        Task<List<JObject>?> GetListByAsync(string tableName, List<QueryField> queryFields, List<MixSortByColumn>? sortByFields = null, string? selectFields = null, bool loadNestedData = false, CancellationToken cancellationToken = default);
        Task<List<JObject>?> GetListByParentAsync(SearchMixDbRequestModel request, MixContentType parentType, object parentId, CancellationToken cancellationToken = default);
        Task<List<JObject>?> GetAllAsync(SearchMixDbRequestModel request, CancellationToken cancellationToken = default);
        Task<PagingResponseModel<JObject>> GetPagingAsync(SearchMixDbRequestModel request, CancellationToken cancellationToken = default);
        Task<object> ExtractIdAsync(string tableName, JObject obj);
        Task<MixDbTableViewModel?> GetMixDb(string tableName);
        #endregion

        #region CREATE
        Task<object> CreateAsync(string tableName, JObject obj, string? createdBy = null, CancellationToken cancellationToken = default);
        Task CreateManyAsync(string tableName, List<JObject> entities, string? createdBy = null, CancellationToken cancellationToken = default);
        Task CreateDataRelationshipAsync(string tableName, CreateDataRelationshipDto obj, string? createdBy = null, CancellationToken cancellationToken = default);
        #endregion

        #region UPDATE
        Task<int?> UpdateManyAsync(string tableName, List<JObject> entities, string? modifiedBy = null, CancellationToken cancellationToken = default);
        Task<object?> UpdateAsync(string tableName, object id, JObject entity, string? modifiedBy = null, IEnumerable<string>? fieldNames = default, CancellationToken cancellationToken = default);
        #endregion

        #region DELETE
        Task<int> DeleteAsync(string tableName, object id, string? modifiedBy = null, CancellationToken cancellationToken = default);
        Task<int> DeleteManyAsync(string tableName, List<MixQueryField> queries, string? modifiedBy = null, CancellationToken cancellationToken = default);
        Task DeleteDataRelationshipAsync(string tableName, int id, string? modifiedBy = null, CancellationToken cancellationToken = default);
        #endregion

        #region HELPERS
        string GetCacheFolder(string databaseName);
        MixCompareOperator ParseMixCompareOperator(ExpressionMethod? searchMethod);
        void InitConnection(MixDatabaseProvider databaseProvider, string connectionString);
        #endregion
        #endregion
    }
}

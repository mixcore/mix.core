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

namespace Mix.Mixdb.Interfaces
{
    public interface IMixDbDataService : IDisposable
    {
        public MixDatabaseProvider DbProvider { get; }
        void Dispose();
        void Init(string connectionString);
        Task<List<JObject>?> GetAllAsync(SearchMixDbRequestModel request, CancellationToken cancellationToken = default);
        Task<JObject?> GetByIdAsync(string tableName, object objId, string? selectColumns, CancellationToken cancellationToken);
        string GetCacheFolder(string databaseName);
        Task<List<JObject>?> GetListByAsync(SearchMixDbRequestModel request, CancellationToken cancellationToken = default);
        Task<List<JObject>?> GetListByParentAsync(SearchMixDbRequestModel request, MixContentType parentType, object parentId, CancellationToken cancellationToken = default);
        Task<PagingResponseModel<JObject>> GetPagingAsync(SearchMixDbRequestModel request, CancellationToken cancellationToken = default);
        Task<JObject?> GetSingleByAsync(string tableName, List<MixQueryField> queries, string? selectColumns = null, CancellationToken cancellationToken = default);
        Task<JObject?> GetSingleByAsync(string tableName, MixQueryField query, string? selectColumns = null, CancellationToken cancellationToken = default);
        Task<JObject?> GetSingleByParentAsync(string tableName, MixContentType parentType, object parentId, string? selectColumns = null, CancellationToken cancellationToken = default);
        Task LoadNestedDataAsync(string tableName, JObject item, List<SearchMixDbRequestModel> relatedDataRequests, CancellationToken cancellationToken);
        Task<object> CreateAsync(string tableName, JObject obj, string? createdBy = null, CancellationToken cancellationToken = default);
        Task CreateManyAsync(string tableName, List<JObject> entities, string? createdBy = null, CancellationToken cancellationToken = default);
        Task CreateDataRelationshipAsync(string tableName, CreateDataRelationshipDto obj, string? createdBy = null, CancellationToken cancellationToken = default);
        Task<object?> UpdateAsync(string tableName, object id, JObject entity, string? modifiedBy = null, IEnumerable<string>? fieldNames = default, CancellationToken cancellationToken = default);
        Task<int?> UpdateManyAsync(string tableName, List<JObject> entities, string? modifiedBy = null, CancellationToken cancellationToken = default);
        Task<int> DeleteManyAsync(string tableName, List<MixQueryField> queries, string? modifiedBy = null, CancellationToken cancellationToken = default);
        Task<int> DeleteAsync(string tableName, object id, string? modifiedBy = null, CancellationToken cancellationToken = default);
        Task DeleteDataRelationshipAsync(string tableName, int id, string? modifiedBy = null, CancellationToken cancellationToken = default);
        Task<Dictionary<string, object>> ParseDtoToEntityAsync(JObject dto, string? username = null);
        MixCompareOperator ParseMixCompareOperator(ExpressionMethod? searchMethod);
        object? ParseObjectValueToDbType(MixDataType? dataType, JToken value);
        void SetDbConnection(UnitOfWorkInfo<MixCmsContext> uow);
        Task<object> ExtractIdAsync(string tableName, JObject obj);

    }
}

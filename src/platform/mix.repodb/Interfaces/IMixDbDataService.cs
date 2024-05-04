using Mix.Constant.Enums;
using Mix.Database.Entities.MixDb;
using Mix.Heart.Models;
using Mix.Heart.UnitOfWork;
using Mix.RepoDb.Dtos;
using Mix.RepoDb.ViewModels;
using Mix.Service.Services;
using Mix.Shared.Dtos;
using Newtonsoft.Json.Linq;
using RepoDb;

namespace Mix.RepoDb.Interfaces
{
    public interface IMixDbDataService : IDisposable
    {
        public Task<JObject?> GetSingleByParent(string tableName, MixContentType parentType, object parentId, bool loadNestedData = false);

        public Task<PagingResponseModel<JObject>> GetMyData(string tableName, SearchMixDbRequestDto req, string username);

        public Task<JObject?> GetMyDataById(string tableName, string username, object id, bool loadNestedData);

        public Task<JObject?> GetById(string tableName, object id, bool loadNestedData);

        public Task<JObject?> GetSingleBy(string tableName, List<QueryField> queries);

        public Task<object> CreateData(string tableName, JObject data);
        public Task<object?> UpdateData(string tableName, JObject data);
        public Task<object> DeleteData(string tableName, object id);
        void SetUOW(UnitOfWorkInfo<MixDbDbContext> uow);
        Task<JObject> ParseDataAsync(string tableName, dynamic obj);
        Task<JObject?> GetSingleByGuidParent(string tableName, MixContentType parentType, Guid parentId, bool loadNestedData = false);
        Task<object?> CreateDataRelationship(CreateDataRelationshipDto dto, CancellationToken cancellationToken);
        Task DeleteDataRelationship(string relTableName, object id, CancellationToken cancellationToken);
        Task<List<JObject>> ParseListDataAsync(string tableName, List<dynamic> objs);
        Task<List<JProperty>> LoadNestedData(RepoDbMixDatabaseViewModel database, FieldNameService fieldNameService, object parentId);
    }
}

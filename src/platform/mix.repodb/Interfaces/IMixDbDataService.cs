using Mix.Constant.Enums;
using Mix.Heart.Models;
using Mix.Shared.Dtos;
using Newtonsoft.Json.Linq;
using RepoDb;

namespace Mix.RepoDb.Interfaces
{
    public interface IMixDbDataService
    {
        public Task<JObject?> GetSingleByParent(string tableName, MixContentType parentType, object parentId, bool loadNestedData = false);

        public Task<PagingResponseModel<JObject>> GetMyData(string tableName, SearchMixDbRequestDto req, string username);

        public Task<JObject> GetMyDataById(string tableName, string username, int id, bool loadNestedData);

        public Task<JObject?> GetById(string tableName, int id, bool loadNestedData);

        public Task<JObject?> GetSingleBy(string tableName, List<QueryField> queries);

        public Task CreateData(string tableName, JObject data);
    }
}

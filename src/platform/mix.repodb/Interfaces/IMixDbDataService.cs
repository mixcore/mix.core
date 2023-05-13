using Mix.Constant.Enums;
using Mix.Database.Entities.MixDb;
using Mix.Heart.Models;
using Mix.Heart.UnitOfWork;
using Mix.Shared.Dtos;
using Newtonsoft.Json.Linq;
using RepoDb;

namespace Mix.RepoDb.Interfaces
{
    public interface IMixDbDataService: IDisposable
    {
        public Task<JObject?> GetSingleByParent(string tableName, MixContentType parentType, object parentId, bool loadNestedData = false);

        public Task<PagingResponseModel<JObject>> GetMyData(string tableName, SearchMixDbRequestDto req, string username);

        public Task<JObject> GetMyDataById(string tableName, string username, int id, bool loadNestedData);

        public Task<JObject?> GetById(string tableName, int id, bool loadNestedData);

        public Task<JObject?> GetSingleBy(string tableName, List<QueryField> queries);

        public Task<long> CreateData(string tableName, JObject data);
        void SetUOW(UnitOfWorkInfo<MixDbDbContext> uow);
    }
}

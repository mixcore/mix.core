using RepoDb.Interfaces;
using System.Data;
using System.Data.Common;

namespace Mix.RepoDb.Interfaces
{
    public interface IRepoDb<TDbConnection, TEntity>
        where TDbConnection : DbConnection
        where TEntity : class
    {
        ICache Cache { get; }
        ITrace Trace { get; }

        TDbConnection CreateConnection();
        int Delete(int id);
        Task<int> DeleteAsync(int id);
        TEntity Get(int id);
        IEnumerable<TEntity> GetAll(string? cacheKey = null);
        Task<IEnumerable<TEntity>> GetAllAsync(string? cacheKey = null);
        Task<TEntity> GetAsync(int id);
        int Merge(TEntity entity, IDbTransaction? transaction = null);
        Task<object> MergeAsync(TEntity entity);
        object Insert(TEntity entity, IDbTransaction? transaction = null);
        Task<object> InsertAsync(TEntity entity);
        int Update(TEntity entity, IDbTransaction? transaction = null);
        Task<int> UpdateAsync(TEntity entity);
    }
}

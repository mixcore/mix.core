using Microsoft.EntityFrameworkCore;
using Mix.Lib.Models.Common;
using Newtonsoft.Json;

namespace Mix.Lib.Interfaces
{
    public interface IRestApiService<TView, TDbContext, TEntity, TPrimaryKey>
        where TPrimaryKey : IComparable
        where TDbContext : DbContext
        where TEntity : class, IEntity<TPrimaryKey>
        where TView : SimpleViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
    {
        public Task<TPrimaryKey> CreateHandlerAsync(TView data, CancellationToken cancellationToken = default);

        public Task UpdateHandler(TPrimaryKey id, TView data, CancellationToken cancellationToken = default);

        public Task DeleteHandler(TView data, CancellationToken cancellationToken = default);

        public Task PatchHandler(
            TPrimaryKey id, 
            TView data, 
            IEnumerable<EntityPropertyModel> properties, 
            CancellationToken cancellationToken = default);

        public Task SaveManyHandler(
            List<TView> data, 
            CancellationToken cancellationToken = default);

        public Task<PagingResponseModel<TView>> SearchHandler(
            SearchRequestDto request, 
            SearchQueryModel<TEntity, TPrimaryKey> searchQuery, 
            CancellationToken cancellationToken = default);

        public PagingResponseModel<TView> ParseSearchResult(
            SearchRequestDto request,
            PagingResponseModel<TView> result,
            JsonSerializer serializer = null);

        public SearchQueryModel<TEntity, TPrimaryKey> BuildSearchRequest(SearchRequestDto request);

        public Task<TView> GetById(TPrimaryKey id);
    }
}

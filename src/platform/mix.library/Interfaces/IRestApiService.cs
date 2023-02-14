using Microsoft.EntityFrameworkCore;
using Mix.Lib.Models.Common;

namespace Mix.Lib.Interfaces
{
    public interface IRestApiService<TView, TDbContext, TEntity, TPrimaryKey>
        where TPrimaryKey : IComparable
        where TDbContext : DbContext
        where TEntity : EntityBase<TPrimaryKey>
        where TView : ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
    {
        public Task<TPrimaryKey> CreateHandlerAsync(TView data, CancellationToken cancellationToken = default);

        public Task UpdateHandler(TPrimaryKey id, TView data, CancellationToken cancellationToken = default);

        public Task DeleteHandler(TView data, CancellationToken cancellationToken = default);

        public Task PatchHandler(TPrimaryKey id, TView data, IEnumerable<EntityPropertyModel> properties, CancellationToken cancellationToken = default);

        public Task SaveManyHandler(List<TView> data, CancellationToken cancellationToken = default);

        public Task<PagingResponseModel<TView>> SearchHandler(SearchRequestDto req, SearchQueryModel<TEntity, TPrimaryKey> searchRequest, CancellationToken cancellationToken = default);

        public PagingResponseModel<TView> ParseSearchResult(SearchRequestDto req, PagingResponseModel<TView> result, CancellationToken cancellationToken = default);

        public SearchQueryModel<TEntity, TPrimaryKey> BuildSearchRequest(SearchRequestDto req);

        public Task<TView> GetById(TPrimaryKey id);
    }
}

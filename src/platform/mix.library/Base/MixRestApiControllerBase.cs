using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mix.Lib.Models.Common;
using Mix.Lib.Services;

namespace Mix.Lib.Base
{
    public class MixRestHandlerApiControllerBase<TView, TDbContext, TEntity, TPrimaryKey>
        : MixTenantApiControllerBase
        where TPrimaryKey : IComparable
        where TDbContext : DbContext
        where TEntity : EntityBase<TPrimaryKey>
        where TView : ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
    {
        protected Repository<TDbContext, TEntity, TPrimaryKey, TView> Repository;
        protected readonly TDbContext Context;
        protected UnitOfWorkInfo Uow;
        protected readonly RestApiService<TView, TDbContext, TEntity, TPrimaryKey> RestApiService;

        public MixRestHandlerApiControllerBase(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<TDbContext> uow,
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor, configuration, cacheService, translator, mixIdentityService, queueService)
        {
            Context = (TDbContext)uow.ActiveDbContext;
            Uow = uow;
            Repository = ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>.GetRepository(Uow, CacheService);
            RestApiService = new(httpContextAccessor, mixIdentityService, uow, queueService, CacheService);
            RestApiService.Repository = Repository;
            Repository.CacheService = CacheService;
        }

        #region Command Handlers

        protected virtual async Task<TPrimaryKey> CreateHandlerAsync(TView data, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await RestApiService.CreateHandlerAsync(data, cancellationToken);
        }

        protected virtual Task UpdateHandler(TPrimaryKey id, TView data, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return RestApiService.UpdateHandler(id, data, cancellationToken);
        }

        protected virtual Task DeleteHandler(TView data, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return RestApiService.DeleteHandler(data, cancellationToken);
        }


        protected virtual Task PatchHandler(TPrimaryKey id, TView data, IEnumerable<EntityPropertyModel> properties, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return RestApiService.PatchHandler(id, data, properties, cancellationToken);
        }

        protected virtual Task SaveManyHandler(List<TView> data, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return RestApiService.SaveManyHandler(data, cancellationToken);
        }
        private async Task RemoveCacheHandler(MixCacheService cacheService, TPrimaryKey id)
        {
            await cacheService.RemoveCacheAsync(id, typeof(TEntity).FullName);
        }


        #endregion

        #region Query Handlers
        protected virtual Task<PagingResponseModel<TView>> SearchHandler(SearchRequestDto req, CancellationToken cancellationToken = default)
        {
            var searchRequest = BuildSearchRequest(req);
            return RestApiService.SearchHandler(req, searchRequest, cancellationToken);
        }

        protected virtual PagingResponseModel<TView> ParseSearchResult(SearchRequestDto req, PagingResponseModel<TView> result)
        {
            return RestApiService.ParseSearchResult(req, result);
        }

        #endregion

        #region Helpers

        protected virtual SearchQueryModel<TEntity, TPrimaryKey> BuildSearchRequest(SearchRequestDto req)
        {
            return RestApiService.BuildSearchRequest(req);
        }

        protected virtual Task<TView> GetById(TPrimaryKey id)
        {
            return RestApiService.GetById(id);
        }

        #endregion
    }
}

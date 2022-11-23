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
        protected readonly Repository<TDbContext, TEntity, TPrimaryKey, TView> Repository;
        protected readonly TDbContext Context;
        protected UnitOfWorkInfo Uow;
        protected readonly RestApiService<TView, TDbContext, TEntity, TPrimaryKey> RestApiService;

        public MixRestHandlerApiControllerBase(
             IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<TDbContext> uow,
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, queueService)
        {
            Context = (TDbContext)uow.ActiveDbContext;
            RestApiService = new(httpContextAccessor, mixIdentityService, uow, queueService);
            Uow = uow;
            Repository = ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>.GetRepository(Uow);
        }

        #region Command Handlers

        protected virtual async Task<TPrimaryKey> CreateHandlerAsync(TView data)
        {
            return await RestApiService.CreateHandlerAsync(data);
        }

        protected virtual Task UpdateHandler(TPrimaryKey id, TView data)
        {
            return RestApiService.UpdateHandler(id, data);
        }

        protected virtual Task DeleteHandler(TView data)
        {
            return RestApiService.DeleteHandler(data);
        }


        protected virtual Task PatchHandler(TPrimaryKey id, TView data, IEnumerable<EntityPropertyModel> properties)
        {
            return RestApiService.PatchHandler(id, data, properties);
        }

        protected virtual Task SaveManyHandler(List<TView> data)
        {
            return RestApiService.SaveManyHandler(data);
        }

        #endregion

        #region Query Handlers
        protected virtual Task<PagingResponseModel<TView>> SearchHandler(SearchRequestDto req)
        {
            var searchRequest = BuildSearchRequest(req);
            return RestApiService.SearchHandler(req, searchRequest);
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

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mix.Lib.Interfaces;
using Mix.Lib.Models.Common;
using Mix.Lib.Services;
using Mix.Mixdb.Helpers;
using Mix.Mq.Lib.Models;
using Mix.SignalR.Interfaces;
using Newtonsoft.Json;

namespace Mix.Lib.Base
{
    public class MixRestHandlerApiControllerBase<TView, TDbContext, TEntity, TPrimaryKey>
        : MixTenantApiControllerBase
        where TPrimaryKey : IComparable
        where TDbContext : DbContext
        where TEntity : class, IEntity<TPrimaryKey>
        where TView : SimpleViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
    {
        protected Repository<TDbContext, TEntity, TPrimaryKey, TView> Repository;
        protected readonly TDbContext Context;
        protected UnitOfWorkInfo<TDbContext> Uow;
        protected readonly RestApiService<TView, TDbContext, TEntity, TPrimaryKey> RestApiService;
        public MixRestHandlerApiControllerBase(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<TDbContext> uow,
            IMemoryQueueService<MessageQueueModel> queueService,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService)
            : base(httpContextAccessor, configuration,
                  cacheService, translator, mixIdentityService, queueService, mixTenantService)
        {
            Context = (TDbContext)uow.ActiveDbContext;
            Uow = uow;
            Repository = SimpleViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>.GetRepository(Uow, CacheService);
            RestApiService = new(httpContextAccessor, mixIdentityService, uow, queueService, CacheService, portalHub, mixTenantService)
            {
                Repository = Repository
            };

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

        protected virtual async Task PatchHandler(JObject obj, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var id = obj.Value<TPrimaryKey>("id");
            List<EntityPropertyModel> properties = ParseObjectToProperties(obj);
            var data = await Repository.GetSingleAsync(id, cancellationToken);
            if (data == null)
            {
                throw new MixException(MixErrorStatus.NotFound);
            }

            await RestApiService.PatchHandler(id, data, properties, cancellationToken);
        }

        protected virtual async Task PatchManyHandler(
            IEnumerable<JObject> lstObj,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            foreach (var obj in lstObj)
            {
                await PatchHandler(obj, cancellationToken);
            }
        }

        private List<EntityPropertyModel> ParseObjectToProperties(JObject obj)
        {
            List<EntityPropertyModel> properties = new();
            foreach (var prop in obj.Properties())
            {
                if (prop.Name != "id")
                {
                    properties.Add(new()
                    {
                        PropertyName = prop.Name,
                        PropertyValue = MixDbHelper.GetJPropertyValue(prop)
                    });
                }
            }

            return properties;
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

        protected virtual Task<PagingResponseModel<TView>> SearchHandler(
            SearchRequestDto request,
            CancellationToken cancellationToken = default)
        {
            var searchRequest = BuildSearchRequest(request);
            return RestApiService.SearchHandler(request, searchRequest, cancellationToken);
        }

        protected virtual PagingResponseModel<TView> ParseSearchResult(
            SearchRequestDto request,
            PagingResponseModel<TView> result)
        {
            return ParseSearchResult(request, result, null);
        }

        protected virtual PagingResponseModel<TView> ParseSearchResult(
            SearchRequestDto request,
            PagingResponseModel<TView> result,
            JsonSerializer serializer = null)
        {
            return RestApiService.ParseSearchResult(request, result, serializer);
        }

        #endregion

        #region Helpers

        protected virtual SearchQueryModel<TEntity, TPrimaryKey> BuildSearchRequest(SearchRequestDto request)
        {
            return RestApiService.BuildSearchRequest(request);
        }

        protected virtual Task<TView> GetById(TPrimaryKey id, CancellationToken cancellationToken = default)
        {
            return RestApiService.GetById(id);
        }

        #endregion
    }
}

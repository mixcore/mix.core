using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Mix.Identity.Constants;
using Mix.Lib.Interfaces;
using Mix.Lib.Models.Common;

namespace Mix.Lib.Services
{
    public class RestApiService<TView, TDbContext, TEntity, TPrimaryKey> : TenantServiceBase, IRestApiService<TView, TDbContext, TEntity, TPrimaryKey>
        where TPrimaryKey : IComparable
        where TDbContext : DbContext
        where TEntity : EntityBase<TPrimaryKey>
        where TView : ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
    {
        protected readonly MixIdentityService MixIdentityService;
        protected readonly IQueueService<MessageQueueModel> QueueService;
        protected UnitOfWorkInfo Uow;
        protected UnitOfWorkInfo CacheUow;
        protected MixCacheService CacheService;
        public Repository<TDbContext, TEntity, TPrimaryKey, TView> Repository { get; set; }
        protected readonly TDbContext Context;

        public RestApiService(
            IHttpContextAccessor httpContextAccessor,
            MixIdentityService identityService,
            UnitOfWorkInfo<TDbContext> uow,
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor)
        {
            MixIdentityService = identityService;
            Uow = uow;
            CacheService = new();
            QueueService = queueService;
            Context = (TDbContext)uow.ActiveDbContext;
            Repository ??= ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>.GetRepository(Uow);
        }

        #region Command Handlers

        public virtual async Task<TPrimaryKey> CreateHandlerAsync(TView data, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (data == null)
            {
                throw new MixException(MixErrorStatus.Badrequest, "Null Object");
            }
            if (ReflectionHelper.HasProperty(typeof(TView), MixRequestQueryKeywords.TenantId))
            {
                ReflectionHelper.SetPropertyValue(data, new EntityPropertyModel()
                {
                    PropertyName = MixRequestQueryKeywords.TenantId,
                    PropertyValue = CurrentTenant?.Id
                });
            }
            data.SetUowInfo(Uow);
            data.CreatedDateTime = DateTime.UtcNow;
            data.CreatedBy = MixIdentityService.GetClaim(HttpContextAccessor.HttpContext!.User, MixClaims.Username);
            data.ModifiedBy = data.CreatedBy;
            var id = await data.SaveAsync(cancellationToken);
            QueueService.PushQueue(MixQueueTopics.MixViewModelChanged, MixRestAction.Put.ToString(), data);
            return id;
        }

        public virtual async Task UpdateHandler(TPrimaryKey id, TView data, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var currentId = ReflectionHelper.GetPropertyValue(data, "id").ToString();
            if (id.ToString() != currentId)
            {
                throw new MixException(MixErrorStatus.Badrequest, "Invalid Id");
            }
            data.SetUowInfo(Uow);
            await data.SaveAsync(cancellationToken);
            await CacheService.RemoveCacheAsync(id, Repository.CacheFolder, cancellationToken);
            QueueService.PushQueue(MixQueueTopics.MixViewModelChanged, MixRestAction.Put.ToString(), data);
        }

        public virtual async Task DeleteHandler(TView data, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            data.SetUowInfo(Uow);
            await data.DeleteAsync(cancellationToken);
            await CacheService.RemoveCacheAsync(data.Id.ToString(), Repository.CacheFolder, cancellationToken);
            QueueService.PushQueue(MixQueueTopics.MixViewModelChanged, MixRestAction.Put.ToString(), data);
        }


        public virtual async Task PatchHandler(TPrimaryKey id, TView data, IEnumerable<EntityPropertyModel> properties, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            data.SetUowInfo(Uow);
            await data.SaveFieldsAsync(properties, cancellationToken);
            await CacheService.RemoveCacheAsync(id.ToString(), Repository.CacheFolder, cancellationToken);
            QueueService.PushQueue(MixQueueTopics.MixViewModelChanged, MixRestAction.Put.ToString(), data);
        }

        public virtual async Task SaveManyHandler(List<TView> data, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            foreach (var item in data)
            {
                item.SetUowInfo(Uow);
                await item.SaveAsync(cancellationToken);
            }
        }

        #endregion

        #region Query Handlers
        public virtual async Task<PagingResponseModel<TView>> SearchHandler(SearchRequestDto req, SearchQueryModel<TEntity, TPrimaryKey> searchRequest, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await Repository.GetPagingAsync(searchRequest.Predicate, searchRequest.PagingData, cancellationToken);
        }

        public virtual PagingResponseModel<TView> ParseSearchResult(SearchRequestDto req, PagingResponseModel<TView> result, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!string.IsNullOrEmpty(req.Columns))
            {
                Repository.SetSelectedMembers(req.Columns.Replace(" ", string.Empty).Split(','));
            }

            if (!string.IsNullOrEmpty(req.Columns))
            {
                List<TView> objects = new List<TView>();
                foreach (var item in result.Items)
                {
                    objects.Add(ReflectionHelper.GetMembers(item, Repository.SelectedMembers).ToObject<TView>());
                }
                return new PagingResponseModel<TView>()
                {
                    Items = objects,
                    PagingData = result.PagingData
                };
            }
            return result;
        }

        #endregion

        #region Helpers

        public virtual SearchQueryModel<TEntity, TPrimaryKey> BuildSearchRequest(SearchRequestDto req)
        {
            if (!req.PageSize.HasValue)
            {
                req.PageSize = CurrentTenant.Configurations.MaxPageSize;
            }

            return new SearchQueryModel<TEntity, TPrimaryKey>(HttpContextAccessor.HttpContext!.Request, req, CurrentTenant.Id);
        }

        public virtual async Task<TView> GetById(TPrimaryKey id)
        {
            return await Repository.GetSingleAsync(id);
        }

        #endregion
    }
}

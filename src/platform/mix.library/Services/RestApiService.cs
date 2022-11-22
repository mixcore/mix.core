using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Mix.Identity.Constants;
using Mix.Lib.Models.Common;

namespace Mix.Lib.Services
{
    public class RestApiService<TView, TDbContext, TEntity, TPrimaryKey> : TenantServiceBase
        where TPrimaryKey : IComparable
        where TDbContext : DbContext
        where TEntity : EntityBase<TPrimaryKey>
        where TView : ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
    {
        protected readonly MixIdentityService _mixIdentityService;
        protected readonly IQueueService<MessageQueueModel> _queueService;
        protected UnitOfWorkInfo _uow;
        protected UnitOfWorkInfo _cacheUOW;
        protected MixCacheService _cacheService;
        protected readonly Repository<TDbContext, TEntity, TPrimaryKey, TView> _repository;
        protected readonly TDbContext _context;

        public RestApiService(
            IHttpContextAccessor httpContextAccessor,
            MixIdentityService identityService,
            UnitOfWorkInfo<TDbContext> uow,
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor)
        {
            _mixIdentityService = identityService;
            _uow = uow;
            _cacheService = new();
            _queueService = queueService;
            _context = (TDbContext)uow.ActiveDbContext;
            _repository = ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>.GetRepository(_uow);
        }

        #region Command Handlers

        public virtual async Task<TPrimaryKey> CreateHandlerAsync(TView data)
        {
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
            data.SetUowInfo(_uow);
            data.CreatedDateTime = DateTime.UtcNow;
            data.CreatedBy = _mixIdentityService.GetClaim(HttpContextAccessor.HttpContext!.User, MixClaims.Username);
            data.ModifiedBy = data.CreatedBy;
            var id = await data.SaveAsync();
            _queueService.PushMessage(data, MixRestAction.Post.ToString(), true);
            return id;
        }

        public virtual async Task UpdateHandler(TPrimaryKey id, TView data)
        {
            var currentId = ReflectionHelper.GetPropertyValue(data, "id").ToString();
            if (id.ToString() != currentId)
            {
                throw new MixException(MixErrorStatus.Badrequest, "Invalid Id");
            }
            data.SetUowInfo(_uow);
            await data.SaveAsync();
            await _cacheService.RemoveCacheAsync(id, typeof(TView));
            _queueService.PushMessage(data, MixRestAction.Put.ToString(), true);
        }

        public virtual async Task DeleteHandler(TView data)
        {
            data.SetUowInfo(_uow);
            await data.DeleteAsync();
            await _cacheService.RemoveCacheAsync(data.Id.ToString(), typeof(TView));
            _queueService.PushMessage(data, MixRestAction.Delete.ToString(), true);
        }


        public virtual async Task PatchHandler(TPrimaryKey id, TView data, IEnumerable<EntityPropertyModel> properties)
        {
            data.SetUowInfo(_uow);
            await data.SaveFieldsAsync(properties);
            await _cacheService.RemoveCacheAsync(id.ToString(), typeof(TView));
            _queueService.PushMessage(data, MixRestAction.Patch.ToString(), true);
        }

        public virtual async Task SaveManyHandler(List<TView> data)
        {
            foreach (var item in data)
            {
                item.SetUowInfo(_uow);
                await item.SaveAsync();
            }
        }

        #endregion

        #region Query Handlers
        public virtual async Task<PagingResponseModel<TView>> SearchHandler(SearchRequestDto req, SearchQueryModel<TEntity, TPrimaryKey> searchRequest)
        {
            return await _repository.GetPagingAsync(searchRequest.Predicate, searchRequest.PagingData);
        }

        public virtual PagingResponseModel<TView> ParseSearchResult(SearchRequestDto req, PagingResponseModel<TView> result)
        {
            if (!string.IsNullOrEmpty(req.Columns))
            {
                _repository.SetSelectedMembers(req.Columns.Replace(" ", string.Empty).Split(','));
            }

            if (!string.IsNullOrEmpty(req.Columns))
            {
                List<TView> objects = new List<TView>();
                foreach (var item in result.Items)
                {
                    objects.Add(ReflectionHelper.GetMembers(item, _repository.SelectedMembers).ToObject<TView>());
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

            return new SearchQueryModel<TEntity, TPrimaryKey>(CurrentTenant.Id, req, HttpContextAccessor.HttpContext!.Request);
        }

        public virtual async Task<TView> GetById(TPrimaryKey id)
        {
            return await _repository.GetSingleAsync(id);
        }

        #endregion
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mix.Identity.Constants;
using Mix.Identity.Interfaces;
using Mix.Lib.Interfaces;
using Mix.Lib.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        protected MixCacheDbContext _cacheDbContext;
        protected MixCacheService _cacheService;
        protected readonly Repository<TDbContext, TEntity, TPrimaryKey, TView> _repository;
        protected readonly TDbContext _context;

        public RestApiService(
            IHttpContextAccessor httpContextAccessor,
            MixIdentityService identityService,
            UnitOfWorkInfo<TDbContext> uow,
            UnitOfWorkInfo cacheUOW,
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor)
        {
            _mixIdentityService = identityService;
            _uow = uow;
            _cacheUOW = cacheUOW;
            _cacheDbContext = (MixCacheDbContext)cacheUOW.ActiveDbContext;
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
            data.CreatedBy = _mixIdentityService.GetClaim(_httpContextAccessor.HttpContext!.User, MixClaims.Username);
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
            await data.DeleteAsync();
            await _cacheService.RemoveCacheAsync(data.Id.ToString(), typeof(TView));
            _queueService.PushMessage(data, MixRestAction.Delete.ToString(), true);
        }


        public virtual async Task PatchHandler(TPrimaryKey id, TView data, IEnumerable<EntityPropertyModel> properties)
        {
            await data.SaveFieldsAsync(properties);
            await _cacheService.RemoveCacheAsync(id.ToString(), typeof(TView));
            _queueService.PushMessage(data, MixRestAction.Patch.ToString(), true);
        }

        public virtual async Task SaveManyHandler(List<TView> data)
        {
            foreach (var item in data)
            {
                await item.SaveAsync();
            }
        }

        #endregion

        #region Query Handlers
        public virtual async Task<PagingResponseModel<TView>> SearchHandler(SearchRequestDto req)
        {
            var searchRequest = BuildSearchRequest(req);
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

            return new SearchQueryModel<TEntity, TPrimaryKey>(CurrentTenant.Id, req, _httpContextAccessor.HttpContext!.Request);
        }

        public virtual async Task<TView> GetById(TPrimaryKey id)
        {
            return await _repository.GetSingleAsync(id);
        }

        #endregion
    }
}

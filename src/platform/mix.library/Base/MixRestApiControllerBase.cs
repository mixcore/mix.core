using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mix.Identity.Constants;
using Mix.Lib.Dtos;
using Mix.Lib.Models.Common;
using Mix.Lib.Services;
using System.Reflection;

namespace Mix.Lib.Base
{
    public class MixRestHandlerApiControllerBase<TView, TDbContext, TEntity, TPrimaryKey>
        : MixApiControllerBase
        where TPrimaryKey : IComparable
        where TDbContext : DbContext
        where TEntity : EntityBase<TPrimaryKey>
        where TView : ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
    {
        protected readonly Repository<TDbContext, TEntity, TPrimaryKey, TView> _repository;
        protected readonly TDbContext _context;
        protected bool _forbidden;
        protected UnitOfWorkInfo _uow;
        protected UnitOfWorkInfo _cacheUOW;
        protected MixCacheDbContext _cacheDbContext;
        protected MixCacheService _cacheService;

        protected ConstructorInfo classConstructor = typeof(TView).GetConstructor(new Type[] { typeof(TEntity) });

        public MixRestHandlerApiControllerBase(
             IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCacheDbContext> cacheUOW,
            UnitOfWorkInfo<TDbContext> uow,
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, queueService)
        {
            _context = (TDbContext)uow.ActiveDbContext;
            _uow = uow;

            _cacheDbContext = (MixCacheDbContext)cacheUOW.ActiveDbContext;
            _cacheUOW = cacheUOW;
            _cacheService = new();
            _repository = ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>.GetRepository(_uow);
        }

        #region Command Handlers

        protected virtual async Task<TPrimaryKey> CreateHandlerAsync(TView data)
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
            data.CreatedBy = _mixIdentityService.GetClaim(User, MixClaims.Username);
            var id = await data.SaveAsync();
            _queueService.PushMessage(data, MixRestAction.Post.ToString(), true);
            return id;
        }

        protected virtual async Task UpdateHandler(TPrimaryKey id, TView data)
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

        protected virtual async Task DeleteHandler(TView data)
        {
            await data.DeleteAsync();
            await _cacheService.RemoveCacheAsync(data.Id.ToString(), typeof(TView));
            _queueService.PushMessage(data, MixRestAction.Delete.ToString(), true);
        }


        protected virtual async Task PatchHandler(TPrimaryKey id, TView data, IEnumerable<EntityPropertyModel> properties)
        {
            await data.SaveFieldsAsync(properties);
            await _cacheService.RemoveCacheAsync(id.ToString(), typeof(TView));
            _queueService.PushMessage(data, MixRestAction.Patch.ToString(), true);
        }

        protected virtual async Task SaveManyHandler(List<TView> data)
        {
            foreach (var item in data)
            {
                await item.SaveAsync();
            }
        }

        #endregion

        #region Query Handlers
        protected virtual async Task<PagingResponseModel<TView>> SearchHandler(SearchRequestDto req)
        {
            var searchRequest = BuildSearchRequest(req);
            return await _repository.GetPagingAsync(searchRequest.Predicate, searchRequest.PagingData);
        }

        protected virtual ActionResult<PagingResponseModel<TView>> ParseSearchResult(SearchRequestDto req, PagingResponseModel<TView> result)
        {
            if (!string.IsNullOrEmpty(req.Columns))
            {
                _repository.SetSelectedMembers(req.Columns.Replace(" ", string.Empty).Split(','));
            }

            if (!string.IsNullOrEmpty(req.Columns))
            {
                List<object> objects = new List<object>();
                foreach (var item in result.Items)
                {
                    objects.Add(ReflectionHelper.GetMembers(item, _repository.SelectedMembers));
                }
                return Ok(new PagingResponseModel<object>()
                {
                    Items = objects,
                    PagingData = result.PagingData
                });
            }
            return result;
        }

        #endregion

        #region Helpers

        protected virtual SearchQueryModel<TEntity, TPrimaryKey> BuildSearchRequest(SearchRequestDto req)
        {
            if (!req.PageSize.HasValue)
            {
                req.PageSize = CurrentTenant.Configurations.MaxPageSize;
            }

            return new SearchQueryModel<TEntity, TPrimaryKey>(CurrentTenant.Id, req, Request);
        }

        protected virtual async Task<TView> GetById(TPrimaryKey id)
        {
            return await _repository.GetSingleAsync(id);
        }

        #endregion
    }
}

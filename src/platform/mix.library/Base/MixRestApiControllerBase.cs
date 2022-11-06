using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mix.Lib.Models.Common;
using Mix.Lib.Services;
using System.Reflection;

namespace Mix.Lib.Base
{
    public class MixRestHandlerApiControllerBase<TView, TDbContext, TEntity, TPrimaryKey>
        : MixTenantApiControllerBase
        where TPrimaryKey : IComparable
        where TDbContext : DbContext
        where TEntity : EntityBase<TPrimaryKey>
        where TView : ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
    {
        protected readonly Repository<TDbContext, TEntity, TPrimaryKey, TView> _repository;
        protected readonly TDbContext _context;
        protected bool _forbidden;
        protected UnitOfWorkInfo _uow;
        protected readonly RestApiService<TView, TDbContext, TEntity, TPrimaryKey> _restApiService;
        protected ConstructorInfo classConstructor = typeof(TView).GetConstructor(new Type[] { typeof(TEntity) });

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
            _context = (TDbContext)uow.ActiveDbContext;
            _restApiService = new(httpContextAccessor, mixIdentityService, uow, queueService);
            _uow = uow;
            _repository = ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>.GetRepository(_uow);
        }

        #region Command Handlers

        protected virtual async Task<TPrimaryKey> CreateHandlerAsync(TView data)
        {
            return await _restApiService.CreateHandlerAsync(data); ;
        }

        protected virtual Task UpdateHandler(TPrimaryKey id, TView data)
        {
            return _restApiService.UpdateHandler(id, data);
        }

        protected virtual Task DeleteHandler(TView data)
        {
            return _restApiService.DeleteHandler(data);
        }


        protected virtual Task PatchHandler(TPrimaryKey id, TView data, IEnumerable<EntityPropertyModel> properties)
        {
            return _restApiService.PatchHandler(id, data, properties);
        }

        protected virtual Task SaveManyHandler(List<TView> data)
        {
            return _restApiService.SaveManyHandler(data);
        }

        #endregion

        #region Query Handlers
        protected virtual Task<PagingResponseModel<TView>> SearchHandler(SearchRequestDto req)
        {
            return _restApiService.SearchHandler(req);
        }

        protected virtual ActionResult<PagingResponseModel<TView>> ParseSearchResult(SearchRequestDto req, PagingResponseModel<TView> result)
        {
            return _restApiService.ParseSearchResult(req, result);
        }

        #endregion

        #region Helpers

        protected virtual SearchQueryModel<TEntity, TPrimaryKey> BuildSearchRequest(SearchRequestDto req)
        {
            return _restApiService.BuildSearchRequest(req);
        }

        protected virtual Task<TView> GetById(TPrimaryKey id)
        {
            return _restApiService.GetById(id);
        }

        #endregion
    }
}

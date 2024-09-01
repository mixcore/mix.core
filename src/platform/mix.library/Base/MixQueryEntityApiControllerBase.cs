using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mix.Lib.Interfaces;
using Mix.Lib.Models.Common;
using Mix.Lib.Services;
using Mix.Mq.Lib.Models;
using NuGet.Protocol.Plugins;
using System.Linq.Expressions;

namespace Mix.Lib.Base
{
    public class MixQueryEntityApiControllerBase<TDbContext, TEntity, TPrimaryKey>
        : MixTenantApiControllerBase
        where TPrimaryKey : IComparable
        where TEntity : class, IEntity<TPrimaryKey>
        where TDbContext : DbContext
    {
        protected readonly EntityRepository<TDbContext, TEntity, TPrimaryKey> Repository;
        protected readonly TDbContext Context;
        protected UnitOfWorkInfo Uow;
        protected UnitOfWorkInfo CacheUow;
        protected MixCacheDbContext CacheDbContext;

        public MixQueryEntityApiControllerBase(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            TDbContext context,
            IMemoryQueueService<MessageQueueModel> queueService, MixCacheDbContext cacheDbContext,
            IMixTenantService mixTenantService)
            : base(httpContextAccessor,
                  configuration, cacheService, translator, mixIdentityService, queueService, mixTenantService)
        {
            Context = context;
            Uow = new(Context);
            Repository = new(Uow);
            CacheDbContext = cacheDbContext;
            CacheUow = new(CacheDbContext);
        }

        #region Overrides
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (Uow.ActiveTransaction != null)
            {
                Uow.Complete();
            }
            Context.Dispose();

            if (CacheUow.ActiveTransaction != null)
            {
                CacheUow.Complete();
            }
            CacheDbContext.Dispose();
            base.OnActionExecuted(context);
        }
        #endregion

        #region Routes

        [HttpGet]
        public virtual async Task<ActionResult<PagingResponseModel<TEntity>>> Get([FromQuery] SearchRequestDto req)
        {
            var result = await GetHandler(req);
            return Ok(result);
        }

        protected virtual async Task<PagingResponseModel<TEntity>> GetHandler(SearchRequestDto req)
        {
            var searchRequest = BuildSearchRequest(req);
            if (!string.IsNullOrEmpty(req.Columns))
            {
                Repository.SetSelectedMembers(req.Columns.Replace(" ", string.Empty).Split(','));
            }
            return await Repository.GetPagingAsync(searchRequest.Predicate, searchRequest.PagingData);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TEntity>> GetSingle(TPrimaryKey id)
        {
            var data = await GetById(id);
            return data != null ? Ok(data) : NotFound(id);
        }



        [HttpGet("default")]
        public ActionResult<TEntity> GetDefault()
        {
            var result = (TEntity)Activator.CreateInstance(typeof(TEntity), Uow);
            return Ok(result);
        }

        #endregion Routes

        #region Helpers

        protected virtual SearchEntityModel<TEntity, TPrimaryKey> BuildSearchRequest(SearchRequestDto req)
        {
            if (!req.PageSize.HasValue)
            {
                req.PageSize = CurrentTenant.Configurations.MaxPageSize;
            }

            var andPredicate = BuildAndPredicate(req);

            return new SearchEntityModel<TEntity, TPrimaryKey>(req, andPredicate);
        }

        protected virtual Expression<Func<TEntity, bool>> BuildAndPredicate(SearchRequestDto req)
        {
            Expression<Func<TEntity, bool>> andPredicate = m => true;

            if (req.Culture != null)
            {
                andPredicate = andPredicate.AndAlso(ReflectionHelper.GetExpression<TEntity>(
                        MixRequestQueryKeywords.Specificulture, req.Culture, ExpressionMethod.Equal));
            }

            if (ReflectionHelper.HasProperty(typeof(TEntity), MixRequestQueryKeywords.TenantId))
            {
                andPredicate = ReflectionHelper.GetExpression<TEntity>(
                        MixRequestQueryKeywords.TenantId, CurrentTenant.Id, ExpressionMethod.Equal);
            }

            return andPredicate;
        }

        protected virtual async Task<TEntity> GetById(TPrimaryKey id)
        {
            return await Repository.GetSingleAsync(id);
        }

        #endregion
    }
}

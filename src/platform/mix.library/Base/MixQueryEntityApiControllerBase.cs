using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mix.Lib.Models.Common;
using Mix.Lib.Services;
using System.Linq.Expressions;
using System.Reflection;

namespace Mix.Lib.Base
{
    public class MixQueryEntityApiControllerBase<TDbContext, TEntity, TPrimaryKey>
        : MixApiControllerBase
        where TPrimaryKey : IComparable
        where TEntity : class, IEntity<TPrimaryKey>
        where TDbContext : DbContext
    {
        protected readonly EntityRepository<TDbContext, TEntity, TPrimaryKey> _repository;
        protected readonly TDbContext _context;
        protected bool _forbidden;
        protected UnitOfWorkInfo _uow;
        protected UnitOfWorkInfo _cacheUOW;
        protected MixCacheDbContext _cacheDbContext;
        protected ConstructorInfo classConstructor = typeof(TEntity).GetConstructor(Array.Empty<Type>());
        protected MixCacheService _cacheService;
        public MixQueryEntityApiControllerBase(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixIdentityService mixIdentityService,
            TDbContext context,
            IQueueService<MessageQueueModel> queueService, MixCacheDbContext cacheDbContext)
            : base(httpContextAccessor, configuration, mixService, translator, cultureRepository, mixIdentityService, queueService)
        {
            _context = context;
            _uow = new(_context);
            _repository = new(_uow);
            _cacheDbContext = cacheDbContext;
            _cacheUOW = new(_cacheDbContext);
            _cacheService = new();
        }

        #region Overrides
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (_uow.ActiveTransaction != null)
            {
                _uow.Complete();
            }
            _context.Dispose();

            if (_cacheUOW.ActiveTransaction != null)
            {
                _cacheUOW.Complete();
            }
            _cacheDbContext.Dispose();
            base.OnActionExecuted(context);
        }
        #endregion

        #region Routes

        [HttpGet]
        public virtual async Task<ActionResult<PagingResponseModel<TEntity>>> Get([FromQuery] SearchRequestDto req)
        {
            var searchRequest = BuildSearchRequest(req);
            if (!string.IsNullOrEmpty(req.Columns))
            {
                _repository.SetSelectedMembers(req.Columns.Replace(" ", string.Empty).Split(','));
            }
            var result = await _repository.GetPagingAsync(searchRequest.Predicate, searchRequest.PagingData);
            if (!string.IsNullOrEmpty(req.Columns))
            {
                List<object> objects = new();
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

        [HttpGet("{id}")]
        public async Task<ActionResult<TEntity>> GetSingle(TPrimaryKey id)
        {
            var data = await GetById(id);
            return data != null ? Ok(data) : NotFound(id);
        }



        [HttpGet("default")]
        public ActionResult<TEntity> GetDefault()
        {
            var result = (TEntity)Activator.CreateInstance(typeof(TEntity), new[] { _uow });
            return Ok(result);
        }

        #endregion Routes

        #region Helpers



        protected virtual SearchEntityModel<TEntity, TPrimaryKey> BuildSearchRequest(SearchRequestDto req)
        {
            if (!req.PageSize.HasValue)
            {
                req.PageSize = GlobalConfigService.Instance.AppSettings.MaxPageSize;
            }

            Expression<Func<TEntity, bool>> andPredicate = BuildAndPredicate(req);

            return new SearchEntityModel<TEntity, TPrimaryKey>(req, andPredicate);
        }

        protected virtual Expression<Func<TEntity, bool>> BuildAndPredicate(SearchRequestDto req)
        {
            Expression<Func<TEntity, bool>> andPredicate = m => true;

            if (req.Culture != null)
            {
                andPredicate = andPredicate.AndAlso(ReflectionHelper.GetExpression<TEntity>(
                        MixRequestQueryKeywords.Specificulture, req.Culture, Heart.Enums.ExpressionMethod.Equal));
            }

            if (ReflectionHelper.HasProperty(typeof(TEntity), MixRequestQueryKeywords.TenantId))
            {
                andPredicate = ReflectionHelper.GetExpression<TEntity>(
                        MixRequestQueryKeywords.TenantId, MixTenantId, ExpressionMethod.Equal);
            }

            return andPredicate;
        }

        protected virtual async Task<TEntity> GetById(TPrimaryKey id)
        {
            return await _repository.GetSingleAsync(id);
        }

        #endregion
    }
}

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
    public class MixQueryApiControllerBase<TView, TDbContext, TEntity, TPrimaryKey>
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
        protected ConstructorInfo classConstructor = typeof(TView).GetConstructor(new Type[] { typeof(TEntity) });

        public MixQueryApiControllerBase(
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixIdentityService mixIdentityService,
            TDbContext context,
            IQueueService<MessageQueueModel> queueService)
            : base(configuration, mixService, translator, cultureRepository, mixIdentityService, queueService)
        {
            _context = context;
            _uow = new(_context);
            _repository = ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>.GetRepository(_uow);
        }

        #region Overrides
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (_uow.ActiveTransaction != null)
            {
                _uow.Complete();
            }
            _context.Dispose();
            base.OnActionExecuted(context);
        }
        #endregion

        #region Routes

        [HttpGet]
        public virtual async Task<ActionResult<PagingResponseModel<TView>>> Get([FromQuery] SearchRequestDto req)
        {
            var searchRequest = BuildSearchRequest(req);
            if (!string.IsNullOrEmpty(req.Columns))
            {
                _repository.SetSelectedMembers(req.Columns.Replace(" ", string.Empty).Split(','));
            }
            var result = await _repository.GetPagingAsync(searchRequest.Predicate, searchRequest.PagingData);
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

        [HttpGet("{id}")]
        public async Task<ActionResult<TView>> GetSingle(TPrimaryKey id)
        {
            var data = await GetById(id);
            return data != null ? Ok(data) : NotFound(id);
        }



        [HttpGet("default")]
        [HttpGet("{lang}/default")]
        public ActionResult<TView> GetDefault(string culture = null)
        {
            var result = (TView)Activator.CreateInstance(typeof(TView), new[] { _uow });
            result.InitDefaultValues(_lang, _culture.Id);
            result.ExpandView();
            return Ok(result);
        }

        #endregion Routes

        #region Helpers



        protected virtual SearchQueryModel<TEntity, TPrimaryKey> BuildSearchRequest(SearchRequestDto req)
        {
            Expression<Func<TEntity, bool>> andPredicate = null;

            if (!req.PageSize.HasValue)
            {
                req.PageSize = GlobalConfigService.Instance.AppSettings.MaxPageSize;
            }

            if (req.Culture != null)
            {
                andPredicate = ReflectionHelper.GetExpression<TEntity>(
                        MixRequestQueryKeywords.Specificulture, req.Culture, Heart.Enums.ExpressionMethod.Eq);
            }

            if (ReflectionHelper.HasProperty(typeof(TEntity), MixRequestQueryKeywords.MixTenantId))
            {
                andPredicate = ReflectionHelper.GetExpression<TEntity>(
                        MixRequestQueryKeywords.MixTenantId, MixTenantRepository.Instance.CurrentTenant.Id, ExpressionMethod.Eq);
            }

            return new SearchQueryModel<TEntity, TPrimaryKey>(req, andPredicate);
        }

        protected virtual async Task<TView> GetById(TPrimaryKey id)
        {
            return await _repository.GetSingleAsync(id);
        }

        #endregion
    }
}

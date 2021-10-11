using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Mix.Heart.Repository;
using Mix.Heart.Entities;
using Mix.Lib.Dtos;
using Mix.Lib.Models.Common;
using Mix.Heart.ViewModel;
using Mix.Shared.Services;
using System.Reflection;
using Mix.Heart.Model;
using Mix.Shared.Constants;
using Mix.Heart.Helpers;
using System.Linq.Expressions;
using Mix.Database.Entities.Cms;
using Microsoft.Extensions.Logging;
using Mix.Lib.Services;
using Mix.Heart.UnitOfWork;

namespace Mix.Lib.Abstracts
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
        protected const int _defaultPageSize = 1000;
        protected bool _forbidden;
        protected UnitOfWorkInfo _uow;
        protected ConstructorInfo classConstructor = typeof(TView).GetConstructor(new Type[] { typeof(TEntity) });

        public MixQueryApiControllerBase(
            ILogger<MixApiControllerBase> logger,
            GlobalConfigService globalConfigService,
            MixService mixService, 
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixIdentityService mixIdentityService,
            TDbContext context) 
            : base(logger, globalConfigService, mixService, translator, cultureRepository, mixIdentityService)
        {
            _context = context;
            _uow = new(_context);
            _repository = ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>.GetRepository(_uow);
        }

        #region Routes

        [HttpGet]
        public virtual async Task<ActionResult<PagingResponseModel<TView>>> Get([FromQuery] SearchRequestDto req)
        {
            var searchRequest = BuildSearchRequest(req);

            return await _repository.GetPagingAsync(searchRequest.Predicate, searchRequest.PagingData);
        }

        protected virtual SearchQueryModel<TEntity, TPrimaryKey> BuildSearchRequest(SearchRequestDto req)
        {
            Expression<Func<TEntity, bool>> andPredicate = null;

            if (!req.PageSize.HasValue)
            {
                req.PageSize = _globalConfigService.GetConfig(MixAppSettingKeywords.MaxPageSize, _defaultPageSize);
            }

            if (req.Culture != null)
            {
                andPredicate = ReflectionHelper.GetExpression<TEntity>(
                        MixRequestQueryKeywords.Specificulture, req.Culture, Heart.Enums.ExpressionMethod.Eq);
            }
            return new SearchQueryModel<TEntity, TPrimaryKey>(req, andPredicate);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TView>> GetSingle(TPrimaryKey id)
        {
            var getData = await _repository.GetSingleAsync(id);
            return Ok(getData);
        }

        [HttpGet("default")]
        [HttpGet("{lang}/default")]
        public ActionResult<TView> GetDefault(string culture = null)
        {
            var result = (TView)Activator.CreateInstance(typeof(TView), new[] { _uow});
            result.InitDefaultValues(_lang, _culture.Id);
            result.ExpandView(_uow);
            return Ok(result);
        }

        #endregion Routes
    }
}

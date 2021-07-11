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
using Mix.Shared.Enums;
using Mix.Shared.Constants;
using Microsoft.AspNetCore.Mvc.Filters;
using Mix.Heart.Helpers;
using System.Linq.Expressions;
using Mix.Database.Entities.Cms.v2;

namespace Mix.Lib.Abstracts
{
    public class MixQueryApiControllerBase<TView, TDbContext, TEntity, TPrimaryKey> : Controller
        where TPrimaryKey : IComparable
        where TDbContext : DbContext
        where TEntity : EntityBase<TPrimaryKey>
        where TView : ViewModelBase<TDbContext, TEntity, TPrimaryKey>
    {
        protected readonly MixAppSettingService _appSettingService;
        protected readonly Repository<TDbContext, TEntity, TPrimaryKey> _repository;
        protected readonly Repository<MixCmsContext, MixCulture, int> _cultureRepository;
        protected const int _defaultPageSize = 1000;
        protected bool _forbidden;
        protected string _lang;
        protected MixCulture _culture;
        protected ConstructorInfo classConstructor = typeof(TView).GetConstructor(new Type[] { typeof(TEntity) });

        public MixQueryApiControllerBase(
            MixAppSettingService appSettingService,
            Repository<TDbContext, TEntity, TPrimaryKey> repository, 
            Repository<MixCmsContext, MixCulture, int> cultureRepository) : base()
        {
            _appSettingService = appSettingService;
            _repository = repository;
            _cultureRepository = cultureRepository;
        }

        #region Routes

        [HttpGet]
        public virtual async Task<ActionResult<PagingResponseModel<TView>>> Get([FromQuery] SearchRequestDto req)
        {
            Expression<Func<TEntity, bool>> andPredicate = null;

            if (!req.PageSize.HasValue)
            {
                req.PageSize = _appSettingService.GetConfig(
                    MixAppSettingsSection.GlobalSettings, MixAppSettingKeywords.MaxPageSize, _defaultPageSize);
            }
            
            if (req.Culture != null)
            {
                andPredicate = ReflectionHelper.GetExpression<TEntity>(
                        MixRequestQueryKeywords.Specificulture, req.Culture, Heart.Enums.ExpressionMethod.Eq);
            }
            var searchRequest = new SearchQueryModel<TEntity, TPrimaryKey>(req, andPredicate);

            return await _repository.GetPagingViewAsync<TView>(searchRequest.Predicate, searchRequest.PagingData);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TView>> GetSingle(TPrimaryKey id)
        {
            var getData = await _repository.GetSingleViewAsync<TView>(id);
            return Ok(getData);
        }

        [HttpGet("default")]
        [HttpGet("{culture}/default")]
        public ActionResult<TView> GetDefault(string culture = null)
        {
            var result = (TView)Activator.CreateInstance(typeof(TView));
            result.InitDefaultValues(_lang, _culture.Id);
            return Ok(result);
        }

        #endregion Routes

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            _lang = RouteData?.Values["culture"] != null
                ? RouteData.Values["culture"].ToString()
                : _appSettingService.GetConfig<string>(MixAppSettingsSection.GlobalSettings, MixAppSettingKeywords.DefaultCulture);
            _culture = _cultureRepository.GetSingleAsync(c => c.Specificulture == _lang).GetAwaiter().GetResult();
        }
    }
}

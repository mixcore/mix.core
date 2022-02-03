using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Services;
using Mix.Common.Helper;
using Mix.Heart.Infrastructure.Repositories;
using Mix.Heart.Models;
using Mix.Heart.Extensions;
using Mix.Heart.Helpers;
using Mix.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.Controllers
{
    [Produces("application/json")]
    public class BaseReadOnlyApiController<TDbContext, TModel, TView> : Controller
        where TDbContext : DbContext
        where TModel : class
        where TView : Mix.Heart.Infrastructure.ViewModels.ViewModelBase<TDbContext, TModel, TView>
    {
        protected static TDbContext _context;
        protected static IDbContextTransaction _transaction;
        protected string _lang;
        protected bool _forbidden;
        protected DefaultRepository<TDbContext, TModel, TView> _repo;

        /// <summary>
        /// The domain
        /// </summary>
        protected string _domain;

        public BaseReadOnlyApiController(DefaultRepository<TDbContext, TModel, TView> repo)
        {
            _repo = repo;   
        }

        #region Routes

        [HttpGet]
        public virtual async Task<ActionResult<PaginationModel<TView>>> Get()
        {
            bool isFromDate = DateTime.TryParse(Request.Query[MixRequestQueryKeywords.FromDate], out DateTime fromDate);
            bool isToDate = DateTime.TryParse(Request.Query[MixRequestQueryKeywords.ToDate], out DateTime toDate);
            int.TryParse(Request.Query[MixRequestQueryKeywords.PageIndex], out int pageIndex);
            bool isDirection = Enum.TryParse(Request.Query[MixRequestQueryKeywords.Direction], out Heart.Enums.DisplayDirection direction);
            bool isPageSize = int.TryParse(Request.Query[MixRequestQueryKeywords.PageSize], out int pageSize);

            RequestPaging request = new RequestPaging()
            {
                PageIndex = pageIndex,
                PageSize = isPageSize ? pageSize : 100,
                OrderBy = Request.Query[MixRequestQueryKeywords.OrderBy].ToString().ToTitleCase(),
                Direction = direction
            };
            Expression<Func<TModel, bool>> predicate = null;
            RepositoryResponse<PaginationModel<TView>> getData = null;
            if (!string.IsNullOrEmpty(_lang))
            {
                predicate = ReflectionHelper.GetExpression<TModel>(MixQueryColumnName.Specificulture, _lang, Heart.Enums.ExpressionMethod.Eq);
                getData = await _repo.GetModelListByAsync(
                            predicate,
                            request.OrderBy, request.Direction,
                            request.PageSize, request.PageIndex, null, null)
                    .ConfigureAwait(false);
            }
            else
            {
                getData = await _repo.GetModelListAsync(
                request.OrderBy, request.Direction, request.PageSize, request.PageIndex, null, null).ConfigureAwait(false);
            }

            if (getData.IsSucceed)
            {
                return Ok(getData.Data);
            }
            else
            {
                return BadRequest(getData.Errors);
            }
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult<TView>> Get(string id)
        {
            var getData = await GetSingleAsync(id);
            if (getData.IsSucceed)
            {
                return getData.Data;
            }
            else
            {
                return NoContent();
            }
        }

        [HttpGet("default")]
        public virtual ActionResult<TView> Default()
        {
            using (TDbContext context = UnitOfWorkHelper<TDbContext>.InitContext())
            {
                var transaction = context.Database.BeginTransaction();
                TView data = ReflectionHelper.InitModel<TView>();
                ReflectionHelper.SetPropertyValue(data, new JProperty(MixQueryColumnName.Specificulture, _lang));
                ReflectionHelper.SetPropertyValue(data, new JProperty(MixQueryColumnName.Status, MixService.GetAppSetting<string>(MixAppSettingKeywords.DefaultContentStatus)));
                data.ExpandView(context, transaction);
                return Ok(data);
            }
        }

        [HttpGet("remove-cache/{id}")]
        public virtual async Task<ActionResult> ClearCacheAsync(string id)
        {
            string key = $"_{id}";
            key += !string.IsNullOrEmpty(_lang) ? $"_{_lang}" : string.Empty;
            await MixCacheService.RemoveCacheAsync(typeof(TModel), key);
            return NoContent();
        }

        [HttpGet("remove-cache")]
        public virtual async Task<ActionResult> ClearCacheAsync()
        {
            await MixCacheService.RemoveCacheAsync(typeof(TModel));
            return NoContent();
        }

        #endregion Routes

        #region Overrides

        /// <summary>
        /// Called before the action method is invoked.
        /// </summary>
        /// <param name="context">The action executing context.</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            GetLanguage();
            if (MixService.GetIpConfig<bool>("IsRetrictIp"))
            {
                var allowedIps = MixService.GetIpConfig<JArray>("AllowedIps") ?? new JArray();
                var exceptIps = MixService.GetIpConfig<JArray>("ExceptIps") ?? new JArray();
                string remoteIp = Request.HttpContext?.Connection?.RemoteIpAddress?.ToString();
                if (
                    // allow localhost
                    //remoteIp != "::1" &&
                    (!allowedIps.Any(t => t.Value<string>() == "*") && !allowedIps.Contains(remoteIp)) ||
                    (exceptIps.Any(t => t.Value<string>() == remoteIp))
                    )
                {
                    _forbidden = true;
                }
            }
            base.OnActionExecuting(context);
        }

        protected void GetLanguage()
        {
            _lang = RouteData?.Values["culture"] != null ? RouteData.Values["culture"].ToString() : string.Empty;
            _domain = string.Format("{0}://{1}", Request.Scheme, Request.Host);
        }

        #endregion Overrides

        #region Helpers

        protected virtual async Task<RepositoryResponse<T>> GetSingleAsync<T>(string id)
            where T : Mix.Heart.Infrastructure.ViewModels.ViewModelBase<TDbContext, TModel, T>
        {
            Expression<Func<TModel, bool>> predicate = ReflectionHelper.GetExpression<TModel>(MixQueryColumnName.Id, id, Heart.Enums.ExpressionMethod.Eq);
            if (!string.IsNullOrEmpty(_lang))
            {
                var idPre = ReflectionHelper.GetExpression<TModel>(MixQueryColumnName.Specificulture, _lang, Heart.Enums.ExpressionMethod.Eq);
                predicate = predicate.AndAlso(idPre);
            }

            return await GetSingleAsync<T>(predicate);
        }

        protected virtual async Task<RepositoryResponse<TView>> GetSingleAsync(string id)
        {
            Expression<Func<TModel, bool>> predicate = ReflectionHelper.GetExpression<TModel>(MixQueryColumnName.Id, id, Heart.Enums.ExpressionMethod.Eq);
            if (!string.IsNullOrEmpty(_lang))
            {
                var idPre = ReflectionHelper.GetExpression<TModel>(MixQueryColumnName.Specificulture, _lang, Heart.Enums.ExpressionMethod.Eq);
                predicate = predicate.AndAlso(idPre);
            }

            return await GetSingleAsync(predicate);
        }

        protected async Task<RepositoryResponse<TView>> GetSingleAsync(Expression<Func<TModel, bool>> predicate = null)
        {
            RepositoryResponse<TView> data = null;
            if (predicate != null)
            {
                data = await _repo.GetSingleModelAsync(predicate);
            }
            return data;
        }

        protected async Task<RepositoryResponse<T>> GetSingleAsync<T>(Expression<Func<TModel, bool>> predicate = null)
            where T : Mix.Heart.Infrastructure.ViewModels.ViewModelBase<TDbContext, TModel, T>
        {
            RepositoryResponse<T> data = null;
            if (predicate != null)
            {
                data = await DefaultRepository<TDbContext, TModel, T>.Instance.GetSingleModelAsync(predicate);
            }
            return data;
        }

        protected async Task<RepositoryResponse<PaginationModel<TView>>> GetListAsync(Expression<Func<TModel, bool>> predicate = null)
        {
            bool isFromDate = DateTime.TryParse(Request.Query[MixRequestQueryKeywords.FromDate], out DateTime fromDate);
            bool isToDate = DateTime.TryParse(Request.Query[MixRequestQueryKeywords.ToDate], out DateTime toDate);
            int.TryParse(Request.Query[MixRequestQueryKeywords.PageIndex], out int pageIndex);
            bool isDirection = Enum.TryParse(Request.Query[MixRequestQueryKeywords.Direction], out Heart.Enums.DisplayDirection direction);
            bool isPageSize = int.TryParse(Request.Query[MixRequestQueryKeywords.PageSize], out int pageSize);

            RequestPaging request = new RequestPaging()
            {
                PageIndex = pageIndex,
                PageSize = isPageSize ? pageSize : 100,
                OrderBy = Request.Query[MixRequestQueryKeywords.OrderBy].ToString().ToTitleCase(),
                Direction = direction
            };

            RepositoryResponse<PaginationModel<TView>> data = null;

            if (data == null)
            {
                if (predicate != null)
                {
                    data = await _repo.GetModelListByAsync(
                        predicate, request.OrderBy, request.Direction, request.PageSize, request.PageIndex, null, null);
                }
                else
                {
                    data = await _repo.GetModelListAsync(request.OrderBy, request.Direction, request.PageSize, request.PageIndex, null, null).ConfigureAwait(false);
                }
            }
            return data;
        }

        #endregion Helpers
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Services;
using Mix.Common.Helper;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.Repository;
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
        where TView : Mix.Domain.Data.ViewModels.ViewModelBase<TDbContext, TModel, TView>
    {
        protected static TDbContext _context;
        protected static IDbContextTransaction _transaction;
        protected string _lang;
        protected bool _forbidden;

        /// <summary>
        /// The domain
        /// </summary>
        protected string _domain;
        #region Routes
        [HttpGet]
        public virtual async Task<ActionResult<PaginationModel<TView>>> Get()
        {
            bool isFromDate = DateTime.TryParse(Request.Query["fromDate"], out DateTime fromDate);
            bool isToDate = DateTime.TryParse(Request.Query["toDate"], out DateTime toDate);
            int.TryParse(Request.Query["pageIndex"], out int pageIndex);
            bool isDirection = Enum.TryParse(Request.Query["direction"], out Heart.Enums.MixHeartEnums.DisplayDirection direction);
            bool isPageSize = int.TryParse(Request.Query["pageSize"], out int pageSize);

            RequestPaging request = new RequestPaging()
            {
                PageIndex = pageIndex,
                PageSize = isPageSize ? pageSize : 100,
                OrderBy = Request.Query["orderBy"].ToString().ToTitleCase(),
                Direction = direction
            };
            Expression<Func<TModel, bool>> predicate = null;
            RepositoryResponse<PaginationModel<TView>> getData = null;
            if (!string.IsNullOrEmpty(_lang))
            {
                predicate = ReflectionHelper.GetExpression<TModel>("Specificulture", _lang, Heart.Enums.MixHeartEnums.ExpressionMethod.Eq);
                getData = await DefaultRepository<TDbContext, TModel, TView>.Instance.GetModelListByAsync(
                            predicate,
                            request.OrderBy, request.Direction,
                            request.PageSize, request.PageIndex, null, null)
                    .ConfigureAwait(false);
            }
            else
            {
                getData = await DefaultRepository<TDbContext, TModel, TView>.Instance.GetModelListAsync(
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

        // GET: api/v1/rest/{culture}/attribute-set-data/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TView>> Get(string id)
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

        // GET: api/v1/rest/{culture}/attribute-set-data/5
        [HttpGet("default")]
        public virtual ActionResult<TView> Default()
        {
            using (TDbContext context = UnitOfWorkHelper<TDbContext>.InitContext())
            {
                var transaction = context.Database.BeginTransaction();
                TView data = ReflectionHelper.InitModel<TView>();
                ReflectionHelper.SetPropertyValue(data, new JProperty("Specificulture", _lang));
                ReflectionHelper.SetPropertyValue(data, new JProperty("Status", MixService.GetConfig<string>("DefaultContentStatus")));
                data.ExpandView(context, transaction);
                return Ok(data);
            }
        }

        [HttpGet("remove-cache/{id}")]
        public virtual async Task<ActionResult> ClearCacheAsync(string id)
        {
            string key = $"_{id}";
            key += !string.IsNullOrEmpty(_lang) ? $"_{_lang}" : string.Empty;
            await MixService.RemoveCacheAsync(typeof(TModel), key);
            return NoContent();
        }

        [HttpGet("remove-cache")]
        public virtual async Task<ActionResult> ClearCacheAsync()
        {
            await MixService.RemoveCacheAsync(typeof(TModel));
            return NoContent();
        }
        #endregion

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
        protected async Task<RepositoryResponse<T>> GetSingleAsync<T>(string id)
            where T : Mix.Domain.Data.ViewModels.ViewModelBase<TDbContext, TModel, T>
        {
            Expression<Func<TModel, bool>> predicate = ReflectionHelper.GetExpression<TModel>("Id", id, Heart.Enums.MixHeartEnums.ExpressionMethod.Eq);
            if (!string.IsNullOrEmpty(_lang))
            {
                var idPre = ReflectionHelper.GetExpression<TModel>("Specificulture", _lang, Heart.Enums.MixHeartEnums.ExpressionMethod.Eq);
                predicate = predicate.AndAlso(idPre);
            }

            return await GetSingleAsync<T>(predicate);
        }

        protected async Task<RepositoryResponse<TView>> GetSingleAsync(string id)
        {
            Expression<Func<TModel, bool>> predicate = ReflectionHelper.GetExpression<TModel>("Id", id, Heart.Enums.MixHeartEnums.ExpressionMethod.Eq);
            if (!string.IsNullOrEmpty(_lang))
            {
                var idPre = ReflectionHelper.GetExpression<TModel>("Specificulture", _lang, Heart.Enums.MixHeartEnums.ExpressionMethod.Eq);
                predicate = predicate.AndAlso(idPre);
            }

            return await GetSingleAsync(predicate);
        }

        protected async Task<RepositoryResponse<TView>> GetSingleAsync(Expression<Func<TModel, bool>> predicate = null)
        {
            RepositoryResponse<TView> data = null;
            if (predicate != null)
            {
                data = await DefaultRepository<TDbContext, TModel, TView>.Instance.GetSingleModelAsync(predicate);
            }
            return data;
        }

        protected async Task<RepositoryResponse<T>> GetSingleAsync<T>(Expression<Func<TModel, bool>> predicate = null)
            where T : Mix.Domain.Data.ViewModels.ViewModelBase<TDbContext, TModel, T>
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
            bool isFromDate = DateTime.TryParse(Request.Query["fromDate"], out DateTime fromDate);
            bool isToDate = DateTime.TryParse(Request.Query["toDate"], out DateTime toDate);
            int.TryParse(Request.Query["pageIndex"], out int pageIndex);
            bool isDirection = Enum.TryParse(Request.Query["direction"], out Heart.Enums.MixHeartEnums.DisplayDirection direction);
            bool isPageSize = int.TryParse(Request.Query["pageSize"], out int pageSize);

            RequestPaging request = new RequestPaging()
            {
                PageIndex = pageIndex,
                PageSize = isPageSize ? pageSize : 100,
                OrderBy = Request.Query["orderBy"].ToString().ToTitleCase(),
                Direction = direction
            };

            RepositoryResponse<PaginationModel<TView>> data = null;

            if (data == null)
            {

                if (predicate != null)
                {
                    data = await DefaultRepository<TDbContext, TModel, TView>.Instance.GetModelListByAsync(
                        predicate, request.OrderBy, request.Direction, request.PageSize, request.PageIndex, null, null);
                }
                else
                {
                    data = await DefaultRepository<TDbContext, TModel, TView>.Instance.GetModelListAsync(request.OrderBy, request.Direction, request.PageSize, request.PageIndex, null, null).ConfigureAwait(false);

                }

            }
            return data;
        }

        #endregion

    }

}
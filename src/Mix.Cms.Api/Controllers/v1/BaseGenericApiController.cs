using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Mix.Cms.Hub;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Services;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.Repository;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Api.Controllers.v1
{
    public class BaseGenericApiController<TDbContext, TModel> : Controller
        where TDbContext : DbContext
        where TModel : class
    {
        protected readonly IHubContext<PortalHub> _hubContext;

        protected IMemoryCache _memoryCache;

        /// <summary>
        /// The language
        /// </summary>
        protected string _lang;

        /// <summary>
        /// The domain
        /// </summary>
        protected string _domain;

        /// <summary>
        /// The repo
        /// </summary>
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseApiController"/> class.
        /// </summary>
        public BaseGenericApiController(IMemoryCache memoryCache, IHubContext<PortalHub> hubContext)
        {
            _hubContext = hubContext;
            _memoryCache = memoryCache;
        }

        #region Overrides

        /// <summary>
        /// Called before the action method is invoked.
        /// </summary>
        /// <param name="context">The action executing context.</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            GetLanguage();
            AlertAsync("Executing request", 200);
            base.OnActionExecuting(context);
        }


        #endregion

        protected async Task<RepositoryResponse<TView>> GetSingleAsync<TView>(string key, Expression<Func<TModel, bool>> predicate = null, TModel model = null)
            where TView : ViewModelBase<TDbContext, TModel, TView>
        {
            var getPage = new RepositoryResponse<Lib.ViewModels.MixPages.ReadMvcViewModel>();
            var cacheKey = $"{typeof(TModel).Name}_details_{_lang}_{key}";

            if (!_memoryCache.TryGetValue<RepositoryResponse<TView>>(cacheKey, out RepositoryResponse<TView> data))
            {

                if (predicate != null)
                {
                    data = await DefaultRepository<TDbContext, TModel, TView>.Instance.GetSingleModelAsync(predicate);
                    _memoryCache.Set(cacheKey, data);
                }
                else
                {
                    data = new RepositoryResponse<TView>()
                    {
                        IsSucceed = true,
                        Data = DefaultRepository<TDbContext, TModel, TView>.Instance.ParseView(model)
                    };

                }
                if (!MixConstants.cachedKeys.Contains(cacheKey))
                {
                    MixConstants.cachedKeys.Add(cacheKey);
                }
                AlertAsync("Add Cache", 200, cacheKey);
            }
            return data;
        }

        protected async Task<RepositoryResponse<TModel>> DeleteAsync<TView>(Expression<Func<TModel, bool>> predicate, bool isDeleteRelated = false)
            where TView : ViewModelBase<TDbContext, TModel, TView>
        {
            var getPage = new RepositoryResponse<Lib.ViewModels.MixPages.ReadMvcViewModel>();
            var data = await DefaultRepository<TDbContext, TModel, TView>.Instance.GetSingleModelAsync(predicate);
            if (data.IsSucceed)
            {
                RemoveCache();
                return await data.Data.RemoveModelAsync(isDeleteRelated).ConfigureAwait(false);
            }
            return new RepositoryResponse<TModel>() { IsSucceed = false };
        }

        protected async Task<RepositoryResponse<PaginationModel<TView>>> GetListAsync<TView>(string key, RequestPaging request, Expression<Func<TModel, bool>> predicate = null, TModel model = null)
            where TView : ViewModelBase<TDbContext, TModel, TView>
        {
            var getData = new RepositoryResponse<Lib.ViewModels.MixPages.ReadMvcViewModel>();
            var cacheKey = $"{typeof(TModel).Name}_list_{_lang}_{key}_{request.Status}_{request.Keyword}_{request.OrderBy}_{request.PageSize}_{request.PageIndex}";
            var data = _memoryCache.Get<RepositoryResponse<PaginationModel<TView>>>(cacheKey);
            if (data == null)
            {
                if (predicate != null)
                {
                    data = await DefaultRepository<TDbContext, TModel, TView>.Instance.GetModelListByAsync(predicate, request.OrderBy, request.Direction, request.PageSize, request.PageIndex).ConfigureAwait(false);
                    _memoryCache.Set(cacheKey, data);
                }
                else
                {
                    data = await DefaultRepository<TDbContext, TModel, TView>.Instance.GetModelListAsync(request.OrderBy, request.Direction, request.PageSize, request.PageIndex).ConfigureAwait(false);
                    _memoryCache.Set(cacheKey, data);

                }
                if (!MixConstants.cachedKeys.Contains(cacheKey))
                {
                    MixConstants.cachedKeys.Add(cacheKey);
                }
                AlertAsync("Add Cache", 200, cacheKey);
            }

            //AlertAsync("Get List Page", 200, $"Get {request.Key} list page");
            return data;
        }

        protected async Task<RepositoryResponse<TView>> SaveAsync<TView>(TView vm, bool isSaveSubModel)
            where TView : ViewModelBase<TDbContext, TModel, TView>
        {
            if (vm != null)
            {
                var result = await vm.SaveModelAsync(isSaveSubModel).ConfigureAwait(false);
                RemoveCache();
                return result;
            }
            return new RepositoryResponse<TView>();
        }

        public JObject SaveEncrypt([FromBody] RequestEncrypted request)
        {
            var key = Convert.FromBase64String(request.Key); //Encoding.UTF8.GetBytes(request.Key);
            var iv = Convert.FromBase64String(request.IV); //Encoding.UTF8.GetBytes(request.IV);
            string encrypted = string.Empty;
            string decrypt = string.Empty;
            if (!string.IsNullOrEmpty(request.PlainText))
            {
                encrypted = MixService.EncryptStringToBytes_Aes(request.PlainText, key, iv).ToString();
            }
            if (!string.IsNullOrEmpty(request.Encrypted))
            {
                decrypt = MixService.DecryptStringFromBytes_Aes(request.Encrypted, key, iv);
            }
            JObject data = new JObject(
                new JProperty("key", request.Key),
                new JProperty("encrypted", encrypted),
                new JProperty("plainText", decrypt));

            return data;
        }

        protected void RemoveCache()
        {
            foreach (var item in MixConstants.cachedKeys)
            {
                _memoryCache.Remove(item);
            }
            MixConstants.cachedKeys = new List<string>();
            AlertAsync("Empty Cache", 200);
        }
        protected void AlertAsync(string action, int status, string message = null)
        {
            var logMsg = new JObject()
                {
                    new JProperty("created_at", DateTime.UtcNow),
                    new JProperty("ip_address", Request.HttpContext.Connection.RemoteIpAddress.ToString()),
                    new JProperty("user", User.Identity?.Name?? User.Claims.SingleOrDefault(c=>c.Subject.Name == "Username")?.Value),
                    new JProperty("request_url", Request.Path.Value),
                    new JProperty("action", action),
                    new JProperty("status", status),
                    new JProperty("message", message)
                };
            _hubContext.Clients.All.SendAsync("ReceiveMessage", logMsg);
        }

        protected void ParseRequestPagingDate(RequestPaging request)
        {
            request.FromDate = request.FromDate.HasValue ? new DateTime(request.FromDate.Value.Year, request.FromDate.Value.Month, request.FromDate.Value.Day).ToUniversalTime()
                : default(DateTime?);
            request.ToDate = request.ToDate.HasValue ? new DateTime(request.ToDate.Value.Year, request.ToDate.Value.Month, request.ToDate.Value.Day).ToUniversalTime().AddDays(1)
                : default(DateTime?);
        }

        /// <summary>
        /// Gets the language.
        /// </summary>
        protected void GetLanguage()
        {
            _lang = RouteData?.Values["culture"] != null ? RouteData.Values["culture"].ToString() : MixService.GetConfig<string>("Language");
            ViewBag.culture = _lang;
            _domain = string.Format("{0}://{1}", Request.Scheme, Request.Host);
        }
    }
}

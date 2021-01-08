﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Services;
using Mix.Cms.Service.SignalR.Hubs;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.Repository;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
namespace Mix.Cms.Api.Controllers.v1
{
    public class BaseApiController<TDbContext> : Controller
        where TDbContext : DbContext
    {
        protected readonly IHubContext<Mix.Cms.Service.SignalR.Hubs.PortalHub> _hubContext;
        protected static TDbContext _context;
        protected static IDbContextTransaction _transaction;
        protected readonly IMemoryCache _memoryCache;

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
        public BaseApiController(TDbContext context, IMemoryCache memoryCache, IHubContext<PortalHub> hubContext)
        {
            _context = context;
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

        #endregion Overrides

        protected async Task<RepositoryResponse<TView>> GetSingleAsync<TView, TModel>(string key, Expression<Func<TModel, bool>> predicate = null, TModel model = null)
            where TView : ViewModelBase<TDbContext, TModel, TView>
            where TModel : class
        {
            var cacheKey = $"{typeof(TModel).Name}_details_{_lang}_{key}";
            RepositoryResponse<TView> data = null;
            //if (MixService.GetConfig<bool>("IsCache"))
            //{
            //    data = await MixCacheService.GetAsync<RepositoryResponse<TView>>(cacheKey);
            //}
            if (data == null)
            {
                if (predicate != null)
                {
                    data = await DefaultRepository<TDbContext, TModel, TView>.Instance.GetSingleModelAsync(predicate);
                    //_memoryCache.Set(cacheKey, data);
                    //await MixCacheService.SetAsync(cacheKey, data);
                }
                else
                {
                    data = new RepositoryResponse<TView>()
                    {
                        IsSucceed = true,
                        Data = DefaultRepository<TDbContext, TModel, TView>.Instance.ParseView(model)
                    };
                }
                AlertAsync("Add Cache", 200, cacheKey);
            }
            data.LastUpdateConfiguration = MixService.GetConfig<DateTime?>("LastUpdateConfiguration");
            return data;
        }

        protected void AlertAsync(string action, int status, string message = null)
        {
            var logMsg = new JObject()
                {
                    new JProperty("created_at", DateTime.UtcNow),
                    new JProperty("ip_address", Request.HttpContext.Connection.RemoteIpAddress.ToString()),
                    new JProperty("user", User.Identity?.Name),
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

        protected QueryString ParseQuery(RequestPaging request)
        {
            return new QueryString(request.Query);
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
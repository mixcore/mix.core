using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Mix.Cms.Hub;
using Mix.Cms.Lib.Services;
using Mix.Domain.Core.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace Mix.Cms.Api.Controllers.v1
{
    public class BaseApiController: Controller
    {
        protected readonly IHubContext<PortalHub> _hubContext;

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
        public BaseApiController(IHubContext<PortalHub> hubContext)
        {
            _hubContext = hubContext;
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

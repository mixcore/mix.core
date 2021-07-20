using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Memory;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.SignalR.Constants;
using Mix.Cms.Lib.SignalR.Hubs;
using Mix.Common.Helper;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Helpers;
using Mix.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Mix.Heart.Models;
using Mix.Heart.Infrastructure.Repositories;
using Mix.Infrastructure.Repositories;

namespace Mix.Cms.Api.Controllers.v1
{
    public class BaseGenericApiController<TDbContext, TModel> : Controller
        where TDbContext : DbContext
        where TModel : class
    {
        protected static TDbContext _context;
        protected static IDbContextTransaction _transaction;
        protected readonly IHubContext<PortalHub> _hubContext;

        protected IMemoryCache _memoryCache;

        /// <summary>
        /// The language
        /// </summary>
        protected string _lang;

        protected bool _forbidden = false;

        protected bool _forbiddenPortal {
            get {
                var allowedIps = MixService.GetIpConfig<JArray>("AllowedPortalIps") ?? new JArray();
                string remoteIp = Request.HttpContext?.Connection?.RemoteIpAddress?.ToString();
                return _forbidden || (
                    // allow localhost
                    //remoteIp != "::1" &&
                    (allowedIps != null && !allowedIps.Any(t => t.Value<string>() == "*") && !allowedIps.Contains(remoteIp))
                );
            }
        }

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
        public BaseGenericApiController(TDbContext context, IMemoryCache memoryCache, IHubContext<PortalHub> hubContext)
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

        #endregion Overrides

        protected async Task<RepositoryResponse<TView>> GetSingleAsync<TView>(string key, Expression<Func<TModel, bool>> predicate = null, TModel model = null)
            where TView : ViewModelBase<TDbContext, TModel, TView>
        {
            var cacheKey = $"api_{_lang}_{typeof(TModel).Name.ToLower()}_details_{key}";
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
            }
            data.LastUpdateConfiguration = MixService.GetAppSetting<DateTime?>("LastUpdateConfiguration");
            return data;
        }

        protected async Task<RepositoryResponse<TView>> GetSingleAsync<TView>(Expression<Func<TModel, bool>> predicate = null, TModel model = null)
            where TView : ViewModelBase<TDbContext, TModel, TView>
        {
            RepositoryResponse<TView> data = null;
            if (data == null)
            {
                if (predicate != null)
                {
                    data = await DefaultRepository<TDbContext, TModel, TView>.Instance.GetSingleModelAsync(predicate);
                }
                else
                {
                    data = new RepositoryResponse<TView>()
                    {
                        IsSucceed = true,
                        Data = DefaultRepository<TDbContext, TModel, TView>.Instance.ParseView(model)
                    };
                }
            }
            AlertAsync($"Get {typeof(TView).Name}", data?.Status ?? 400, data?.ResponseKey);
            data.LastUpdateConfiguration = MixService.GetAppSetting<DateTime?>("LastUpdateConfiguration");
            return data;
        }

        protected async Task<RepositoryResponse<TModel>> DeleteAsync<TView>(Expression<Func<TModel, bool>> predicate, bool isDeleteRelated = false)
           where TView : ViewModelBase<TDbContext, TModel, TView>
        {
            var data = await DefaultRepository<TDbContext, TModel, TView>.Instance.GetSingleModelAsync(predicate);
            if (data.IsSucceed)
            {
                var result = await data.Data.RemoveModelAsync(isDeleteRelated).ConfigureAwait(false);
                return result;
            }
            return new RepositoryResponse<TModel>() { IsSucceed = false };
        }

        protected async Task<RepositoryResponse<TModel>> DeleteAsync<TView>(TView data, bool isDeleteRelated = false)
            where TView : ViewModelBase<TDbContext, TModel, TView>
        {
            if (data != null)
            {
                var result = await data.RemoveModelAsync(isDeleteRelated).ConfigureAwait(false);

                return result;
            }
            return new RepositoryResponse<TModel>() { IsSucceed = false };
        }

        protected async Task<RepositoryResponse<List<TModel>>> DeleteListAsync<TView>(Expression<Func<TModel, bool>> predicate, bool isRemoveRelatedModel = false)
            where TView : ViewModelBase<TDbContext, TModel, TView>
        {
            var data = await DefaultRepository<TDbContext, TModel, TView>.Instance.RemoveListModelAsync(isRemoveRelatedModel, predicate);

            return data;
        }

        protected async Task<RepositoryResponse<FileViewModel>> ExportListAsync(Expression<Func<TModel, bool>> predicate, MixStructureType type)
        {
            var getData = await DefaultModelRepository<TDbContext, TModel>.Instance.GetModelListByAsync(predicate, _context);
            FileViewModel file = null;
            if (getData.IsSucceed)
            {
                string exportPath = $"Exports/Structures/{typeof(TModel).Name}/{_lang}";
                string filename = $"{type.ToString()}_{DateTime.UtcNow.ToString("ddMMyyyy")}";
                var objContent = new JObject(
                    new JProperty("type", type.ToString()),
                    new JProperty("data", JArray.FromObject(getData.Data))
                    );
                file = new FileViewModel()
                {
                    Filename = filename,
                    Extension = MixFileExtensions.Json,
                    FileFolder = exportPath,
                    Content = objContent.ToString()
                };
                // Copy current templates file
                MixFileRepository.Instance.SaveWebFile(file);
            }
            UnitOfWorkHelper<TDbContext>.HandleTransaction(getData.IsSucceed, true, _transaction);
            return new RepositoryResponse<FileViewModel>()
            {
                IsSucceed = true,
                Data = file,
            };
        }

        protected async Task<RepositoryResponse<PaginationModel<TView>>> GetListAsync<TView>(RequestPaging request, Expression<Func<TModel, bool>> predicate = null, TModel model = null)
            where TView : ViewModelBase<TDbContext, TModel, TView>
        {
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
            data.LastUpdateConfiguration = MixService.GetAppSetting<DateTime?>("LastUpdateConfiguration");
            AlertAsync($"Get List {typeof(TView).Name}", data?.Status ?? 400, data?.ResponseKey);
            return data;
        }

        protected async Task<RepositoryResponse<TView>> SaveAsync<TView>(TView vm, bool isSaveSubModel)
            where TView : ViewModelBase<TDbContext, TModel, TView>
        {
            if (vm != null)
            {
                var result = await vm.SaveModelAsync(isSaveSubModel).ConfigureAwait(false);

                return result;
            }
            return new RepositoryResponse<TView>();
        }

        protected async Task<RepositoryResponse<TModel>> SaveAsync<TView>(JObject obj, Expression<Func<TModel, bool>> predicate)
            where TView : ViewModelBase<TDbContext, TModel, TView>
        {
            if (obj != null)
            {
                List<EntityField> fields = new List<EntityField>();
                Type type = typeof(TModel);
                foreach (var item in obj.Properties())
                {
                    var propName = System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(item.Name);
                    PropertyInfo propertyInfo = type.GetProperty(propName);
                    if (propertyInfo != null)
                    {
                        object val = Convert.ChangeType(item.Value, propertyInfo.PropertyType);
                        var field = new EntityField()
                        {
                            PropertyName = propName,
                            PropertyValue = val
                        };
                        fields.Add(field);
                    }
                }

                var result = await DefaultRepository<TDbContext, TModel, TView>.Instance.UpdateFieldsAsync(predicate, fields);

                return result;
            }
            return new RepositoryResponse<TModel>();
        }

        protected async Task<RepositoryResponse<List<TView>>> SaveListAsync<TView>(List<TView> lstVm, bool isSaveSubModel)
            where TView : ViewModelBase<TDbContext, TModel, TView>
        {
            var result = await DefaultRepository<TDbContext, TModel, TView>.Instance.SaveListModelAsync(lstVm, isSaveSubModel);

            return result;
        }

        protected RepositoryResponse<List<TView>> SaveList<TView>(List<TView> lstVm, bool isSaveSubModel)
            where TView : ViewModelBase<TDbContext, TModel, TView>
        {
            var result = new RepositoryResponse<List<TView>>() { IsSucceed = true };
            if (lstVm != null)
            {
                foreach (var vm in lstVm)
                {
                    var tmp = vm.SaveModel(isSaveSubModel,
                        _context, _transaction);
                    result.IsSucceed = result.IsSucceed && tmp.IsSucceed;
                    if (!tmp.IsSucceed)
                    {
                        result.Exception = tmp.Exception;
                        result.Errors.AddRange(tmp.Errors);
                    }
                }
                return result;
            }

            return result;
        }

        protected void AlertAsync(string action, int status, string message = null)
        {
            var address = Request.Headers["X-Forwarded-For"];
            if (String.IsNullOrEmpty(address))
            {
                address = Request.Host.Value;
            }
            var logMsg = new JObject()
                {
                    new JProperty("created_at", DateTime.UtcNow),
                    new JProperty("id", Request.HttpContext.Connection.Id.ToString()),
                    new JProperty("address", address),
                    new JProperty("ip_address", Request.HttpContext.Connection.RemoteIpAddress.ToString()),
                    new JProperty("user", User.Identity?.Name?? User.Claims.SingleOrDefault(c=>c.Type == "Username")?.Value),
                    new JProperty("request_url", Request.Path.Value),
                    new JProperty("action", action),
                    new JProperty("status", status),
                    new JProperty("message", message)
                };

            //It's not possible to configure JSON serialization in the JavaScript client at this time (March 25th 2020).
            //https://docs.microsoft.com/en-us/aspnet/core/signalr/configuration?view=aspnetcore-3.1&tabs=dotnet

            _hubContext.Clients.All.SendAsync(HubMethods.ReceiveMethod, logMsg.ToString(Newtonsoft.Json.Formatting.None));
        }

        public static void Log(dynamic request, dynamic response)
        {
            string fullPath = $"{Environment.CurrentDirectory}/logs/api/{DateTime.Now.ToString("dd-MM-yyyy")}";
            if (!string.IsNullOrEmpty(fullPath) && !Directory.Exists(fullPath))
            {
                Directory.CreateDirectory(fullPath);
            }
            string filePath = $"{fullPath}/log_api.json";

            try
            {
                FileInfo file = new FileInfo(filePath);
                string content = "[]";
                if (file.Exists)
                {
                    using (StreamReader s = file.OpenText())
                    {
                        content = s.ReadToEnd();
                    }
                    System.IO.File.Delete(filePath);
                }

                JArray arrExceptions = JArray.Parse(content);
                JObject jex = new JObject
                {
                    new JProperty("CreatedDateTime", DateTime.UtcNow),
                    new JProperty("request", JObject.FromObject(request)),
                    new JProperty("response", JObject.FromObject(response)  )
                };
                arrExceptions.Add(jex);
                content = arrExceptions.ToString(Newtonsoft.Json.Formatting.None);

                using (var writer = System.IO.File.CreateText(filePath))
                {
                    writer.WriteLine(content);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                // File invalid
            }
        }

        protected void ParseRequestPagingDate(RequestPaging request)
        {
            if (request.FromDate.HasValue)
            {
                request.FromDate = request.FromDate.Value.Kind == DateTimeKind.Utc ? request.FromDate.Value
                    : request.FromDate.Value.ToUniversalTime();
            }
            if (request.ToDate.HasValue)
            {
                request.ToDate = request.ToDate.Value.Kind == DateTimeKind.Utc ? request.ToDate.Value
                    : request.ToDate.Value.ToUniversalTime();
            }
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
            _lang = RouteData?.Values["culture"] != null ? RouteData.Values["culture"].ToString() : MixService.GetAppSetting<string>("Language");
            ViewBag.culture = _lang;
            _domain = string.Format("{0}://{1}", Request.Scheme, Request.Host);
        }
    }
}
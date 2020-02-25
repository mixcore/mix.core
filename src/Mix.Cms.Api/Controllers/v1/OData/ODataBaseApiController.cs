using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.OData;
using Mix.Cms.Hub;
using Mix.Cms.Lib.Extensions;
using Mix.Cms.Lib.Helpers;
using Mix.Cms.Lib.Repositories;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.ViewModels;
using Mix.Common.Helper;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.Repository;
using Mix.Domain.Data.ViewModels;
using Mix.Heart.Helpers;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using static Mix.Cms.Lib.MixEnums;

namespace Mix.Cms.Api.Controllers.v1.OData
{
    public class ODataBaseApiController<TDbContext, TModel> : ODataController
        where TDbContext : DbContext
        where TModel : class
    {
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
        public ODataBaseApiController(IMemoryCache memoryCache, IHubContext<PortalHub> hubContext)
        {
            _hubContext = hubContext;
            _memoryCache = memoryCache;
            GetLanguage();
        }

        protected async Task<RepositoryResponse<TModel>> DeleteAsync<TView>(Expression<Func<TModel, bool>> predicate, bool isDeleteRelated = false)
            where TView : ODataViewModelBase<TDbContext, TModel, TView>
        {
            var result = new RepositoryResponse<TModel>();
            var data = await ODataDefaultRepository<TDbContext, TModel, TView>.Instance.GetSingleModelAsync(predicate);
            if (data.IsSucceed)
            {
                result = await data.Data.RemoveModelAsync(isDeleteRelated).ConfigureAwait(false);
            }

            return result;
        }

        protected async Task<RepositoryResponse<TModel>> DeleteAsync<TView>(TView data, bool isDeleteRelated = false)
            where TView : ODataViewModelBase<TDbContext, TModel, TView>
        {
            if (data != null)
            {
                var result = await data.RemoveModelAsync(isDeleteRelated).ConfigureAwait(false);
                return result;
            }
            return new RepositoryResponse<TModel>() { IsSucceed = false };
        }

        protected async Task<RepositoryResponse<List<TModel>>> DeleteListAsync<TView>(Expression<Func<TModel, bool>> predicate, bool isDeleteRelated = false)
            where TView : ODataViewModelBase<TDbContext, TModel, TView>
        {
            var data = await ODataDefaultRepository<TDbContext, TModel, TView>.Instance.RemoveListModelAsync(isDeleteRelated, predicate);

            return data;
        }

        protected async Task<RepositoryResponse<FileViewModel>> ExportListAsync(Expression<Func<TModel, bool>> predicate, MixStructureType type)
        {
            var getData = await DefaultModelRepository<TDbContext, TModel>.Instance.GetModelListByAsync(predicate);
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
                    Extension = ".json",
                    FileFolder = exportPath,
                    Content = objContent.ToString()
                };
                // Copy current templates file
                FileRepository.Instance.SaveWebFile(file);
            }

            return new RepositoryResponse<FileViewModel>()
            {
                IsSucceed = true,
                Data = file,
            };
        }

        protected async Task<List<TView>> GetListAsync<TView>(ODataQueryOptions<TModel> queryOptions)
            where TView : ODataViewModelBase<TDbContext, TModel, TView>
        {
            Expression<Func<TModel, bool>> predicate = null;
            if (queryOptions.Filter != null)
            {
                ODataHelper<TModel>.ParseFilter(queryOptions.Filter.FilterClause.Expression, ref predicate);
            }
            int? top = queryOptions.Top?.Value;
            var skip = queryOptions.Skip?.Value ?? 0;
            RequestPaging request = new RequestPaging()
            {
                PageIndex = 0,
                PageSize = top.HasValue ? top + top * (skip / top + 1) : null,
                OrderBy = queryOptions.OrderBy?.RawValue
                //Top = queryOptions.Top?.Value,
                //Skip = queryOptions.Skip?.Value
            };
            List<TView> data = null;
            //var cacheKey = $"odata_{_lang}_{typeof(TView).FullName}_{SeoHelper.GetSEOString(queryOptions.Filter?.RawValue, '_')}_ps-{request.PageSize}";
            //if (MixService.GetConfig<bool>("IsCache"))
            //{
            //    var getData = await MixCacheService.GetAsync<RepositoryResponse<PaginationModel<TView>>>(cacheKey);
            //    if (getData != null)
            //    {
            //        data = getData.Data.Items;
            //    }
            //}

            if (data == null)
            {
                if (predicate != null)
                {
                    var getData = await ODataDefaultRepository<TDbContext, TModel, TView>.Instance.GetModelListByAsync(predicate,
                        request.OrderBy, request.Direction, request.PageSize, request.PageIndex, request.Skip, request.Top).ConfigureAwait(false);
                    if (getData.IsSucceed)
                    {
                        data = getData.Data.Items;
                    }
                }
                else
                {
                    var getData = await ODataDefaultRepository<TDbContext, TModel, TView>.Instance.GetModelListAsync(
                        request.OrderBy, request.Direction, request.PageSize, request.PageIndex
                        , null, null).ConfigureAwait(false);
                    if (getData.IsSucceed)
                    {
                        data = getData.Data.Items;
                    }
                }
            }

            return data;
        }

        protected async Task<List<TView>> GetListAsync<TView>(Expression<Func<TModel, bool>> predicate, ODataQueryOptions<TModel> queryOptions)
            where TView : ODataViewModelBase<TDbContext, TModel, TView>
        {
            Expression<Func<TModel, bool>> pre = null;
            if (queryOptions.Filter != null)
            {
                ODataHelper<TModel>.ParseFilter(queryOptions.Filter.FilterClause.Expression, ref pre);
                predicate = ODataHelper<TModel>.CombineExpression(predicate, pre, Microsoft.OData.UriParser.BinaryOperatorKind.And);
            }

            int? top = queryOptions.Top?.Value;
            var skip = queryOptions.Skip?.Value ?? 0;
            RequestPaging request = new RequestPaging()
            {
                PageIndex = 0,
                PageSize = top.HasValue ? top + top * (skip / top + 1) : null,
                OrderBy = queryOptions.OrderBy?.RawValue
            };
            List<TView> data = null;
            if (data == null)
            {
                if (predicate != null)
                {
                    var getData = await ODataDefaultRepository<TDbContext, TModel, TView>.Instance.GetModelListByAsync(predicate,
                        request.OrderBy, request.Direction, request.PageSize, request.PageIndex, request.Skip, request.Top).ConfigureAwait(false);
                    if (getData.IsSucceed)
                    {
                        //await MixCacheService.SetAsync(cacheKey, getData);
                        data = getData.Data.Items;
                    }
                }
                else
                {
                    var getData = await ODataDefaultRepository<TDbContext, TModel, TView>.Instance.GetModelListAsync(
                        request.OrderBy, request.Direction, request.PageSize, request.PageIndex
                        , null, null).ConfigureAwait(false);
                    if (getData.IsSucceed)
                    {
                        //await MixCacheService.SetAsync(cacheKey, getData);
                        data = getData.Data.Items;
                    }
                }
            }

            return data;
        }

        protected async Task<List<TView>> GetListAsync<TView>(Expression<Func<TModel, bool>> predicate, string key, ODataQueryOptions<TModel> queryOptions)
           where TView : ODataViewModelBase<TDbContext, TModel, TView>
        {
            if (queryOptions.Filter != null)
            {
                ODataHelper<TModel>.ParseFilter(queryOptions.Filter.FilterClause.Expression, ref predicate);
            }
            int? top = queryOptions.Top?.Value;
            var skip = queryOptions.Skip?.Value ?? 0;
            RequestPaging request = new RequestPaging()
            {
                PageIndex = 0,
                PageSize = top.HasValue ? top + top * (skip / top + 1) : null,
                OrderBy = queryOptions.OrderBy?.RawValue
                //Top = queryOptions.Top?.Value,
                //Skip = queryOptions.Skip?.Value
            };
            var cacheKey = $"odata_{_lang}_{typeof(TView).FullName}_{key}_{SeoHelper.GetSEOString(queryOptions.Filter?.RawValue, '_')}_ps-{request.PageSize}";
            List<TView> data = null;
            //if (MixService.GetConfig<bool>("IsCache"))
            //{
            //    var getData = await MixCacheService.GetAsync<RepositoryResponse<PaginationModel<TView>>>(cacheKey);
            //    if (getData != null)
            //    {
            //        data = getData.Data.Items;
            //    }
            //}

            if (data == null)
            {
                if (predicate != null)
                {
                    var getData = await ODataDefaultRepository<TDbContext, TModel, TView>.Instance.GetModelListByAsync(predicate,
                        request.OrderBy, request.Direction, request.PageSize, request.PageIndex, request.Skip, request.Top).ConfigureAwait(false);
                    //if (getData.IsSucceed)
                    //{
                    //    await MixCacheService.SetAsync(cacheKey, getData);
                    //    data = getData.Data.Items;
                    //}
                }
                else
                {
                    var getData = await ODataDefaultRepository<TDbContext, TModel, TView>.Instance.GetModelListAsync(
                        request.OrderBy, request.Direction, request.PageSize, request.PageIndex
                        , null, null).ConfigureAwait(false);
                    //if (getData.IsSucceed)
                    //{
                    //    await MixCacheService.SetAsync(cacheKey, getData);
                    //    data = getData.Data.Items;
                    //}
                }
            }

            return data;
        }

        protected async Task<RepositoryResponse<TView>> SaveAsync<TView>(TView vm, bool isSaveSubModel)
            where TView : ODataViewModelBase<TDbContext, TModel, TView>
        {
            if (vm != null)
            {
                var result = await vm.SaveModelAsync(isSaveSubModel).ConfigureAwait(false);

                return result;
            }
            return new RepositoryResponse<TView>();
        }

        protected async Task<RepositoryResponse<TModel>> SaveAsync<TView>(JObject obj, Expression<Func<TModel, bool>> predicate)
            where TView : ODataViewModelBase<TDbContext, TModel, TView>
        {
            if (obj != null)
            {
                List<EntityField> fields = new List<EntityField>();
                Type type = typeof(TModel);
                foreach (var item in obj.Properties())
                {
                    var propName = item.Name.ToTitleCase();
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
                var result = await ODataDefaultRepository<TDbContext, TModel, TView>.Instance.UpdateFieldsAsync(predicate, fields);
                return result;
            }
            return new RepositoryResponse<TModel>();
        }

        protected async Task<RepositoryResponse<List<TView>>> SaveListAsync<TView>(List<TView> lstVm, bool isSaveSubModel)
            where TView : ODataViewModelBase<TDbContext, TModel, TView>
        {
            var result = await ODataDefaultRepository<TDbContext, TModel, TView>.Instance.SaveListModelAsync(lstVm, isSaveSubModel);
            return result;
        }

        protected RepositoryResponse<List<TView>> SaveList<TView>(List<TView> lstVm, bool isSaveSubModel)
            where TView : ODataViewModelBase<TDbContext, TModel, TView>
        {
            var result = new RepositoryResponse<List<TView>>() { IsSucceed = true };
            if (lstVm != null)
            {
                foreach (var vm in lstVm)
                {
                    var tmp = vm.SaveModel(isSaveSubModel);
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

        #region Cached

        protected async Task<RepositoryResponse<TView>> GetSingleAsync<TView>(Expression<Func<TModel, bool>> predicate = null, TModel model = null)
          where TView : ODataViewModelBase<TDbContext, TModel, TView>
        {
            RepositoryResponse<TView> data = null;
            if (predicate != null)
            {
                data = await ODataDefaultRepository<TDbContext, TModel, TView>.Instance.GetSingleModelAsync(predicate);
            }
            else
            {
                data = new RepositoryResponse<TView>()
                {
                    IsSucceed = true,
                    Data = ODataDefaultRepository<TDbContext, TModel, TView>.Instance.ParseView(model)
                };
            }

            data.LastUpdateConfiguration = MixService.GetConfig<DateTime?>("LastUpdateConfiguration");
            return data;
        }

        #endregion Cached

        #region Helpers

        public JObject SaveEncrypt([FromBody] RequestEncrypted request)
        {
            //var key = Convert.FromBase64String(request.Key); //Encoding.UTF8.GetBytes(request.Key);
            //var iv = Convert.FromBase64String(request.IV); //Encoding.UTF8.GetBytes(request.IV);
            string encrypted = string.Empty;
            string decrypt = string.Empty;
            if (!string.IsNullOrEmpty(request.PlainText))
            {
                encrypted = AesEncryptionHelper.EncryptStringToBytes_Aes(new JObject()).ToString();
            }
            if (!string.IsNullOrEmpty(request.Encrypted))
            {
                //decrypt = MixService.DecryptStringFromBytes_Aes(request.Encrypted, request.Key, request.IV);
            }
            JObject data = new JObject(
                new JProperty("key", request.Key),
                new JProperty("encrypted", encrypted),
                new JProperty("plainText", decrypt));

            return data;
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
        }

        private void ValidateODataRequest(ODataQueryOptions<TModel> options)
        {
            var settings = new ODataValidationSettings
            {
                AllowedFunctions = AllowedFunctions.AllFunctions,
                AllowedLogicalOperators = AllowedLogicalOperators.All,
                AllowedArithmeticOperators = AllowedArithmeticOperators.All,
                AllowedQueryOptions = AllowedQueryOptions.All
            };
            try
            {
                options.Validate(settings);
            }
            catch (ODataException ex)
            {
                // TODO Handle exception
                Console.Write(ex.Message);
            }
        }

        public override ContentResult Content(string content)
        {
            return base.Content(content);
        }

        #endregion Helpers
    }
}
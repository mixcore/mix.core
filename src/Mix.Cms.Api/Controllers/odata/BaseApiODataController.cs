using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.OData.UriParser;
using Mix.Cms.Hub;
using Mix.Cms.Lib.Helpers;
using Mix.Cms.Lib.Repositories;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.ViewModels;
using Mix.Common.Helper;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.Repository;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using static Mix.Cms.Lib.MixEnums;

namespace Mix.Cms.Api.Controllers.OData
{
    public class BaseApiODataController<TDbContext, TModel> : ODataController
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
        protected bool _forbiddenPortal
        {
            get
            {
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
        public BaseApiODataController(IMemoryCache memoryCache, IHubContext<PortalHub> hubContext)
        {
            _hubContext = hubContext;
            _memoryCache = memoryCache;
            GetLanguage();
        }

        protected async Task<RepositoryResponse<TView>> GetSingleAsync<TView>(string key, Expression<Func<TModel, bool>> predicate = null, TModel model = null)
            where TView : ViewModelBase<TDbContext, TModel, TView>
        {
            var cacheKey = $"odata_{_lang}_{typeof(TView).FullName}_details_{key}";
            RepositoryResponse<TView> data = null;
            if (MixService.GetConfig<bool>("IsCache"))
            {
                data = await MixCacheService.GetAsync<RepositoryResponse<TView>>(cacheKey);
            }
            if (data == null)
            {

                if (predicate != null)
                {
                    data = await DefaultRepository<TDbContext, TModel, TView>.Instance.GetSingleModelAsync(predicate);
                    if (data.IsSucceed)
                    {
                        //_memoryCache.Set(cacheKey, data);
                        await MixCacheService.SetAsync(cacheKey, data);
                        AlertAsync("Add Cache", 200, cacheKey);
                    }
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
            data.LastUpdateConfiguration = MixService.GetConfig<DateTime?>("LastUpdateConfiguration");
            return data;
        }

        protected async Task<RepositoryResponse<TModel>> DeleteAsync<TView>(Expression<Func<TModel, bool>> predicate, bool isDeleteRelated = false)
            where TView : ViewModelBase<TDbContext, TModel, TView>
        {
            var data = await DefaultRepository<TDbContext, TModel, TView>.Instance.GetSingleModelAsync(predicate);
            if (data.IsSucceed)
            {
                var result = await data.Data.RemoveModelAsync(isDeleteRelated).ConfigureAwait(false);
                if (result.IsSucceed)
                {
                    await MixCacheService.RemoveCacheAsync();
                }
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
                if (result.IsSucceed)
                {
                    await MixCacheService.RemoveCacheAsync();
                }
                return result;
            }
            return new RepositoryResponse<TModel>() { IsSucceed = false };
        }

        protected async Task<RepositoryResponse<List<TModel>>> DeleteListAsync<TView>(bool isRemoveRelatedModel, Expression<Func<TModel, bool>> predicate, bool isDeleteRelated = false)
            where TView : ViewModelBase<TDbContext, TModel, TView>
        {
            var data = await DefaultRepository<TDbContext, TModel, TView>.Instance.RemoveListModelAsync(isRemoveRelatedModel, predicate);
            if (data.IsSucceed)
            {
                await MixCacheService.RemoveCacheAsync();

            }
            return data;
        }


        protected async Task<RepositoryResponse<FileViewModel>> ExportListAsync(Expression<Func<TModel, bool>> predicate, MixStructureType type)
        {
            var getData = await DefaultModelRepository<TDbContext, TModel>.Instance.GetModelListByAsync(predicate);
            if (getData.IsSucceed)
            {
                string exportPath = $"Exports/Structures/{typeof(TModel).Name}/{_lang}";
                string filename = $"{type.ToString()}_{DateTime.UtcNow.ToString("ddMMyyyy")}";
                var objContent = new JObject(
                    new JProperty("type", type.ToString()),
                    new JProperty("data", JArray.FromObject(getData.Data))
                    );
                var file = new FileViewModel()
                {
                    Filename = filename,
                    Extension = ".json",
                    FileFolder = exportPath,
                    Content = objContent.ToString()
                };
                // Copy current templates file
                FileRepository.Instance.SaveWebFile(file);
                return new RepositoryResponse<FileViewModel>()
                {
                    IsSucceed = true,
                    Data = file,
                };
            }
            return new RepositoryResponse<FileViewModel>();
        }
        protected async Task<List<TView>> GetListAsync<TView>(ODataQueryOptions<TModel> queryOptions)
            where TView : ViewModelBase<TDbContext, TModel, TView>
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
                PageSize = top.HasValue ? top + top * (skip / top + 1) : null
                //Top = queryOptions.Top?.Value,
                //Skip = queryOptions.Skip?.Value
            };
            var cacheKey = $"odata_{_lang}_{typeof(TView).FullName}_{SeoHelper.GetSEOString(queryOptions.Filter?.RawValue, '_')}_ps-{request.PageSize}";
            List<TView> data = null;
            if (MixService.GetConfig<bool>("IsCache"))
            {
                var getData = await MixCacheService.GetAsync<RepositoryResponse<PaginationModel<TView>>>(cacheKey);
                if (getData != null)
                {
                    data = getData.Data.Items;
                }
            }

            if (data == null)
            {
                if (predicate != null)
                {
                    var getData = await DefaultRepository<TDbContext, TModel, TView>.Instance.GetModelListByAsync(predicate,
                        request.OrderBy, request.Direction, request.PageSize, request.PageIndex, request.Skip, request.Top
                        ).ConfigureAwait(false);
                    if (getData.IsSucceed)
                    {
                        await MixCacheService.SetAsync(cacheKey, getData);
                        data = getData.Data.Items;
                    }
                }
                else
                {
                    var getData = await DefaultRepository<TDbContext, TModel, TView>.Instance.GetModelListAsync(request.OrderBy, request.Direction, request.PageSize, request.PageIndex).ConfigureAwait(false);
                    if (getData.IsSucceed)
                    {
                        await MixCacheService.SetAsync(cacheKey, getData);
                        data = getData.Data.Items;
                    }
                }
            }
            return data;
        }

        protected async Task<RepositoryResponse<TView>> SaveAsync<TView>(TView vm, bool isSaveSubModel)
            where TView : ViewModelBase<TDbContext, TModel, TView>
        {
            if (vm != null)
            {
                var result = await vm.SaveModelAsync(isSaveSubModel).ConfigureAwait(false);
                await MixCacheService.RemoveCacheAsync();
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
                await MixCacheService.RemoveCacheAsync();
                return result;
            }
            return new RepositoryResponse<TModel>();
        }

        protected async Task<RepositoryResponse<List<TView>>> SaveListAsync<TView>(List<TView> lstVm, bool isSaveSubModel)
            where TView : ViewModelBase<TDbContext, TModel, TView>
        {
            var result = await DefaultRepository<TDbContext, TModel, TView>.Instance.SaveListModelAsync(lstVm, isSaveSubModel);
            if (result.IsSucceed)
            {
                await MixCacheService.RemoveCacheAsync();
            }
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
                    var tmp = vm.SaveModel(isSaveSubModel);
                    result.IsSucceed = result.IsSucceed && tmp.IsSucceed;
                    if (!tmp.IsSucceed)
                    {
                        result.Exception = tmp.Exception;
                        result.Errors.AddRange(tmp.Errors);
                    }
                }
                Task.Run(() => MixCacheService.RemoveCacheAsync());
                return result;
            }
            return result;
        }

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
            var logMsg = new JObject()
                {
                    new JProperty("created_at", DateTime.UtcNow),
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


        public class Poco
        {
            public int id { get; set; }
            public string name { get; set; }
            public string type { get; set; }
        }
        public class Rule
        {
            public string field { get; set; }
            public string op { get; set; }
            public string data { get; set; }
        }
        public static Expression<Func<TModel, bool>> FilterObjectSet(Rule rule, string name)
        {
            Type type = typeof(TModel);
            var par = Expression.Parameter(type, name);

            Type fieldPropertyType;
            Expression fieldPropertyExpression;

            FieldInfo fieldInfo = type.GetField(rule.field);

            if (fieldInfo == null)
            {
                PropertyInfo propertyInfo = type.GetProperty(rule.field);

                if (propertyInfo == null)
                {
                    throw new Exception();
                }

                fieldPropertyType = propertyInfo.PropertyType;
                fieldPropertyExpression = Expression.Property(par, propertyInfo);
            }
            else
            {
                fieldPropertyType = fieldInfo.FieldType;
                fieldPropertyExpression = Expression.Field(par, fieldInfo);
            }

            object data2 = Convert.ChangeType(rule.data, fieldPropertyType);
            var eq = Expression.Equal(fieldPropertyExpression,
                                      Expression.Constant(data2))

                    ;

            return Expression.Lambda<Func<TModel, bool>>(eq, par);
        }
        protected Expression<Func<T, bool>> AndAlso<T>(
      Expression<Func<T, bool>> expr1,
      Expression<Func<T, bool>> expr2)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);

            var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            return Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(left, right), parameter);
        }
        private class ReplaceExpressionVisitor
        : ExpressionVisitor
        {
            private readonly Expression _oldValue;
            private readonly Expression _newValue;

            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public override Expression Visit(Expression node)
            {
                if (node == _oldValue)
                    return _newValue;
                return base.Visit(node);
            }
        }

        protected Expression<Func<TModel, bool>> HandleRawFilter(string culture, string[] filters)
        {
            Expression<Func<TModel, bool>> expr1 = null;
            var rule = new Rule()
            {
                field = "Specificulture",
                op = "eq",
                data = culture
            };
            expr1 = FilterObjectSet(rule, "var1");
            foreach (var item in filters)
            {
                //return exp;
                if (!string.IsNullOrEmpty(item))
                {
                    try
                    {
                        string[] arr = item.Split(' ');
                        if (arr.Length >= 3)
                        {
                            rule = new Rule()
                            {
                                field = arr[0],
                                op = arr[1],
                                data = arr[2]
                            };
                            var expr2 = FilterObjectSet(rule, "var1");
                            ParameterExpression param = expr1.Parameters[0];

                            // simple version
                            expr1 = AndAlso(expr1, expr2);
                        }
                    }
                    catch
                    {

                    }
                }
            }
            return expr1;
        }

        protected void HandleRawFilter(string culture, ODataQueryOptions<TModel> options)
        {
            // These validation settings prevent anything except an equals filter
            ValidateODataRequest(options);

            // Parsing a filter, e.g. /Products?$filter=Name eq 'beer'        
            ParsingFilter(options);

        }
        public static Expression<Func<TModel, bool>> FilterObjectSet(SingleValuePropertyAccessNode rule,
            ConstantNode constant, BinaryOperatorKind OperatorKind)
        {
            Type type = typeof(TModel);
            var par = Expression.Parameter(type, rule.Property.Name);

            Type fieldPropertyType;
            Expression fieldPropertyExpression;

            FieldInfo fieldInfo = type.GetField(rule.Property.Name);

            if (fieldInfo == null)
            {
                PropertyInfo propertyInfo = type.GetProperty(rule.Property.Name);

                if (propertyInfo == null)
                {
                    throw new Exception();
                }

                fieldPropertyType = propertyInfo.PropertyType;
                fieldPropertyExpression = Expression.Property(par, propertyInfo);
            }
            else
            {
                fieldPropertyType = fieldInfo.FieldType;
                fieldPropertyExpression = Expression.Field(par, fieldInfo);
            }

            object data2 = Convert.ChangeType(constant.LiteralText, fieldPropertyType);
            BinaryExpression eq = null;
            switch (OperatorKind)
            {
                case BinaryOperatorKind.Or:
                    eq = Expression.Or(fieldPropertyExpression,
                                     Expression.Constant(data2));
                    break;
                case BinaryOperatorKind.And:
                    eq = Expression.And(fieldPropertyExpression,
                                      Expression.Constant(data2));
                    break;
                case BinaryOperatorKind.Equal:
                    eq = Expression.Equal(fieldPropertyExpression,
                                      Expression.Constant(data2));
                    break;
                case BinaryOperatorKind.NotEqual:
                    eq = Expression.NotEqual(fieldPropertyExpression,
                                      Expression.Constant(data2));
                    break;
                case BinaryOperatorKind.GreaterThan:
                    eq = Expression.GreaterThan(fieldPropertyExpression,
                                      Expression.Constant(data2));
                    break;
                case BinaryOperatorKind.GreaterThanOrEqual:
                    eq = Expression.GreaterThanOrEqual(fieldPropertyExpression,
                                      Expression.Constant(data2));
                    break;
                case BinaryOperatorKind.LessThan:
                    eq = Expression.LessThan(fieldPropertyExpression,
                                      Expression.Constant(data2));
                    break;
                case BinaryOperatorKind.LessThanOrEqual:
                    eq = Expression.LessThanOrEqual(fieldPropertyExpression,
                                      Expression.Constant(data2));
                    break;
                case BinaryOperatorKind.Add:
                    eq = Expression.Add(fieldPropertyExpression,
                                      Expression.Constant(data2));
                    break;
                case BinaryOperatorKind.Subtract:
                    eq = Expression.Subtract(fieldPropertyExpression,
                                      Expression.Constant(data2));
                    break;
                case BinaryOperatorKind.Multiply:
                    eq = Expression.Multiply(fieldPropertyExpression,
                                      Expression.Constant(data2));
                    break;
                case BinaryOperatorKind.Divide:
                    eq = Expression.Divide(fieldPropertyExpression,
                                      Expression.Constant(data2));
                    break;
                case BinaryOperatorKind.Modulo:
                    eq = Expression.Modulo(fieldPropertyExpression,
                                      Expression.Constant(data2));
                    break;
                case BinaryOperatorKind.Has:
                    break;
            }
            return Expression.Lambda<Func<TModel, bool>>(eq, par);
        }
        private void ParsingFilter(ODataQueryOptions<TModel> options)
        {
            // Parsing a filter, e.g. /Products?$filter=Name eq 'beer'        

            if (options.Filter != null && options.Filter.FilterClause != null)
            {
                var binaryOperator = options.Filter.FilterClause.Expression as BinaryOperatorNode;
                if (binaryOperator != null)
                {
                    var property = binaryOperator.Left as SingleValuePropertyAccessNode ?? binaryOperator.Right as SingleValuePropertyAccessNode;
                    var constant = binaryOperator.Left as ConstantNode ?? binaryOperator.Right as ConstantNode;

                    if (property != null && property.Property != null && constant != null && constant.Value != null)
                    {
                        var exp = FilterObjectSet(property, constant, binaryOperator.OperatorKind);
                    }
                }
            }
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

            }
        }
    }
}

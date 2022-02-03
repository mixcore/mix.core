using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Common;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.SignalR.Constants;
using Mix.Cms.Lib.ViewModels;
using Mix.Common.Helper;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Enums;
using Mix.Heart.Extensions;
using Mix.Heart.Helpers;
using Mix.Identity.Constants;
using Mix.Identity.Helpers;
using Mix.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Mix.Heart.Models;
using Mix.Heart.Infrastructure.Repositories;
using Mix.Cms.Lib.Attributes;
using Mix.Cms.Lib.Repositories;

namespace Mix.Cms.Lib.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Produces("application/json")]
    public class BaseAuthorizedRestApiController<TDbContext, TModel, TUpdate, TRead, TDelete> : Controller
        where TDbContext : DbContext
        where TModel : class
        where TUpdate : ViewModelBase<TDbContext, TModel, TUpdate>
        where TRead : ViewModelBase<TDbContext, TModel, TRead>
        where TDelete : ViewModelBase<TDbContext, TModel, TDelete>
    {
        protected static TDbContext _context;
        protected static IDbContextTransaction _transaction;
        protected string _lang;
        protected bool _forbidden;
        protected DefaultRepository<TDbContext, TModel, TRead> _repo;
        protected DefaultRepository<TDbContext, TModel, TUpdate> _updRepo;
        protected DefaultRepository<TDbContext, TModel, TDelete> _delRepo;
        protected MixIdentityHelper _mixIdentityHelper;
        protected AuditLogRepository _auditlogRepo;
        public BaseAuthorizedRestApiController(
            DefaultRepository<TDbContext, TModel, TRead> repo,
            DefaultRepository<TDbContext, TModel, TUpdate> updRepo,
            DefaultRepository<TDbContext, TModel, TDelete> delRepo,
            MixIdentityHelper mixIdentityHelper, 
            AuditLogRepository auditlogRepo)
        {
            _repo = repo;
            _updRepo = updRepo;
            _delRepo = delRepo;
            _mixIdentityHelper = mixIdentityHelper;
            _auditlogRepo = auditlogRepo;
        }

        /// <summary>
        /// The domain
        /// </summary>
        protected string _domain;

        #region Routes

        [MixAuthorize]
        [HttpGet]
        public virtual async Task<ActionResult<PaginationModel<TRead>>> Get()
        {
            bool isFromDate = DateTime.TryParse(Request.Query[MixRequestQueryKeywords.FromDate], out DateTime fromDate);
            bool isToDate = DateTime.TryParse(Request.Query[MixRequestQueryKeywords.ToDate], out DateTime toDate);
            int.TryParse(Request.Query[MixRequestQueryKeywords.PageIndex], out int pageIndex);
            bool isDirection = Enum.TryParse(Request.Query[MixRequestQueryKeywords.Direction], out DisplayDirection direction);
            bool isPageSize = int.TryParse(Request.Query[MixRequestQueryKeywords.PageSize], out int pageSize);

            RequestPaging request = new RequestPaging()
            {
                PageIndex = pageIndex,
                PageSize = isPageSize ? pageSize : 100,
                OrderBy = Request.Query[MixRequestQueryKeywords.OrderBy].ToString().ToTitleCase(),
                Direction = direction
            };

            RepositoryResponse<PaginationModel<TRead>> getData = await _repo.GetModelListAsync(
                request.OrderBy, request.Direction, request.PageSize, request.PageIndex, null, null).ConfigureAwait(false);

            return GetResponse(getData);
        }

        [MixAuthorize]
        [HttpGet("{id}")]
        public virtual async Task<ActionResult<TUpdate>> Get(string id)
        {
            var getData = await GetSingleAsync(id);
            return GetResponse(getData, MixErrorStatus.NotFound);
        }

        [MixAuthorize]
        [HttpGet("clone/{id}/{cloneCulture}")]
        public virtual async Task<ActionResult<TUpdate>> Clone(string id, string cloneCulture)
        {
            var getData = await GetSingleAsync(id);
            var cultures = new List<SupportedCulture>() { new SupportedCulture()
            {
                Specificulture = cloneCulture
            } };
            var result = await getData.Data.CloneAsync(getData.Data.Model, cultures);
            if (result.IsSucceed)
            {
                return Ok(result.Data.FirstOrDefault());
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [MixAuthorize]
        [HttpGet("duplicate/{id}")]
        public virtual async Task<ActionResult<TUpdate>> Duplicate(string id)
        {
            var getData = await GetSingleAsync(id);
            if (getData.IsSucceed)
            {
                var data = getData.Data;
                var idProperty = ReflectionHelper.GetPropertyType(data.GetType(), MixQueryColumnName.Id);
                switch (idProperty.Name.ToLower())
                {
                    case "int32":
                        ReflectionHelper.SetPropertyValue(data, new JProperty("id", 0));
                        break;

                    default:
                        ReflectionHelper.SetPropertyValue(data, new JProperty("id", default));
                        break;
                }

                var saveResult = await data.SaveModelAsync(true);
                return GetResponse(saveResult);
            }
            else
            {
                return NoContent();
            }
        }

        [HttpGet("default")]
        public virtual ActionResult<TUpdate> Default()
        {
            using (TDbContext context = UnitOfWorkHelper<TDbContext>.InitContext())
            {
                var transaction = context.Database.BeginTransaction();
                TUpdate data = ReflectionHelper.InitModel<TUpdate>();
                ReflectionHelper.SetPropertyValue(data, new JProperty("Specificulture", _lang));
                ReflectionHelper.SetPropertyValue(data, new JProperty("Status", MixService.GetAppSetting<string>(MixAppSettingKeywords.DefaultContentStatus)));
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

        [MixAuthorize]
        [HttpPost]
        public virtual async Task<ActionResult<TUpdate>> Create([FromBody] TUpdate data)
        {
            ReflectionHelper.SetPropertyValue(data, new JProperty("CreatedBy", User.Claims.FirstOrDefault(
                    c => c.Type == "Username")?.Value));
            ReflectionHelper.SetPropertyValue(data, new JProperty("Specificulture", _lang));
            var result = await SaveAsync(data, true);
            return GetResponse(result);
        }

        [MixAuthorize]
        [HttpPut("{id}")]
        public virtual async Task<ActionResult<TUpdate>> Update(string id, [FromBody] TUpdate data)
        {
            if (data != null)
            {
                ReflectionHelper.SetPropertyValue(data, new JProperty("ModifiedBy", _mixIdentityHelper.GetClaim(User, MixClaims.Username)));
                ReflectionHelper.SetPropertyValue(data, new JProperty("LastModified", DateTime.UtcNow));
                var currentId = ReflectionHelper.GetPropertyValue(data, MixQueryColumnName.Id).ToString();
                if (id != currentId)
                {
                    return BadRequest();
                }
                var result = await SaveAsync(data, true);
                return GetResponse(result);
            }
            else
            {
                return BadRequest(new NullReferenceException());
            }
        }

        [MixAuthorize]
        [HttpPatch("{id}")]
        public virtual async Task<ActionResult<bool>> Patch(string id, [FromBody] JObject fields)
        {
            var result = await GetSingleAsync(id);
            if (result.IsSucceed)
            {
                ReflectionHelper.SetPropertyValue(result.Data, new JProperty("ModifiedBy", User.Claims.FirstOrDefault(
                    c => c.Type == "Username")?.Value));
                ReflectionHelper.SetPropertyValue(result.Data, new JProperty("LastModified", DateTime.UtcNow));
                var saveResult = await result.Data.UpdateFieldsAsync(fields);
                return GetResponse(saveResult);
            }
            else
            {
                return NotFound();
            }
        }

        [MixAuthorize]
        [HttpDelete("{id}")]
        public virtual async Task<ActionResult<TModel>> Delete(string id)
        {
            var result = await DeleteAsync(id, true);
            return GetResponse(result);
        }

        // POST api/update-infos
        [HttpPost]
        [Route("save-many")]
        public async Task<RepositoryResponse<List<TUpdate>>> UpdateInfos([FromBody] List<TUpdate> models)
        {
            if (models != null)
            {
                return await SaveManyAsync(models, false);
            }
            else
            {
                return new RepositoryResponse<List<TUpdate>>();
            }
        }

        [HttpPost]
        [Route("list-action")]
        public async Task<ActionResult<JObject>> ListActionAsync([FromBody] ListAction<string> data)
        {
            Expression<Func<TModel, bool>> predicate = ReflectionHelper.GetExpression<TModel>("Specificulture", _lang, Heart.Enums.ExpressionMethod.Eq);
            Expression<Func<TModel, bool>> idPre = null;
            foreach (var id in data.Data)
            {
                var temp = ReflectionHelper.GetExpression<TModel>(MixQueryColumnName.Id, id, Heart.Enums.ExpressionMethod.Eq);

                idPre = idPre != null
                    ? idPre.Or(temp)
                    : temp;
            }
            if (idPre != null)
            {
                predicate = predicate.AndAlso(idPre);

                switch (data.Action)
                {
                    case "Delete":
                        return Ok(JObject.FromObject(await DeleteListAsync(predicate, true)));

                    case "Publish":
                        return Ok(JObject.FromObject(await PublishListAsync(predicate)));

                    case "Export":
                        return Ok(JObject.FromObject(await ExportListAsync(predicate)));

                    default:
                        return JObject.FromObject(new RepositoryResponse<bool>());
                }
            }
            else
            {
                return BadRequest();
            }
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
            _lang = RouteData?.Values["culture"] != null ? RouteData.Values["culture"].ToString() : null;
            _domain = string.Format("{0}://{1}", Request.Scheme, Request.Host);
        }

        #endregion Overrides

        #region Helpers

        protected ActionResult<T> GetResponse<T>(RepositoryResponse<T> result, MixErrorStatus status = MixErrorStatus.Badrequest)
        {
            _auditlogRepo.Log(_mixIdentityHelper.GetClaim(User, MixClaims.Username), Request, result.IsSucceed, result.Exception);
            if (result.IsSucceed)
            {
                return Ok(result.Data);
            }
            else
            {
                switch (status)
                {
                    case MixErrorStatus.NotFound:
                        return NotFound();
                    case MixErrorStatus.UnAuthorized:
                        return Unauthorized();
                    case MixErrorStatus.Forbidden:
                        return Forbid();
                    case MixErrorStatus.Badrequest:
                    case MixErrorStatus.ServerError:
                    default:
                        return BadRequest(result.Errors);
                }
                
            }
        }

        private async Task<RepositoryResponse<List<TUpdate>>> PublishListAsync(Expression<Func<TModel, bool>> predicate)
        {
            var data = await GetListAsync<TUpdate>(predicate);
            foreach (var item in data.Data.Items)
            {
                ReflectionHelper.SetPropertyValue(item, new JProperty("Status", MixContentStatus.Published));
            }
            return await SaveManyAsync(data.Data.Items, false);
        }

        protected virtual async Task<RepositoryResponse<T>> GetSingleAsync<T>(string id)
            where T : Mix.Heart.Infrastructure.ViewModels.ViewModelBase<TDbContext, TModel, T>
        {
            Expression<Func<TModel, bool>> predicate = ReflectionHelper.GetExpression<TModel>(MixQueryColumnName.Id, id, ExpressionMethod.Eq);
            if (!string.IsNullOrEmpty(_lang))
            {
                var idPre = ReflectionHelper.GetExpression<TModel>("Specificulture", _lang, ExpressionMethod.Eq);
                predicate = predicate.AndAlso(idPre);
            }

            return await GetSingleAsync<T>(predicate);
        }

        protected virtual async Task<RepositoryResponse<TUpdate>> GetSingleAsync(string id)
        {
            Expression<Func<TModel, bool>> predicate = ReflectionHelper.GetExpression<TModel>(
                MixQueryColumnName.Id, id, Heart.Enums.ExpressionMethod.Eq);
            if (!string.IsNullOrEmpty(_lang))
            {
                var idPre = ReflectionHelper.GetExpression<TModel>("Specificulture", _lang, Heart.Enums.ExpressionMethod.Eq);
                predicate = predicate.AndAlso(idPre);
            }

            return await GetSingleAsync(predicate);
        }

        protected async Task<RepositoryResponse<TUpdate>> GetSingleAsync(Expression<Func<TModel, bool>> predicate = null)
        {
            RepositoryResponse<TUpdate> data = null;
            if (predicate != null)
            {
                data = await _updRepo.GetSingleModelAsync(predicate);
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

        protected async Task<RepositoryResponse<TModel>> DeleteAsync<T>(string id, bool isDeleteRelated = false)
            where T : Mix.Heart.Infrastructure.ViewModels.ViewModelBase<TDbContext, TModel, T>
        {
            var data = await GetSingleAsync<T>(id);
            if (data.IsSucceed)
            {
                var result = await DeleteAsync<T>(data.Data, isDeleteRelated);

                return result;
            }
            return new RepositoryResponse<TModel>() { IsSucceed = false };
        }

        protected async Task<RepositoryResponse<TModel>> DeleteAsync(string id, bool isDeleteRelated = false)
        {
            var data = await GetSingleAsync<TDelete>(id);
            if (data.IsSucceed)
            {
                var result = await DeleteAsync(data.Data, isDeleteRelated);

                return result;
            }
            return new RepositoryResponse<TModel>() { IsSucceed = false };
        }

        protected async Task<RepositoryResponse<TModel>> DeleteAsync(TUpdate data, bool isDeleteRelated = false)
        {
            if (data != null)
            {
                var result = await data.RemoveModelAsync(isDeleteRelated).ConfigureAwait(false);

                return result;
            }
            return new RepositoryResponse<TModel>() { IsSucceed = false };
        }

        protected async Task<RepositoryResponse<TModel>> DeleteAsync<T>(T data, bool isDeleteRelated = false)
            where T : Mix.Heart.Infrastructure.ViewModels.ViewModelBase<TDbContext, TModel, T>
        {
            if (data != null)
            {
                var result = await data.RemoveModelAsync(isDeleteRelated).ConfigureAwait(false);

                return result;
            }
            return new RepositoryResponse<TModel>() { IsSucceed = false };
        }

        protected async Task<RepositoryResponse<List<TModel>>> DeleteListAsync(Expression<Func<TModel, bool>> predicate, bool isRemoveRelatedModel = false)
        {
            var data = await _delRepo.RemoveListModelAsync(isRemoveRelatedModel, predicate);

            return data;
        }

        protected async Task<RepositoryResponse<FileViewModel>> ExportListAsync(Expression<Func<TModel, bool>> predicate)
        {
            string type = typeof(TModel).Name;
            var getData = await _repo.GetModelListByAsync(predicate, _context);
            var jData = new List<JObject>();
            if (getData.IsSucceed)
            {
                string exportPath = $"{MixFolders.ExportFolder}/{typeof(TModel).Name}";
                foreach (var item in JArray.FromObject(getData.Data))
                {
                    jData.Add(JObject.FromObject(item));
                }

                var result = Lib.ViewModels.MixDatabaseDatas.Helper.ExportAttributeToExcel(
                        jData, string.Empty, exportPath, $"{type}", null);

                return result;
            }
            else
            {
                return new RepositoryResponse<FileViewModel>()
                {
                    Errors = getData.Errors
                };
            }
        }

        protected async Task<RepositoryResponse<PaginationModel<T>>> GetListAsync<T>(Expression<Func<TModel, bool>> predicate = null, SearchQueryModel searchQuery = null)
            where T : ViewModelBase<TDbContext, TModel, T>
        {
            searchQuery ??= new SearchQueryModel(Request);
            RequestPaging request = new RequestPaging()
            {
                PageIndex = searchQuery.PagingData.PageIndex,
                PageSize = searchQuery.PagingData.PageSize,
                OrderBy = searchQuery.PagingData.OrderBy,
                Direction = searchQuery.PagingData.Direction
            };

            RepositoryResponse<PaginationModel<T>> data = null;

            if (data == null)
            {
                if (predicate != null)
                {
                    data = await DefaultRepository<TDbContext, TModel, T>.Instance.GetModelListByAsync(
                        predicate, request.OrderBy, request.Direction, request.PageSize, request.PageIndex, null, null);
                }
                else
                {
                    data = await DefaultRepository<TDbContext, TModel, T>.Instance.GetModelListAsync(request.OrderBy, request.Direction, request.PageSize, request.PageIndex, null, null).ConfigureAwait(false);
                }
            }
            return data;
        }

        protected virtual Task<RepositoryResponse<TUpdate>> SaveAsync(TUpdate vm, bool isSaveSubModel)
        {
            return SaveGenericAsync(vm, isSaveSubModel);
        }
        
        protected async Task<RepositoryResponse<T>> SaveGenericAsync<T>(T vm, bool isSaveSubModel)
            where T : Mix.Heart.Infrastructure.ViewModels.ViewModelBase<TDbContext, TModel, T>
        {
            if (vm != null)
            {
                var result = await vm.SaveModelAsync(isSaveSubModel).ConfigureAwait(false);

                return result;
            }
            return new RepositoryResponse<T>();
        }

        protected async Task<RepositoryResponse<TModel>> SavePropertiesAsync(JObject obj, Expression<Func<TModel, bool>> predicate)
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

                var result = await DefaultRepository<TDbContext, TModel, TUpdate>.Instance.UpdateFieldsAsync(predicate, fields);

                return result;
            }
            return new RepositoryResponse<TModel>();
        }

        protected async Task<RepositoryResponse<List<TUpdate>>> SaveManyAsync(List<TUpdate> lstVm, bool isSaveSubModel)
        {
            var result = await DefaultRepository<TDbContext, TModel, TUpdate>.Instance.SaveListModelAsync(lstVm, isSaveSubModel);

            return result;
        }

        protected RepositoryResponse<List<TUpdate>> SaveList(List<TUpdate> lstVm, bool isSaveSubModel)
        {
            var result = new RepositoryResponse<List<TUpdate>>() { IsSucceed = true };
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

        public virtual async Task AlertAsync<T>(IClientProxy clients, string action, int status, T message)
        {
            var address = Request.Headers["X-Forwarded-For"];
            if (string.IsNullOrEmpty(address))
            {
                address = Request.Host.Value;
            }
            var logMsg = new JObject()
                {
                    new JProperty("created_at", DateTime.UtcNow),
                    new JProperty("id", Request.HttpContext.Connection.Id.ToString()),
                    new JProperty("address", address),
                    new JProperty("ip_address", Request.HttpContext.Connection.RemoteIpAddress.ToString()),
                    new JProperty("user", _mixIdentityHelper.GetClaim(User, MixClaims.Username)),
                    new JProperty("request_url", Request.Path.Value),
                    new JProperty("action", action),
                    new JProperty("status", status),
                    new JProperty("message", message)
                };

            //It's not possible to configure JSON serialization in the JavaScript client at this time (March 25th 2020).
            //https://docs.microsoft.com/en-us/aspnet/core/signalr/configuration?view=aspnetcore-3.1&tabs=dotnet
            await clients.SendAsync(
                HubMethods.ReceiveMethod, logMsg.ToString(Formatting.None));
        }

        #endregion Helpers
    }
}
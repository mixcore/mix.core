using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.ViewModels;
using Mix.Common.Helper;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Enums;
using Mix.Heart.Extensions;
using Mix.Heart.Helpers;
using Mix.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Mix.Heart.Models;
using Mix.Heart.Infrastructure.Repositories;

namespace Mix.Cms.Lib.Controllers
{
    [Produces("application/json")]
    public class BaseRestApiController<TDbContext, TModel, TView> : Controller
        where TDbContext : DbContext
        where TModel : class
        where TView : ViewModelBase<TDbContext, TModel, TView>
    {
        protected static TDbContext _context;
        protected static IDbContextTransaction _transaction;
        protected DefaultRepository<TDbContext, TModel, TView> _repo;

        public BaseRestApiController(DefaultRepository<TDbContext, TModel, TView> repo)
        {
            _repo = repo;
        }

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

            RepositoryResponse<PaginationModel<TView>> getData = await _repo.GetModelListAsync(
                request.OrderBy, request.Direction, request.PageSize, request.PageIndex, null, null).ConfigureAwait(false);

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

        [HttpGet("duplicate/{id}")]
        public virtual async Task<ActionResult<TView>> Duplicate(string id)
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
                if (saveResult.IsSucceed)
                {
                    string key = $"_{id}";
                    key += !string.IsNullOrEmpty(_lang) ? $"_{_lang}" : string.Empty;
                    await MixCacheService.RemoveCacheAsync(typeof(TModel), key);
                    return Ok(saveResult.Data);
                }
                else
                {
                    return BadRequest(saveResult.Errors);
                }
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

       [HttpPost]
        public virtual async Task<ActionResult<TModel>> Create([FromBody] TView data)
        {
            ReflectionHelper.SetPropertyValue(data, new JProperty("CreatedBy", User.Claims.FirstOrDefault(
                    c => c.Type == "Username")?.Value));
            ReflectionHelper.SetPropertyValue(data, new JProperty(MixQueryColumnName.Specificulture, _lang));
            ReflectionHelper.SetPropertyValue(data, new JProperty(MixQueryColumnName.Status, MixService.GetEnumConfig<MixContentStatus>(MixAppSettingKeywords.DefaultContentStatus)));

            var result = await SaveAsync(data, true);
            if (result.IsSucceed)
            {
                return Ok(result.Data);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Update(string id, [FromBody] TView data)
        {
            if (data != null)
            {
                ReflectionHelper.SetPropertyValue(data, new JProperty("ModifiedBy", User.Claims.FirstOrDefault(
                    c => c.Type == "Username")?.Value));
                ReflectionHelper.SetPropertyValue(data, new JProperty("LastModified", DateTime.UtcNow));
                var currentId = ReflectionHelper.GetPropertyValue(data, MixQueryColumnName.Id).ToString();
                if (id != currentId)
                {
                    return BadRequest();
                }
                var result = await SaveAsync(data, true);
                if (result.IsSucceed)
                {
                    return Ok(result.Data);
                }
                else
                {
                    var current = await GetSingleAsync(currentId);
                    if (!current.IsSucceed)
                    {
                        return NotFound();
                    }
                    else
                    {
                        return BadRequest(result.Errors);
                    }
                }
            }
            else
            {
                return BadRequest(new NullReferenceException());
            }
        }

        [HttpPatch("{id}")]
        public virtual async Task<IActionResult> Patch(string id, [FromBody] JObject fields)
        {
            var result = await GetSingleAsync(id);
            if (result.IsSucceed)
            {
                ReflectionHelper.SetPropertyValue(result.Data, new JProperty("ModifiedBy", User.Claims.FirstOrDefault(
                    c => c.Type == "Username")?.Value));
                ReflectionHelper.SetPropertyValue(result.Data, new JProperty("LastModified", DateTime.UtcNow));
                var saveResult = await result.Data.UpdateFieldsAsync(fields);
                if (saveResult.IsSucceed)
                {
                    return NoContent();
                }
                else
                {
                    return BadRequest(saveResult.Errors);
                }
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public virtual async Task<ActionResult<TModel>> Delete(string id)
        {
            var result = await DeleteAsync(id, true);
            if (result.IsSucceed)
            {
                return Ok(result.Data);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPost, HttpOptions]
        [Route("list-action")]
        public async Task<ActionResult<JObject>> ListActionAsync([FromBody] ListAction<string> data)
        {
            Expression<Func<TModel, bool>> predicate = ReflectionHelper.GetExpression<TModel>(
                MixQueryColumnName.Specificulture,
                _lang,
                ExpressionMethod.Eq);
            Expression<Func<TModel, bool>> idPre = null;
            foreach (var id in data.Data)
            {
                var temp = ReflectionHelper.GetExpression<TModel>(MixQueryColumnName.Id, id, ExpressionMethod.Eq);

                idPre = idPre != null
                    ? idPre.AndAlso(temp)
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

        protected virtual void GetLanguage()
        {
            _lang = RouteData?.Values["culture"] != null ? RouteData.Values["culture"].ToString() : string.Empty;
            _domain = string.Format("{0}://{1}", Request.Scheme, Request.Host);
        }

        #endregion Overrides

        #region Helpers

        protected ActionResult<T> GetResponse<T>(RepositoryResponse<T> result)
        {
            if (result.IsSucceed)
            {
                return Ok(result.Data);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        private async Task<RepositoryResponse<List<TView>>> PublishListAsync(Expression<Func<TModel, bool>> predicate)
        {
            var data = await GetListAsync<TView>(predicate);
            foreach (var item in data.Data.Items)
            {
                ReflectionHelper.SetPropertyValue(item, new JProperty(MixQueryColumnName.Status, MixContentStatus.Published));
            }
            return await SaveListAsync(data.Data.Items, false);
        }

        protected async Task<RepositoryResponse<T>> GetSingleAsync<T>(string id)
            where T : ViewModelBase<TDbContext, TModel, T>
        {
            Expression<Func<TModel, bool>> predicate = ReflectionHelper.GetExpression<TModel>(MixQueryColumnName.Id, id, ExpressionMethod.Eq);
            if (!string.IsNullOrEmpty(_lang))
            {
                var idPre = ReflectionHelper.GetExpression<TModel>(MixQueryColumnName.Specificulture, _lang, ExpressionMethod.Eq);
                predicate = predicate.AndAlso(idPre);
            }

            return await GetSingleAsync<T>(predicate);
        }

        protected async Task<RepositoryResponse<TView>> GetSingleAsync(string id)
        {
            Expression<Func<TModel, bool>> predicate = ReflectionHelper.GetExpression<TModel>(MixQueryColumnName.Id, id, ExpressionMethod.Eq);
            if (!string.IsNullOrEmpty(_lang))
            {
                var idPre = ReflectionHelper.GetExpression<TModel>(MixQueryColumnName.Specificulture, _lang, ExpressionMethod.Eq);
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
            where T : ViewModelBase<TDbContext, TModel, T>
        {
            RepositoryResponse<T> data = null;
            if (predicate != null)
            {
                data = await DefaultRepository<TDbContext, TModel, T>.Instance.GetSingleModelAsync(predicate);
            }
            return data;
        }

        protected async Task<RepositoryResponse<TModel>> DeleteAsync<T>(string id, bool isDeleteRelated = false)
            where T : ViewModelBase<TDbContext, TModel, T>
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
            var data = await GetSingleAsync<TView>(id);
            if (data.IsSucceed)
            {
                var result = await DeleteAsync(data.Data, isDeleteRelated);

                return result;
            }
            return new RepositoryResponse<TModel>() { IsSucceed = false };
        }

        protected async Task<RepositoryResponse<TModel>> DeleteAsync(TView data, bool isDeleteRelated = false)
        {
            if (data != null)
            {
                var result = await data.RemoveModelAsync(isDeleteRelated).ConfigureAwait(false);

                return result;
            }
            return new RepositoryResponse<TModel>() { IsSucceed = false };
        }

        protected async Task<RepositoryResponse<TModel>> DeleteAsync<T>(T data, bool isDeleteRelated = false)
            where T : ViewModelBase<TDbContext, TModel, T>
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
            var data = await _repo.RemoveListModelAsync(isRemoveRelatedModel, predicate);

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

        protected async Task<RepositoryResponse<PaginationModel<T>>> GetListAsync<T>(Expression<Func<TModel, bool>> predicate = null)
            where T : ViewModelBase<TDbContext, TModel, T>
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

        protected async Task<RepositoryResponse<T>> SaveAsync<T>(T vm, bool isSaveSubModel)
            where T : ViewModelBase<TDbContext, TModel, T>
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

                var result = await _repo.UpdateFieldsAsync(predicate, fields);

                return result;
            }
            return new RepositoryResponse<TModel>();
        }

        protected async Task<RepositoryResponse<List<TView>>> SaveListAsync(List<TView> lstVm, bool isSaveSubModel)
        {
            var result = await _repo.SaveListModelAsync(lstVm, isSaveSubModel);

            return result;
        }

        protected RepositoryResponse<List<TView>> SaveList(List<TView> lstVm, bool isSaveSubModel)
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

        #endregion Helpers
    }
}
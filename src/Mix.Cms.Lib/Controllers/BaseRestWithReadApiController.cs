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
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.Controllers
{
    [Produces("application/json")]
    public class BaseRestApiController<TDbContext, TModel, TView, TRead> : Controller
        where TDbContext : DbContext
        where TModel : class
        where TView : Mix.Domain.Data.ViewModels.ViewModelBase<TDbContext, TModel, TView>
        where TRead : Mix.Domain.Data.ViewModels.ViewModelBase<TDbContext, TModel, TRead>
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
        public virtual async Task<ActionResult<PaginationModel<TRead>>> Get()
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
            RepositoryResponse<PaginationModel<TRead>> getData = null;
            if (!string.IsNullOrEmpty(_lang))
            {
                predicate = ReflectionHelper.GetExpression<TModel>("Specificulture", _lang, Heart.Enums.MixHeartEnums.ExpressionMethod.Eq);
                getData = await DefaultRepository<TDbContext, TModel, TRead>.Instance.GetModelListByAsync(
                            predicate,
                            request.OrderBy, request.Direction, 
                            request.PageSize, request.PageIndex, null, null)
                    .ConfigureAwait(false);
            }
            else
            {
                getData = await DefaultRepository<TDbContext, TModel, TRead>.Instance.GetModelListAsync(
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
            await CacheService.RemoveCacheAsync(typeof(TView), key);
            return NoContent();
        }

        [HttpGet("remove-cache")]
        public virtual async Task<ActionResult> ClearCacheAsync()
        {
            await CacheService.RemoveCacheAsync(typeof(TView));
            return NoContent();
        }

        // POST: api/s
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public virtual async Task<ActionResult<TModel>> Create([FromBody] TView data)
        {
            ReflectionHelper.SetPropertyValue(data, new JProperty("CreatedBy", User.Identity.Name));
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

        // PUT: api/s/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Update(string id, [FromBody] TView data)
        {
            if (data != null)
            {
                ReflectionHelper.SetPropertyValue(data, new JProperty("ModifiedBy", User.Identity.Name));
                ReflectionHelper.SetPropertyValue(data, new JProperty("LastModified", DateTime.UtcNow));
                var currentId = ReflectionHelper.GetPropertyValue(data, "Id").ToString();
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

        // PATCH: api/v1/rest/en-us/attribute-set/portal/5
        [HttpPatch("{id}")]
        public virtual async Task<IActionResult> Patch(string id, [FromBody] JObject fields)
        {
            var result = await GetSingleAsync(id);
            if (result.IsSucceed)
            {
                ReflectionHelper.SetPropertyValue(result.Data, new JProperty("ModifiedBy", User.Identity.Name));
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

        // DELETE: api/v1/rest/en-us/attribute-set/portal/5
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
                predicate = ReflectionHelper.CombineExpression(predicate, idPre, Heart.Enums.MixHeartEnums.ExpressionMethod.And);
            }

            return await GetSingleAsync<T>(predicate);
        }

        protected async Task<RepositoryResponse<TView>> GetSingleAsync(string id)
        {
            Expression<Func<TModel, bool>> predicate = ReflectionHelper.GetExpression<TModel>("Id", id, Heart.Enums.MixHeartEnums.ExpressionMethod.Eq);
            if (!string.IsNullOrEmpty(_lang))
            {
                var idPre = ReflectionHelper.GetExpression<TModel>("Specificulture", _lang, Heart.Enums.MixHeartEnums.ExpressionMethod.Eq);
                predicate = ReflectionHelper.CombineExpression(predicate, idPre, Heart.Enums.MixHeartEnums.ExpressionMethod.And);
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

        protected async Task<RepositoryResponse<TModel>> DeleteAsync<T>(string id, bool isDeleteRelated = false)
            where T : Mix.Domain.Data.ViewModels.ViewModelBase<TDbContext, TModel, T>
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

            var data = await GetSingleAsync(id);
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
            where T : Mix.Domain.Data.ViewModels.ViewModelBase<TDbContext, TModel, T>
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

            var data = await DefaultRepository<TDbContext, TModel, TView>.Instance.RemoveListModelAsync(isRemoveRelatedModel, predicate);

            return data;
        }

        protected async Task<RepositoryResponse<Lib.ViewModels.FileViewModel>> ExportListAsync(Expression<Func<TModel, bool>> predicate, string type)
        {

            var getData = await DefaultModelRepository<TDbContext, TModel>.Instance.GetModelListByAsync(predicate, _context);
            Lib.ViewModels.FileViewModel file = null;
            if (getData.IsSucceed)
            {
                string exportPath = $"Exports/Structures/{typeof(TModel).Name}";
                string filename = $"{type.ToString()}_{DateTime.UtcNow.ToString("ddMMyyyy")}";
                var objContent = new JObject(
                    new JProperty("type", type.ToString()),
                    new JProperty("data", JArray.FromObject(getData.Data))
                    );
                file = new Lib.ViewModels.FileViewModel()
                {
                    Filename = filename,
                    Extension = ".json",
                    FileFolder = exportPath,
                    Content = objContent.ToString()
                };
                // Copy current templates file
                Lib.Repositories.FileRepository.Instance.SaveWebFile(file);

            }
            UnitOfWorkHelper<TDbContext>.HandleTransaction(getData.IsSucceed, true, _transaction);
            return new RepositoryResponse<Lib.ViewModels.FileViewModel>()
            {
                IsSucceed = true,
                Data = file,
            };

        }
        protected async Task<RepositoryResponse<PaginationModel<TRead>>> GetListAsync(Expression<Func<TModel, bool>> predicate = null)
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

            RepositoryResponse<PaginationModel<TRead>> data = null;

            if (data == null)
            {

                if (predicate != null)
                {
                    data = await DefaultRepository<TDbContext, TModel, TRead>.Instance.GetModelListByAsync(
                        predicate, request.OrderBy, request.Direction, request.PageSize, request.PageIndex, null, null);
                }
                else
                {
                    data = await DefaultRepository<TDbContext, TModel, TRead>.Instance.GetModelListAsync(request.OrderBy, request.Direction, request.PageSize, request.PageIndex, null, null).ConfigureAwait(false);

                }

            }
            return data;
        }

        protected async Task<RepositoryResponse<TView>> SaveAsync(TView vm, bool isSaveSubModel)
        {
            if (vm != null)
            {

                var result = await vm.SaveModelAsync(isSaveSubModel).ConfigureAwait(false);

                return result;
            }
            return new RepositoryResponse<TView>();
        }

        protected async Task<RepositoryResponse<TModel>> SaveAsync(JObject obj, Expression<Func<TModel, bool>> predicate)
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

        protected async Task<RepositoryResponse<List<TView>>> SaveListAsync(List<TView> lstVm, bool isSaveSubModel)
        {

            var result = await DefaultRepository<TDbContext, TModel, TView>.Instance.SaveListModelAsync(lstVm, isSaveSubModel);

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


        #endregion

    }

}
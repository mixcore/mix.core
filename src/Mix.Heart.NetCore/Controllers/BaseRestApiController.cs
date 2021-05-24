using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Common.Helper;
using Mix.Heart.Infrastructure.Repositories;
using Mix.Heart.Models;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Extensions;
using Mix.Heart.Helpers;
using Mix.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Mix.Infrastructure.Repositories;

namespace Mix.Heart.NetCore.Controllers
{
    [Produces("application/json")]
    public class BaseRestApiController<TDbContext, TModel, TView> : Controller
        where TDbContext : DbContext
        where TModel : class
        where TView : ViewModelBase<TDbContext, TModel, TView>
    {
        protected static DefaultRepository<TDbContext, TModel, TView> _repository = DefaultRepository<TDbContext, TModel, TView>.Instance;
        protected static TDbContext _context;
        protected static IDbContextTransaction _transaction;
        protected string _lang;
        protected bool _forbidden;

        /// <summary>
        /// The domain
        /// </summary>
        protected string _domain;



        #region Helpers

        protected async Task<RepositoryResponse<TView>> GetSingleAsync(string id)
        {
            Expression<Func<TModel, bool>> predicate = ReflectionHelper.GetExpression<TModel>("Id", id, Heart.Enums.ExpressionMethod.Eq);
            RepositoryResponse<TView> data = null;
            if (predicate != null)
            {
                data = await DefaultRepository<TDbContext, TModel, TView>.Instance.GetSingleModelAsync(predicate);
            }
            return data;
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

        protected async Task<RepositoryResponse<TModel>> DeleteAsync(Expression<Func<TModel, bool>> predicate, bool isDeleteRelated = false)
        {
            var data = await DefaultRepository<TDbContext, TModel, TView>.Instance.GetSingleModelAsync(predicate);
            if (data.IsSucceed)
            {
                var result = await data.Data.RemoveModelAsync(isDeleteRelated).ConfigureAwait(false);

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

        protected async Task<RepositoryResponse<List<TModel>>> DeleteListAsync(Expression<Func<TModel, bool>> predicate, bool isRemoveRelatedModel = false)
        {
            var data = await DefaultRepository<TDbContext, TModel, TView>.Instance.RemoveListModelAsync(isRemoveRelatedModel, predicate);
            return data;
        }

        protected async Task<RepositoryResponse<FileViewModel>> ExportListAsync(Expression<Func<TModel, bool>> predicate, string type)
        {
            var getData = await DefaultModelRepository<TDbContext, TModel>.Instance.GetModelListByAsync(predicate, _context);
            FileViewModel file = null;
            if (getData.IsSucceed)
            {
                string exportPath = $"Export/Structures/{typeof(TModel).Name}";
                string filename = $"{type}_{DateTime.UtcNow.ToString("ddMMyyyy")}";
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
                MixFileRepository.Instance.SaveWebFile(file);
            }
            UnitOfWorkHelper<TDbContext>.HandleTransaction(getData.IsSucceed, true, _transaction);
            return new RepositoryResponse<FileViewModel>()
            {
                IsSucceed = true,
                Data = file,
            };
        }

        protected async Task<RepositoryResponse<PaginationModel<TView>>> GetListAsync(Expression<Func<TModel, bool>> predicate = null)
        {
            int.TryParse(Request.Query["pageIndex"], out int pageIndex);
            Enum.TryParse(Request.Query["direction"], out Heart.Enums.DisplayDirection direction);
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

        #endregion Helpers

        #region Routes

        [HttpGet]
        public virtual async Task<ActionResult<PaginationModel<TView>>> Get()
        {
            bool isFromDate = DateTime.TryParse(Request.Query["fromDate"], out DateTime fromDate);
            bool isToDate = DateTime.TryParse(Request.Query["toDate"], out DateTime toDate);
            int.TryParse(Request.Query["pageIndex"], out int pageIndex);
            Enum.TryParse(Request.Query["direction"], out Heart.Enums.DisplayDirection direction);
            bool isPageSize = int.TryParse(Request.Query["pageSize"], out int pageSize);

            RequestPaging request = new RequestPaging()
            {
                PageIndex = pageIndex,
                PageSize = isPageSize ? pageSize : 100,
                OrderBy = Request.Query["orderBy"].ToString().ToTitleCase(),
                Direction = direction
            };

            RepositoryResponse<PaginationModel<TView>> getData = await DefaultRepository<TDbContext, TModel, TView>.Instance.GetModelListAsync(
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
                return NotFound();
            }
        }

        // POST: api/s
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<TModel>> Create([FromBody] TView data)
        {
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
        public async Task<IActionResult> Update(string id, [FromBody] TView data)
        {
            var currentId = ReflectionHelper.GetPropertyValue(data, "id").ToString();
            if (id != currentId)
            {
                return BadRequest();
            }
            var result = await SaveAsync(data, true);
            if (result.IsSucceed)
            {
                return NoContent();
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

        // PATCH: api/v1/rest/en-us/attribute-set/portal/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(string id, [FromBody] JObject fields)
        {
            var result = await GetSingleAsync(id);
            if (result.IsSucceed)
            {
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
        public async Task<ActionResult<TModel>> Delete(string id)
        {
            var predicate = ReflectionHelper.GetExpression<TModel>("id", id, Enums.ExpressionMethod.Eq);
            var result = await DeleteAsync(predicate, false);
            if (result.IsSucceed)
            {
                return Ok(result.Data);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpGet("clear-cache")]
        protected async Task ClearCacheAsync(Type type)
        {
            await MixCacheService.RemoveCacheAsync(type: type);
        }

        #endregion Routes
    }
}
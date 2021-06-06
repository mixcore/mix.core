using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mix.Heart.Repository;
using Mix.Heart.Helpers;
using Mix.Heart.Entity;

namespace Mix.Lib.Controllers
{
    public class MixRestApiControllerBase<TDbContext, TEntity, TPrimaryKey> : Controller
        where TPrimaryKey : IComparable
        where TDbContext : DbContext
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected readonly QueryRepository<TDbContext, TEntity, TPrimaryKey> _repository;
        protected static TDbContext _context;
        protected static IDbContextTransaction _transaction;
        protected string _lang;
        protected bool _forbidden;

        /// <summary>
        /// The domain
        /// </summary>
        protected string _domain;

        public MixRestApiControllerBase(QueryRepository<TDbContext, TEntity, TPrimaryKey> repository)
        {
            _repository = repository;
        }

        #region Helpers

        protected async Task<TEntity> GetSingleAsync(string id)
        {
            Expression<Func<TEntity, bool>> predicate = ReflectionHelper.GetExpression<TEntity>("Id", id, Heart.Enums.ExpressionMethod.Eq);
            TEntity data = null;
            if (predicate != null)
            {
                data = await _repository.GetSingleAsync(predicate);
            }
            return data;
        }

        protected async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            TEntity  data = null;
            if (predicate != null)
            {
                data = await _repository.GetSingleAsync(predicate);
            }
            return data;
        }

        protected async Task<TEntity> DeleteAsync(Expression<Func<TEntity, bool>> predicate, bool isDeleteRelated = false)
        {
            return await _repository.GetSingleAsync(predicate);
        }

        //protected async Task<TModel> DeleteAsync(TModel data, bool isDeleteRelated = false)
        //{
        //    return new RepositoryResponse<TModel>() { IsSucceed = false };
        //}

        //protected async Task<RepositoryResponse<List<TModel>>> DeleteListAsync(Expression<Func<TModel, bool>> predicate, bool isRemoveRelatedModel = false)
        //{
        //    var data = await _repository.RemoveListModelAsync(isRemoveRelatedModel, predicate);
        //    return data;
        //}

        //protected async Task<RepositoryResponse<FileViewModel>> ExportListAsync(Expression<Func<TModel, bool>> predicate, string type)
        //{
        //    var getData = await DefaultModelRepository<TDbContext, TModel>.Instance.GetModelListByAsync(predicate, _context);
        //    FileViewModel file = null;
        //    if (getData.IsSucceed)
        //    {
        //        string exportPath = $"Export/Structures/{typeof(TModel).Name}";
        //        string filename = $"{type}_{DateTime.UtcNow.ToString("ddMMyyyy")}";
        //        var objContent = new JObject(
        //            new JProperty("type", type.ToString()),
        //            new JProperty("data", JArray.FromObject(getData.Data))
        //            );
        //        file = new FileViewModel()
        //        {
        //            Filename = filename,
        //            Extension = ".json",
        //            FileFolder = exportPath,
        //            Content = objContent.ToString()
        //        };
        //        // Copy current templates file
        //        MixFileRepository.Instance.SaveWebFile(file);
        //    }
        //    UnitOfWorkHelper<TDbContext>.HandleTransaction(getData.IsSucceed, true, _transaction);
        //    return new RepositoryResponse<FileViewModel>()
        //    {
        //        IsSucceed = true,
        //        Data = file,
        //    };
        //}

        //protected async Task<RepositoryResponse<PaginationModel<TView>>> GetListAsync(Expression<Func<TModel, bool>> predicate = null)
        //{
        //    var query = new SearchQueryModel(Request);
        //    RepositoryResponse<PaginationModel<TView>> data = null;

        //    if (data == null)
        //    {
        //        if (predicate != null)
        //        {
        //            data = await _repository.GetModelListByAsync(
        //                predicate, query.PagingData.OrderBy, query.PagingData.Direction, query.PagingData.PageSize, query.PagingData.PageIndex, null, null);
        //        }
        //        else
        //        {
        //            data = await _repository.GetModelListAsync(
        //                query.PagingData.OrderBy, query.PagingData.Direction, query.PagingData.PageSize, query.PagingData.PageIndex, null, null).ConfigureAwait(false);
        //        }
        //    }
        //    return data;
        //}

        //protected async Task<RepositoryResponse<TView>> SaveAsync(TView vm, bool isSaveSubModel)
        //{
        //    if (vm != null)
        //    {
        //        var result = await vm.SaveModelAsync(isSaveSubModel).ConfigureAwait(false);

        //        return result;
        //    }
        //    return new RepositoryResponse<TView>();
        //}

        //protected async Task<RepositoryResponse<TModel>> SaveAsync(JObject obj, Expression<Func<TModel, bool>> predicate)
        //{
        //    if (obj != null)
        //    {
        //        List<EntityField> fields = new List<EntityField>();
        //        Type type = typeof(TModel);
        //        foreach (var item in obj.Properties())
        //        {
        //            var propName = System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToTitleCase(item.Name);
        //            PropertyInfo propertyInfo = type.GetProperty(propName);
        //            if (propertyInfo != null)
        //            {
        //                object val = Convert.ChangeType(item.Value, propertyInfo.PropertyType);
        //                var field = new EntityField()
        //                {
        //                    PropertyName = propName,
        //                    PropertyValue = val
        //                };
        //                fields.Add(field);
        //            }
        //        }

        //        var result = await _repository.UpdateFieldsAsync(predicate, fields);

        //        return result;
        //    }
        //    return new RepositoryResponse<TModel>();
        //}

        //protected async Task<RepositoryResponse<List<TView>>> SaveListAsync(List<TView> lstVm, bool isSaveSubModel)
        //{
        //    var result = await _repository.SaveListModelAsync(lstVm, isSaveSubModel);

        //    return result;
        //}

        //protected RepositoryResponse<List<TView>> SaveList(List<TView> lstVm, bool isSaveSubModel)
        //{
        //    var result = new RepositoryResponse<List<TView>>() { IsSucceed = true };
        //    if (lstVm != null)
        //    {
        //        foreach (var vm in lstVm)
        //        {
        //            var tmp = vm.SaveModel(isSaveSubModel,
        //                _context, _transaction);
        //            result.IsSucceed = result.IsSucceed && tmp.IsSucceed;
        //            if (!tmp.IsSucceed)
        //            {
        //                result.Exception = tmp.Exception;
        //                result.Errors.AddRange(tmp.Errors);
        //            }
        //        }
        //        return result;
        //    }

        //    return result;
        //}

        #endregion Helpers

        //#region Routes

        //[HttpGet]
        //public virtual async Task<ActionResult<PaginationModel<TView>>> Get([FromQuery] SearchRequestDto req)
        //{
        //    var query = new SearchQueryModel(req, _lang);

        //    RepositoryResponse<PaginationModel<TView>> getData = await _repository.GetModelListAsync(
        //        query.PagingData.OrderBy, query.PagingData.Direction, query.PagingData.PageSize, query.PagingData.PageIndex, null, null).ConfigureAwait(false);

        //    if (getData.IsSucceed)
        //    {
        //        return Ok(getData.Data);
        //    }
        //    else
        //    {
        //        return BadRequest(getData.Errors);
        //    }
        //}

        //// GET: api/v1/rest/{culture}/attribute-set-data/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<TView>> Get(string id)
        //{
        //    var getData = await GetSingleAsync(id);
        //    if (getData.IsSucceed)
        //    {
        //        return getData.Data;
        //    }
        //    else
        //    {
        //        return NotFound();
        //    }
        //}

        //// POST: api/s
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for
        //// more details see https://aka.ms/RazorPagesCRUD.
        //[HttpPost]
        //public async Task<ActionResult<TModel>> Create([FromBody] TView data)
        //{
        //    var result = await SaveAsync(data, true);
        //    if (result.IsSucceed)
        //    {
        //        return Ok(result.Data);
        //    }
        //    else
        //    {
        //        return BadRequest(result.Errors);
        //    }
        //}

        //// PUT: api/s/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for
        //// more details see https://aka.ms/RazorPagesCRUD.
        //[HttpPut("{id}")]
        //public async Task<IActionResult> Update(string id, [FromBody] TView data)
        //{
        //    var currentId = ReflectionHelper.GetPropertyValue(data, "id").ToString();
        //    if (id != currentId)
        //    {
        //        return BadRequest();
        //    }
        //    var result = await SaveAsync(data, true);
        //    if (result.IsSucceed)
        //    {
        //        return NoContent();
        //    }
        //    else
        //    {
        //        var current = await GetSingleAsync(currentId);
        //        if (!current.IsSucceed)
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            return BadRequest(result.Errors);
        //        }
        //    }
        //}

        //// PATCH: api/v1/rest/en-us/attribute-set/portal/5
        //[HttpPatch("{id}")]
        //public async Task<IActionResult> Patch(string id, [FromBody] JObject fields)
        //{
        //    var result = await GetSingleAsync(id);
        //    if (result.IsSucceed)
        //    {
        //        var saveResult = await result.Data.UpdateFieldsAsync(fields);
        //        if (saveResult.IsSucceed)
        //        {
        //            return NoContent();
        //        }
        //        else
        //        {
        //            return BadRequest(saveResult.Errors);
        //        }
        //    }
        //    else
        //    {
        //        return NotFound();
        //    }
        //}

        //// DELETE: api/v1/rest/en-us/attribute-set/portal/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<TModel>> Delete(string id)
        //{
        //    var predicate = ReflectionHelper.GetExpression<TModel>("id", id, ExpressionMethod.Eq);
        //    var result = await DeleteAsync(predicate, false);
        //    if (result.IsSucceed)
        //    {
        //        return Ok(result.Data);
        //    }
        //    else
        //    {
        //        return BadRequest(result.Errors);
        //    }
        //}

        //[HttpGet("clear-cache")]
        //protected async Task ClearCacheAsync(Type type)
        //{
        //    await MixCacheService.RemoveCacheAsync(type: type);
        //}

        //#endregion Routes
    }
}

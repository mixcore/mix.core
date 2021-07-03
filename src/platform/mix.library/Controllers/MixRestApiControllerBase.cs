using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mix.Heart.Repository;
using Mix.Heart.Helpers;
using Mix.Heart.Entities;
using Mix.Lib.Dtos;
using Mix.Lib.Models.Common;
using Mix.Heart.ViewModel;
using Mix.Shared.Services;
using Mix.Heart.Extensions;
using System.Reflection;
using Mix.Heart.Exceptions;
using Mix.Heart.Enums;
using Mix.Heart.Model;

namespace Mix.Lib.Controllers
{
    public class MixRestApiControllerBase<TView, TDbContext, TEntity, TPrimaryKey> : ControllerBase
        where TPrimaryKey : IComparable
        where TDbContext : DbContext
        where TEntity : EntityBase<TPrimaryKey>
        where TView : ViewModelBase<TDbContext, TEntity, TPrimaryKey>
    {
        protected readonly CommandRepository<TDbContext, TEntity, TPrimaryKey> _repository;
        protected readonly MixAppSettingService _appSettingService;
        protected static TDbContext _context;
        protected static IDbContextTransaction _transaction;
        protected string _lang;
        protected bool _forbidden;
        protected ConstructorInfo classConstructor = typeof(TView).GetConstructor(new Type[] { typeof(TEntity) });
        /// <summary>
        /// The domain
        /// </summary>
        protected string _domain;

        public MixRestApiControllerBase(
            CommandRepository<TDbContext, TEntity, TPrimaryKey> repository,
            MixAppSettingService appSettingService)
        {
            _repository = repository;
            _appSettingService = appSettingService;
        }

        #region Helpers

        protected async Task<PagingResponseModel<TView>> GetListAsync(SearchRequestDto req)
        {
            var searchRequest = new SearchQueryModel<TEntity, TPrimaryKey>(req);
            var query = _repository.GetPagingQuery(searchRequest.Predicate, searchRequest.PagingData);

            if (query != null)
            {
                var result = await query.ToPagingViewModelAsync<TDbContext, TView, TEntity, TPrimaryKey>(
                    _repository, searchRequest.PagingData, false);
                return result;
            }
            throw new MixHttpResponseException(MixErrorStatus.Badrequest, "Invalid Request");
        }

        protected async Task<TView> GetSingleAsync(string id)
        {
            Expression<Func<TEntity, bool>> predicate = ReflectionHelper.GetExpression<TEntity>("Id", id, Heart.Enums.ExpressionMethod.Eq);
            TEntity data = null;
            if (predicate != null)
            {
                data = await _repository.GetSingleAsync(predicate);
            }
            return data == null
                    ? throw new MixHttpResponseException(MixErrorStatus.NotFound, $"{id} not found")
                    : BuildViewModel(data);
        }

        protected async Task<TEntity> GetSingleAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            TEntity data = null;
            if (predicate != null)
            {
                data = await _repository.GetSingleAsync(predicate);
            }
            return data;
        }

        protected async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            await _repository.DeleteAsync(predicate);
        }

        protected async Task DeleteAsync(TEntity data)
        {
            await _repository.DeleteAsync(data);
        }

        protected async Task DeleteListAsync(Expression<Func<TEntity, bool>> predicate)
        {
            await _repository.DeleteManyAsync(predicate);
        }

        protected async Task<TPrimaryKey> SaveAsync(TView vm)
        {
            if (vm != null)
            {
                var id = await vm.SaveAsync().ConfigureAwait(false);
                return id;
            }
            throw new MixHttpResponseException(MixErrorStatus.Badrequest, "Invalid Object");
        }

        private TView BuildViewModel(TEntity data)
        {
            return (TView)classConstructor.Invoke(new object[] { data });
        }

        //protected async Task<RepositoryResponse<TEntity>> SaveAsync(JObject obj, Expression<Func<TEntity, bool>> predicate)
        //{
        //    if (obj != null)
        //    {
        //        List<EntityField> fields = new List<EntityField>();
        //        Type type = typeof(TEntity);
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
        //    return new RepositoryResponse<TEntity>();
        //}

        //protected async Task<RepositoryResponse<List<TView>>> SaveListAsync(List<TView> lstVm, bool isSaveSubModel)
        //{
        //    var result = await _repository.SaveLisTEntityAsync(lstVm, isSaveSubModel);

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

        //protected async Task<RepositoryResponse<FileViewModel>> ExportListAsync(Expression<Func<TEntity, bool>> predicate, string type)
        //{
        //    var getData = await DefaulTEntityRepository<TDbContext, TEntity>.Instance.GeTEntityListByAsync(predicate, _context);
        //    FileViewModel file = null;
        //    if (getData.IsSucceed)
        //    {
        //        string exportPath = $"Export/Structures/{typeof(TEntity).Name}";
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

        #endregion Helpers

        #region Routes

        [HttpGet]
        public virtual async Task<ActionResult<PagingResponseModel<TView>>> Get([FromQuery] SearchRequestDto req)
        {
            var searchRequest = new SearchQueryModel<TEntity, TPrimaryKey>(req);

            var query = _repository.GetPagingQuery(searchRequest.Predicate, searchRequest.PagingData);

            if (query != null)
            {
                var result = await query.ToPagingViewModelAsync<TDbContext, TView, TEntity, TPrimaryKey>(
                    _repository, searchRequest.PagingData, false);
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TView>> Get(string id)
        {
            var getData = await GetSingleAsync(id);
            if (getData != null)
            {
                return Ok(getData);
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
        public async Task<ActionResult<TEntity>> Create([FromBody] TView data)
        {
            var id = await SaveAsync(data);
            return Ok(id);
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
            var result = await SaveAsync(data);
            return Ok(result);
        }

        // DELETE: api/v1/rest/en-us/attribute-set/portal/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var predicate = ReflectionHelper.GetExpression<TEntity>("id", id, ExpressionMethod.Eq);
            await DeleteAsync(predicate);
            return Ok();
        }

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



        //[HttpGet("clear-cache")]
        //protected async Task ClearCacheAsync(Type type)
        //{
        //    await MixCacheService.RemoveCacheAsync(type: type);
        //}

        #endregion Routes
    }
}

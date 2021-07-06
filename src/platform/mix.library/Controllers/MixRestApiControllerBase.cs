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
using System.Reflection;
using Mix.Heart.Exceptions;
using Mix.Heart.Enums;
using Mix.Heart.Model;
using System.Collections.Generic;

namespace Mix.Lib.Controllers
{
    public class MixRestApiControllerBase<TView, TDbContext, TEntity, TPrimaryKey> : ControllerBase
        where TPrimaryKey : IComparable
        where TDbContext : DbContext
        where TEntity : EntityBase<TPrimaryKey>
        where TView : ViewModelBase<TDbContext, TEntity, TPrimaryKey>
    {
        protected readonly Repository<TDbContext, TEntity, TPrimaryKey> _repository;
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
            MixAppSettingService appSettingService,
            Repository<TDbContext, TEntity, TPrimaryKey> repository)
        {
            _repository = repository;
            _appSettingService = appSettingService;
        }

        #region Routes

        [HttpGet]
        public virtual async Task<ActionResult<PagingResponseModel<TView>>> Get([FromQuery] SearchRequestDto req)
        {
            var searchRequest = new SearchQueryModel<TEntity, TPrimaryKey>(req);

            return await _repository.GetPagingViewAsync<TView>(searchRequest.Predicate, searchRequest.PagingData);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TView>> Get(TPrimaryKey id)
        {
            var getData = await GetSingleAsync(id);
            return Ok(getData);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<TPrimaryKey>> Create([FromBody] TView data)
        {
            var id = await SaveAsync(data);
            return Ok(id);
        }

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

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var predicate = ReflectionHelper.GetExpression<TEntity>("id", id, ExpressionMethod.Eq);
            await DeleteAsync(predicate);
            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(TPrimaryKey id, [FromBody] IEnumerable<EntityPropertyModel> properties)
        {
            var result = await GetSingleAsync(id);
            await result.SaveFieldsAsync(properties);
            return Ok();
        }

        #endregion Routes

        #region Helpers

        protected async Task<PagingResponseModel<TView>> GetListAsync(SearchRequestDto req)
        {
            var searchRequest = new SearchQueryModel<TEntity, TPrimaryKey>(req);
            return await _repository.GetPagingViewAsync<TView>(searchRequest.Predicate, searchRequest.PagingData);
        }

        protected async Task<TView> GetSingleAsync(TPrimaryKey id)
        {
            return await _repository.GetSingleViewAsync<TView>(id);
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
            throw new MixException(MixErrorStatus.Badrequest, "Invalid Object");
        }

        #endregion Helpers


    }
}

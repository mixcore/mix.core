using GraphQL.AspNet.Attributes;
using GraphQL.AspNet.Controllers;
using GraphQL.AspNet.Interfaces.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mix.Constant.Constants;
using Mix.Heart.Entities;
using Mix.Heart.Models;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Base;
using Mix.Lib.Models.Common;
using Mix.Lib.Services;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Service.Models;
using Mix.Shared.Dtos;
using Mix.Shared.Extensions;

namespace Mix.Services.Graphql.Lib.Base
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class MixGraphQueryApiControllerBase<TDbContext, TEntity, TPrimaryKey>
        : GraphController
        where TPrimaryKey : IComparable
        where TDbContext : DbContext
        where TEntity : EntityBase<TPrimaryKey>
    {
        protected IHttpContextAccessor _httpContextAccessor;
        public MixGraphQueryApiControllerBase(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<TDbContext> uow, IQueueService<MessageQueueModel> queueService)
        {
            _httpContextAccessor = httpContextAccessor;
            Repository = new(uow);
        }

        #region Properties
        public EntityRepository<TDbContext, TEntity, TPrimaryKey> Repository { get; set; }
        private MixTenantSystemModel _currentTenant;

        protected MixTenantSystemModel? CurrentTenant
        {
            get
            {
                if (_currentTenant != null)
                {
                    return _currentTenant;
                }

                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext is not null)
                {
                    _currentTenant = httpContext.Session.Get<MixTenantSystemModel>(MixRequestQueryKeywords.Tenant);
                }

                return _currentTenant;
            }
        }

        #endregion

        #region Overrides

        #endregion

        #region Handler

        protected virtual async Task<IEnumerable<TEntity>> GetHanlder([FromQuery] SearchRequestDto req, CancellationToken cancellationToken = default)
        {
            var request = BuildSearchRequest(req);
            var result = await SearchHandler(request, cancellationToken);
            return result;
        }


        //[HttpGet("{id}")]
        //public async Task<ActionResult<TView>> GetSingle(TPrimaryKey id)
        //{
        //    var data = await GetById(id);
        //    return data != null ? Ok(data) : throw new MixException(MixErrorStatus.NotFound, id);
        //}



        //[HttpGet("default")]
        //public async Task<ActionResult<TView>> GetDefaultAsync()
        //{
        //    var result = (TView)Activator.CreateInstance(typeof(TView), Uow);
        //    if (result == null)
        //    {
        //        return BadRequest($"Cannot create {nameof(TView)} instance.");
        //    }

        //    result.InitDefaultValues(Culture?.Specificulture, Culture?.Id);
        //    await result.ExpandView();
        //    return Ok(result);
        //}

        #endregion Routes

        #region Helpers

        public virtual async Task<IEnumerable<TEntity>> SearchHandler(
            SearchQueryModel<TEntity, TPrimaryKey> searchRequest, 
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var result = await Repository.GetPagingAsync(searchRequest.Predicate, searchRequest.PagingData, cancellationToken);
            return result.Items;
        }

        public virtual SearchQueryModel<TEntity, TPrimaryKey> BuildSearchRequest(SearchRequestDto req)
        {
            if (!req.PageSize.HasValue)
            {
                req.PageSize = CurrentTenant?.Configurations.MaxPageSize;
            }

            return new SearchQueryModel<TEntity, TPrimaryKey>(_httpContextAccessor.HttpContext!.Request, req, CurrentTenant?.Id);
        }

        #endregion


    }
}

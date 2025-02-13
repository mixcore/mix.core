using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mix.Lib.Interfaces;
using Mix.Lib.Services;
using Mix.Mq.Lib.Models;
using Mix.SignalR.Interfaces;

namespace Mix.Lib.Base
{
    [ResponseCache(CacheProfileName = "Default")]
    public class MixQueryApiControllerBase<TView, TDbContext, TEntity, TPrimaryKey>
        : MixRestHandlerApiControllerBase<TView, TDbContext, TEntity, TPrimaryKey>
        where TPrimaryKey : IComparable
        where TDbContext : DbContext
        where TEntity : class, IEntity<TPrimaryKey>
        where TView : SimpleViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
    {
        public MixQueryApiControllerBase(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<TDbContext> uow, IMemoryQueueService<MessageQueueModel> queueService,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService)
            : base(httpContextAccessor, configuration,
                  cacheService, translator, mixIdentityService, uow, queueService, portalHub, mixTenantService)
        {
        }

        #region Overrides

        #endregion

        #region Routes

        [HttpGet]
        public virtual async Task<ActionResult<PagingResponseModel<TView>>> Get([FromQuery] SearchRequestDto request)
        {
            var result = await SearchHandler(request);
            return Ok(ParseSearchResult(request, result));
        }
        
        [HttpPost("filter")]
        public virtual async Task<ActionResult<PagingResponseModel<TView>>> Filter([FromBody] SearchRequestDto request)
        {
            var result = await SearchHandler(request);
            return Ok(ParseSearchResult(request, result));
        }


        [HttpGet("{id}")]
        public virtual async Task<ActionResult<TView>> GetSingle(TPrimaryKey id)
        {
            var data = await GetById(id);
            return data != null ? Ok(data) : throw new MixException(MixErrorStatus.NotFound, id);
        }



        [HttpGet("default")]
        public virtual async Task<ActionResult<TView>> GetDefaultAsync()
        {
            var result = (TView)Activator.CreateInstance(typeof(TView), Uow);
            if (result == null)
            {
                return BadRequest($"Cannot create {nameof(TView)} instance.");
            }

            result.InitDefaultValues(Culture?.Specificulture, Culture?.Id);
            await result.ExpandView();
            return Ok(result);
        }

        #endregion Routes


    }
}

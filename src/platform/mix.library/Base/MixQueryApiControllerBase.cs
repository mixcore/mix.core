using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mix.Lib.Services;
using Mix.SignalR.Interfaces;

namespace Mix.Lib.Base
{
    [ResponseCache(CacheProfileName = "Default")]
    public class MixQueryApiControllerBase<TView, TDbContext, TEntity, TPrimaryKey>
        : MixRestHandlerApiControllerBase<TView, TDbContext, TEntity, TPrimaryKey>
        where TPrimaryKey : IComparable
        where TDbContext : DbContext
        where TEntity : EntityBase<TPrimaryKey>
        where TView : ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
    {
        public MixQueryApiControllerBase(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<TDbContext> uow, IQueueService<MessageQueueModel> queueService,
            IPortalHubClientService portalHub)
            : base(httpContextAccessor, configuration, cacheService, translator, mixIdentityService, uow, queueService, portalHub)
        {
        }

        #region Overrides

        #endregion

        #region Routes

        [HttpGet]
        public async Task<ActionResult<PagingResponseModel<TView>>> Get([FromQuery] SearchRequestDto req)
        {
            var result = await SearchHandler(req);
            return Ok(ParseSearchResult(req, result));
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<TView>> GetSingle(TPrimaryKey id)
        {
            var data = await GetById(id);
            return data != null ? Ok(data) : throw new MixException(MixErrorStatus.NotFound, id);
        }



        [HttpGet("default")]
        public async Task<ActionResult<TView>> GetDefaultAsync()
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

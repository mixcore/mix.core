using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mix.Lib.Models.Common;
using Mix.Lib.Services;
using System.Reflection;

namespace Mix.Lib.Base
{
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
            MixService mixService, 
            TranslatorService translator, 
            MixIdentityService mixIdentityService, 
            UnitOfWorkInfo<MixCacheDbContext> cacheUOW, 
            UnitOfWorkInfo<TDbContext> uow, IQueueService<MessageQueueModel> queueService) 
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, cacheUOW, uow, queueService)
        {
        }

        #region Overrides

        #endregion

        #region Routes

        [HttpGet]
        public async Task<ActionResult<PagingResponseModel<TView>>> Get([FromQuery] SearchRequestDto req)
        {
            var result = await SearchHandler(req);
            return ParseSearchResult(req, result);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<TView>> GetSingle(TPrimaryKey id)
        {
            var data = await GetById(id);
            return data != null ? Ok(data) : throw new MixException(MixErrorStatus.NotFound, id);
        }



        [HttpGet("default")]
        [HttpGet($"default/{MixRequestQueryKeywords.Specificulture}")]
        public ActionResult<TView> GetDefault(string culture = null)
        {
            var result = (TView)Activator.CreateInstance(typeof(TView), new[] { _uow });
            result.InitDefaultValues(_culture?.Specificulture, _culture?.Id);
            result.ExpandView();
            return Ok(result);
        }

        #endregion Routes

       
    }
}

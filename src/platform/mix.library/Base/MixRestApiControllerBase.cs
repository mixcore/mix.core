using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mix.Heart.Repository;
using Mix.Heart.Helpers;
using Mix.Heart.Entities;
using Mix.Heart.Model;
using Mix.Lib.Services;
using Mix.Identity.Constants;
using Microsoft.Extensions.Configuration;

namespace Mix.Lib.Base
{
    public class MixRestApiControllerBase<TView, TDbContext, TEntity, TPrimaryKey> 
        : MixQueryApiControllerBase<TView, TDbContext, TEntity, TPrimaryKey>
        where TPrimaryKey : IComparable
        where TDbContext : DbContext
        where TEntity : EntityBase<TPrimaryKey>
        where TView : ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
    {
        public MixRestApiControllerBase(
            IConfiguration configuration,
            MixService mixService, 
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixIdentityService mixIdentityService,
            TDbContext context,
            MixCacheService cacheService,
            IQueueService<MessageQueueModel> queueService)
            : base(configuration, mixService, translator, cultureRepository, mixIdentityService, context, cacheService, queueService)
        {
            _repository = new(_uow);
        }

        protected Repository<TDbContext, TEntity, TPrimaryKey, TView> _repository;

        #region Routes

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public virtual async Task<ActionResult<TPrimaryKey>> Create([FromBody] TView data)
        {
            if (data == null)
            {
                return BadRequest("Null Object");
            }
            data.SetDbContext(_context);
            data.CreatedDateTime = DateTime.UtcNow;
            data.CreatedBy = _mixIdentityService.GetClaim(User, MixClaims.Username);
            var id = await data.SaveAsync(_uow);
            _queueService.PushMessage(data, MixRestAction.Post, MixRestStatus.Success);
            return Ok(id);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Update(string id, [FromBody] TView data)
        {
            var currentId = ReflectionHelper.GetPropertyValue(data, "id").ToString();
            if (id != currentId)
            {
                return BadRequest();
            }
            data.SetDbContext(_context);
            var result = await data.SaveAsync(_uow);
            await _cacheService.RemoveCacheAsync(id, typeof(TView));
            _queueService.PushMessage(data, MixRestAction.Put, MixRestStatus.Success);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public virtual async Task<ActionResult> Delete(TPrimaryKey id)
        {
            var data = await _repository.GetSingleAsync(id);
            await _repository.DeleteAsync(id);
            await _cacheService.RemoveCacheAsync(id.ToString(), typeof(TView));
            _queueService.PushMessage(data, MixRestAction.Delete, MixRestStatus.Success);
            return Ok();
        }

        [HttpPatch("{id}")]
        public virtual async Task<IActionResult> Patch(TPrimaryKey id, [FromBody] IEnumerable<EntityPropertyModel> properties)
        {
            var result = await _repository.GetSingleAsync(id);
            result.SetDbContext(_context);
            await result.SaveFieldsAsync(properties);
            await _cacheService.RemoveCacheAsync(id.ToString(), typeof(TView));
            _queueService.PushMessage(result, MixRestAction.Patch, MixRestStatus.Success);
            return Ok();
        }


        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost("save-many")]
        public virtual async Task<ActionResult<bool>> SaveMany([FromBody] List<TView> data)
        {
            if (data == null)
            {
                return BadRequest("Null Object");
            }
            foreach (var item in data)
            {
                await item.SaveAsync(_uow);
            }
            return Ok(true);
        }

        #endregion Routes
    }
}

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
            IQueueService<MessageQueueModel> queueService)
            : base(configuration, mixService, translator, cultureRepository, mixIdentityService, context, queueService)
        {
        }


        #region Routes

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<TPrimaryKey>> Create([FromBody] TView data)
        {
            if (data == null)
            {
                return BadRequest("Null Object");
            }
            data.SetUowInfo(_uow);
            return await CreateHandlerAsync(data);
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
            data.SetUowInfo(_uow);
            return await UpdateHandler(id, data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(TPrimaryKey id)
        {
            var data = await _repository.GetSingleAsync(id);
            data.SetUowInfo(_uow);
            return await DeleteHandler(data);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(TPrimaryKey id, [FromBody] IEnumerable<EntityPropertyModel> properties)
        {
            var data = await _repository.GetSingleAsync(id);
            data.SetUowInfo(_uow);
            return await PatchHandler(id, data, properties);
        }


        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost("save-many")]
        public async Task<ActionResult<bool>> SaveMany([FromBody] List<TView> data)
        {
            if (data == null)
            {
                return BadRequest("Null Object");
            }
            foreach (var item in data)
            {
                item.SetUowInfo(_uow);
            }
            return await SaveManyHandler(data);
        }

        
        #endregion Routes

        #region Handlers

        protected virtual async Task<ActionResult<TPrimaryKey>> CreateHandlerAsync(TView data)
        {
            data.CreatedDateTime = DateTime.UtcNow;
            data.CreatedBy = _mixIdentityService.GetClaim(User, MixClaims.Username);
            var id = await data.SaveAsync();
            _queueService.PushMessage(data, MixRestAction.Post, MixRestStatus.Success);
            return Ok(id);
        }

        protected virtual async Task<IActionResult> UpdateHandler(string id, TView data)
        {
            var result = await data.SaveAsync();
            await MixCacheService.Instance.RemoveCacheAsync(id, typeof(TView));
            _queueService.PushMessage(data, MixRestAction.Put, MixRestStatus.Success);
            return Ok(result);
        }

        protected virtual async Task<ActionResult> DeleteHandler(TView data)
        {
            await data.DeleteAsync();
            await MixCacheService.Instance.RemoveCacheAsync(data.Id.ToString(), typeof(TView));
            _queueService.PushMessage(data, MixRestAction.Delete, MixRestStatus.Success);
            return Ok();
        }


        protected virtual async Task<IActionResult> PatchHandler(TPrimaryKey id, TView data, IEnumerable<EntityPropertyModel> properties)
        {
            await data.SaveFieldsAsync(properties);
            await MixCacheService.Instance.RemoveCacheAsync(id.ToString(), typeof(TView));
            _queueService.PushMessage(data, MixRestAction.Patch, MixRestStatus.Success);
            return Ok();
        }

        protected virtual async Task<ActionResult<bool>> SaveManyHandler(List<TView> data)
        {
            foreach (var item in data)
            {
                await item.SaveAsync();
            }
            return Ok();
        }

        #endregion
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mix.Lib.Interfaces;
using Mix.Lib.Services;
using Mix.Mq.Lib.Models;

namespace Mix.Lib.Base
{
    public class MixRestEntityApiControllerBase<TDbContext, TEntity, TPrimaryKey>
        : MixQueryEntityApiControllerBase<TDbContext, TEntity, TPrimaryKey>
        where TPrimaryKey : IComparable
        where TDbContext : DbContext
        where TEntity : class, IEntity<TPrimaryKey>
    {

        public MixRestEntityApiControllerBase(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            MixCacheDbContext cacheDbContext,
            TDbContext context,
            IQueueService<MessageQueueModel> queueService,
            IMixTenantService mixTenantService)
            : base(httpContextAccessor, configuration,
                  cacheService, translator, mixIdentityService, context, queueService, cacheDbContext, mixTenantService)
        {
        }


        #region Routes

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<TPrimaryKey>> Create([FromBody] TEntity data)
        {
            if (data == null)
            {
                return BadRequest("Null Object");
            }
            if (ReflectionHelper.HasProperty(typeof(TEntity), MixRequestQueryKeywords.TenantId))
            {
                ReflectionHelper.SetPropertyValue(data, new EntityPropertyModel()
                {
                    PropertyName = MixRequestQueryKeywords.TenantId,
                    PropertyValue = CurrentTenant?.Id
                });
            }
            var result = await CreateHandlerAsync(data);
            return Ok(result);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] TEntity data)
        {
            var currentId = ReflectionHelper.GetPropertyValue(data, "id").ToString();
            if (id != currentId)
            {
                return BadRequest();
            }
            await UpdateHandler(id, data);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(TPrimaryKey id)
        {
            var data = await Repository.GetSingleAsync(id);
            await DeleteHandler(data);
            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(TPrimaryKey id, [FromBody] IEnumerable<EntityPropertyModel> properties)
        {
            var data = await Repository.GetSingleAsync(id);
            await PatchHandler(id, data, properties);
            return Ok();
        }


        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost("save-many")]
        public async Task<ActionResult> SaveMany([FromBody] List<TEntity> data)
        {
            if (data == null)
            {
                return BadRequest("Null Object");
            }
            await SaveManyHandler(data);
            return Ok();
        }


        #endregion Routes

        #region Handlers

        protected virtual async Task<TPrimaryKey> CreateHandlerAsync(TEntity data)
        {
            await Repository.CreateAsync(data);
            QueueService.PushMessage(CurrentTenant.Id, data, MixRestAction.Post.ToString(), true);
            return data.Id;
        }

        protected virtual async Task UpdateHandler(string id, TEntity data)
        {
            await Repository.UpdateAsync(data);
            await CacheService.RemoveCacheAsync(id, typeof(TEntity).FullName);
            QueueService.PushMessage(CurrentTenant.Id, data, MixRestAction.Put.ToString(), true);
        }

        protected virtual async Task DeleteHandler(TEntity data)
        {
            await Repository.DeleteAsync(data);
            await CacheService.RemoveCacheAsync(data.Id.ToString(), typeof(TEntity).FullName);
            QueueService.PushMessage(CurrentTenant.Id, data, MixRestAction.Delete.ToString(), true);
        }


        protected virtual async Task PatchHandler(TPrimaryKey id, TEntity data, IEnumerable<EntityPropertyModel> properties)
        {
            await Repository.SaveFieldsAsync(data, properties);
            await CacheService.RemoveCacheAsync(id.ToString(), typeof(TEntity).FullName);
            QueueService.PushMessage(CurrentTenant.Id, data, MixRestAction.Patch.ToString(), true);
        }

        protected virtual async Task SaveManyHandler(List<TEntity> data)
        {
            foreach (var item in data)
            {
                await Repository.SaveAsync(item);
            }
        }

        #endregion
    }
}

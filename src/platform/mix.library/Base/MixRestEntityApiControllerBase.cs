using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mix.Lib.Services;

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
            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixIdentityService mixIdentityService,
            MixCacheDbContext cacheDbContext,
            TDbContext context,
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor, configuration, mixService, translator, cultureRepository, mixIdentityService, context, queueService, cacheDbContext)
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
            var data = await _repository.GetSingleAsync(id);
            await DeleteHandler(data);
            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(TPrimaryKey id, [FromBody] IEnumerable<EntityPropertyModel> properties)
        {
            var data = await _repository.GetSingleAsync(id);
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
            await _repository.CreateAsync(data);
            _queueService.PushMessage(data, MixRestAction.Post.ToString(), true);
            return data.Id;
        }

        protected virtual async Task UpdateHandler(string id, TEntity data)
        {
            await _repository.UpdateAsync(data);
            await _cacheService.RemoveCacheAsync(id, typeof(TEntity));
            _queueService.PushMessage(data, MixRestAction.Put.ToString(), true);
        }

        protected virtual async Task DeleteHandler(TEntity data)
        {
            await _repository.DeleteAsync(data);
            await _cacheService.RemoveCacheAsync(data.Id.ToString(), typeof(TEntity));
            _queueService.PushMessage(data, MixRestAction.Delete.ToString(), true);
        }


        protected virtual async Task PatchHandler(TPrimaryKey id, TEntity data, IEnumerable<EntityPropertyModel> properties)
        {
            await _repository.SaveFieldsAsync(data, properties);
            await _cacheService.RemoveCacheAsync(id.ToString(), typeof(TEntity));
            _queueService.PushMessage(data, MixRestAction.Patch.ToString(), true);
        }

        protected virtual async Task SaveManyHandler(List<TEntity> data)
        {
            foreach (var item in data)
            {
                await _repository.SaveAsync(item);
            }
        }

        #endregion
    }
}

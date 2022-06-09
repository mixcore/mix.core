using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mix.Identity.Constants;
using Mix.Lib.Services;

namespace Mix.Lib.Base
{
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class MixRestApiControllerBase<TView, TDbContext, TEntity, TPrimaryKey>
        : MixQueryApiControllerBase<TView, TDbContext, TEntity, TPrimaryKey>
        where TPrimaryKey : IComparable
        where TDbContext : DbContext
        where TEntity : EntityBase<TPrimaryKey>
        where TView : ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
    {

        public MixRestApiControllerBase(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCacheDbContext> cacheUOW,
            UnitOfWorkInfo<TDbContext> uow,
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor, configuration, mixService, translator, cultureRepository, mixIdentityService, cacheUOW, uow, queueService)
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
                throw new MixException(MixErrorStatus.Badrequest, "Null Object");
            }
            if (ReflectionHelper.HasProperty(typeof(TView), MixRequestQueryKeywords.MixTenantId))
            {
                ReflectionHelper.SetPropertyValue(data, new EntityPropertyModel()
                {
                    PropertyName = MixRequestQueryKeywords.MixTenantId,
                    PropertyValue = MixTenantId
                });
            }
            data.SetUowInfo(_uow);
            var result = await CreateHandlerAsync(data);
            return Ok(result);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] TView data)
        {
            var currentId = ReflectionHelper.GetPropertyValue(data, "id").ToString();
            if (id != currentId)
            {
                throw new MixException(MixErrorStatus.Badrequest, "Invalid Id");
            }
            data.SetUowInfo(_uow);
            await UpdateHandler(id, data);
            return Ok(id);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(TPrimaryKey id)
        {
            var data = await _repository.GetSingleAsync(id);
            if (data != null)
            {
                data.SetUowInfo(_uow);
                await DeleteHandler(data);
                return Ok(id);
            }
            throw new MixException(MixErrorStatus.NotFound, "Not Found");
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(TPrimaryKey id, [FromBody] IEnumerable<EntityPropertyModel> properties)
        {
            var data = await _repository.GetSingleAsync(id);
            if (data == null)
            {
                return NotFound();
            }
            data.SetUowInfo(_uow);
            await PatchHandler(id, data, properties);
            return Ok(id);
        }


        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost("save-many")]
        public async Task<ActionResult> SaveMany([FromBody] List<TView> data)
        {
            if (data == null)
            {
                throw new MixException(MixErrorStatus.Badrequest, "Null Object");
            }
            foreach (var item in data)
            {
                item.SetUowInfo(_uow);
            }
            await SaveManyHandler(data);
            return Ok();
        }


        #endregion Routes

        #region Handlers

        protected virtual async Task<TPrimaryKey> CreateHandlerAsync(TView data)
        {
            data.CreatedDateTime = DateTime.UtcNow;
            data.CreatedBy = _mixIdentityService.GetClaim(User, MixClaims.Username);
            var id = await data.SaveAsync();
            _queueService.PushMessage(data, MixRestAction.Post.ToString(), true);
            return id;
        }

        protected virtual async Task UpdateHandler(string id, TView data)
        {
            var result = await data.SaveAsync();
            await _cacheService.RemoveCacheAsync(id, typeof(TView));
            _queueService.PushMessage(data, MixRestAction.Put.ToString(), true);
        }

        protected virtual async Task DeleteHandler(TView data)
        {
            await data.DeleteAsync();
            await _cacheService.RemoveCacheAsync(data.Id.ToString(), typeof(TView));
            _queueService.PushMessage(data, MixRestAction.Delete.ToString(), true);
        }


        protected virtual async Task PatchHandler(TPrimaryKey id, TView data, IEnumerable<EntityPropertyModel> properties)
        {
            await data.SaveFieldsAsync(properties);
            await _cacheService.RemoveCacheAsync(id.ToString(), typeof(TView));
            _queueService.PushMessage(data, MixRestAction.Patch.ToString(), true);
        }

        protected virtual async Task SaveManyHandler(List<TView> data)
        {
            foreach (var item in data)
            {
                await item.SaveAsync();
            }
        }

        #endregion
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mix.Database.Entities.Account;
using Mix.Lib.Dtos;
using Mix.Lib.Interfaces;
using Mix.Lib.Services;
using Mix.Mq.Lib.Models;
using Mix.SignalR.Interfaces;

namespace Mix.Lib.Base
{
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public class MixRestfulApiControllerBase<TView, TDbContext, TEntity, TPrimaryKey>
        : MixQueryApiControllerBase<TView, TDbContext, TEntity, TPrimaryKey>
        where TPrimaryKey : IComparable
        where TDbContext : DbContext
        where TEntity : EntityBase<TPrimaryKey>
        where TView : ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
    {
        private UnitOfWorkInfo<MixCmsAccountContext> uow;
        private IMemoryQueueService<MessageQueueModel> queueService;
        private IPortalHubClientService portalHub;

        public MixRestfulApiControllerBase(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration, MixCacheService cacheService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<TDbContext> uow, IMemoryQueueService<MessageQueueModel> queueService,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService)
            : base(httpContextAccessor, configuration, 
                  cacheService, translator, mixIdentityService, uow, queueService, portalHub, mixTenantService)
        {
        }

        public MixRestfulApiControllerBase(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, MixCacheService cacheService, TranslatorService translator, MixIdentityService mixIdentityService, UnitOfWorkInfo<TDbContext> uow, UnitOfWorkInfo<MixCmsAccountContext> accUow, IMemoryQueueService<MessageQueueModel> queueService, IPortalHubClientService portalHub, IMixTenantService mixTenantService)
             : base(httpContextAccessor, configuration,
                  cacheService, translator, mixIdentityService, uow, queueService, portalHub, mixTenantService)
        {
            HttpContextAccessor = httpContextAccessor;
            this.uow = accUow;
            this.queueService = queueService;
            this.portalHub = portalHub;
            MixTenantService = mixTenantService;
        }

        #region Routes

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public virtual async Task<ActionResult<TView>> Create([FromBody] TView data)
        {
            var id = await CreateHandlerAsync(data);
            var result = await GetById(id);
            return Ok(result);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Update(TPrimaryKey id, [FromBody] TView data)
        {
            await UpdateHandler(id, data);
            var result = await GetById(id);
            return Ok(result);
        }

        [HttpDelete("remove-cache/{id}")]
        public virtual async Task<ActionResult> DeleteCache(TPrimaryKey id, [FromServices] MixCacheService cacheService, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await RemoveCacheHandler(cacheService, id);
            return Ok();
        }

        [HttpDelete("{id}")]
        public virtual async Task<ActionResult> Delete(TPrimaryKey id)
        {
            var data = await Repository.GetSingleAsync(id);
            if (data != null)
            {
                await DeleteHandler(data);
                return Ok(id);
            }
            throw new MixException(MixErrorStatus.NotFound, "Not Found");
        }

        [HttpPatch]
        public virtual async Task<IActionResult> Patch([FromBody] JObject obj)
        {
            await PatchHandler(obj);
            return Ok();
        }


        [HttpPatch("patch-many")]
        public virtual async Task<IActionResult> PatchMany([FromBody] IEnumerable<JObject> lstObj,
                CancellationToken cancellationToken = default)
        {
            await PatchManyHandler(lstObj, cancellationToken);
            return Ok();
        }


        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost("save-many")]
        public virtual async Task<ActionResult> SaveMany([FromBody] List<TView> data)
        {
            if (data == null)
            {
                throw new MixException(MixErrorStatus.Badrequest, "Null Object");
            }
            await SaveManyHandler(data);
            return Ok();
        }


        [HttpPut("update-priority/{id}")]
        public virtual async Task<ActionResult> UpdatePriority(UpdatePriorityDto<TPrimaryKey> dto)
        {
            var data = await Repository.GetSingleAsync(dto.Id);
            if (data == null)
            {
                return NotFound();
            }

            var min = Math.Min(data.Priority, dto.Priority);
            var max = Math.Max(data.Priority, dto.Priority);
            var query = await Repository.GetListAsync(m => !m.Id.Equals(dto.Id) && m.Priority >= min & m.Priority <= max);
            int start = min;
            if (dto.Priority == min)
            {
                data.Priority = dto.Priority;
                start++;
            }
            foreach (var item in query.OrderBy(m => m.Priority))
            {
                item.Priority = start;
                await item.SaveAsync();
                start++;
            }
            if (dto.Priority == max)
            {
                data.Priority = start;
            }
            await data.SaveAsync();
            return Ok();
        }




        #endregion Routes

        #region Privates

        private async Task RemoveCacheHandler(MixCacheService cacheService, TPrimaryKey id)
        {
            await cacheService.RemoveCacheAsync(id, typeof(TEntity).FullName);
        }

        #endregion
    }
}

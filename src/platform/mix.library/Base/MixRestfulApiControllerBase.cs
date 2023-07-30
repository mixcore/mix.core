using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mix.Lib.Dtos;
using Mix.Lib.Services;
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
        public MixRestfulApiControllerBase(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration, MixCacheService cacheService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<TDbContext> uow, IQueueService<MessageQueueModel> queueService,
            IPortalHubClientService portalHub)
            : base(httpContextAccessor, configuration, cacheService, translator, mixIdentityService, uow, queueService, portalHub)
        {
        }

        #region Routes

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<TView>> Create([FromBody] TView data)
        {
            var id = await CreateHandlerAsync(data);
            var result = await GetById(id);
            return Ok(result);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(TPrimaryKey id, [FromBody] TView data)
        {
            await UpdateHandler(id, data);
            var result = await GetById(id);
            return Ok(result);
        }

        [HttpDelete("remove-cache/{id}")]
        public async Task<ActionResult> DeleteCache(TPrimaryKey id, [FromServices] MixCacheService cacheService, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await RemoveCacheHandler(cacheService, id);
            return Ok();
        }

        private async Task RemoveCacheHandler(MixCacheService cacheService, TPrimaryKey id)
        {
            await cacheService.RemoveCacheAsync(id, typeof(TEntity).FullName);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(TPrimaryKey id)
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
        public async Task<IActionResult> Patch([FromBody] JObject obj)
        {
            await PatchHandler(obj);
            return Ok();
        }


        [HttpPatch("patch-many")]
        public async Task<IActionResult> PatchMany([FromBody] IEnumerable<JObject> lstObj,
                CancellationToken cancellationToken = default)
        {
            await PatchManyHandler(lstObj, cancellationToken);
            return Ok();
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
            await SaveManyHandler(data);
            return Ok();
        }


        [HttpPut("update-priority/{id}")]
        public async Task<ActionResult> UpdatePriority(UpdatePriorityDto<TPrimaryKey> dto)
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
    }
}

﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mix.Identity.Constants;
using Mix.Lib.Dtos;
using Mix.Lib.Services;

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
        public MixRestfulApiControllerBase(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, MixService mixService, TranslatorService translator, MixIdentityService mixIdentityService, UnitOfWorkInfo<MixCacheDbContext> cacheUOW, UnitOfWorkInfo<TDbContext> uow, IQueueService<MessageQueueModel> queueService) : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, cacheUOW, uow, queueService)
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
            var result = await GetById(id);
            return Ok(result);
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


        [HttpPut("update-priority/{id}")]
        public async Task<ActionResult> UpdatePriority(UpdatePriorityDto<TPrimaryKey> dto)
        {
            var data = await _repository.GetSingleAsync(dto.Id);
            if (data == null)
            {
                return NotFound();
            }

            var min = Math.Min(data.Priority, dto.Priority);
            var max = Math.Max(data.Priority, dto.Priority);
            var query = await _repository.GetListAsync(m => !m.Id.Equals(dto.Id) && m.Priority >= min & m.Priority <= max);
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
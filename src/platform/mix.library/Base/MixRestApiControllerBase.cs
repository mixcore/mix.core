using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Mix.Heart.Repository;
using Mix.Heart.Helpers;
using Mix.Heart.Entities;
using Mix.Heart.ViewModel;
using Mix.Shared.Services;
using Mix.Heart.Model;
using System.Collections.Generic;
using Mix.Database.Entities.Cms;
using Mix.Lib.Services;
using Mix.Identity.Constants;
using Microsoft.Extensions.Configuration;
using Mix.Heart.Services;

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
            GlobalConfigService globalConfigService,
            MixService mixService, 
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixIdentityService mixIdentityService,
            TDbContext context,
            MixCacheService cacheService)
            : base(configuration, globalConfigService, mixService, translator, cultureRepository, mixIdentityService, context, cacheService)
        {
        }

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
            data.CreatedBy = MixIdentityService.GetClaim(User, MixClaims.Username);
            var id = await data.SaveAsync(_uow);
            return Ok(id);
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
            data.SetDbContext(_context);
            var result = await data.SaveAsync(_uow);
            await _cacheService.RemoveCacheAsync(id, typeof(TView));
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(TPrimaryKey id)
        {
            await _repository.DeleteAsync(id);
            await _cacheService.RemoveCacheAsync(id.ToString(), typeof(TView));
            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(TPrimaryKey id, [FromBody] IEnumerable<EntityPropertyModel> properties)
        {
            var result = await _repository.GetSingleAsync(id);
            result.SetDbContext(_context);
            await result.SaveFieldsAsync(properties);
            await _cacheService.RemoveCacheAsync(id.ToString(), typeof(TView));
            return Ok();
        }

        #endregion Routes
    }
}

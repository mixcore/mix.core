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
using Microsoft.Extensions.Logging;
using Mix.Lib.Services;
using Mix.Identity.Constants;

namespace Mix.Lib.Abstracts
{
    public class MixRestApiControllerBase<TView, TDbContext, TEntity, TPrimaryKey> 
        : MixQueryApiControllerBase<TView, TDbContext, TEntity, TPrimaryKey>
        where TPrimaryKey : IComparable
        where TDbContext : DbContext
        where TEntity : EntityBase<TPrimaryKey>
        where TView : ViewModelBase<TDbContext, TEntity, TPrimaryKey, TView>
    {
        public MixRestApiControllerBase(
            ILogger<MixApiControllerBase> logger,
            GlobalConfigService globalConfigService,
            MixService mixService, 
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixIdentityService mixIdentityService,
            TDbContext context)
            : base(logger, globalConfigService, mixService, translator, cultureRepository, mixIdentityService, context)
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
            data.CreatedDateTime = DateTime.UtcNow;
            data.CreatedBy = MixIdentityService.GetClaim(User, MixClaims.Username);
            var id = await data.SaveAsync();
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
            var result = await data.SaveAsync();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(TPrimaryKey id)
        {
            var getData = await _repository.GetSingleAsync(id);
            await getData.DeleteAsync();
            return Ok(getData);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(TPrimaryKey id, [FromBody] IEnumerable<EntityPropertyModel> properties)
        {
            var result = await _repository.GetSingleAsync(id);
            await result.SaveFieldsAsync(properties);
            return Ok();
        }

        #endregion Routes
    }
}

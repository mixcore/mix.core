using Microsoft.AspNetCore.Mvc;
using Mix.Database.Entities.Cms.v2;
using Mix.Heart.Repository;
using Mix.Lib.Abstracts;
using Mix.Portal.Domain.ViewModels;
using Mix.Shared.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-data")]
    [ApiController]
    public class MixDataPortalController : MixRestApiControllerBase<MixDataViewModel, MixCmsContext, MixData, Guid>
    {
        public MixDataPortalController(
            MixAppSettingService appSettingService, 
            Repository<MixCmsContext, MixData, Guid> repository, 
            Repository<MixCmsContext, MixCulture, int> cultureRepository) : base(appSettingService, repository, cultureRepository)
        {
        }

        [HttpPost("submit-data/{lang}/{databaseName}")]
        public async Task<ActionResult> CreateData([FromRoute] string databaseName, [FromBody] JObject data)
        {
            var mixData = new MixDataViewModel(_lang, _culture.Id, databaseName, data);
            var result = await mixData.SaveAsync();
            return Ok(result);
        }
    }
}

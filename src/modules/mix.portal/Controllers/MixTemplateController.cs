using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mix.Database.Entities.Cms;
using Mix.Heart.Repository;
using Mix.Lib.Abstracts;
using Mix.Lib.Services;
using Mix.Lib.ViewModels;
using Mix.Shared.Services;
using System.Threading.Tasks;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-template")]
    [ApiController]
    public class MixTemplateController
        : MixRestApiControllerBase<MixTemplateViewModel, MixCmsContext, MixViewTemplate, int>
    {
        public MixTemplateController(
            ILogger<MixApiControllerBase> logger,
            GlobalConfigService globalConfigService,
            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixIdentityService mixIdentityService,
            MixCmsContext context)
            : base(logger, globalConfigService, mixService, translator, cultureRepository, mixIdentityService, context)
        {

        }
        [HttpGet("copy/{id}")]
        public async Task<ActionResult<MixTemplateViewModel>> Copy(int id)
        {
            var getData = await _repository.GetSingleAsync(id);
            if (getData != null)
            {
                var copyResult = await getData.CopyAsync();
                if (copyResult != null)
                {
                    return Ok(copyResult);
                }
                else
                {
                    return BadRequest(copyResult.Errors);
                }
            }
            else
            {
                return NotFound();
            }
        }
    }
}

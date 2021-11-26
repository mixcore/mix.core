using Microsoft.AspNetCore.Mvc;
using Mix.Heart.Repository;
using Mix.Lib.Dtos;
using Mix.Lib.Services;
using Mix.Lib.ViewModels;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-theme")]
    [ApiController]
    public class MixThemeController
        : MixRestApiControllerBase<MixThemeViewModel, MixCmsContext, MixTheme, int>
    {
        private readonly MixThemeExportService _exportService;
        private readonly MixThemeImportService _importService;
        
        public MixThemeController(
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixIdentityService mixIdentityService,
            MixThemeExportService exportService,
            MixCmsContext context, MixThemeImportService importService,
            IQueueService<MessageQueueModel> queueService,
            MixCacheService cacheService)
            : base(configuration, mixService, translator, cultureRepository, mixIdentityService, context, cacheService, queueService)
        {

            _exportService = exportService;
            _importService = importService;
        }

        // POST api/theme
        /// Swagger cannot generate multi-form value api
        [HttpPost]
        [DisableRequestSizeLimit]
        [Route("save")]
        public async Task<ActionResult<MixThemeViewModel>> Save(
            [FromForm] string model, [FromForm] IFormFile theme)
        {
            var data = JsonConvert.DeserializeObject<MixThemeViewModel>(model);
            data.CreatedBy = _mixIdentityService.GetClaim(User, MixClaims.Username);
            var saveResult = await data.SaveThemeAsync(theme, _uow);
            if (saveResult)
            {
                return Ok(data);
            }
            return BadRequest();
        }

        [HttpPost("export")]
        public async Task<ActionResult<SiteDataViewModel>> ExportThemeAsync([FromBody] ExportThemeDto dto)
        {
            var siteData = await _exportService.ExportTheme(dto);
            return Ok(siteData);
        }
    }
}

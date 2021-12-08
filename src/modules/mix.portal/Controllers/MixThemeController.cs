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
        [HttpPost]
        [Route("save")]
        public async Task<ActionResult<MixThemeViewModel>> Save(MixThemeViewModel data)
        {
            data.SetUowInfo(_uow);
            data.CreatedBy = _mixIdentityService.GetClaim(User, MixClaims.Username);
            await data.SaveAsync();
            return Ok(data);
        }

        [HttpPost("export")]
        public async Task<ActionResult<SiteDataViewModel>> ExportThemeAsync([FromBody] ExportThemeDto dto)
        {
            var siteData = await _exportService.ExportTheme(dto);
            return Ok(siteData);
        }

        [HttpPost("load-theme")]
        public ActionResult<SiteDataViewModel> LoadTheme([FromForm] IFormFile theme)
        {
            _importService.ExtractTheme(theme);
            var siteData = _importService.LoadSchema();
            return Ok(siteData);
        }

        [HttpPost("import-theme")]
        public async Task<ActionResult<SiteDataViewModel>> ImportThemeAsync([FromBody] SiteDataViewModel siteData)
        {
            if (ModelState.IsValid)
            {
                siteData.CreatedBy = _mixIdentityService.GetClaim(User, MixClaims.Username);
                var result = await _importService.ImportSelectedItemsAsync(siteData);
                return Ok(result);
            }
            return BadRequest(ModelState);
        }
    }
}

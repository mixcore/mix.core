using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mix.Database.Entities.Cms;
using Mix.Heart.Repository;
using Mix.Lib.Abstracts;
using Mix.Lib.Dtos;
using Mix.Lib.Services;
using Mix.Lib.ViewModels;
using Mix.Shared.Services;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-theme")]
    [ApiController]
    public class MixThemeController
        : MixRestApiControllerBase<MixThemeViewModel, MixCmsContext, MixTheme, int>
    {
        private readonly MixThemeExportService _exportService;
        public MixThemeController(
            ILogger<MixApiControllerBase> logger,
            GlobalConfigService globalConfigService,
            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixIdentityService mixIdentityService, 
            MixThemeExportService exportService,
            MixCmsContext context)
            : base(logger, globalConfigService, mixService, translator, cultureRepository, mixIdentityService, context)
        {
            
            _exportService = exportService;
        }

        [HttpPost("export")]
        public ActionResult<SiteDataViewModel> ExportTheme(ExportThemeDto dto)
        {
            var siteData = _exportService.ExportTheme(dto);
            return Ok(siteData);
        }
    }
}

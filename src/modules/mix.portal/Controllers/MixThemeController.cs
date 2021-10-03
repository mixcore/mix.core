using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mix.Database.Entities.Cms;
using Mix.Heart.Repository;
using Mix.Lib.Abstracts;
using Mix.Lib.Dtos;
using Mix.Lib.Services;
using Mix.Lib.ViewModels;
using Mix.Portal.Domain.ViewModels;
using Mix.Shared.Services;
using System.Threading.Tasks;

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
            Repository<MixCmsContext, MixCulture, int> cultureRepository,
            Repository<MixCmsContext, MixTheme, int> repository,
            MixIdentityService mixIdentityService, 
            MixThemeExportService exportService)
            : base(logger, globalConfigService, mixService, translator, cultureRepository, repository, mixIdentityService)
        {
            
            _exportService = exportService;
        }

        [HttpPost("export")]
        public ActionResult<SiteDataViewModel> ExportTheme(ExportThemeDto dto)
        {
            var siteData = _exportService.ExportSelectedItems(dto);
            return Ok(siteData);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mix.Database.Entities.Cms;
using Mix.Heart.Repository;
using Mix.Lib.Abstracts;
using Mix.Lib.Attributes;
using Mix.Lib.Services;
using Mix.Portal.Domain.ViewModels;
using Mix.Shared.Services;
using System.ComponentModel;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-theme")]
    [ApiController]
    public class MixThemeController
        : MixRestApiControllerBase<MixThemeViewModel, MixCmsContext, MixTheme, int>
    {
        public MixThemeController(
            ILogger<MixApiControllerBase> logger, 
            GlobalConfigService globalConfigService, 
            MixService mixService, 
            TranslatorService translator, 
            Repository<MixCmsContext, MixCulture, int> cultureRepository, 
            Repository<MixCmsContext, MixTheme, int> repository, 
            MixIdentityService mixIdentityService) 
            : base(logger, globalConfigService, mixService, translator, cultureRepository, repository, mixIdentityService)
        {
        }
    }
}

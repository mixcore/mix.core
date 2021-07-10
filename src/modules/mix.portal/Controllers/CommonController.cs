using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mix.Lib.Abstracts;
using Mix.Lib.Services;
using Mix.Portal.Domain.Models;
using Mix.Shared.Services;
using System;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/portal/common")]
    [ApiController]
    public class CommonController : MixApiControllerBase
    {
        public CommonController(
            ILogger<MixApiControllerBase> logger, 
            MixAppSettingService appSettingService, 
            MixService mixService, 
            TranslatorService translator) : base(logger, appSettingService, mixService, translator)
        { }

        [HttpGet, HttpOptions]
        [Route("{culture}/dashboard")]
        public ActionResult<DashboardModel> Dashboard(string culture)
        {
            var result = new DashboardModel(culture);
            return Ok(result);
        }
    }
}

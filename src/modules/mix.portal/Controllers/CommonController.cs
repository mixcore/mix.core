using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mix.Database.Entities.Cms;
using Mix.Database.Services;
using Mix.Heart.Repository;
using Mix.Lib.Base;
using Mix.Lib.Services;
using Mix.Portal.Domain.Models;
using Mix.Shared.Services;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/common")]
    [ApiController]
    public class CommonController : MixApiControllerBase
    {
        private readonly MixCmsContext _context;
        public CommonController(
            IConfiguration configuration,
            MixCmsContext context, 
            MixService mixService, 
            TranslatorService translator, 
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixIdentityService mixIdentityService)
            : base(configuration, mixService, translator, cultureRepository, mixIdentityService)
        {
            _context = context;
        }

        [HttpGet]
        [Route("{culture}/dashboard")]
        public ActionResult<DashboardModel> Dashboard(string culture)
        {
            var result = new DashboardModel(culture, _context);
            return Ok(result);
        }
    }
}

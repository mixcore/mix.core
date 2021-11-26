using Microsoft.AspNetCore.Mvc;
using Mix.Heart.Repository;
using Mix.Lib.Services;
using Mix.Portal.Domain.Models;

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
            MixIdentityService mixIdentityService,
            IQueueService<MessageQueueModel> queueService)
            : base(configuration, mixService, translator, cultureRepository, mixIdentityService, queueService)
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

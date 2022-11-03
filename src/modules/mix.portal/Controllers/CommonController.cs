using Microsoft.AspNetCore.Mvc;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/common")]
    [ApiController]
    public class CommonController : MixTenantApiControllerBase
    {
        private readonly MixCmsContext _context;
        public CommonController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCmsContext context,
            MixService mixService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, queueService)
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

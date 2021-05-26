using Microsoft.AspNetCore.Mvc;

namespace Mix.Theme.Controllers.v2
{
    [Route("api/v2/mix-theme")]
    [ApiController]
    public class ThemeController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            return Ok("test");
        }
    }
}

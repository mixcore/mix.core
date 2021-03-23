using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Mix.Cms.Api.RestFul.Controllers.v1
{
    [Route("api/v1/rest/shared")]
    [ApiController]
    public class MixSharedController : ControllerBase
    {
        [HttpGet, HttpOptions]
        [Route("get-shared-settings")]
        public JObject GetSharedSettingsAsync()
        {
            return new JObject();
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mix.Infrastructure.Repositories;
using Mix.Lib.Constants;
using Newtonsoft.Json.Linq;

namespace Mix.Common.Controllers.v2
{
    [Route("api/v2/portal")]
    [Produces("application/json")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        [Route("jarray-data/{name}")]
        public ActionResult<JArray> LoadData(string name)
        {
            try
            {
                var cultures = MixFileRepository.Instance.GetFile(name, MixFolders.JsonDataFolder, true, "[]");
                var obj = JObject.Parse(cultures.Content);
                return Ok(obj["data"] as JArray);
            }
            catch
            {
                return NotFound();
            }
        }
    }
}

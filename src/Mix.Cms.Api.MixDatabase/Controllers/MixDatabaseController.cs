using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.MixDatabase.Repositories;
using Mix.Cms.Lib.Services;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Threading.Tasks;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Models.Common;
using Mix.Heart.Models;

namespace Mix.Cms.Api.MixDatabase.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/v1/rest/{culture}/mixdb/{tableName}")]
    [Route("api/v1/rest/mixdb/{tableName}")]
    public class MixDatabaseController : Controller
    {
        protected string _lang;
        protected string _tableName;
        protected bool _forbidden;
        protected string _domain;
        private readonly MixDbRepository _repo;
        public MixDatabaseController(MixDbRepository repo)
        {
            _repo = repo;
        }

        #region Overrides

        /// <summary>
        /// Called before the action method is invoked.
        /// </summary>
        /// <param name="context">The action executing context.</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            GetLanguage();
            if (MixService.GetIpConfig<bool>("IsRetrictIp"))
            {
                var allowedIps = MixService.GetIpConfig<JArray>("AllowedIps") ?? new JArray();
                var exceptIps = MixService.GetIpConfig<JArray>("ExceptIps") ?? new JArray();
                string remoteIp = Request.HttpContext?.Connection?.RemoteIpAddress?.ToString();
                if (
                    // allow localhost
                    //remoteIp != "::1" &&
                    (!allowedIps.Any(t => t.Value<string>() == "*") && !allowedIps.Contains(remoteIp)) ||
                    (exceptIps.Any(t => t.Value<string>() == remoteIp))
                    )
                {
                    _forbidden = true;
                }
            }
            base.OnActionExecuting(context);
        }

        protected void GetLanguage()
        {
            _lang = RouteData?.Values["culture"] != null ? RouteData.Values["culture"].ToString()
                    : MixService.GetAppSetting<string>(MixAppSettingKeywords.DefaultCulture);
            _tableName = $"{MixConstants.CONST_MIXDB_PREFIX}{RouteData.Values["tableName"]}";
            _domain = string.Format("{0}://{1}", Request.Scheme, Request.Host);
        }

        #endregion Overrides

        #region Routes

        [HttpGet]
        public virtual async Task<ActionResult<PaginationModel<dynamic>>> Get([FromQuery] string query)
        {
            var pagingData = new PagingRequest(Request);
            JObject jsonQueries = null;
            if (!string.IsNullOrEmpty(query))
            {
                jsonQueries = JObject.Parse(query);
            }
            var result = await _repo.GetAllAsync(_tableName, jsonQueries, pagingData);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult<dynamic>> GetSingle(string id)
        {
            var result = await _repo.GetAsync(_tableName, id);
            return result != null ? Ok(result)
                : NotFound();
        }

        #endregion

    }
}

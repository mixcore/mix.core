using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Services;
using Mix.Identity.Models;
using System.Threading.Tasks;

namespace Mix.Cms.Web.Controllers
{
    public class PageController : BaseController
    {

        #region contructor

        protected override void ValidateRequest()
        {
            base.ValidateRequest();

            // If this site has not been inited yet
            if (MixService.GetConfig<bool>("IsInit"))
            {
                isValid = false;
                if (string.IsNullOrEmpty(MixService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION)))
                {
                    _redirectUrl = $"Init";
                }
                else
                {
                    var status = MixService.GetConfig<string>("InitStatus");
                    _redirectUrl = $"/init/step{status}";
                }
            }
        }

        #endregion contructor

        #region Routes

        [Route("page/{seoName}")]
        [Route("page/{culture}/{seoName}")]
        public async Task<IActionResult> Index(string culture, string seoName)
        {
            if (isValid)
            {
                return await Page(seoName);
            }
            else
            {
                return Redirect(_redirectUrl);
            }
        }

        #endregion Routes
    }
}
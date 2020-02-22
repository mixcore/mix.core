using Microsoft.AspNetCore.Mvc;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Services;
using System.Threading.Tasks;

namespace Mix.Cms.Web.Controllers
{
    public class ModuleController : BaseController
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

        [Route("module/{id}/{seoName}")]
        [Route("module/{culture}/{id}/{seoName}")]
        public async Task<IActionResult> Index(int id, string culture, string seoName)
        {
            if (isValid)
            {
                return await Module(id);
            }
            else
            {
                return Redirect(_redirectUrl);
            }
        }

        #endregion Routes
    }
}
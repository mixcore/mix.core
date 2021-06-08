using Microsoft.AspNetCore.Mvc;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Services;
using System.Threading.Tasks;

namespace Mix.Cms.Web.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class PostController : BaseController
    {
        #region contructor

        protected override void ValidateRequest()
        {
            base.ValidateRequest();

            // If this site has not been inited yet
            if (MixService.GetConfig<bool>(MixAppSettingKeywords.IsInit))
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

        [Route("post/{id}")]
        [Route("post/{id}/{seoName}")]
        [Route("{culture}/post/{id}")]
        [Route("{culture}/post/{id}/{seoName}")]
        [Route("blog/{id}")]
        [Route("blog/{id}/{seoName}")]
        [Route("{culture}/blog/{id}")]
        [Route("{culture}/blog/{id}/{seoName}")]
        public async Task<IActionResult> Index(int id, string culture, string seoName)
        {
            if (isValid)
            {
                return await Post(id);
            }
            else
            {
                return Redirect(_redirectUrl);
            }
        }

        #endregion Routes
    }
}
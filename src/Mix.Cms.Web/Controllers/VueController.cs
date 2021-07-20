using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Services;
using System.Threading.Tasks;

namespace Mix.Cms.Web.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class VueController : BaseController
    {
        #region contructor

        protected override void ValidateRequest()
        {
            base.ValidateRequest();

            // If this site has not been inited yet
            if (MixService.GetAppSetting<bool>(MixAppSettingKeywords.IsInit))
            {
                isValid = false;
                if (string.IsNullOrEmpty(MixService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION)))
                {
                    _redirectUrl = $"Init";
                }
                else
                {
                    var status = MixService.GetAppSetting<string>("InitStatus");
                    _redirectUrl = $"/init/step{status}";
                }
            }
        }

        #endregion contructor

        #region Routes

        [Route("vue/{seoName}")]
        [Route("vue/{culture}/{seoName}")]
        public async Task<PartialViewResult> Index(string culture, string seoName)
        {
            var getModule = await Mix.Cms.Lib.ViewModels.MixModules.ReadMvcViewModel.Repository.GetSingleModelAsync(
                m => m.Name == seoName && m.Specificulture == culture);
            if (getModule.IsSucceed)
            {
                var myViewData = new ViewDataDictionary(new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(),
                new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary()) { { "ModuleViewModel",
                    getModule.Data} };
                myViewData.Model = getModule.Data;

                PartialViewResult result = new PartialViewResult()
                {
                    ViewName = "VueComponent",
                    ViewData = myViewData,
                };

                return result;
            }
            return null;
        }

        #endregion Routes
    }
}
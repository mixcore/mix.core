using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Mix.Infrastructure.Repositories;
using Mix.Lib.Constants;
using Mix.Lib.Entities.Cms;
using Mix.Lib.Services;
using Mixcore.Domain.ViewModels.Mvc;
using Mixcore.Models;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mixcore.Controllers
{
    public class HomeController : MixControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TranslatorService _translator;
        public HomeController(
            ILogger<HomeController> logger,
            MixService mixService,
            TranslatorService translator) : base(mixService)
        {
            _logger = logger;
            _translator = translator;
        }
        protected override void ValidateRequest()
        {
            base.ValidateRequest();

            // If this site has not been inited yet
            if (MixAppSettingService.GetConfig<bool>(MixAppSettingKeywords.IsInit))
            {
                isValid = false;
                if (string.IsNullOrEmpty(MixService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION)))
                {
                    _redirectUrl = "Init";
                }
                else
                {
                    var status = MixAppSettingService.GetConfig<string>("InitStatus");
                    _redirectUrl = $"/init/step{status}";
                }
            }
        }

        [Route("")]
        [Route("{seoName}")]
        [Route("{seoName}/{keyword}")]
        [Route("{culture}/{seoName}/{keyword}")]
        public async Task<IActionResult> Index(string seoName, string keyword)
        {
            if (!isValid)
            {
                return Redirect(_redirectUrl);
            }


            seoName = seoName ?? Request.Query["alias"];
            if (!string.IsNullOrEmpty(seoName))
            {
                if (CheckIsVueRoute(seoName))
                {
                    var staticFile = MixFileRepository.Instance.GetFile(seoName, MixFolders.WebRootPath);
                    if (staticFile != null)
                    {
                        return Ok(staticFile.Content);
                    }
                    else
                    {
                        var getModule = await MvcModuleViewModel.Repository.GetSingleModelAsync(
            m => m.Name == seoName && m.Specificulture == Culture);
                        if (getModule.IsSucceed)
                        {
                            var myViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(),
                            new ModelStateDictionary()) { { "ModuleViewModel",
                    getModule.Data} };
                            myViewData.Model = getModule.Data;

                            PartialViewResult result = new PartialViewResult()
                            {
                                ViewName = "VueComponent",
                                ViewData = myViewData,
                            };

                            return result;
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                }
                else
                {
                    HandleSeoName(ref seoName, ref keyword);
                }
            }
            return await AliasAsync(seoName, keyword);
        }

        [HttpPost]
        [Route("search")]
        [Route("{culture}/search")]
        public async Task<IActionResult> Search([FromBody] string keyword)
        {
            return await Page("search", keyword);
        }

        private void HandleSeoName(ref string seoName, ref string keyword)
        {
            using var ctx = new MixCmsContext();
            string temp = $"{seoName}/{keyword}";
            if (ctx.MixUrlAlias.Any(u => u.Alias == temp))
            {
                seoName = temp;
                keyword = string.Empty;
            }
            else
            {
                // Check url is end with '/' or '?'
                // Ex: en-us/page-name/ => seoName = en-us/page-name
                string regex = @"(.*)[(\/|\?|#)]$";
                System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex(regex, RegexOptions.IgnoreCase);
                Match m = r.Match(seoName);
                if (m.Success)
                {
                    seoName = m.Groups[1].Value;
                }

                // Check first group is culture
                // Ex: en-us/page-name => culture = en-us , seoName = page-name
                regex = @"^([A-Za-z]{1,8}|[A-Za-z]{1,8}(-[A-Za-z0-9]{1,8})|[A-Za-z]{1,8}(-[A-Za-z0-9]{1,8})(-[A-Za-z0-9]{1,8}))\/(.*)$";
                r = new System.Text.RegularExpressions.Regex(regex, RegexOptions.IgnoreCase);
                m = r.Match(seoName);
                if (m.Success)
                {
                    if (MixAppSettingService.Instance.CheckValidCulture(m.Groups[1].Value))
                    {
                        Culture = m.Groups[1].Value;
                        seoName = m.Groups[5].Value;
                    }
                }

                if (MixAppSettingService.Instance.CheckValidCulture(seoName))
                {
                    Culture = seoName;
                    seoName = keyword;
                    keyword = string.Empty;
                }
            }
        }

        protected bool CheckIsVueRoute(string seoName)
        {
            // Check if seoname is vue route
            var regex = @"^(.*)\.((vue)$)";
            var r = new System.Text.RegularExpressions.Regex(regex, RegexOptions.IgnoreCase);
            var m = r.Match(seoName);
            return m.Success;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

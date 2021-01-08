﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Repositories;
using Mix.Cms.Lib.Services;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
namespace Mix.Cms.Web.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HomeController : BaseController
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

        [HttpGet]
        [Route("")]
        [Route("{seoName}")]
        [Route("{culture}/{seoName}")]
        public async Task<IActionResult> Index(string seoName)
        {
            if (isValid)
            {
                seoName = seoName ?? Request.Query["alias"];
                if (!string.IsNullOrEmpty(seoName))
                {
                    if (CheckIsVueRoute(seoName))
                    {
                        var staticFile = FileRepository.Instance.GetFile(seoName, MixFolders.WebRootPath);
                        if (staticFile != null)
                        {
                            return Ok(staticFile.Content);
                        }
                        else
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
                            else
                            {
                                return NotFound();
                            }
                        }
                    }
                    else
                    {
                        HandleSeoName(ref seoName);
                    }
                }
                ViewData["Layout"] = "Masters/_Layout";
                return await AliasAsync(seoName);
            }
            else
            {
                return Redirect(_redirectUrl);
            }
        }

        private void HandleSeoName(ref string seoName)
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
                if (MixService.Instance.CheckValidCulture(m.Groups[1].Value))
                {
                    culture = m.Groups[1].Value;
                    seoName = m.Groups[5].Value;
                }
            }

            if (MixService.Instance.CheckValidCulture(seoName))
            {
                culture = seoName;
                seoName = string.Empty;
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

        #endregion Routes
    }
}
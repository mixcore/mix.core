// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Helpers;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Repositories;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.SignalR.Hubs;
using Mix.Cms.Lib.ViewModels;
using Mix.Common.Helper;
using Mix.Heart.Helpers;
using Mix.Heart.Models;
using Mix.Identity.Models;
using Mix.Infrastructure.Repositories;
using Mix.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Cms.Api.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/portal")]
    public class ApiPortalController :
        BaseApiController<MixCmsContext>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ApiPortalController(
           UserManager<ApplicationUser> userManager,
           SignInManager<ApplicationUser> signInManager,
           RoleManager<IdentityRole> roleManager,
           MixCmsContext context,
            Microsoft.AspNetCore.SignalR.IHubContext<PortalHub> hubContext,
            IMemoryCache memoryCache
            )
            : base(context, memoryCache, hubContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        #region Get

        [AllowAnonymous]
        [HttpGet, HttpOptions]
        [Route("jarray-data/{name}")]
        public RepositoryResponse<JArray> loadData(string name)
        {
            try
            {
                var cultures = MixFileRepository.Instance.GetFile(name, MixFolders.DataFolder, true, "[]");
                var obj = JObject.Parse(cultures.Content);
                return new RepositoryResponse<JArray>()
                {
                    IsSucceed = true,
                    Data = obj["data"] as JArray
                };
            }
            catch
            {
                return new RepositoryResponse<JArray>()
                {
                    IsSucceed = true,
                    Data = null
                };
            }
        }

        [AllowAnonymous]
        [HttpGet, HttpOptions]
        [Route("json-data/{name}")]
        public RepositoryResponse<JObject> loadJsonData(string name)
        {
            var cultures = MixFileRepository.Instance.GetFile(name, MixFolders.DataFolder, true, "{}");
            var obj = JObject.Parse(cultures.Content);
            return new RepositoryResponse<JObject>()
            {
                IsSucceed = true,
                Data = obj["data"] as JObject
            };
        }

        // GET api/category/id
        [HttpGet, HttpOptions]
        [Route("{culture}/translator")]
        [Route("translator")]
        public RepositoryResponse<JObject> Languages()
        {
            return new RepositoryResponse<JObject>()
            {
                IsSucceed = true,
                Data = MixService.GetTranslator(_lang)
            };
        }

        // GET api/category/id
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet, HttpOptions]
        [Route("{culture}/dashboard")]
        public RepositoryResponse<DashboardViewModel> Dashboard(string culture)
        {
            return new RepositoryResponse<DashboardViewModel>()
            {
                IsSucceed = true,
                Data = new DashboardViewModel(culture)
            };
        }

        // GET api/category/id
        [HttpGet, HttpOptions]
        [Route("sitemap")]
        public async Task<RepositoryResponse<FileViewModel>> SiteMapAsync()
        {
            return await SitemapService.ParseSitemapAsync();
        }

        // GET
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]
        [HttpGet, HttpOptions]
        [Route("app-settings/details")]
        public RepositoryResponse<JObject> LoadAppSettings()
        {
            var settings = MixFileRepository.Instance.GetFile("appsettings", MixFileExtensions.Json, string.Empty, true, "{}");
            return new RepositoryResponse<JObject>() { IsSucceed = true, Data = JObject.Parse(settings.Content) };
        }

        // GET api/category/id
        [HttpGet, HttpOptions]
        [Route("translate-url/{culture}/{type}/{id}")]
        public async Task<RepositoryResponse<string>> TranslateUrlAsync(string culture, string type, int id)
        {
            switch (type)
            {
                case "Post":
                    var getPost = await Lib.ViewModels.MixPosts.ReadListItemViewModel.Repository.GetSingleModelAsync(
                        a => a.Id == id && a.Specificulture == culture);
                    if (getPost.IsSucceed)
                    {
                        return new RepositoryResponse<string>()
                        {
                            IsSucceed = getPost.IsSucceed,
                            Data = MixCmsHelper.GetRouterUrl(new { action = "post", culture = _lang, id = getPost.Data.Id, getPost.Data.SeoName }, Request, Url)
                        };
                    }
                    else
                    {
                        return new RepositoryResponse<string>()
                        {
                            IsSucceed = getPost.IsSucceed,
                            Data = $"/{culture}"
                        };
                    }

                case "Page":
                    var getPage = await Lib.ViewModels.MixPages.ReadListItemViewModel.Repository.GetSingleModelAsync(
                        a => a.Id == id && a.Specificulture == culture);
                    if (getPage.IsSucceed)
                    {
                        return new RepositoryResponse<string>()
                        {
                            IsSucceed = getPage.IsSucceed,
                            Data = MixCmsHelper.GetRouterUrl(
                                new { culture = _lang, seoName = getPage.Data.SeoName }, Request, Url)
                        };
                    }
                    else
                    {
                        return new RepositoryResponse<string>()
                        {
                            IsSucceed = true,
                            Data = $"/{culture}"
                        };
                    }
                default:
                    return new RepositoryResponse<string>()
                    {
                        IsSucceed = true,
                        Data = $"/{culture}"
                    };
            }
        }

        #endregion Get

        #region Post

        [AllowAnonymous]
        [HttpPost, HttpOptions]
        [Route("encrypt")]
        public RepositoryResponse<string> Encrypt([FromBody] JObject model)
        {
            string data = model.GetValue("data").Value<string>();
            var encrypted = new JObject(new JProperty("encrypted", data));
            var key = MixService.GetAppSetting<string>(MixAppSettingKeywords.ApiEncryptKey);
            return new RepositoryResponse<string>()
            {
                Data = AesEncryptionHelper.EncryptString(data, key)
            };
        }

        [AllowAnonymous]
        [HttpPost, HttpOptions]
        [Route("decrypt")]
        public RepositoryResponse<string> Decrypt([FromBody] JObject model)
        {
            string data = model.GetValue("data")?.Value<string>();
            var key = MixService.GetAppSetting<string>(MixAppSettingKeywords.ApiEncryptKey);
            return new RepositoryResponse<string>()
            {
                Data = AesEncryptionHelper.DecryptString(data, key)
            };
        }

        // POST api/category
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]
        [HttpPost, HttpOptions]
        [Route("app-settings/save")]
        public async Task<RepositoryResponse<JObject>> SaveAppSettingsAsync([FromBody] JObject model)
        {
            var settings = MixFileRepository.Instance.GetFile("appsettings", MixFileExtensions.Json, string.Empty, true, "{}");
            if (model != null)
            {
                settings.Content = model.ToString();
                if (MixFileRepository.Instance.SaveFile(settings))
                {
                    MixService.Reload();
                    if (!MixService.GetMixConfig<bool>("IsCache"))
                    {
                        await Services.MixCacheService.RemoveCacheAsync();
                    }
                }
                MixService.SetConfig("LastUpdateConfiguration", DateTime.UtcNow);
            }
            return new RepositoryResponse<JObject>() { IsSucceed = model != null, Data = model };
        }

        // POST api/category
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]
        [HttpPost, HttpOptions]
        [Route("app-settings/save-global/{name}")]
        public RepositoryResponse<string> SaveGlobalSettings(string name, [FromBody] JObject model)
        {
            switch (name)
            {
                case "PortalThemeSettings":
                    MixService.SetConfig(name, model);
                    break;

                default:
                    MixService.SetConfig(name, model["value"].ToString());
                    break;
            }
            var result = MixService.SaveSettings();
            return new RepositoryResponse<string>() { IsSucceed = result };
        }

        // POST api/category
        [HttpGet, HttpOptions]
        [Route("app-settings/save-default")]
        public RepositoryResponse<bool> SaveDefaultAppSettings()
        {
            return new RepositoryResponse<bool>() { IsSucceed = true, Data = MixService.SaveSettings() };
        }

        [AllowAnonymous]
        [HttpPost, HttpOptions]
        [Route("sendmail")]
        public void SendMail([FromBody] JObject model)
        {
            MixService.SendMail(model.Value<string>("subject"), model.Value<string>("body"), MixService.GetConfig<string>("ContactEmail", _lang));
        }

        // POST api/category
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]
        [HttpPost, HttpOptions]
        [Route("import")]
        [Route("{culture}/import")]
        public async Task<RepositoryResponse<bool>> ImportAsync([FromForm] IFormFile assets)
        {
            string importFolder = $"Imports/Structures/{_lang}";
            var result = new RepositoryResponse<bool>();
            var saveFile = MixFileRepository.Instance.SaveWebFile(assets, $"{importFolder}/{assets.FileName}");
            if (saveFile != null)
            {
                var fileContent = MixFileRepository.Instance.GetWebFile($"{saveFile.Filename}{saveFile.Extension}",
                    saveFile.FileFolder);
                var obj = JObject.Parse(fileContent.Content);
                switch (obj["type"].Value<string>())
                {
                    case "Language":
                        var arrLanguage = obj["data"].ToObject<List<MixLanguage>>();
                        result = await Lib.ViewModels.MixLanguages.UpdateViewModel.ImportLanguages(arrLanguage, _lang);
                        return result;

                    case "Configuration":
                        var arrConfiguration = obj["data"].ToObject<List<MixConfiguration>>();
                        result = await Lib.ViewModels.MixConfigurations.UpdateViewModel.ImportConfigurations(arrConfiguration, _lang);
                        return result;

                    case "Module":
                        var arrModule = obj["data"].ToObject<List<MixModule>>();
                        result = await Lib.ViewModels.MixModules.Helper.Import(arrModule, _lang);
                        return result;

                    default:
                        return new RepositoryResponse<bool>() { IsSucceed = false };
                }
            }
            return new RepositoryResponse<bool>();
        }

        #endregion Post
    }
}
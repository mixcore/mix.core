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
using Mix.Cms.Lib.Helpers;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Repositories;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.ViewModels;
using Mix.Domain.Core.ViewModels;
using Mix.Heart.Helpers;
using Mix.Identity.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Mix.Cms.Lib.MixEnums;

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
            Microsoft.AspNetCore.SignalR.IHubContext<Hub.PortalHub> hubContext,
            IMemoryCache memoryCache
            )
            : base(context, memoryCache, hubContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        #region Get

        // GET api/category/id
        [AllowAnonymous]
        [HttpGet, HttpOptions]
        [Route("{culture}/settings")]
        [Route("settings")]
        public RepositoryResponse<GlobalSettingsViewModel> Settings()
        {
            var cultures = CommonRepository.Instance.LoadCultures();
            var culture = cultures.FirstOrDefault(c => c.Specificulture == _lang);
            GlobalSettingsViewModel settings = new GlobalSettingsViewModel()
            {
                Domain = MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.Domain),
                Lang = _lang,
                PortalThemeSettings = MixService.GetConfig<JObject>(MixConstants.ConfigurationKeyword.PortalThemeSettings),
                ThemeId = MixService.GetConfig<int>(MixConstants.ConfigurationKeyword.ThemeId, _lang),
                Cultures = cultures,
                PageTypes = Enum.GetNames(typeof(MixPageType)).ToList(),
                ModuleTypes = Enum.GetNames(typeof(MixModuleType)).ToList(),
                AttributeSetTypes = Enum.GetNames(typeof(MixAttributeSetDataType)).ToList(),
                DataTypes = Enum.GetNames(typeof(MixDataType)).ToList(),
                Statuses = Enum.GetNames(typeof(MixContentStatus)).ToList(),
                LastUpdateConfiguration = MixService.GetConfig<DateTime?>("LastUpdateConfiguration")
            };
            settings.LangIcon = culture?.Icon ?? MixService.GetConfig<string>("Language");
            return new RepositoryResponse<GlobalSettingsViewModel>()
            {
                IsSucceed = true,
                Data = settings
            };
        }

        // GET api/category/id
        [AllowAnonymous]
        [HttpGet, HttpOptions]
        [Route("{culture}/all-settings")]
        [Route("all-settings")]
        public RepositoryResponse<JObject> AllSettingsAsync()
        {
            return GetAllSettings();
        }

        [AllowAnonymous]
        [HttpGet, HttpOptions]
        [Route("jarray-data/{name}")]
        public RepositoryResponse<JArray> loadData(string name)
        {
            try
            {
                var cultures = FileRepository.Instance.GetFile(name, "data", true, "[]");
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
            var cultures = FileRepository.Instance.GetFile(name, "data", true, "{}");
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

        // GET api/configurations/id
        [AllowAnonymous]
        [HttpGet, HttpOptions]
        [Route("{culture}/global-settings")]
        [Route("global-settings")]
        public RepositoryResponse<JObject> GetGlobalSettings()
        {
            var cultures = CommonRepository.Instance.LoadCultures();
            var culture = cultures.FirstOrDefault(c => c.Specificulture == _lang);

            // Get Settings
            GlobalSettingsViewModel configurations = new GlobalSettingsViewModel()
            {
                Domain = MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.Domain),
                Lang = _lang,
                PortalThemeSettings = MixService.GetConfig<JObject>(MixConstants.ConfigurationKeyword.PortalThemeSettings),
                ThemeId = MixService.GetConfig<int>(MixConstants.ConfigurationKeyword.ThemeId, _lang),
                ApiEncryptKey = MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.ApiEncryptKey),
                ApiEncryptIV = MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.ApiEncryptIV),
                IsEncryptApi = MixService.GetConfig<bool>(MixConstants.ConfigurationKeyword.IsEncryptApi),
                Cultures = cultures,
                PageTypes = Enum.GetNames(typeof(MixPageType)).ToList(),
                ModuleTypes = Enum.GetNames(typeof(MixModuleType)).ToList(),
                AttributeSetTypes = Enum.GetNames(typeof(MixAttributeSetDataType)).ToList(),
                DataTypes = Enum.GetNames(typeof(MixDataType)).ToList(),
                Statuses = Enum.GetNames(typeof(MixContentStatus)).ToList(),
                LastUpdateConfiguration = MixService.GetConfig<DateTime?>("LastUpdateConfiguration")
            };

            configurations.LangIcon = culture?.Icon ?? MixService.GetConfig<string>("Language");
            return new RepositoryResponse<JObject>()
            {
                IsSucceed = true,
                Data = JObject.FromObject(configurations)
            };
        }

        // GET api/category/id
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

        // GET api/v1/portal/check-config
        [AllowAnonymous]
        [HttpGet, HttpOptions]
        [Route("check-config/{lastSync}")]
        public RepositoryResponse<JObject> checkConfig(DateTime lastSync)
        {
            var lastUpdate = MixService.GetConfig<DateTime>("LastUpdateConfiguration");
            if (lastSync.ToUniversalTime() < lastUpdate)
            {
                return GetAllSettings();
            }
            else
            {
                return new RepositoryResponse<JObject>()
                {
                    IsSucceed = true,
                };
            }
        }

        // GET api/category/id
        [HttpGet, HttpOptions]
        [Route("sitemap")]
        public RepositoryResponse<FileViewModel> SiteMap()
        {
            try
            {
                XNamespace aw = "http://www.sitemaps.org/schemas/sitemap/0.9";
                var root = new XElement(aw + "urlset");
                var pages = Lib.ViewModels.MixPages.ReadListItemViewModel.Repository.GetModelList();
                List<int> handledPageId = new List<int>();
                foreach (var page in pages.Data)
                {
                    page.DetailsUrl = MixCmsHelper.GetRouterUrl(
                                    new { culture = page.Specificulture, seoName = page.SeoName }, Request, Url);
                    var otherLanguages = pages.Data.Where(p => p.Id == page.Id && p.Specificulture != page.Specificulture);
                    var lstOther = new List<SitemapLanguage>();
                    foreach (var item in otherLanguages)
                    {
                        lstOther.Add(new SitemapLanguage()
                        {
                            HrefLang = item.Specificulture,
                            Href = MixCmsHelper.GetRouterUrl(
                                       new { culture = item.Specificulture, seoName = page.SeoName }, Request, Url)
                        });
                    }

                    var sitemap = new SiteMap()
                    {
                        ChangeFreq = "monthly",
                        LastMod = DateTime.UtcNow,
                        Loc = page.DetailsUrl,
                        Priority = 0.3,
                        OtherLanguages = lstOther
                    };
                    root.Add(sitemap.ParseXElement());
                }

                var posts = Lib.ViewModels.MixPosts.ReadListItemViewModel.Repository.GetModelList();
                foreach (var post in posts.Data)
                {
                    post.DetailsUrl = MixCmsHelper.GetRouterUrl(
                                    new { culture = post.Specificulture, action = "post", id = post.Id, seoName = post.SeoName }, Request, Url);
                    var otherLanguages = pages.Data.Where(p => p.Id == post.Id && p.Specificulture != post.Specificulture);
                    var lstOther = new List<SitemapLanguage>();
                    foreach (var item in otherLanguages)
                    {
                        lstOther.Add(new SitemapLanguage()
                        {
                            HrefLang = item.Specificulture,
                            Href = MixCmsHelper.GetRouterUrl(
                                        new { culture = item.Specificulture, seoName = post.SeoName }, Request, Url)
                        });
                    }
                    var sitemap = new SiteMap()
                    {
                        ChangeFreq = "monthly",
                        LastMod = DateTime.UtcNow,
                        Loc = post.DetailsUrl,
                        OtherLanguages = lstOther,
                        Priority = 0.3
                    };
                    root.Add(sitemap.ParseXElement());
                }

                string folder = $"Sitemaps";
                FileRepository.Instance.CreateDirectoryIfNotExist(folder);
                string filename = $"sitemap";
                string filePath = $"wwwroot/{folder}/{filename}.xml";
                root.Save(filePath);
                return new RepositoryResponse<FileViewModel>()
                {
                    IsSucceed = true,
                    Data = new FileViewModel()
                    {
                        Extension = ".xml",
                        Filename = filename,
                        FileFolder = folder
                    }
                };
            }
            catch (Exception ex)
            {
                return new RepositoryResponse<FileViewModel>() { Exception = ex };
            }
        }

        // GET
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]
        [HttpGet, HttpOptions]
        [Route("app-settings/details")]
        public RepositoryResponse<JObject> LoadAppSettings()
        {
            var settings = FileRepository.Instance.GetFile("appsettings", ".json", string.Empty, true, "{}");
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
        [Route("encrypt-rsa")]
        public RepositoryResponse<string> EncryptRsa([FromBody]JObject model)
        {
            string data = model.GetValue("data").Value<string>();
            return new RepositoryResponse<string>()
            {
                Data = RSAEncryptionHelper.GetEncryptedText(data)
            };
        }

        [AllowAnonymous]
        [HttpPost, HttpOptions]
        [Route("decrypt-rsa")]
        public RepositoryResponse<string> DecryptRsa([FromBody]JObject model)
        {
            string data = model.GetValue("data").Value<string>();
            return new RepositoryResponse<string>()
            {
                Data = Lib.Helpers.RSAEncryptionHelper.GetDecryptedText(data)
            };
        }

        [AllowAnonymous]
        [HttpPost, HttpOptions]
        [Route("encrypt")]
        public RepositoryResponse<string> Encrypt([FromBody]JObject model)
        {
            string data = model.GetValue("data").Value<string>();
            var encrypted = new JObject(new JProperty("encrypted", data));
            var key = System.Text.Encoding.UTF8.GetBytes("sw-cms-secret-key");
            return new RepositoryResponse<string>()
            {
                Data = AesEncryptionHelper.EncryptString(data, Convert.ToBase64String(key))
            };
        }

        [AllowAnonymous]
        [HttpPost, HttpOptions]
        [Route("decrypt")]
        public RepositoryResponse<string> Decrypt([FromBody]JObject model)
        {
            string data = model.GetValue("data")?.Value<string>();
            //string key = model.GetValue("key")?.Value<string>();
            var key = System.Text.Encoding.UTF8.GetBytes("sw-cms-secret-key");
            return new RepositoryResponse<string>()
            {
                Data = AesEncryptionHelper.DecryptString(data, Convert.ToBase64String(key))
            };
        }

        // POST api/category
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]
        [HttpPost, HttpOptions]
        [Route("app-settings/save")]
        public RepositoryResponse<JObject> SaveAppSettings([FromBody]JObject model)
        {
            var settings = FileRepository.Instance.GetFile("appsettings", ".json", string.Empty, true, "{}");
            if (model != null)
            {
                settings.Content = model.ToString();
                if (FileRepository.Instance.SaveFile(settings))
                {
                    MixService.Reload();
                    if (!MixService.GetConfig<bool>("IsCache"))
                    {
                        MixCacheService.RemoveCacheAsync();
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
        public RepositoryResponse<string> SaveGlobalSettings(string name, [FromBody]JObject model)
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
        public void SendMail([FromBody]JObject model)
        {
            MixService.SendMail(model.Value<string>("subject"), model.Value<string>("body"), MixService.GetConfig<string>("ContactEmail", _lang));
        }

        // POST api/category
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]
        [HttpPost, HttpOptions]
        [Route("import")]
        [Route("{culture}/import")]
        public async Task<RepositoryResponse<bool>> ImportAsync([FromForm]IFormFile assets)
        {
            string importFolder = $"Imports/Structures/{_lang}";
            var result = new RepositoryResponse<bool>();
            var saveFile = FileRepository.Instance.SaveWebFile(assets, assets.FileName, importFolder);
            if (saveFile.IsSucceed)
            {
                var fileContent = FileRepository.Instance.GetWebFile($"{saveFile.Data.Filename}{saveFile.Data.Extension}", saveFile.Data.FileFolder);
                var obj = JObject.Parse(fileContent.Content);
                switch (obj["type"].Value<string>())
                {
                    case "Language":
                        var arrLanguage = obj["data"].ToObject<List<MixLanguage>>();
                        result = await Lib.ViewModels.MixLanguages.ReadMvcViewModel.ImportLanguages(arrLanguage, _lang);
                        if (result.IsSucceed)
                        {
                            base.RemoveCache();
                        }
                        return result;

                    case "Configuration":
                        var arrConfiguration = obj["data"].ToObject<List<MixConfiguration>>();
                        result = await Lib.ViewModels.MixConfigurations.ReadMvcViewModel.ImportConfigurations(arrConfiguration, _lang);
                        if (result.IsSucceed)
                        {
                            base.RemoveCache();
                        }
                        return result;

                    case "Module":
                        var arrModule = obj["data"].ToObject<List<MixModule>>();
                        result = await Lib.ViewModels.MixModules.Helper.Import(arrModule, _lang);
                        if (result.IsSucceed)
                        {
                            base.RemoveCache();
                        }
                        return result;

                    default:
                        return new RepositoryResponse<bool>() { IsSucceed = false };
                }
            }
            return new RepositoryResponse<bool>();
        }

        #endregion Post

        #region Helpers

        private RepositoryResponse<JObject> GetAllSettings()
        {
            var cultures = CommonRepository.Instance.LoadCultures();
            var culture = cultures.FirstOrDefault(c => c.Specificulture == _lang);

            // Get Settings
            GlobalSettingsViewModel configurations = new GlobalSettingsViewModel()
            {
                Domain = MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.Domain),
                Lang = _lang,
                PortalThemeSettings = MixService.GetConfig<JObject>(MixConstants.ConfigurationKeyword.PortalThemeSettings),
                ThemeId = MixService.GetConfig<int>(MixConstants.ConfigurationKeyword.ThemeId, _lang),
                ApiEncryptKey = MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.ApiEncryptKey),
                ApiEncryptIV = MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.ApiEncryptIV),
                IsEncryptApi = MixService.GetConfig<bool>(MixConstants.ConfigurationKeyword.IsEncryptApi),
                Cultures = cultures,
                PageTypes = Enum.GetNames(typeof(MixPageType)).ToList(),
                ModuleTypes = Enum.GetNames(typeof(MixModuleType)).ToList(),
                AttributeSetTypes = Enum.GetNames(typeof(MixAttributeSetDataType)).ToList(),
                DataTypes = Enum.GetNames(typeof(MixDataType)).ToList(),
                Statuses = Enum.GetNames(typeof(MixContentStatus)).ToList(),
                LastUpdateConfiguration = MixService.GetConfig<DateTime?>("LastUpdateConfiguration")
            };

            configurations.LangIcon = culture?.Icon ?? MixService.GetConfig<string>("Language");

            // Get translator
            var translator = new JObject()
            {
                new JProperty("lang",_lang),
                new JProperty("data", MixService.GetTranslator(_lang))
            };

            // Get Configurations
            var settings = new JObject()
            {
                new JProperty("lang",_lang),
                new JProperty("langIcon",configurations.LangIcon),

                new JProperty("data", MixService.GetLocalSettings(_lang))
            };
            JObject result = new JObject()
            {
                new JProperty("globalSettings", JObject.FromObject(configurations)),
                new JProperty("translator", translator),
                new JProperty("settings", JObject.FromObject(settings))
            };

            return new RepositoryResponse<JObject>()
            {
                IsSucceed = true,
                Data = result
            };
        }

        #endregion Helpers
    }
}
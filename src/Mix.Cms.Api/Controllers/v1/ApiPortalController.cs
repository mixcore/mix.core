// Licensed to the Mix I/O Foundation under one or more agreements.
// The Mix I/O Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Mix.Cms.Lib.Repositories;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mix.Identity.Models;
using Mix.Domain.Core.ViewModels;
using Mix.Cms.Lib;
using static Mix.Cms.Lib.MixEnums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.ViewModels.Account;
using Mix.Cms.Lib.ViewModels.MixInit;

namespace Mix.Cms.Api.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/portal")]
    public class ApiPortalController :
        BaseApiController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public ApiPortalController(
           UserManager<ApplicationUser> userManager,
           SignInManager<ApplicationUser> signInManager,
           RoleManager<IdentityRole> roleManager,
            Microsoft.AspNetCore.SignalR.IHubContext<Hub.PortalHub> hubContext
            )
            : base(hubContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        #region Get

        // GET api/category/id
        [HttpGet, HttpOptions]
        [Route("{culture}/settings")]
        [Route("settings")]
        public RepositoryResponse<GlobalSettingsViewModel> Settings()
        {
            var cultures = CommonRepository.Instance.LoadCultures();
            var culture = cultures.FirstOrDefault(c => c.Specificulture == _lang);
            GlobalSettingsViewModel settings = new GlobalSettingsViewModel()
            {
                Lang = _lang,
                ThemeId = MixService.GetConfig<int>(MixConstants.ConfigurationKeyword.ThemeId, _lang),
                Cultures = cultures,
                PageTypes = Enum.GetNames(typeof(MixPageType)).ToList(),
                Statuses = Enum.GetNames(typeof(MixContentStatus)).ToList()

            };
            settings.LangIcon = culture?.Icon ?? MixService.GetConfig<string>("Language");
            return new RepositoryResponse<GlobalSettingsViewModel>()
            {
                IsSucceed = true,
                Data = settings
            };
        }

        // GET api/category/id
        [HttpGet, HttpOptions]
        [Route("{culture}/all-settings")]
        [Route("all-settings")]
        public RepositoryResponse<JObject> AllSettingsAsync()
        {
            var cultures = CommonRepository.Instance.LoadCultures();
            var culture = cultures.FirstOrDefault(c => c.Specificulture == _lang);

            // Get Settings
            GlobalSettingsViewModel configurations = new GlobalSettingsViewModel()
            {
                Lang = _lang,
                ThemeId = MixService.GetConfig<int>(MixConstants.ConfigurationKeyword.ThemeId, _lang),
                ApiEncryptKey = MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.ApiEncryptKey),
                ApiEncryptIV = MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.ApiEncryptIV),
                IsEncryptApi = MixService.GetConfig<bool>(MixConstants.ConfigurationKeyword.IsEncryptApi),
                Cultures = cultures,
                PageTypes = Enum.GetNames(typeof(MixPageType)).ToList(),
                Statuses = Enum.GetNames(typeof(MixContentStatus)).ToList()
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
                Lang = _lang,
                ThemeId = MixService.GetConfig<int>(MixConstants.ConfigurationKeyword.ThemeId, _lang),
                ApiEncryptKey = MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.ApiEncryptKey),
                ApiEncryptIV = MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.ApiEncryptIV),
                IsEncryptApi = MixService.GetConfig<bool>(MixConstants.ConfigurationKeyword.IsEncryptApi),
                Cultures = cultures,
                PageTypes = Enum.GetNames(typeof(MixPageType)).ToList(),
                Statuses = Enum.GetNames(typeof(MixContentStatus)).ToList()

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
        [Route("dashboard")]
        public RepositoryResponse<DashboardViewModel> Dashboard(int id)
        {
            return new RepositoryResponse<DashboardViewModel>()
            {
                IsSucceed = true,
                Data = new DashboardViewModel()
            };
        }

        #endregion Get

        #region Post

        // GET 
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]
        [HttpGet, HttpOptions]
        [Route("app-settings/details")]
        public RepositoryResponse<JObject> LoadAppSettings()
        {
            return new RepositoryResponse<JObject>() { IsSucceed = true, Data = MixService.GetGlobalSetting() };
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
                FileRepository.Instance.SaveFile(settings);
                //MixCmsService.Instance.RefreshConfigurations();
            }
            return new RepositoryResponse<JObject>() { IsSucceed = model != null, Data = model };
        }

        // POST api/category
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Admin")]
        [HttpPost, HttpOptions]
        [Route("import/{type}")]
        [Route("{culture}/import/{type}")]
        public async Task<RepositoryResponse<bool>> SaveAppSettingsAsync(string type, [FromBody]FileViewModel model)
        {
            var result = FileRepository.Instance.SaveWebFile(model);
            if (result)
            {
                var fileContent = FileRepository.Instance.GetWebFile($"{model.Filename}{model.Extension}", model.FileFolder);
                var obj = JObject.Parse(fileContent.Content);
                switch (type)
                {
                    case "Language":
                        var arrLanguage = obj["data"].ToObject<List<MixLanguage>>();
                        return await Lib.ViewModels.MixLanguages.ReadMvcViewModel.ImportLanguages(arrLanguage, _lang);
                    case "Configuration":
                        var arrConfiguration = obj["data"].ToObject<List<MixConfiguration>>();
                        return await Lib.ViewModels.MixConfigurations.ReadMvcViewModel.ImportConfigurations(arrConfiguration, _lang);

                    default:
                        return new RepositoryResponse<bool>() { IsSucceed = false };
                }
            }
            return new RepositoryResponse<bool>();

        }

        // POST api/category
        [HttpPost, HttpOptions]
        [Route("init-cms")]
        public async Task<RepositoryResponse<bool>> Post([FromBody]InitCmsViewModel model)
        {
            if (model != null)
            {
                var result = await InitCmsAsync(model).ConfigureAwait(false);
                return result;
            }
            return new RepositoryResponse<bool>();
        }

        #endregion Post

        #region Helpers
        private async Task<RepositoryResponse<bool>> InitCmsAsync(InitCmsViewModel model)
        {
            var result = new RepositoryResponse<bool>();

            MixService.SetConnectionString(MixConstants.CONST_CMS_CONNECTION, model.ConnectionString);
            MixService.SetConnectionString(MixConstants.CONST_ACCOUNT_CONNECTION, model.ConnectionString);
            MixService.SetConfig(MixConstants.CONST_SETTING_IS_SQLITE, model.IsSqlite);
            MixService.SetConfig(MixConstants.CONST_SETTING_LANGUAGE, model.Culture.Specificulture);

            InitCmsService sv = new InitCmsService();
            var initResult = await sv.InitCms(model.Culture);
            if (initResult.IsSucceed)
            {
                await InitRolesAsync();
                result.IsSucceed = true;
                MixService.LoadFromDatabase();
                MixService.SetConfig<bool>("IsInit", true);
                MixService.SetConfig<string>("DefaultCulture", model.Culture.Specificulture);
                MixService.Save();
            }
            else
            {
                MixService.Reload();
                if (initResult.Exception != null)
                {
                    result.Errors.Add(initResult.Exception.Message);
                    result.Exception = initResult.Exception;
                }
                foreach (var item in initResult.Errors)
                {
                    result.Errors.Add(item);
                }
            }
            return result;
        }

        private async Task<bool> InitRolesAsync()
        {
            bool isSucceed = true;
            var getRoles = await RoleViewModel.Repository.GetModelListAsync();
            if (getRoles.IsSucceed && getRoles.Data.Count == 0)
            {
                var saveResult = await _roleManager.CreateAsync(new IdentityRole()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "SuperAdmin"
                });
                isSucceed = saveResult.Succeeded;
            }
            return isSucceed;
        }

        #endregion
    }
}

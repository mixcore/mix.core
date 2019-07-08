// Licensed to the Mix I/O Foundation under one or more agreements.
// The Mix I/O Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Mix.Cms.Lib.Repositories;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using Mix.Identity.Models;
using Mix.Domain.Core.ViewModels;
using Mix.Cms.Lib;
using static Mix.Cms.Lib.MixEnums;
using Mix.Cms.Lib.ViewModels.Account;
using Mix.Cms.Lib.ViewModels.MixInit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Attributes;
using Microsoft.AspNetCore.Http;
using Mix.Cms.Lib.Models.Account;
using Mix.Cms.Api.Helpers;
using Mix.Common.Helper;

namespace Mix.Cms.Api.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/init")]
    public class ApiInitCmsController :
        BaseApiController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IApplicationLifetime _appLifetime;
        private readonly IHostingEnvironment _env;
        private readonly IdentityHelper _idHelper;
        public ApiInitCmsController(
           UserManager<ApplicationUser> userManager,
           SignInManager<ApplicationUser> signInManager,
           RoleManager<IdentityRole> roleManager,
            Microsoft.AspNetCore.SignalR.IHubContext<Hub.PortalHub> hubContext,
            IApplicationLifetime appLifetime,
            IHostingEnvironment env,
            IMemoryCache memoryCache
            )
            : base(memoryCache, hubContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _appLifetime = appLifetime;
            _env = env;
            _idHelper = new IdentityHelper(userManager, signInManager, roleManager);
        }
        public void ShutdownSite()
        {
            _appLifetime.StopApplication();
        }
        
        #region Post

        
        /// <summary>
        /// Step 1 when status = 0
        ///     - Init Cms Database
        ///     - Init Account Database
        ///     - Init Selected Culture as default
        ///     - Init Menu Positions
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("init-cms/step-1")]
        public async Task<RepositoryResponse<bool>> Step1([FromBody]InitCmsViewModel model)
        {
            if (model != null)
            {
                var result = new RepositoryResponse<bool>() { IsSucceed = true};
                if (MixService.GetConfig<int>("InitStatus") == 0)
                {
                    result = await InitStep1Async(model).ConfigureAwait(false);
                    if (result.IsSucceed)
                    {
                        InitCmsService sv = new InitCmsService();
                        await sv.InitPositionsAsync();
                    }
                }
                return result;
            }
            return new RepositoryResponse<bool>();
        }

        /// <summary>
        /// Step 2 when status = 1
        ///     - Init SuperAdmin
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("init-cms/step-2")]
        public async Task<RepositoryResponse<AccessTokenViewModel>> InitSuperAdmin([FromBody] MixRegisterViewModel model)
        {
            RepositoryResponse<AccessTokenViewModel> result = new RepositoryResponse<AccessTokenViewModel>();
            if (ModelState.IsValid)
            {
                if (_userManager.Users.Count() == 0)
                {
                    var user = new ApplicationUser
                    {
                        UserName = model.Username,
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Avatar = model.Avatar ?? MixService.GetConfig<string>("DefaultAvatar"),
                        JoinDate = DateTime.UtcNow
                    };
                    var createResult = await _userManager.CreateAsync(user, password: model.Password).ConfigureAwait(false);
                    if (createResult.Succeeded)
                    {
                        user = await _userManager.FindByEmailAsync(model.Email).ConfigureAwait(false);
                        model.Id = user.Id;
                        model.CreatedDateTime = DateTime.UtcNow;
                        // Save to cms db context

                        await model.SaveModelAsync();
                        await _userManager.AddToRoleAsync(user, "SuperAdmin");
                        var token = await _idHelper.GenerateAccessTokenAsync(user, true);
                        if (token != null)
                        {
                            result.IsSucceed = true;
                            MixService.LoadFromDatabase();
                            MixService.SetConfig("InitStatus", 2);
                            MixService.SaveSettings();
                            MixService.Reload();
                            result.Data = token;
                            return result;
                        }
                        else
                        {
                            return result;
                        }
                    }
                    else
                    {
                        foreach (var error in createResult.Errors)
                        {
                            result.Errors.Add(error.Description);
                        }
                        return result;
                    }
                }
            }

            return result;
        }


        /// <summary>
        /// Step 3 Run when status = 2
        ///     - Init Cms Configurations from files data/configuration.json
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("init-cms/step-3")]
        public async Task<RepositoryResponse<bool>> InitConfigurations([FromBody]List<MixConfiguration> model)
        {
            if (model != null)
            {
                var result = new RepositoryResponse<bool>();
                if (MixService.GetConfig<int>("InitStatus") == 2)
                {
                    string culture = MixService.GetConfig<string>("DefaultCulture");
                    InitCmsService sv = new InitCmsService();
                    result = await sv.InitConfigurationsAsync(culture, model);
                    if (result.IsSucceed)
                    {
                        MixService.LoadFromDatabase();
                        MixService.SetConfig("InitStatus", 3);
                        MixService.SaveSettings();
                        MixService.Reload();
                    }
                }
                return result;
            }
            return new RepositoryResponse<bool>();
        }

        /// <summary>
        /// Step 4 when status = 3
        ///     - Init Languages for translate from languages.json
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("init-cms/step-4")]
        public async Task<RepositoryResponse<bool>> InitLanguages([FromBody]List<MixLanguage> model)
        {
            if (model != null)
            {
                var result = new RepositoryResponse<bool>();
                if (MixService.GetConfig<int>("InitStatus") == 3)
                {
                    string culture = MixService.GetConfig<string>("DefaultCulture");
                    InitCmsService sv = new InitCmsService();
                    result = await sv.InitLanguagesAsync(culture, model);
                    if (result.IsSucceed)
                    {
                        MixService.LoadFromDatabase();
                        MixService.SetConfig("InitStatus", 4);
                        MixService.SaveSettings();
                        MixService.Reload();
                    }
                }
                return result;
            }
            return new RepositoryResponse<bool>();
        }
        
        
        /// <summary>
        /// Step 5 when status = 4 (Finished)
        ///     - Init default theme
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("init-cms/step-5")]
        [RequestFormSizeLimit(valueCountLimit: 214748364)] // 200Mb
        public async Task<RepositoryResponse<Mix.Cms.Lib.ViewModels.MixThemes.InitViewModel>> Save([FromForm]string model, [FromForm]IFormFile assets, [FromForm]IFormFile theme)
        {
            var json = JObject.Parse(model);
            var data = json.ToObject<Lib.ViewModels.MixThemes.InitViewModel>();
            
            if (theme != null)
            {
                string importFolder = $"Imports/Themes/{DateTime.UtcNow.ToString("dd-MM-yyyy")}/{data.Name}";
                FileRepository.Instance.SaveWebFile(theme, importFolder);
                data.TemplateAsset = new Lib.ViewModels.FileViewModel(theme, importFolder);
            }
            else
            {
                if (data.IsCreateDefault)
                {
                    data.TemplateAsset = new Lib.ViewModels.FileViewModel() {
                        Filename="default",
                        Extension=".zip",
                        FileFolder= "Imports/Themes"
                    };
                }
                else
                {
                    data.TemplateAsset = new Lib.ViewModels.FileViewModel() {
                        Filename="default_blank",
                        Extension=".zip",
                        FileFolder= "Imports/Themes"
                    };
                }
            }


            if (data != null)
            {
                string culture = MixService.GetConfig<string>("DefaultCulture");
                data.Title = MixService.GetConfig<string>("SiteName", culture);
                data.Name = SeoHelper.GetSEOString(data.Title);
                data.CreatedBy = User.Claims.FirstOrDefault(c => c.Type == "Username")?.Value ?? "Init";
                data.Specificulture = _lang;
                var result = await data.SaveModelAsync(true);
                if (result.IsSucceed)
                {
                    MixService.LoadFromDatabase();
                    MixService.SetConfig("InitStatus", 5);
                    MixService.SetConfig("IsInit", true);
                    MixService.SaveSettings();
                    MixService.Reload();
                }
                return result;
            }
            return new RepositoryResponse<Lib.ViewModels.MixThemes.InitViewModel>() { Status = 501 };
        }
        #endregion Post

        #region Helpers

        private async Task<RepositoryResponse<bool>> InitStep1Async(InitCmsViewModel model)
        {
            var result = new RepositoryResponse<bool>();

            MixService.SetConnectionString(MixConstants.CONST_CMS_CONNECTION, model.ConnectionString);
            MixService.SetConnectionString(MixConstants.CONST_MESSENGER_CONNECTION, model.ConnectionString);
            MixService.SetConnectionString(MixConstants.CONST_ACCOUNT_CONNECTION, model.ConnectionString);
            MixService.SetConfig(MixConstants.CONST_SETTING_IS_MYSQL, model.IsMysql);
            MixService.SetConfig(MixConstants.CONST_SETTING_DATABASE_PROVIDER, model.DatabaseProvider);
            MixService.SetConfig(MixConstants.CONST_SETTING_LANGUAGE, model.Culture.Specificulture);

            var initResult = await InitCmsService.InitCms(model.Culture);
            if (initResult.IsSucceed)
            {
                await InitRolesAsync();
                result.IsSucceed = true;
                MixService.LoadFromDatabase();
                MixService.SetConfig<string>("DefaultCulture", model.Culture.Specificulture);
                MixService.SetConfig("InitStatus", 1);
                MixService.SaveSettings();
                MixService.Reload();
            }
            else
            {
                // if cannot init cms 
                //  => reload from default settings
                //  => save to appSettings
                MixService.Reload();
                MixService.SaveSettings();
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

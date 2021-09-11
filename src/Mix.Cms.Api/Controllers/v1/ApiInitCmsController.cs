// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Helpers;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.SignalR.Hubs;
using Mix.Cms.Lib.ViewModels.Account;
using Mix.Cms.Lib.ViewModels.MixInit;
using Mix.Heart.Constants;
using Mix.Heart.Helpers;
using Mix.Heart.Models;
using Mix.Identity.Constants;
using Mix.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Cms.Api.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1/init")]
    public class ApiInitCmsController :
        BaseApiController<MixCmsContext>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly MixIdentityService _idHelper;

        public ApiInitCmsController(
           UserManager<ApplicationUser> userManager,
           SignInManager<ApplicationUser> signInManager,
           RoleManager<IdentityRole> roleManager,
            IHubContext<PortalHub> hubContext,
            IMemoryCache memoryCache, 
            MixIdentityService idHelper)
            : base(null, memoryCache, hubContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _idHelper = idHelper;
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
        public async Task<RepositoryResponse<bool>> Step1([FromBody] InitCmsViewModel model)
        {
            if (model != null)
            {
                var result = new RepositoryResponse<bool>() { IsSucceed = true };
                if (MixService.GetAppSetting<int>("InitStatus") == 0)
                {
                    result = await InitStep1Async(model).ConfigureAwait(false);
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
                        Avatar = model.Avatar ?? MixService.GetAppSetting<string>("DefaultAvatar"),
                        JoinDate = DateTime.UtcNow
                    };
                    var createResult = await _userManager.CreateAsync(user, password: model.Password).ConfigureAwait(false);
                    if (createResult.Succeeded)
                    {
                        user = await _userManager.FindByEmailAsync(model.Email).ConfigureAwait(false);
                        await _userManager.AddToRoleAsync(user, MixDefaultRoles.SuperAdmin);
                        await MixAccountHelper.LoadUserInfoAsync(user.UserName);
                        var rsaKeys = RSAEncryptionHelper.GenerateKeys();
                        var aesKey = MixService.GetAppSetting<string>(MixAppSettingKeywords.ApiEncryptKey);
                        
                        var token = await _idHelper.GenerateAccessTokenAsync(user, true, aesKey, rsaKeys[MixConstants.CONST_RSA_PUBLIC_KEY]);
                        if (token != null)
                        {
                            result.IsSucceed = true;
                            MixService.LoadFromDatabase();
                            MixService.SetConfig<string>(MixAppSettingKeywords.ApiEncryptKey, aesKey);
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

        // /// <summary>
        // /// Step 3 Run when status = 2
        // ///     - Init Cms Configurations from files data/configuration.json
        // /// </summary>
        // /// <param name="model"></param>
        // /// <returns></returns>
        // [HttpPost, HttpOptions]
        // [Route("init-cms/step-5")]
        // public async Task<RepositoryResponse<bool>> InitConfigurations([FromBody]List<MixConfiguration> model)
        // {
        //     if (model != null)
        //     {
        //         var result = new RepositoryResponse<bool>();
        //         if (MixService.GetConfig<int>("InitStatus") == 4)
        //         {
        //             string culture = MixService.GetConfig<string>("DefaultCulture");
        //             InitCmsService sv = new InitCmsService();
        //             result = await sv.InitConfigurationsAsync(culture, model);
        //             if (result.IsSucceed)
        //             {
        //                 MixService.LoadFromDatabase();
        //                 MixService.SetConfig("InitStatus", 5);
        //                 MixService.SaveSettings();
        //                 MixService.Reload();
        //             }
        //         }
        //         return result;
        //     }
        //     return new RepositoryResponse<bool>();
        // }

        /// <summary>
        /// Step 4 when status = 3
        ///     - Init Languages for translate from languages.json
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("init-cms/step-4")]
        public async Task<RepositoryResponse<bool>> InitLanguages([FromBody] List<MixLanguage> model)
        {
            if (model != null)
            {
                var result = new RepositoryResponse<bool>();
                if (MixService.GetAppSetting<int>("InitStatus") == 3)
                {
                    string culture = MixService.GetAppSetting<string>("DefaultCulture");
                    InitCmsService sv = new InitCmsService();
                    result = await sv.InitLanguagesAsync(culture, model);
                    if (result.IsSucceed)
                    {
                        MixService.LoadFromDatabase();
                        MixService.SetConfig("InitStatus", 4);
                        MixService.SetConfig(MixAppSettingKeywords.IsInit, true);
                        MixService.SaveSettings();
                        _ = Services.MixCacheService.RemoveCacheAsync();
                        MixService.Reload();
                    }
                }
                return result;
            }
            return new RepositoryResponse<bool>();
        }

        /// <summary>
        /// Step 3 when status = 3 (Finished)
        ///     - Init default theme
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("init-cms/step-3")]
        [DisableRequestSizeLimit]
        public async Task<RepositoryResponse<Cms.Lib.ViewModels.MixThemes.InitViewModel>> Save([FromForm] string model, [FromForm] IFormFile assets, [FromForm] IFormFile theme)
        {
            string user = _idHelper._idHelper.GetClaim(User, MixClaims.Username);
            return await Mix.Cms.Lib.ViewModels.MixThemes.Helper.InitTheme(model, user, _lang, assets, theme);
        }
        
        /// <summary>
        /// Step 3 when status = 3 (Finished)
        ///     - Init default theme
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, HttpOptions]
        [Route("init-cms/step-3/active")]
        [DisableRequestSizeLimit]
        public async Task<ActionResult<bool>> Active([FromBody] Lib.ViewModels.MixThemes.UpdateViewModel model)
        {
            model.IsActived = true;
            var result = await Cms.Lib.ViewModels.MixThemes.Helper.ActivedThemeAsync(model.Id, model.Name, model.Specificulture);
            if (result.IsSucceed)
            {
                // MixService.SetConfig<string>(MixAppSettingKeywords.SiteName, _lang, data.Title);
                MixService.LoadFromDatabase();
                MixService.SetConfig("InitStatus", 3);
                MixService.SetConfig(MixAppSettingKeywords.IsInit, false);
                MixService.SaveSettings();
                _ = Mix.Services.MixCacheService.RemoveCacheAsync();
                MixService.Reload();
            }
            return Ok(result);
        }

        #endregion Post

        #region Helpers

        private async Task<RepositoryResponse<bool>> InitStep1Async(InitCmsViewModel model)
        {
            MixService.SetConnectionString(MixConstants.CONST_CMS_CONNECTION, model.ConnectionString);
            MixService.SetConnectionString(MixConstants.CONST_MESSENGER_CONNECTION, model.ConnectionString);
            MixService.SetConnectionString(MixConstants.CONST_ACCOUNT_CONNECTION, model.ConnectionString);
            MixService.SetConfig(MixConstants.CONST_SETTING_DATABASE_PROVIDER, model.DatabaseProvider.ToString());
            MixService.SetConfig(MixConstants.CONST_SETTING_LANGUAGE, model.Culture.Specificulture);
            MixService.SetMixConfig<string>(WebConfiguration.MixCacheConnectionString, model.ConnectionString);
            MixService.SetMixConfig<string>(WebConfiguration.MixCacheDbProvider, model.DatabaseProvider.ToString());
            MixService.SaveSettings();
            MixService.Reload();
            var result = await InitCmsService.InitCms(model.SiteName, model.Culture);

            if (result.IsSucceed)
            {
                await InitCmsService.InitRolesAsync(_roleManager);
                result.IsSucceed = true;
                MixService.LoadFromDatabase();
                MixService.SetConfig<string>("DefaultCulture", model.Culture.Specificulture);
                MixService.SetConfig("InitStatus", 1);
                MixService.SaveSettings();
                //MixService.Reload();
            }
            else
            {
                // if cannot init cms
                //  => reload from default settings
                //  => save to appSettings
                MixService.Reload();
                MixService.SaveSettings();
            }
            return result;
        }

        #endregion Helpers
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mix.Account.Domain.Dtos;
using Mix.Database.Entities.Account;
using Mix.Heart.Helpers;
using Mix.Heart.Repository;
using Mix.Identity.Models.AccountViewModels;
using Mix.Identity.Services;
using Mix.Shared.Constants;
using Mix.Shared.Enums;
using Mix.Shared.Services;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Mix.Account.Controllers
{
    [Route("api/v2/account")]
    [ApiController]
    public class MixAccountController : ControllerBase
    {
        private readonly UserManager<MixUser> _userManager;
        private readonly SignInManager<MixUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<MixAccountController> _logger;
        private readonly MixIdentityService _idService;
        private readonly MixAppSettingService _appSettingService;
        private readonly Repository<MixCmsAccountContext, RefreshTokens, Guid> _refreshTokenRepo;
        public MixAccountController(
            UserManager<MixUser> userManager,
            SignInManager<MixUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<MixAccountController> logger,
            MixIdentityService idService, Repository<MixCmsAccountContext, RefreshTokens, Guid> refreshTokenRepo, 
            MixAppSettingService appSettingService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _idService = idService;
            _refreshTokenRepo = refreshTokenRepo;
            _appSettingService = appSettingService;
        }


        [Route("Logout")]
        [HttpGet]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync().ConfigureAwait(false);
            await _refreshTokenRepo.DeleteAsync(r => r.Username == User.Identity.Name);
            return Ok();
        }

        [Route("login")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] LoginDto requestDto)
        {
            string key = _appSettingService.GetConfig<string>(MixAppSettingsSection.GlobalSettings, MixAppSettingKeywords.ApiEncryptKey);
            string decryptMsg = AesEncryptionHelper.DecryptString(requestDto.Message, key);
            var model = JsonConvert.DeserializeObject<LoginViewModel>(decryptMsg);
            var loginResult = await _idService.Login(model);
            return Ok(loginResult);
        }
    }
}

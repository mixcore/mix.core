using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mix.Account.Domain.Dtos;
using Mix.Account.Domain.ViewModels;
using Mix.Database.Entities.Account;
using Mix.Heart.Exceptions;
using Mix.Heart.Helpers;
using Mix.Heart.Repository;
using Mix.Identity.Constants;
using Mix.Identity.Dtos;
using Mix.Identity.Models.AccountViewModels;
using Mix.Lib.Attributes;
using Mix.Lib.Services;
using Mix.Shared.Constants;
using Mix.Shared.Services;
using Newtonsoft.Json;
using System;
using System.Linq;
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
        private readonly GlobalConfigService _globalConfigService;
        private readonly EntityRepository<MixCmsAccountContext, RefreshTokens, Guid> _refreshTokenRepo;
        public MixAccountController(
            UserManager<MixUser> userManager,
            SignInManager<MixUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<MixAccountController> logger,
            MixIdentityService idService, EntityRepository<MixCmsAccountContext, RefreshTokens, Guid> refreshTokenRepo,
            GlobalConfigService globalConfigService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _idService = idService;
            _refreshTokenRepo = refreshTokenRepo;
            _globalConfigService = globalConfigService;
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
            string key = _globalConfigService.GetConfig<string>(MixAppSettingKeywords.ApiEncryptKey);
            string decryptMsg = AesEncryptionHelper.DecryptString(requestDto.Message, key);
            var model = JsonConvert.DeserializeObject<LoginViewModel>(decryptMsg);
            var loginResult = await _idService.Login(model);
            return Ok(loginResult);
        }

        [Route("renew-token")]
        [HttpPost]
        public async Task<ActionResult> RenewToken([FromBody] RenewTokenDto refreshTokenDto)
        {
            var token = await _idService.RenewTokenAsync(refreshTokenDto);
            return Ok(token);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [Route("my-profile")]
        public async Task<ActionResult<MixUserViewModel>> MyProfile()
        {
            string id = MixIdentityService.GetClaim(User, MixClaims.Id);
            MixUser user = await _userManager.FindByIdAsync(id); ;

            if (user != null)
            {
                var mixUser = new MixUserViewModel(user);
                await mixUser.LoadUserDataAsync();
                return Ok(mixUser);
            }
            return BadRequest();
        }

        // POST api/template
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [MixAuthorize]
        [HttpPost]
        [Route("save")]
        public async Task<ActionResult<MixUserViewModel>> Save(
            [FromBody] MixUserViewModel model)
        {
            if (model != null && model.User != null)
            {
                var user = await _userManager.FindByIdAsync(model.User.Id);
                user.Email = model.User.Email;
                user.FirstName = model.User.FirstName;
                user.LastName = model.User.LastName;
                user.Avatar = model.User.Avatar;
                var updInfo = await _userManager.UpdateAsync(user);
                //var saveData = await model.UserData.SaveAsync();
                if (model.IsChangePassword)
                {
                    var changePwd = await _userManager.ChangePasswordAsync(
                        model.User, 
                        model.ChangePassword.OldPassword, 
                        model.ChangePassword.NewPassword);
                    if (!changePwd.Succeeded)
                    {
                        throw new MixException(string.Join(",", changePwd.Errors));
                    }
                    else
                    {
                        // Remove other token if change password success
                        var refreshToken = User.Claims.SingleOrDefault(c => c.Type == "RefreshToken")?.Value;
                        //await RefreshTokenViewModel.Repository.RemoveModelAsync(r => r.Id != refreshToken);
                    }
                }
                return Ok(model);
            }
            return BadRequest();
        }
    }
}

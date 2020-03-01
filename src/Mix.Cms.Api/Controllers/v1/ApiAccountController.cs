using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mix.Cms.Api.Helpers;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.ViewModels.Account;
using Mix.Domain.Core.ViewModels;
using Mix.Identity.Models;
using Mix.Identity.Models.AccountViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Mix.Cms.Lib.MixEnums;

namespace Mix.Cms.Api.Controllers.v1
{
    //[Authorize(Roles = "SuperAdmin,Admin")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/v1/account")]
    public class ApiAccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger _logger;
        private readonly IdentityHelper _helper;

        public ApiAccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<ApiAccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _helper = new IdentityHelper(userManager, signInManager, roleManager);
        }

        [TempData]
        public string ErrorMessage { get; set; }

        //
        // POST: /Account/Logout
        [Route("Logout")]
        [HttpGet, HttpPost, HttpOptions]
        public async Task<RepositoryResponse<bool>> Logout()
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true, Data = true };
            await _signInManager.SignOutAsync().ConfigureAwait(false);
            await RefreshTokenViewModel.Repository.RemoveModelAsync(r => r.Username == User.Identity.Name);
            return result;
        }

        [Route("login")]
        [HttpPost, HttpOptions]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult<RepositoryResponse<AccessTokenViewModel>>> Login([FromBody] LoginViewModel model)
        {
            RepositoryResponse<AccessTokenViewModel> loginResult = new RepositoryResponse<AccessTokenViewModel>();
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(
                    model.UserName, model.Password, isPersistent: model.RememberMe, lockoutOnFailure: true).ConfigureAwait(false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.UserName).ConfigureAwait(false);

                    var token = await _helper.GenerateAccessTokenAsync(user, model.RememberMe);
                    if (token != null)
                    {
                        var info = await UserInfoViewModel.Repository.GetSingleModelAsync(u => u.Username == user.UserName);
                        if (!info.IsSucceed)
                        {
                            info.Data = new UserInfoViewModel();
                        }
                        token.UserData = info.Data;

                        loginResult.IsSucceed = true;
                        loginResult.Status = 1;
                        loginResult.Data = token;
                        _logger.LogInformation("User logged in.");
                        return Ok(loginResult);
                    }
                    else
                    {
                        return Ok(loginResult);
                    }
                }
                else
                {
                    loginResult.Errors.Add("login failed");
                    return BadRequest(loginResult);
                }
            }
            else
            {
                return BadRequest(loginResult);
            }
        }

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("refresh-token/{refreshTokenId}")]
        [HttpGet, HttpOptions]
        public async Task<RepositoryResponse<AccessTokenViewModel>> RefreshToken(string refreshTokenId)
        {
            RepositoryResponse<AccessTokenViewModel> result = new RepositoryResponse<AccessTokenViewModel>();
            var getRefreshToken = await RefreshTokenViewModel.Repository.GetSingleModelAsync(t => t.Id == refreshTokenId);
            if (getRefreshToken.IsSucceed)
            {
                var oldToken = getRefreshToken.Data;
                if (oldToken.ExpiresUtc > DateTime.UtcNow)
                {
                    var user = await _userManager.FindByEmailAsync(oldToken.Email);
                    await _signInManager.SignInAsync(user, true).ConfigureAwait(false);

                    var token = await _helper.GenerateAccessTokenAsync(user, true);
                    if (token != null)
                    {
                        await oldToken.RemoveModelAsync();
                        var info = await UserInfoViewModel.Repository.GetSingleModelAsync(u => u.Username == user.UserName);
                        if (!info.IsSucceed)
                        {
                            info.Data = new UserInfoViewModel();
                        }
                        token.UserData = info.Data;

                        result.IsSucceed = true;
                        result.Status = 1;
                        result.Data = token;
                        _logger.LogInformation("User refresh token.");
                        return result;
                    }
                    else
                    {
                        result.IsSucceed = false;
                        result.Data = token;
                        return result;
                    }
                }
                else
                {
                    await oldToken.RemoveModelAsync();
                    result.Errors.Add("Token expired");
                    return result;
                }
            }
            else
            {
                result.Errors.Add("Token expired");
                return result;
            }
        }

        [Route("Register")]
        [HttpPost, HttpOptions]
        [AllowAnonymous]
        public async Task<ActionResult<RepositoryResponse<AccessTokenViewModel>>> Register([FromBody] MixRegisterViewModel model)
        {
            RepositoryResponse<AccessTokenViewModel> result = new RepositoryResponse<AccessTokenViewModel>();
            if (ModelState.IsValid)
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
                    _logger.LogInformation("User created a new account with password.");
                    user = await _userManager.FindByEmailAsync(model.Email).ConfigureAwait(false);
                    model.Id = user.Id;
                    model.CreatedDateTime = DateTime.UtcNow;
                    // Save to cms db context
                    await model.SaveModelAsync();
                    var token = await _helper.GenerateAccessTokenAsync(user, true);
                    if (token != null)
                    {
                        result.IsSucceed = true;
                        result.Data = token;
                        _logger.LogInformation("User logged in.");
                        return result;
                    }
                    else
                    {
                        return Ok(result);
                    }
                }
                else
                {
                    foreach (var error in createResult.Errors)
                    {
                        result.Errors.Add(error.Description);
                    }
                    return BadRequest(result);
                }
            }

            return BadRequest(result);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Roles = "SuperAdmin")]
        [Route("user-in-role")]
        [HttpPost, HttpOptions]
        public async Task<RepositoryResponse<bool>> ManageUserInRole([FromBody]UserRoleModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.RoleId);
            var result = new RepositoryResponse<bool>();

            List<string> errors = new List<string>();

            if (role == null)
            {
                errors.Add($"Role: {model.RoleId} does not exists");
            }
            else if (model.IsUserInRole)
            {
                var appUser = await _userManager.FindByIdAsync(model.UserId);

                if (appUser == null)
                {
                    errors.Add($"User: {model.UserId} does not exists");
                }
                else if (!(await _userManager.IsInRoleAsync(appUser, role.Name)))
                {
                    var addResult = await _userManager.AddToRoleAsync(appUser, role.Name);

                    if (!addResult.Succeeded)
                    {
                        errors.Add($"User: {model.UserId} could not be added to role");
                    }
                }
            }
            else
            {
                var appUser = await _userManager.FindByIdAsync(model.UserId);

                if (appUser == null)
                {
                    errors.Add($"User: {model.UserId} does not exists");
                }

                var removeResult = await _userManager.RemoveFromRoleAsync(appUser, role.Name);
                if (!removeResult.Succeeded)
                {
                    errors.Add($"User: {model.UserId} could not be removed from role");
                }
            }
            result.IsSucceed = errors.Count == 0;
            result.Data = errors.Count == 0;
            result.Errors = errors;
            return result;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet, HttpOptions]
        [Route("details/{viewType}/{id}")]
        [Route("details/{viewType}")]
        public async Task<JObject> Details(string viewType, string id = null)
        {
            switch (viewType)
            {
                case "portal":
                    if (!string.IsNullOrEmpty(id))
                    {
                        var beResult = await Lib.ViewModels.Account.MixUsers.UpdateViewModel.Repository.GetSingleModelAsync(
                            model => model.Id == id).ConfigureAwait(false);
                        return JObject.FromObject(beResult);
                    }
                    else
                    {
                        var model = new MixCmsUser() { Status = (int)MixUserStatus.Actived };

                        RepositoryResponse<Lib.ViewModels.Account.MixUsers.UpdateViewModel> result = new RepositoryResponse<Lib.ViewModels.Account.MixUsers.UpdateViewModel>()
                        {
                            IsSucceed = true,
                            Data = await Lib.ViewModels.Account.MixUsers.UpdateViewModel.InitViewAsync(model)
                        };
                        return JObject.FromObject(result);
                    }
                default:
                    if (!string.IsNullOrEmpty(id))
                    {
                        var beResult = await UserInfoViewModel.Repository.GetSingleModelAsync(
                            model => model.Id == id).ConfigureAwait(false);
                        return JObject.FromObject(beResult);
                    }
                    else
                    {
                        var model = new MixCmsUser() { Status = (int)MixUserStatus.Actived };

                        RepositoryResponse<UserInfoViewModel> result = new RepositoryResponse<UserInfoViewModel>()
                        {
                            IsSucceed = true,
                            Data = await UserInfoViewModel.InitViewAsync(model)
                        };
                        return JObject.FromObject(result);
                    }
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet, HttpOptions]
        [Route("my-profile")]
        public async Task<JObject> MyProfile()
        {
            string id = User.Claims.SingleOrDefault(c => c.Type == "Id")?.Value;
            if (!string.IsNullOrEmpty(id))
            {
                var beResult = await Lib.ViewModels.Account.MixUsers.UpdateViewModel.Repository.GetSingleModelAsync(
                    model => model.Id == id).ConfigureAwait(false);
                return JObject.FromObject(beResult);
            }
            else
            {
                RepositoryResponse<Lib.ViewModels.Account.MixUsers.UpdateViewModel> result = new RepositoryResponse<Lib.ViewModels.Account.MixUsers.UpdateViewModel>()
                {
                    IsSucceed = false,
                    Data = null
                };
                return JObject.FromObject(result);
            }
        }

        // POST api/template
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost, HttpOptions]
        [Route("save")]
        public async Task<RepositoryResponse<UserInfoViewModel>> Save(
            [FromBody] UserInfoViewModel model)
        {
            if (model != null)
            {
                var result = await model.SaveModelAsync(true).ConfigureAwait(false);
                if (model.IsChangePassword)
                {
                    var user = await _userManager.FindByNameAsync(model.Username);
                    if (user != null)
                    {
                        var tmp = await _userManager.ChangePasswordAsync(user, model.ChangePassword.CurrentPassword, model.ChangePassword.NewPassword);
                        result.IsSucceed = tmp.Succeeded;
                        if (!result.IsSucceed)
                        {
                            foreach (var err in tmp.Errors)
                            {
                                result.Errors.Add(err.Description);
                            }
                        }
                        else
                        {
                            // Remove other token if change password success
                            var refreshToken = User.Claims.SingleOrDefault(c => c.Type == "RefreshToken")?.Value;
                            await RefreshTokenViewModel.Repository.RemoveModelAsync(r => r.Id != refreshToken);
                        }
                    }
                    else
                    {
                        Request.HttpContext.Response.StatusCode = 401;
                    }
                }
                return result;
            }
            return new RepositoryResponse<UserInfoViewModel>();
        }

        // POST api/account/list
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost, HttpOptions]
        [Route("list")]
        public async Task<RepositoryResponse<PaginationModel<UserInfoViewModel>>> GetList(RequestPaging request)
        {
            Expression<Func<MixCmsUser, bool>> predicate = model =>
                (!request.Status.HasValue || model.Status == request.Status.Value)
                && (string.IsNullOrWhiteSpace(request.Keyword)
                || (
                    model.Username.Contains(request.Keyword)
                   || model.FirstName.Contains(request.Keyword)
                   || model.LastName.Contains(request.Keyword)
                   )
                )
                && (!request.FromDate.HasValue
                    || (model.CreatedDateTime >= request.FromDate.Value.ToUniversalTime())
                )
                && (!request.ToDate.HasValue
                    || (model.CreatedDateTime <= request.ToDate.Value.ToUniversalTime())
                );

            var data = await UserInfoViewModel.Repository.GetModelListByAsync(predicate, request.OrderBy, request.Direction, request.PageSize, request.PageIndex).ConfigureAwait(false);
            if (data.IsSucceed)
            {
                data.Data.Items.ForEach(a =>
                {
                    a.DetailsUrl = MixCmsHelper.GetRouterUrl(
                        new { action = "profile", a.Id }, Request, Url);
                }
                );
            }
            return data;
        }

        [HttpPost, HttpOptions]
        [Route("forgot-password")]
        public async Task<RepositoryResponse<string>> ForgotPassword([FromBody]Mix.Identity.Models.AccountViewModels.ForgotPasswordViewModel model)
        {
            var result = new RepositoryResponse<string>() { IsSucceed = true };
            if (string.IsNullOrEmpty(model.Email))
            {
                result.IsSucceed = false;
                result.Data = "Invalid Email";
                result.Errors.Add("Invalid Email");
                return result;
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                result.IsSucceed = false;
                result.Data = "Email Not Exist";
                result.Errors.Add("Email Not Exist");
                return result;
            }

            //if (!await _userManager.IsEmailConfirmedAsync(user))
            //    result.Data = "Invalid Email";

            var confrimationCode =
                    await _userManager.GeneratePasswordResetTokenAsync(user);

            var callbackurl = $"{Request.Scheme}://{Request.Host}/security/reset-password/?token={System.Web.HttpUtility.UrlEncode(confrimationCode)}";
            var getEdmTemplate = await Lib.ViewModels.MixTemplates.ReadViewModel.Repository.GetSingleModelAsync(
                m => m.FolderType == MixConstants.TemplateFolder.Edms && m.FileName == "ForgotPassword");
            string content = callbackurl;
            if (getEdmTemplate.IsSucceed)
            {
                content = getEdmTemplate.Data.Content.Replace("[URL]", callbackurl);
                
            }
            MixService.SendMail(
                    to: user.Email,
                    subject: "Reset Password",
                    message: content);

            return result;
        }

        [HttpPost]
        [Route("reset-password")]
        public async Task<RepositoryResponse<string>> ResetPassword([FromBody]Mix.Identity.Models.AccountViewModels.ResetPasswordViewModel model)
        {
            var result = new RepositoryResponse<string>() { IsSucceed = true };
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                result.IsSucceed = false;
                result.Data = "Invalid User";
                return result;
            }
            string code = System.Web.HttpUtility.UrlDecode(model.Code).Replace(' ', '+');
            var idRresult = await _userManager.ResetPasswordAsync(
                                        user, model.Code, model.Password);
            result.IsSucceed = idRresult.Succeeded;
            foreach (var err in idRresult.Errors)
            {
                result.Errors.Add($"{err.Code}: {err.Description}");
            }

            return result;
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        [Route("remove-user/{id}")]
        public async Task<RepositoryResponse<string>> RemoveUser(string id)
        {
            var result = new RepositoryResponse<string>() { IsSucceed = true };
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                result.IsSucceed = false;
                result.Data = "Invalid User";
                return result;
            }
            var idRresult = await _userManager.DeleteAsync(user);
            result.IsSucceed = idRresult.Succeeded;
            if (idRresult.Succeeded)
            {
                await UserInfoViewModel.Repository.RemoveModelAsync(u => u.Id == id);
            }
            foreach (var err in idRresult.Errors)
            {
                result.Errors.Add($"{err.Code}: {err.Description}");
            }

            return result;
        }
    }
}
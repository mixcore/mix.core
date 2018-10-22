using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Models.Account;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.ViewModels.Account;
using Mix.Domain.Core.ViewModels;
using Mix.Identity.Models;
using Mix.Identity.Models.AccountViewModels;
using Newtonsoft.Json.Linq;
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
        }

        [TempData]
        public string ErrorMessage { get; set; }

        //
        // POST: /Account/Logout
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Route("Logout")]
        [HttpGet, HttpPost, HttpOptions]
        public async Task<RepositoryResponse<bool>> Logout()
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true, Data = true };
            await _signInManager.SignOutAsync().ConfigureAwait(false);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme).ConfigureAwait(false);
            await RefreshTokenViewModel.Repository.RemoveModelAsync(r => r.Username == User.Identity.Name);
            return result;
        }

        [Route("login")]
        [HttpPost, HttpOptions]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<RepositoryResponse<AccessTokenViewModel>> Login([FromBody] LoginViewModel model)
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

                    var token = await GenerateAccessTokenAsync(user, model.RememberMe);
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
                        return loginResult;
                    }
                    else
                    {
                        return loginResult;
                    }
                }
                else
                {
                    loginResult.Errors.Add("login failed");
                    return loginResult;
                }
            }
            else
            {
                return loginResult;
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

                    var token = await GenerateAccessTokenAsync(user, true);
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
        public async Task<RepositoryResponse<AccessTokenViewModel>> Register([FromBody] MixRegisterViewModel model)
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
                    if (_userManager.Users.Count() == 1)
                    {
                        await _userManager.AddToRoleAsync(user, "SuperAdmin");
                    }
                    var token = await GenerateAccessTokenAsync(user, true);
                    if (token != null)
                    {
                        result.IsSucceed = true;
                        result.Data = token;
                        _logger.LogInformation("User logged in.");
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

            return result;
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
                        "Profile", new { a.Id }, Request, Url);
                }
                );
            }
            return data;
        }

        private async Task<AccessTokenViewModel> GenerateAccessTokenAsync(ApplicationUser user, bool isRemember)
        {
            var dtIssued = DateTime.UtcNow;
            var dtExpired = dtIssued.AddMinutes(MixService.GetAuthConfig<int>("CookieExpiration"));
            var dtRefreshTokenExpired = dtIssued.AddMinutes(MixService.GetAuthConfig<int>("RefreshTokenExpiration"));
            string refreshTokenId = string.Empty;
            string refreshToken = string.Empty;
            if (isRemember)
            {
                refreshToken = Guid.NewGuid().ToString();
                RefreshTokenViewModel vmRefreshToken = new RefreshTokenViewModel(
                            new RefreshTokens()
                            {
                                Id = refreshToken,
                                Email = user.Email,
                                IssuedUtc = dtIssued,
                                ClientId = MixService.GetAuthConfig<string>("Audience"),
                                Username = user.UserName,
                                //Subject = SWCmsConstants.AuthConfiguration.Audience,
                                ExpiresUtc = dtRefreshTokenExpired
                            });

                var saveRefreshTokenResult = await vmRefreshToken.SaveModelAsync();
                refreshTokenId = saveRefreshTokenResult.Data?.Id;
            }

            AccessTokenViewModel token = new AccessTokenViewModel()
            {
                Access_token = await GenerateTokenAsync(user, dtExpired, refreshToken),
                Refresh_token = refreshTokenId,
                Token_type = MixService.GetAuthConfig<string>("TokenType"),
                Expires_in = MixService.GetAuthConfig<int>("CookieExpiration"),
                //UserData = user,
                Issued = dtIssued,
                Expires = dtExpired,
            };
            return token;
        }

        private async Task<string> GenerateTokenAsync(ApplicationUser user, DateTime expires, string refreshToken)
        {
            List<Claim> claims = await GetClaimsAsync(user);
            claims.AddRange(new[]
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Username", user.UserName),
                    new Claim("RefreshToken", refreshToken)
                });
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: MixService.GetAuthConfig<string>("Issuer"),
                audience: MixService.GetAuthConfig<string>("Audience"),
                notBefore: DateTime.UtcNow,
                claims: claims,
                // our token will live 1 hour, but you can change you token lifetime here
                expires: expires,
                signingCredentials: new SigningCredentials(JwtSecurityKey.Create(MixService.GetAuthConfig<string>("SecretKey")), SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        protected async Task<List<Claim>> GetClaimsAsync(ApplicationUser user)
        {
            List<Claim> claims = new List<Claim>();
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var claim in user.Claims)
            {
                claims.Add(CreateClaim(claim.ClaimType, claim.ClaimValue));
            }

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await _roleManager.FindByNameAsync(userRole);
                if (role != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    foreach (Claim roleClaim in roleClaims)
                    {
                        claims.Add(roleClaim);
                    }
                }
            }
            return claims;
        }

        protected Claim CreateClaim(string type, string value)
        {
            return new Claim(type, value, ClaimValueTypes.String);
        }

        public static class JwtSecurityKey
        {
            public static SymmetricSecurityKey Create(string secret)
            {
                return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
            }
        }
    }
}
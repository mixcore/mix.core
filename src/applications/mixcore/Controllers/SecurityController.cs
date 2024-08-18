using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mix.Auth.Enums;
using Mix.Auth.Models;
using Mix.Database.Entities.Account;
using Mix.Heart.Exceptions;
using Mix.Lib.Services;
using Mix.Shared.Services;
using Quartz.Listener;
using System.Security.Claims;

namespace Mixcore.Controllers
{
    public class SecurityController(
        IHttpContextAccessor httpContextAccessor,
        IMixCmsService mixCmsService,
        IPSecurityConfigService ipSecurityConfigService,
        SignInManager<MixUser> signInManager,
        ILogger<SecurityController> logger,
        MixIdentityService idService,
        TenantUserManager userManager,
        MixEndpointService mixEndpointService,
        IMixTenantService tenantService,
         IConfiguration configuration) : MixControllerBase(httpContextAccessor, mixCmsService, ipSecurityConfigService, tenantService, configuration)
    {
        private readonly SignInManager<MixUser> _signInManager = signInManager;
        private readonly TenantUserManager _userManager = userManager;
        private readonly ILogger<SecurityController> _logger = logger;
        private readonly MixIdentityService _idService = idService;
        private readonly MixEndpointService _mixEndpointService = mixEndpointService;

        [HttpGet]
        [Route("security/{page}")]
        public IActionResult Index(string page)
        {
            if (page is null)
            {
                throw new MixException(MixErrorStatus.Badrequest, nameof(page));
            }

            if (IsValid)
            {
                return View();
            }
            else
            {
                return Redirect(RedirectUrl);
            }
        }

        [Route("security/external-login")]
        [HttpPost]
        [AllowAnonymous]
        public ActionResult<JObject> ExternalLogin([FromForm] string returnUrl, [FromForm] MixExternalLoginProviders provider)
        {
            // Request a redirect to the external login provider.
            returnUrl ??= _mixEndpointService.Mixcore;
            returnUrl = returnUrl.Contains(_mixEndpointService.Mixcore) || returnUrl.StartsWith("http")
                                ? returnUrl
                                : $"{_mixEndpointService.Mixcore.TrimEnd('/')}/{returnUrl.TrimStart('/')}";
            var redirectUrl = $"{_mixEndpointService.Mixcore}/security/external-login-result";
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider.ToString(), redirectUrl);
            return new ChallengeResult(provider.ToString(), properties);
        }

        [Route("security/external-login-result")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<JObject>> ExternalLoginResultAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl ??= Url.Content("~/");
            if (remoteError != null)
            {
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToPage("./Security/Login", new { ReturnUrl = returnUrl });
            }

            // Sign in the user with this external login provider if the user already has a login.
            var siginResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (siginResult.Succeeded)
            {
                string email = info.Principal.FindFirstValue(ClaimTypes.Email);
                if (string.IsNullOrEmpty(email))
                {
                    throw new MixException(MixErrorStatus.Badrequest, "Email not exist");
                }
                var user = await _userManager.FindByEmailAsync(email);
                var token = await _idService.GetAuthData(user, true, CurrentTenant.Id);
                return View(new ExternalLoginResultModel()
                {
                    Token = JObject.FromObject(token).ToString(Formatting.None),
                    ReturnUrl = returnUrl
                });
            }
            if (siginResult.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                string email = info.Principal.FindFirstValue(ClaimTypes.Email);
                if (string.IsNullOrEmpty(email))
                {
                    throw new MixException(MixErrorStatus.Badrequest, "Email not exist");
                }
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    await _userManager.AddLoginAsync(user, info);
                    await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);
                    var token = await _idService.GetAuthData(user, true, CurrentTenant.Id);
                    return View(new ExternalLoginResultModel()
                    {
                        Token = JObject.FromObject(token).ToString(Formatting.None),
                        ReturnUrl = returnUrl
                    });
                }
                else
                {
                    user = new MixUser()
                    {
                        Email = email,
                        UserName = email,
                        CreatedDateTime = DateTime.UtcNow
                    };
                    var result = await _userManager.CreateAsync(user);
                    if (result.Succeeded)
                    {
                        result = await _userManager.AddLoginAsync(user, info);
                        if (result.Succeeded)
                        {
                            _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                            await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);
                            var token = await _idService.GetAuthData(user, true, CurrentTenant.Id);
                            return View(new ExternalLoginResultModel()
                            {
                                Token = JObject.FromObject(token).ToString(Formatting.None),
                                ReturnUrl = returnUrl
                            });
                        }
                    }
                }


                return BadRequest();
            }
        }
    }
}

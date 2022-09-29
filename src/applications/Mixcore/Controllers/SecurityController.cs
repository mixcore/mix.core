using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Mix.Database.Entities.Account;
using Mix.Identity.Models.AccountViewModels;
using Mix.Lib.Services;
using Mix.Shared.Services;
using System.Security.Claims;

namespace Mixcore.Controllers
{
    public class SecurityController : MixControllerBase
    {
        private readonly SignInManager<MixUser> _signInManager;
        private readonly TenantUserManager _userManager;
        private readonly ILogger<ExternalLoginModel> _logger;
        private readonly MixIdentityService _idService;
        private readonly MixCmsContext _cmsContext;
        public SecurityController(
            IHttpContextAccessor httpContextAccessor,
            MixService mixService,
            IPSecurityConfigService ipSecurityConfigService,
            MixCacheService cacheService,
            SignInManager<MixUser> signInManager,
            ILogger<ExternalLoginModel> logger,
            MixIdentityService idService,
            TenantUserManager userManager,
            MixCmsContext cmsContext)
            : base(httpContextAccessor, mixService, ipSecurityConfigService)
        {
            _signInManager = signInManager;
            _logger = logger;
            _idService = idService;
            _userManager = userManager;
            _cmsContext = cmsContext;
        }

        [HttpGet]
        [Route("security/{page}")]
        public IActionResult Index(string page)
        {
            if (isValid)
            {
                return View();
            }
            else
            {
                return Redirect(_redirectUrl);
            }
        }

        [Route("security/external-login")]
        [HttpPost]
        [AllowAnonymous]
        public ActionResult<JObject> ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            returnUrl ??= Url.Content("~/");
            var redirectUrl = $"/security/external-login-result?returnUrl={returnUrl}";
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        [Route("security/external-login-result")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<JObject>> ExternalLoginResultAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
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
                var user = await _userManager.FindByEmailAsync(email);
                var token = await _idService.GetAuthData(user, true, CurrentTenant.Id);
                return View(new ExternalLoginResultModel()
                {
                    Token = token.ToString(Formatting.None),
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
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    await _userManager.AddLoginAsync(user, info);
                    await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);
                    var token = await _idService.GetAuthData(user, true, CurrentTenant.Id);
                    return View(new ExternalLoginResultModel()
                    {
                        Token = token.ToString(Formatting.None),
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
                                Token = token.ToString(Formatting.None),
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

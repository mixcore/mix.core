using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Account;
using Mix.Database.Services;
using Mix.Heart.Models;
using Mix.Identity.Domain.Models;
using Mix.Identity.Models;
using Mix.Lib.Services;
using Mix.RepoDb.Repositories;
using Mix.Shared.Services;
using Newtonsoft.Json;
using RepoDb;
using System.Linq.Expressions;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Newtonsoft.Json.Linq;
using System.Web;
using Mix.Identity.Models.ManageViewModels;
using Mix.Lib.Interfaces;
using Mix.OAuth.OauthRequest;
using Mix.Auth.Dtos;
using Mix.Auth.Constants;
using Mix.OAuth.Services;

namespace Mix.Account.Controllers
{
    [Route("api/v2/rest/mix-account/user")]
    public class MixAccountController : MixTenantApiControllerBase
    {
        private readonly TenantUserManager _userManager;
        private readonly SignInManager<MixUser> _signInManager;
        private readonly RoleManager<MixRole> _roleManager;
        private readonly MixIdentityService _idService;
        private readonly IAuthorizeResultService _authResultService;
        private readonly IMixEdmService _edmService;
        private readonly EntityRepository<MixCmsAccountContext, MixUser, Guid> _repository;
        private readonly MixRepoDbRepository _repoDbRepository;
        private readonly UnitOfWorkInfo _accountUow;
        private readonly UnitOfWorkInfo<MixCmsContext> _cmsUow;
        private readonly MixCmsAccountContext _accContext;
        private readonly EntityRepository<MixCmsAccountContext, RefreshTokens, Guid> _refreshTokenRepo;
        private readonly AuthConfigService _authConfigService;

        public MixAccountController(
            TenantUserManager userManager,
            SignInManager<MixUser> signInManager,
            RoleManager<MixRole> roleManager,
            MixIdentityService idService,
            EntityRepository<MixCmsAccountContext, RefreshTokens, Guid> refreshTokenRepo,
            MixCmsAccountContext accContext,
            MixCmsContext cmsContext,
            MixRepoDbRepository repoDbRepository,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService mixService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            IQueueService<MessageQueueModel> queueService,
            AuthConfigService authConfigService,
            IMixEdmService edmService,
            IMixTenantService mixTenantService,
            IAuthorizeResultService authResultService)
            : base(httpContextAccessor, configuration, mixService,
                translator, mixIdentityService, queueService, mixTenantService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _idService = idService;
            _refreshTokenRepo = refreshTokenRepo;
            _accountUow = new(accContext);
            _cmsUow = new(cmsContext);
            _repository = new(_accountUow);
            _accContext = accContext;


            _repoDbRepository = repoDbRepository;
            _authConfigService = authConfigService;
            _edmService = edmService;
            _authResultService = authResultService;
        }

        #region Overrides

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (_accountUow.ActiveTransaction != null)
            {
                _accountUow.Complete();
            }

            if (_cmsUow.ActiveTransaction != null)
            {
                _cmsUow.Complete();
            }

            base.OnActionExecuted(context);
        }

        #endregion

        [MixAuthorize]
        [Route("my-tenants")]
        [HttpGet]
        public async Task<ActionResult> MyTenants()
        {
            var userId = Guid.Parse(_idService.GetClaim(User, MixClaims.Id));
            var tenantIds = await _accContext.MixUserTenants.Where(m => m.MixUserId == userId).Select(m => m.TenantId)
                .ToListAsync();
            var tenants = await MixTenantSystemViewModel.GetRepository(_cmsUow, CacheService)
                .GetListAsync(m => tenantIds.Contains(m.Id));
            return Ok(tenants);
        }

        [Route("register")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register([FromBody] RegisterRequestModel model)
        {
            await _idService.RegisterAsync(model, CurrentTenant.Id, _cmsUow);
            var user = await _userManager.FindByNameAsync(model.UserName).ConfigureAwait(false);
            var result = _idService.GetAuthData(user, true, CurrentTenant.Id);
            if (result != null && user != null)
            {
                if (_authConfigService.AppSettings.RequireConfirmedEmail)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink =
                        $"{_authConfigService.AppSettings.ConfirmedEmailUrl}?token={HttpUtility.UrlEncode(token)}&email={user.Email}";

                    var data = new JObject(new JProperty("Url", confirmationLink));
                    await _edmService.SendMailWithEdmTemplate("Email Confirmation", "ActiveEmail", data, user.Email);
                }
                else
                {
                    await _edmService.SendMailWithEdmTemplate("Welcome", "Welcome", ReflectionHelper.ParseObject(user),
                        user.Email);
                }

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("connect/token")]
        public JsonResult Token([FromBody] TokenRequest tokenRequest)
        {
            var result = _authResultService.GenerateToken(tokenRequest);

            if (result.HasError)
                return Json(new
                {
                    error = result.Error,
                    error_description = result.ErrorDescription
                });

            return Json(result);
        }


        [AllowAnonymous]
        [HttpGet("resend-confirm-email/{id}")]
        public async Task<ActionResult> ResendConfirmEmail(string id)
        {
            var user = await _userManager.FindByIdAsync(id).ConfigureAwait(false);
            if (user is { EmailConfirmed: false })
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink =
                    $"{_authConfigService.AppSettings.ConfirmedEmailUrl}?token={HttpUtility.UrlEncode(token)}&email={user.Email}";
                var data = new JObject(new JProperty("Url", confirmationLink));
                await _edmService.SendMailWithEdmTemplate("Email Confirmation", "ActiveEmail", data, user.Email);
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return View("Error");
            var result = await _userManager.ConfirmEmailAsync(user, token);
            string redirectUrl = result.Succeeded
                ? _authConfigService.AppSettings.ConfirmedEmailUrlSuccess
                : $"{_authConfigService.AppSettings.ConfirmedEmailUrlFail}?error={result.Errors.First().Description}";
            await _edmService.SendMailWithEdmTemplate("Welcome", "Welcome", ReflectionHelper.ParseObject(user),
                user.Email);
            return Redirect(redirectUrl);
        }

        [Route("Logout")]
        [HttpGet]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync().ConfigureAwait(false);
            await _refreshTokenRepo.DeleteAsync(r => r.Username == User.Identity.Name);
            return Ok();
        }

        [Route("token")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> GetToken([FromBody] LoginDto requestDto)
        {
            string key = GlobalConfigService.Instance.AppSettings.ApiEncryptKey;
            string decryptMsg = AesEncryptionHelper.DecryptString(requestDto.Message, key);
            var model = JsonConvert.DeserializeObject<GetTokenModel>(decryptMsg);
            var loginResult = await _idService.GetTokenAsync(model);
            if (loginResult != null)
            {
                return Ok(loginResult);
            }

            return Unauthorized();
        }

        [Route("login")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] LoginDto requestDto)
        {
            string key = GlobalConfigService.Instance.AppSettings.ApiEncryptKey;
            string decryptMsg = AesEncryptionHelper.DecryptString(requestDto.Message, key);
            var model = JsonConvert.DeserializeObject<LoginRequestModel>(decryptMsg);
            var loginResult = await _idService.LoginAsync(model);
            return Ok(loginResult);
        }

        [Route("external-login")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLogin([FromBody] LoginDto requestDto)
        {
            string key = GlobalConfigService.Instance.AppSettings.ApiEncryptKey;
            string decryptMsg = AesEncryptionHelper.DecryptString(requestDto.Message, key);
            var model = JsonConvert.DeserializeObject<RegisterExternalBindingModel>(decryptMsg);
            var loginResult = await _idService.ExternalLogin(model);
            return Ok(loginResult);
        }

        [Route("login-unsecure")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> LoginUnSecure([FromBody] LoginRequestModel model)
        {
            var loginResult = await _idService.LoginAsync(model);
            return Ok(loginResult);
        }

        [AllowAnonymous]
        [HttpPost("external-login-unsecure")]
        public async Task<ActionResult> ExternalLoginUnSecure([FromBody] RegisterExternalBindingModel model)
        {
            var loginResult = await _idService.ExternalLogin(model);
            return Ok(loginResult);
        }

        [Route("get-external-login-providers")]
        [HttpGet]
        public async Task<ActionResult> GetExternalLoginProviders()
        {
            var providers = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            return Ok(providers);
        }

        [Route("renew-token")]
        [HttpPost]
        public async Task<ActionResult> RenewToken([FromBody] RenewTokenDto refreshTokenDto)
        {
            var token = await _idService.RenewTokenAsync(refreshTokenDto);
            return Ok(token);
        }

        [MixAuthorize]
        [HttpGet]
        [Route("my-profile")]
        public async Task<ActionResult<MixUserViewModel>> MyProfile([FromServices] DatabaseService databaseService)
        {
            try
            {
                var id = _idService.GetClaim(User, MixClaims.Id);
                var user = await _userManager.FindByIdAsync(id);

                if (user == null)
                {
                    return BadRequest();
                }

                var result = new MixUserViewModel(user, _cmsUow);
                await result.LoadUserDataAsync(CurrentTenant.Id, _repoDbRepository, _accContext, CacheService);
                return Ok(result);
            }
            catch (MixException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.ServerError, ex);
            }
        }

        [HttpGet]
        [MixAuthorize(roles: MixRoles.Owner)]
        [Route("details/{id}")]
        public async Task<ActionResult> Details([FromServices] DatabaseService databaseService, string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = new MixUserViewModel(user, _cmsUow);
                await result.LoadUserDataAsync(CurrentTenant.Id, _repoDbRepository, _accContext, CacheService);
                return Ok(result);
            }

            return BadRequest();
        }

        [HttpDelete]
        [MixAuthorize(roles: MixRoles.Owner)]
        [Route("remove-user/{id}")]
        public async Task<ActionResult> Remove(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var logins = await _userManager.GetLoginsAsync(user);
                foreach (var login in logins)
                {
                    var result = await _userManager.RemoveLoginAsync(user, login.LoginProvider, login.ProviderKey);
                    if (!result.Succeeded)
                    {
                        throw new MixException(MixErrorStatus.Badrequest, result.Errors.First());
                    }
                }

                var idResult = await _userManager.DeleteAsync(user);
                if (idResult.Succeeded)
                {
                    _repoDbRepository.InitTableName(MixDatabaseNames.SYSTEM_USER_DATA);
                    await _repoDbRepository.DeleteAsync(new List<QueryField>()
                    {
                        new QueryField("parentId", user.Id),
                        new QueryField("parentType", MixContentType.User)
                    });
                    return Ok();
                }
            }

            throw new MixException(MixErrorStatus.NotFound);
        }

        [HttpPost]
        [MixAuthorize(roles: MixRoles.Owner)]
        [Route("user-in-role")]
        public async Task<ActionResult> Details([FromBody] UserRoleModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.RoleId);
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
                else if (role.Name != null && !await (_userManager.IsInRoleAsync(appUser, role.Name)))
                {
                    await _userManager.AddToRoleAsync(appUser, role.Name, CurrentTenant.Id);
                    return Ok();
                }
            }
            else
            {
                var appUser = await _userManager.FindByIdAsync(model.UserId);

                if (appUser == null)
                {
                    errors.Add($"User: {model.UserId} does not exists");
                }

                await _userManager.RemoveFromRoleAsync(appUser, role.Name, CurrentTenant.Id);
                return Ok();
            }

            return BadRequest(errors);
        }

        [MixAuthorize(roles: MixRoles.Owner)]
        [HttpGet("list")]
        public virtual async Task<ActionResult<PagingResponseModel<MixUserViewModel>>> Get(
            [FromQuery] SearchRequestDto request)
        {
            Expression<Func<MixUser, bool>> predicate = model =>
                (string.IsNullOrWhiteSpace(request.Keyword)
                 || (
                     (EF.Functions.Like(model.UserName, $"%{request.Keyword}%"))
                     || (EF.Functions.Like(model.Email, $"%{request.Keyword}%"))
                 )
                )
                && (!request.FromDate.HasValue
                    || (model.CreatedDateTime >= request.FromDate.Value.ToUniversalTime())
                )
                && (!request.ToDate.HasValue
                    || (model.CreatedDateTime <= request.ToDate.Value.ToUniversalTime())
                );

            var searchQuery = new SearchAccountQueryModel(request);
            var data = await _repository
                .GetPagingEntitiesAsync(predicate, searchQuery.PagingData)
                .ConfigureAwait(false);

            var items = new List<MixUserViewModel>();
            foreach (var user in data.Items)
            {
                var result = new MixUserViewModel(user, _cmsUow);
                await result.LoadUserDataAsync(CurrentTenant.Id, _repoDbRepository, _accContext, CacheService);
                items.Add(result);
            }

            return new PagingResponseModel<MixUserViewModel>(items, searchQuery.PagingData);
        }

        // POST api/template
        [MixAuthorize(roles: MixRoles.Owner)]
        [HttpPost]
        [Route("save")]
        public async Task<ActionResult<MixUserViewModel>> Save(
            [FromBody] MixUserViewModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            if (user == null)
            {
                return BadRequest();
            }

            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            if (!model.IsChangePassword)
            {
                return Ok(model);
            }

            var changePwd = await _userManager.ChangePasswordAsync(
                user,
                model.ChangePassword.CurrentPassword,
                model.ChangePassword.NewPassword);
            if (!changePwd.Succeeded)
            {
                throw new MixException(string.Join(",", changePwd.Errors));
            }
            else
            {
                // Remove other token if change password success
                if (Guid.TryParse(_idService.GetClaim(User, MixClaims.RefreshToken), out var refreshTokenId))
                {
                    await _refreshTokenRepo.DeleteManyAsync(m =>
                        m.Username == user.UserName && m.Id != refreshTokenId);
                }
            }

            return Ok(model);
        }


        [HttpPost]
        [Route("forgot-password")]
        public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordRequestModel model)
        {
            if (string.IsNullOrEmpty(model.Email))
            {
                throw new MixException(MixErrorStatus.Badrequest, "Invalid Email");
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                throw new MixException(MixErrorStatus.Badrequest, "Email Not Exist");
            }

            //if (!await _userManager.IsEmailConfirmedAsync(user))
            //    result.Data = "Invalid Email";

            var confirmationCode =
                await _userManager.GeneratePasswordResetTokenAsync(user);

            var callbackUrl =
                $"{Request.Scheme}://{Request.Host}/security/reset-password/?token={HttpUtility.UrlEncode(confirmationCode)}";
            var obj = ReflectionHelper.ParseObject(user);
            obj.Add(new JProperty("URL", callbackUrl));

            await _edmService.SendMailWithEdmTemplate("Reset Password", "ForgotPassword", obj, user.Email);

            return Ok();
        }

        [HttpPost]
        [Route("reset-password")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordRequestModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                throw new MixException(MixErrorStatus.Badrequest, "Invalid User");
            }

            var idResult = await _userManager.ResetPasswordAsync(
                user, model.Code, model.Password);
            if (!idResult.Succeeded)
            {
                var errors = idResult.Errors.Select(m => m.Description);
                throw new MixException(MixErrorStatus.Badrequest, errors);
            }

            return Ok();
        }

        [MixAuthorize]
        [HttpPost]
        [Route("change-password")]
        public async Task<ActionResult> ResetPassword([FromBody] ChangePasswordViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest();
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            return result.Succeeded ? Ok() : BadRequest(result.Errors.Select(m => m.Description).ToList());
        }
    }
}
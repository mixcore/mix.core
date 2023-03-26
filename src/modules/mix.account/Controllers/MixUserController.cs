using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Mix.Communicator.Services;
using Mix.Database.Entities.Account;
using Mix.Database.Services;
using Mix.Heart.Models;
using Mix.Identity.Domain.Models;
using Mix.Identity.Dtos;
using Mix.Identity.Models;
using Mix.Identity.Models.AccountViewModels;
using Mix.Lib.Services;
using Mix.RepoDb.Repositories;
using Mix.Shared.Services;
using Newtonsoft.Json;
using RepoDb;
using System.Linq.Expressions;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Communicator.Models;
using Newtonsoft.Json.Linq;
using System.Web;
using Mix.Identity.Models.ManageViewModels;
using Mix.Mixdb.Entities;
using Mix.Lib.Interfaces;

namespace Mix.Account.Controllers
{
    [Route("api/v2/rest/mix-account/user")]
    public class MixUserController : MixTenantApiControllerBase
    {
        private readonly TenantUserManager _userManager;
        private readonly SignInManager<MixUser> _signInManager;
        private readonly RoleManager<MixRole> _roleManager;
        private readonly MixIdentityService _idService;
        private readonly EmailService _emailService;
        private readonly IMixEdmService _edmService;
        private readonly EntityRepository<MixCmsAccountContext, MixUser, Guid> _repository;
        private readonly MixRepoDbRepository _repoDbRepository;
        protected readonly MixIdentityService _mixIdentityService;
        protected UnitOfWorkInfo AccountUow;
        protected UnitOfWorkInfo<MixDbDbContext> MixDbUow;
        protected UnitOfWorkInfo<MixCmsContext> CmsUow;
        private readonly MixCmsAccountContext _accContext;
        private readonly EntityRepository<MixCmsAccountContext, RefreshTokens, Guid> _refreshTokenRepo;
        private readonly AuthConfigService _authConfigService;

        public MixUserController(
             TenantUserManager userManager,
             SignInManager<MixUser> signInManager,
             RoleManager<MixRole> roleManager,
             MixIdentityService idService,
             EntityRepository<MixCmsAccountContext, RefreshTokens, Guid> refreshTokenRepo,
             MixCmsAccountContext accContext,
             MixCmsContext cmsContext,
             MixRepoDbRepository repoDbRepository,
             EmailService emailService,
             IHttpContextAccessor httpContextAccessor,
             IConfiguration configuration,
             MixCacheService mixService,
             TranslatorService translator,
             MixIdentityService mixIdentityService,
             IQueueService<MessageQueueModel> queueService,
             UnitOfWorkInfo<MixDbDbContext> mixDbUow,
             AuthConfigService authConfigService,
             IMixEdmService edmService)
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, queueService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _idService = idService;
            _refreshTokenRepo = refreshTokenRepo;
            _mixIdentityService = mixIdentityService;
            AccountUow = new(accContext);
            CmsUow = new(cmsContext);
            _repository = new(AccountUow);
            _accContext = accContext;


            _repoDbRepository = repoDbRepository;
            _emailService = emailService;
            MixDbUow = mixDbUow;
            _authConfigService = authConfigService;
            _edmService = edmService;
        }

        #region Overrides

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (AccountUow.ActiveTransaction != null)
            {
                AccountUow.Complete();
            }

            if (CmsUow.ActiveTransaction != null)
            {
                CmsUow.Complete();
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
            var tenantIds = await _accContext.MixUserTenants.Where(m => m.MixUserId == userId).Select(m => m.TenantId).ToListAsync();
            var tenants = await MixTenantSystemViewModel.GetRepository(CmsUow, CacheService).GetListAsync(m => tenantIds.Contains(m.Id));
            return Ok(tenants);
        }

        [Route("register")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register([FromBody] RegisterViewModel model)
        {
            await _idService.RegisterAsync(model, CurrentTenant.Id, CmsUow);
            var user = await _userManager.FindByNameAsync(model.UserName).ConfigureAwait(false);
            var result = _idService.GetAuthData(user, true, CurrentTenant.Id);
            if (result != null && user != null)
            {
                if (_authConfigService.AppSettings.RequireConfirmedEmail)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = $"{_authConfigService.AppSettings.ConfirmedEmailUrl}?token={HttpUtility.UrlEncode(token)}&email={user.Email}";

                    var data = new JObject(new JProperty("Url", confirmationLink));
                    await _edmService.SendMailWithEdmTemplate("Email Confirmation", "ActiveEmail", data, user.Email);
                }
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpGet("resend-confirm-email/{id}")]
        public async Task<ActionResult> ResendConfirmEmail(string id)
        {
            var user = await _userManager.FindByIdAsync(id).ConfigureAwait(false);
            if (user != null && !user.EmailConfirmed)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = $"{_authConfigService.AppSettings.ConfirmedEmailUrl}?token={HttpUtility.UrlEncode(token)}&email={user.Email}";
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
            string redirectUrl = result.Succeeded ? _authConfigService.AppSettings.ConfirmedEmailUrlSuccess
                : $"{_authConfigService.AppSettings.ConfirmedEmailUrlFail}?error={result.Errors.First().Description}";
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
            var model = JsonConvert.DeserializeObject<LoginViewModel>(decryptMsg);
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
        public async Task<ActionResult> LoginUnSecure(LoginViewModel model)
        {
            var loginResult = await _idService.LoginAsync(model);
            return Ok(loginResult);
        }

        [AllowAnonymous]
        [HttpPost("external-login-unsecure")]
        public async Task<ActionResult> ExternalLoginUnSecure(RegisterExternalBindingModel model)
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
            var id = _idService.GetClaim(User, MixClaims.Id);
            var user = await _userManager.FindByIdAsync(id); ;

            if (user != null)
            {
                var result = new MixUserViewModel(user, CmsUow);
                await result.LoadUserDataAsync(CurrentTenant.Id, _repoDbRepository, _accContext, CacheService);
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpGet]
        [MixAuthorize(roles: MixRoles.Owner)]
        [Route("details/{id}")]
        public async Task<ActionResult> Details([FromServices] DatabaseService databaseService, string id = null)
        {
            MixUser user = await _userManager.FindByIdAsync(id); ;

            if (user != null)
            {
                var result = new MixUserViewModel(user, CmsUow);
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
            MixUser user = await _userManager.FindByIdAsync(id); ;
            if (user != null)
            {
                var logins = await _userManager.GetLoginsAsync(user);
                foreach (var login in logins)
                {
                    var result = await _userManager.RemoveLoginAsync(user, login.LoginProvider, login.ProviderKey);
                    if (!result.Succeeded)
                    {
                        Console.WriteLine(result.Errors);
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
                else if (!await (_userManager.IsInRoleAsync(appUser, role.Name)))
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

                var removeResult = _userManager.RemoveFromRoleAsync(appUser, role.Name, CurrentTenant.Id);
                return Ok();
            }
            return BadRequest(errors);
        }

        [MixAuthorize(roles: MixRoles.Owner)]
        [HttpGet("list")]
        public virtual async Task<ActionResult<PagingResponseModel<MixUser>>> Get([FromQuery] SearchRequestDto request)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var roles = await _userManager.GetRolesAsync(user);
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
            var data = await _repository.GetPagingEntitiesAsync(predicate, searchQuery.PagingData)
               .ConfigureAwait(false);
            return data;
        }

        // POST api/template
        [MixAuthorize(roles: MixRoles.Owner)]
        [HttpPost]
        [Route("save")]
        public async Task<ActionResult<MixUserViewModel>> Save(
            [FromBody] MixUserViewModel model)
        {
            if (model != null)
            {
                var user = await _userManager.FindByIdAsync(model.Id.ToString());
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                var updInfo = await _userManager.UpdateAsync(user);
                if (model.IsChangePassword)
                {
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
                            await _refreshTokenRepo.DeleteManyAsync(m => m.Username == user.UserName && m.Id != refreshTokenId);
                        }
                    }
                }
                return Ok(model);
            }
            return BadRequest();
        }


        [HttpPost]
        [Route("forgot-password")]
        public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordViewModel model)
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

            var callbackUrl = $"{Request.Scheme}://{Request.Host}/security/reset-password/?token={System.Web.HttpUtility.UrlEncode(confirmationCode)}";
            var edmTemplate = await MixTemplateViewModel.GetRepository(CmsUow, CacheService).GetSingleAsync(
                m => m.FolderType == MixTemplateFolderType.Edms && m.FileName == "ForgotPassword");
            string content = callbackUrl;
            if (edmTemplate != null)
            {
                content = edmTemplate.Content.Replace("[URL]", callbackUrl);
            }
            EmailMessageModel msg = new()
            {
                Subject = "Reset Password",
                Message = content,
                To = user.Email
            };

            await _emailService.SendMail(msg);

            return Ok();
        }

        [HttpPost]
        [Route("reset-password")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                throw new MixException(MixErrorStatus.Badrequest, "Invalid User");
            }
            string code = HttpUtility.UrlDecode(model.Code)?.Replace(' ', '+');
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
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            return result.Succeeded ? Ok() : BadRequest(result.Errors.Select(m => m.Description).ToList());
        }
        #region Helpers

        #endregion
    }
}

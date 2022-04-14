using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mix.Account.Domain.Dtos;
using Mix.Database.Entities.Account;
using Mix.Heart.Models;
using Mix.Identity.Domain.Models;
using Mix.Identity.Dtos;
using Mix.Identity.Models;
using Mix.Identity.Models.AccountViewModels;
using Mix.Lib.Services;

using Mix.Shared.Services;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace Mix.Account.Controllers
{
    [Route("api/v2/rest/mix-account/user")]
    [ApiController]
    public class MixUserController : ControllerBase
    {
        private readonly TenantUserManager _userManager;
        private readonly SignInManager<MixUser> _signInManager;
        private readonly RoleManager<MixRole> _roleManager;
        private readonly ILogger<MixUserController> _logger;
        private readonly MixIdentityService _idService;
        private readonly EntityRepository<MixCmsAccountContext, MixUser, Guid> _repository;
        protected readonly MixIdentityService _mixIdentityService;
        protected UnitOfWorkInfo _accUOW;
        protected UnitOfWorkInfo _cmsUOW;
        private readonly MixCmsAccountContext _accContext;
        private readonly MixCmsContext _cmsContext;
        private readonly EntityRepository<MixCmsAccountContext, RefreshTokens, Guid> _refreshTokenRepo;

        protected int MixTenantId { get; set; }
        public MixUserController(
            IHttpContextAccessor httpContextAccessor,
            TenantUserManager userManager,
            SignInManager<MixUser> signInManager,
            RoleManager<MixRole> roleManager,
            ILogger<MixUserController> logger,
            MixIdentityService idService, EntityRepository<MixCmsAccountContext, RefreshTokens, Guid> refreshTokenRepo,
            MixCmsAccountContext accContext, MixIdentityService mixIdentityService, MixCmsContext cmsContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _idService = idService;
            _refreshTokenRepo = refreshTokenRepo;
            _mixIdentityService = mixIdentityService;
            _accUOW = new(accContext);
            _cmsUOW = new(cmsContext);
            _repository = new(_accUOW);
            _cmsContext = cmsContext;
            _accContext = accContext;

            if (httpContextAccessor.HttpContext.Session.GetInt32(MixRequestQueryKeywords.MixTenantId).HasValue)
            {
                MixTenantId = httpContextAccessor.HttpContext.Session.GetInt32(MixRequestQueryKeywords.MixTenantId).Value;
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin")]
        [Route("my-tenants")]
        [HttpGet]
        public async Task<ActionResult> MyTenants()
        {
            var userId = Guid.Parse(_idService.GetClaim(User, MixClaims.Id));
            var tenantIds = await _accContext.MixUserTenants.Where(m => m.MixUserId == userId).Select(m => m.TenantId).ToListAsync();
            var tenants = await MixTenantViewModel.GetRepository(_cmsUOW).GetListAsync(m => tenantIds.Contains(m.Id));
            return Ok(tenants);
        }

        [Route("register")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register([FromBody] RegisterViewModel model)
        {
            var result = await _idService.Register(model, MixTenantId, _cmsUOW);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
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
            var loginResult = await _idService.GetToken(model);
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
            var loginResult = await _idService.Login(model);
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

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [Route("my-profile")]
        public async Task<ActionResult<MixUserViewModel>> MyProfile()
        {
            var id = _idService.GetClaim(User, MixClaims.Id);
            var user = await _userManager.FindByIdAsync(id); ;

            if (user != null)
            {
                var result = new MixUserViewModel(user, _cmsUOW);
                await result.LoadUserDataAsync(MixTenantId);
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Owner")]
        [Route("details/{id}")]
        public async Task<ActionResult> Details(string id = null)
        {
            MixUser user = await _userManager.FindByIdAsync(id); ;

            if (user != null)
            {
                var result = new MixUserViewModel(user, _cmsUOW);
                await result.LoadUserDataAsync(MixTenantId);
                return Ok(result);
            }
            return BadRequest();
        }

        [HttpDelete]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Owner")]
        [Route("remove-user/{id}")]
        public async Task<ActionResult> Remove(string id)
        {
            MixUser user = await _userManager.FindByIdAsync(id); ;

            var idRresult = await _userManager.DeleteAsync(user);
            if (idRresult.Succeeded)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "SuperAdmin, Owner")]
        [Route("user-in-role")]
        public async Task<ActionResult> Details(UserRoleModel model)
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
                    await _userManager.AddToRoleAsync(appUser, role.Name, MixTenantId);
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

                var removeResult = _userManager.RemoveFromRoleAsync(appUser, role.Name, MixTenantId);
                return Ok();
            }
            return BadRequest(errors);
        }

        [MixAuthorize(roles: "SuperAdmin, Owner")]
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [Route("save")]
        public async Task<ActionResult<MixUserViewModel>> Save(
            [FromBody] MixUserViewModel model)
        {
            if (model != null)
            {
                var user = await _userManager.FindByIdAsync(model.Id.ToString());
                user.Email = model.Email;
                var updInfo = await _userManager.UpdateAsync(user);
                //var saveData = await model.UserData.SaveAsync();
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
                        var refreshToken = User.Claims.SingleOrDefault(c => c.Type == "RefreshToken")?.Value;
                        //await RefreshTokenViewModel.Repository.RemoveModelAsync(r => r.Id != refreshToken);
                    }
                }
                return Ok(model);
            }
            return BadRequest();
        }

        #region Helpers



        #endregion
    }
}

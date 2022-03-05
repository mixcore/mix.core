using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mix.Account.Domain.Dtos;
using Mix.Database.Entities.Account;
using Mix.Heart.Models;
using Mix.Identity.Dtos;
using Mix.Identity.Models;
using Mix.Identity.Models.AccountViewModels;
using Mix.Lib.Services;
using Mix.Shared.Dtos;
using Mix.Shared.Services;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace Mix.Account.Controllers
{
    [Route("api/v2/rest/mix-account/user")]
    [ApiController]
    public class MixUserController : ControllerBase
    {
        private readonly UserManager<MixUser> _userManager;
        private readonly SignInManager<MixUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<MixUserController> _logger;
        private readonly MixIdentityService _idService;
        private readonly EntityRepository<ApplicationDbContext, MixUser, string> _repository;
        protected readonly MixIdentityService _mixIdentityService;
        protected UnitOfWorkInfo _uow;
        private readonly EntityRepository<MixCmsAccountContext, RefreshTokens, Guid> _refreshTokenRepo;
        public MixUserController(
            UserManager<MixUser> userManager,
            SignInManager<MixUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<MixUserController> logger,
            MixIdentityService idService, EntityRepository<MixCmsAccountContext, RefreshTokens, Guid> refreshTokenRepo,
            ApplicationDbContext context, MixIdentityService mixIdentityService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _idService = idService;
            _refreshTokenRepo = refreshTokenRepo;
            _mixIdentityService = mixIdentityService;
            _uow = new(context);
            _repository = new(_uow);
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
            string key = GlobalConfigService.Instance.AppSettings.ApiEncryptKey;
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

        [HttpGet("list")]
        public virtual async Task<ActionResult<PagingResponseModel<MixUser>>> Get([FromQuery] SearchRequestDto request)
        {
            Expression<Func<MixUser, bool>> predicate = model =>
                (string.IsNullOrWhiteSpace(request.Keyword)
                || (
                    (EF.Functions.Like(model.UserName, $"%{request.Keyword}%"))
                   || (EF.Functions.Like(model.FirstName, $"%{request.Keyword}%"))
                   || (EF.Functions.Like(model.LastName, $"%{request.Keyword}%"))
                   )
                )
                && (!request.FromDate.HasValue
                    || (model.JoinDate >= request.FromDate.Value.ToUniversalTime())
                )
                && (!request.ToDate.HasValue
                    || (model.JoinDate <= request.ToDate.Value.ToUniversalTime())
                );
            var searchQuery = new SearchAccountQueryModel(request);
            var data = await _repository.GetPagingEntitiesAsync(predicate, searchQuery.PagingData)
               .ConfigureAwait(false);
            return data;
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

        #region Helpers



        #endregion
    }
}

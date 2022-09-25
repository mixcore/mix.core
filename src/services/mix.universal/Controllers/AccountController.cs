using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mix.Database.Entities.Cms;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Identity.Constants;
using Mix.Identity.Models.AccountViewModels;
using Mix.Lib.Attributes;
using Mix.Lib.Base;
using Mix.Lib.Services;
using Mix.Lib.ViewModels;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Service.Services;
using Mix.Universal.Lib.Dtos;
using Mix.Universal.Lib.Entities;
using Mix.Universal.Lib.Helpers;

namespace Mix.Universal.Controllers
{
    [ApiController]
    [Route("api/v2/rest/mix-universal/account")]
    public class AccountController : MixApiControllerBase
    {
        private readonly MixUniversalDbContext _context;
        private readonly TenantUserManager _userManager;
        private readonly MixIdentityService _idService;
        private readonly ILogger<AccountController> _logger;
        protected UnitOfWorkInfo _cmsUOW;
        public AccountController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository, MixIdentityService mixIdentityService,
            IQueueService<MessageQueueModel> queueService,
            MixIdentityService idService,
            MixCmsContext cmsContext,
            TenantUserManager userManager,
            ILogger<AccountController> logger, MixUniversalDbContext context) : base(httpContextAccessor, configuration, mixService, translator, cultureRepository, mixIdentityService, queueService)
        {
            _idService = idService;
            _cmsUOW = new(cmsContext);
            _userManager = userManager;
            _logger = logger;
            _context = context;
        }


        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        public async Task<ActionResult> Register([FromBody] RegisterDto dto)
        {
            var register = MixUniversalHelper.ParseRegisterDto(dto);
            var result = await _idService.Register(register, CurrentTenant.Id, _cmsUOW);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }


        [MixAuthorize]
        [HttpGet("my-profile")]
        public async Task<ActionResult<MixUserViewModel>> MyProfile()
        {
            var id = _idService.GetClaim(User, MixClaims.Id);
            var user = await _userManager.FindByIdAsync(id); ;

            if (user != null)
            {
                var result = new MixUserViewModel(user, _cmsUOW);
                return Ok(result);
            }
            return BadRequest();
        }
    }
}
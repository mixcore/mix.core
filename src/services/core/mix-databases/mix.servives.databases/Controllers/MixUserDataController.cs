using Microsoft.AspNetCore.Mvc;
using Mix.Heart.Services;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Attributes;
using Mix.Lib.Base;
using Mix.Lib.Interfaces;
using Mix.Lib.Services;
using Mix.Mixdb.ViewModels;
using Mix.Mq.Lib.Models;
using Mix.Queue.Interfaces;
using Mix.Services.Databases.Lib.Dtos;
using Mix.Services.Databases.Lib.Interfaces;
using Mix.SignalR.Interfaces;

namespace Mix.Services.Databases.Controllers
{
    [Route("api/v2/rest/mix-services/user-data")]
    public sealed class MixUserDataController :
        MixRestHandlerApiControllerBase<MixUserDataViewModel, MixDbDbContext, MixUserData, int>
    {
        private readonly TenantUserManager _userManager;
        private readonly IMixUserDataService _userDataService;
        public MixUserDataController(
            IMixUserDataService metadataService,
            TenantUserManager userManager,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixDbDbContext> uow,
            IMemoryQueueService<MessageQueueModel> queueService,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService)
            : base(httpContextAccessor, configuration,
                  cacheService, translator, mixIdentityService, uow, queueService, portalHub, mixTenantService)
        {
            _userDataService = metadataService;
            _userManager = userManager;
            Repository.IsCache = false;
        }

        #region Routes

        [MixAuthorize]
        [HttpGet("my-profile")]
        public async Task<ActionResult<MixUserDataViewModel>> GetUserData(CancellationToken cancellationToken = default)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return BadRequest();
            }

            var result = await _userDataService.GetUserDataAsync(user.Id, cancellationToken);
            return Ok(result);
        }

        [MixAuthorize]
        [HttpPut("update-profile")]
        public async Task<ActionResult<MixUserDataViewModel>> UpdateProfile([FromBody] MixUserDataViewModel profile, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user?.Id != profile.ParentId)
            {
                return BadRequest();
            }
            if (!string.IsNullOrEmpty(profile.Email))
            {
                user.Email = profile.Email;
                await _userManager.UpdateAsync(user);
            }
            await base.UpdateHandler(profile.Id, profile, cancellationToken);
            return Ok();
        }

        [MixAuthorize]
        [HttpPost("add-address")]
        public async Task<ActionResult<MixUserDataViewModel>> AddUserAddress([FromBody] CreateUserAddressDto dto, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return BadRequest();
            }

            var result = await _userDataService.CreateUserAddress(dto, user, cancellationToken);
            return Ok(result);
        }

        [MixAuthorize]
        [HttpPut("update-address")]
        public async Task<ActionResult<MixUserDataViewModel>> UpdateUserAddress([FromBody] MixContactAddressViewModel address, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return BadRequest();
            }

            await _userDataService.UpdateUserAddress(address, user, cancellationToken);
            return Ok();
        }

        [MixAuthorize]
        [HttpDelete("delete-address/{addressId}")]
        public async Task<ActionResult> DeleteUserAddress(int addressId, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return BadRequest();
            }

            await _userDataService.DeleteUserAddress(addressId, user, cancellationToken);
            return Ok();
        }
        #endregion

        #region Overrides

        #endregion
    }
}

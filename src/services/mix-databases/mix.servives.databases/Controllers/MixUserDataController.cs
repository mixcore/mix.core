using Microsoft.AspNetCore.Mvc;
using Mix.Mixdb.ViewModels;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Attributes;
using Mix.Lib.Base;
using Mix.Lib.Services;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Service.Services;
using Mix.Services.Databases.Lib.Dtos;
using Mix.Services.Databases.Lib.Interfaces;

namespace Mix.Services.Databases.Controllers
{
    [Route("api/v2/rest/mix-services/user-data")]
    public sealed class MixUserDataController :
        MixRestHandlerApiControllerBase<MixUserDataViewModel, MixDbDbContext, MixUserData, int>
    {
        private readonly TenantUserManager _userManager;
        private readonly IMixUserDataService _userDataService;
        public MixUserDataController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixDbDbContext> uow, 
            IQueueService<MessageQueueModel> queueService, 
            IMixUserDataService metadataService, 
            TenantUserManager userManager)
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, uow, queueService)
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
            var result = await _userDataService.GetUserDataAsync(user.Id, cancellationToken);
            return Ok(result);
        }
        
        [MixAuthorize]
        [HttpPut("update-profile")]
        public async Task<ActionResult<MixUserDataViewModel>> UpdateProfile([FromBody] MixUserDataViewModel profile, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user.Id != profile.ParentId)
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
            var result = await _userDataService.CreateUserAddress(dto, user, cancellationToken);
            return Ok(result);
        }

        [MixAuthorize]
        [HttpPut("update-address")]
        public async Task<ActionResult<MixUserDataViewModel>> UpdateUserAddress([FromBody] MixContactAddressViewModel address, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.GetUserAsync(User);
            await _userDataService.UpdateUserAddress(address, user, cancellationToken);
            return Ok();
        }

        [MixAuthorize]
        [HttpDelete("delete-address/{addressId}")]
        public async Task<ActionResult> DeleteUserAddress(int addressId, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.GetUserAsync(User);
            await _userDataService.DeleteUserAddress(addressId, user, cancellationToken);
            return Ok();
        }
        #endregion

        #region Overrides

        #endregion
    }
}

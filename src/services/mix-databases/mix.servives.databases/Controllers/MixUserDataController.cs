using Microsoft.AspNetCore.Mvc;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Attributes;
using Mix.Lib.Base;
using Mix.Lib.Services;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.Service.Services;
using Mix.Services.Databases.Lib.Dtos;
using Mix.Services.Databases.Lib.Entities;
using Mix.Services.Databases.Lib.Services;
using Mix.Services.Databases.Lib.ViewModels;

namespace Mix.Services.Permission.Controllers
{
    [Route("api/v2/rest/mix-services/userdata")]
    public sealed class MixUserDataController :
        MixRestHandlerApiControllerBase<MixUserDataViewModel, MixServiceDatabaseDbContext, MixUserData, int>
    {
        private readonly TenantUserManager _userManager;
        private readonly MixUserDataService _userDataService;
        public MixUserDataController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixServiceDatabaseDbContext> uow, IQueueService<MessageQueueModel> queueService, MixUserDataService metadataService, TenantUserManager userManager)
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, uow, queueService)
        {
            _userDataService = metadataService;
            _userManager = userManager;
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
        [HttpPost("add-address")]
        public async Task<ActionResult<MixUserDataViewModel>> AddUserAddress(CreateUserAddressDto dto, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _userDataService.CreateUserAddress(dto, user, cancellationToken);
            return Ok(result);
        }
        
        [MixAuthorize]
        [HttpPut("update-address")]
        public async Task<ActionResult<MixUserDataViewModel>> UpdateUserAddress(MixContactAddressViewModel address, CancellationToken cancellationToken = default)
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

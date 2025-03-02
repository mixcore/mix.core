using Microsoft.AspNetCore.Mvc;
using Mix.Mixdb.ViewModels;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Attributes;
using Mix.Lib.Base;
using Mix.Lib.Services;
using Mix.Queue.Interfaces;
using Mix.Service.Services;
using Mix.Services.Databases.Lib.Dtos;
using Mix.Services.Databases.Lib.Interfaces;
using Mix.Heart.Services;
using Mix.Database.Entities.Cms;
using Mix.Database.Entities.MixDb;
using Mix.SignalR.Interfaces;
using Mix.Lib.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Mix.Mq.Lib.Models;

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
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixDbDbContext> uow,
            IMemoryQueueService<MessageQueueModel> queueService,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService)
            : base(httpContextAccessor, configuration,
                  cacheService, mixIdentityService, uow, queueService, portalHub, mixTenantService)
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
        #endregion

        #region Overrides

        #endregion
    }
}

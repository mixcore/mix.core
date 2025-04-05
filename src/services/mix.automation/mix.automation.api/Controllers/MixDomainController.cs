using Microsoft.AspNetCore.Mvc;
using Mix.Auth.Constants;
using Mix.Automation.Lib.Entities;
using Mix.Automation.Lib.ViewModels;
using Mix.Heart.Services;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Attributes;
using Mix.Lib.Base;
using Mix.Lib.Interfaces;
using Mix.Lib.Services;
using Mix.Mq.Lib.Models;
using Mix.Queue.Interfaces;
using Mix.SignalR.Interfaces;

namespace Mix.Automation.Api.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-workflow-trigger")]
    [ApiController]
    [MixAuthorize(MixRoles.Owner)]
    public class WorkflowTriggerController
        : MixRestfulApiControllerBase<WorkflowTriggerViewModel, WorkflowDbContext, WorkflowTrigger, int>
    {
        public WorkflowTriggerController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            MixIdentityService mixIdentityService, UnitOfWorkInfo<WorkflowDbContext> uow, IMemoryQueueService<MessageQueueModel> queueService,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService)
            : base(httpContextAccessor, configuration,
                  cacheService, mixIdentityService, uow, queueService, portalHub, mixTenantService)
        {

        }

        #region Overrides


        #endregion
    }
}

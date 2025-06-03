using Microsoft.AspNetCore.Mvc;
using Mix.Auth.Constants;
using Mix.Automation.Lib.Entities;
using Mix.Automation.Lib.Models;
using Mix.Automation.Lib.Services;
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
    //[MixAuthorize(MixRoles.Owner)]
    public class WorkflowTriggerController
        : MixRestfulApiControllerBase<WorkflowTriggerViewModel, WorkflowDbContext, WorkflowTrigger, int>
    {
        private WorkflowService _workflowService;
        public WorkflowTriggerController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            MixIdentityService mixIdentityService, UnitOfWorkInfo<WorkflowDbContext> uow, IMemoryQueueService<MessageQueueModel> queueService,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService, WorkflowService workflowService)
            : base(httpContextAccessor, configuration,
                  cacheService, mixIdentityService, uow, queueService, portalHub, mixTenantService)
        {
            _workflowService = workflowService;
        }

        #region Overrides

        [HttpPost("trigger")]
        public virtual async Task<ActionResult> Trigger([FromBody] CreateWorkflowTriggerModel dto, CancellationToken cancellationToken = default)
        {
            await _workflowService.CreateTrigger(dto, cancellationToken);
            return Ok();
        }

        #endregion
    }
}

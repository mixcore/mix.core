using Microsoft.AspNetCore.Mvc;
using Mix.Database.Entities.Queue;
using Mix.Heart.Services;
using Mix.Heart.UnitOfWork;
using Mix.Identity.Constants;
using Mix.Lib.Attributes;
using Mix.Lib.Base;
using Mix.Lib.Interfaces;
using Mix.Lib.Services;
using Mix.Lib.ViewModels;
using Mix.Queue.Interfaces;
using Mix.Queue.Models;
using Mix.SignalR.Interfaces;

namespace mix.queue.log.Controllers
{
	[Route("api/v2/rest/mix-queue-log")]
	[ApiController]
	//[MixAuthorize(MixRoles.Owner)]
	public class MixQueueLogController : MixRestfulApiControllerBase<MixQueueMessageLogViewModel, MixQueueDbContext, MixQueueMessageLog, Guid>
	{
		public MixQueueLogController(
			IHttpContextAccessor httpContextAccessor,
			IConfiguration configuration,
			MixCacheService cacheService,
			TranslatorService translator,
			MixIdentityService mixIdentityService,
			UnitOfWorkInfo<MixQueueDbContext> uow,
			IQueueService<MessageQueueModel> queueService,
			IPortalHubClientService portalHub,
			IMixTenantService mixTenantService)
			: base(
				httpContextAccessor,
				configuration,
				cacheService,
				translator,
				mixIdentityService,
				uow,
				queueService,
				portalHub,
				mixTenantService)
		{
		}
	}
}

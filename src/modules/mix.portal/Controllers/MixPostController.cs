﻿using Microsoft.AspNetCore.Mvc;
using Mix.Lib.Interfaces;
using Mix.Mq.Lib.Models;
using Mix.SignalR.Interfaces;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-post")]
    [ApiController]
    [MixAuthorize(MixRoles.Owner)]
    public class MixPostController : MixRestfulApiControllerBase<MixPostContentViewModel, MixCmsContext, MixPostContent, int>
    {
        public MixPostController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> uow,
            IMemoryQueueService<MessageQueueModel> queueService,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService)
            : base(httpContextAccessor, configuration, cacheService, translator, mixIdentityService, uow, queueService, portalHub, mixTenantService)
        {
        }
    }
}

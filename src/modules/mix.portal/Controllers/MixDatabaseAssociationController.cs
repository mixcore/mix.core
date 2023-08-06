using Microsoft.AspNetCore.Mvc;
using Mix.Lib.Interfaces;
using Mix.SignalR.Interfaces;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-db-association")]
    [ApiController]
    [MixAuthorize(MixRoles.Owner)]
    public class MixDatabaseAssociationController
        : MixRestfulApiControllerBase<MixDatabaseAssociationViewModel, MixCmsContext, MixDatabaseAssociation, Guid>
    {
        public MixDatabaseAssociationController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixCacheService cacheService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> uow,
            IQueueService<MessageQueueModel> queueService,
            IPortalHubClientService portalHub,
            IMixTenantService mixTenantService)
            : base(httpContextAccessor, configuration, 
                  cacheService, translator, mixIdentityService, uow, queueService, portalHub, mixTenantService)
        {

        }

        #region Routes

        [HttpDelete("{parentDbName}/{childDbName}/{parentId}/{childId}")]
        public async Task<ActionResult> DeleteAssociation(
          string parentDbName, string childDbName, string parentId, int childId)
        {
            if (int.TryParse(parentId, out int intParentId))
            {
                await Repository.DeleteAsync(
                m => m.ParentDatabaseName == parentDbName && m.ChildDatabaseName == childDbName && m.ParentId == intParentId && m.ChildId == childId);
            }

            if (Guid.TryParse(parentId, out Guid guidParentId))
            {
                await Repository.DeleteAsync(
                m => m.ParentDatabaseName == parentDbName && m.ChildDatabaseName == childDbName && m.GuidParentId == guidParentId && m.ChildId == childId);
            }

            return Ok();
        }

        [HttpGet("{parentDbName}/{childDbName}/{parentId}/{childId}")]
        public async Task<ActionResult> GetAssociation(
          string parentDbName, string childDbName, int parentId, int childId)
        {

            var result = await Repository.GetSingleAsync(
                m => m.ParentDatabaseName == parentDbName && m.ChildDatabaseName == childDbName && m.ParentId == parentId && m.ChildId == childId);
            return result != null ? Ok(result) : NotFound();
        }

        #endregion

        #region Overrides
        #endregion
    }
}

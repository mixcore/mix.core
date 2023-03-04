using Microsoft.AspNetCore.Mvc;

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
            MixService mixService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            UnitOfWorkInfo<MixCmsContext> cmsUOW,
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, cmsUOW, queueService)
        {

        }

        #region Routes

        [HttpDelete("{parentDbName}/{childDbName}/{parentId}/{childId}")]
        public async Task<ActionResult> DeleteAssociation(
          string parentDbName, string childDbName, int parentId, int childId)
        {

            await Repository.DeleteAsync(
                m => m.ParentDatabaseName == parentDbName && m.ChildDatabaseName == childDbName && m.ParentId == parentId && m.ChildId == childId);
            return Ok();
        }
        #endregion
        #region Overrides
        #endregion
    }
}

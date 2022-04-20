using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Account;
using Mix.Identity.Enums;
using Mix.Lib.Repositories;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-tenant")]
    [Route($"api/v2/rest/mix-portal/mix-tenant/{MixRequestQueryKeywords.Specificulture}")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MixTenantController
        : MixRestApiControllerBase<MixTenantViewModel, MixCmsContext, MixTenant, int>
    {
        private readonly TenantUserManager _userManager;
        private readonly MixCmsAccountContext _accContext;
        public MixTenantController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixIdentityService mixIdentityService,
            MixCmsContext context,
            IQueueService<MessageQueueModel> queueService,
            TenantUserManager userManager,
            MixCmsAccountContext accContext)
            : base(httpContextAccessor, configuration, mixService, translator, cultureRepository, mixIdentityService, context, queueService)
        {
            _userManager = userManager;
            _accContext = accContext;
        }

        #region Overrides
        protected override async Task<int> CreateHandlerAsync(MixTenantViewModel data)
        {
            data.InitDomain();
            data.CloneCulture(_culture);
            var tenantId = await base.CreateHandlerAsync(data);
            
            await _uow.CompleteAsync();
            var user = await _userManager.FindByIdAsync(_mixIdentityService.GetClaim(User, MixClaims.Id));
            await _userManager.AddToRoleAsync(user, MixRoleEnums.Owner.ToString(), tenantId);
            await _userManager.AddToTenant(user, tenantId);
            return tenantId;
        }

        protected override async Task DeleteHandler(MixTenantViewModel data)
        {
            if (data.Id == 1)
            {
                throw new MixException(MixErrorStatus.Badrequest, "Cannot delete root tenant");
            }

            await base.DeleteHandler(data);
            
            // Complete and close cms context transaction (signalr cannot open parallel context)
            await _uow.CompleteAsync();

            await DeleteTenantAccount(data.Id);
            _queueService.PushQueue(new MessageQueueModel()
            {
                Action = "Delete",
                Success = true,
                TopicId = typeof(MixTenantViewModel).FullName
            });
        }

        private async Task DeleteTenantAccount(int tenantId)
        {
            foreach (var item in _accContext.AspNetUserRoles.Where(m => m.MixTenantId == tenantId))
            {
                _accContext.Entry(item).State = EntityState.Deleted;
            }
            foreach (var item in _accContext.MixUserTenants.Where(m => m.TenantId == tenantId))
            {
                _accContext.Entry(item).State = EntityState.Deleted;
            }
            await _accContext.SaveChangesAsync();
        }
        #endregion


    }
}

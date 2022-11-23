using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Account;
using Mix.Heart.Helpers;
using Mix.Identity.Enums;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-tenant")]
    [Route($"api/v2/rest/mix-portal/mix-tenant/{MixRequestQueryKeywords.Specificulture}")]
    [ApiController]
    [MixAuthorize]
    public class MixTenantController
        : MixRestfulApiControllerBase<MixTenantViewModel, MixCmsContext, MixTenant, int>
    {
        private readonly TenantUserManager _userManager;
        private readonly MixCmsAccountContext _accContext;
        private readonly MixTenantService _mixTenantService;
        public MixTenantController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            MixIdentityService mixIdentityService,
            TenantUserManager userManager,
            MixCmsAccountContext accContext,
            UnitOfWorkInfo<MixCmsContext> cmsUow,
            IQueueService<MessageQueueModel> queueService,
            MixTenantService mixTenantService)
            : base(httpContextAccessor, configuration, mixService, translator, mixIdentityService, cmsUow, queueService)
        {
            _userManager = userManager;
            _accContext = accContext;
            _mixTenantService = mixTenantService;
        }

        #region Overrides
        protected override async Task<int> CreateHandlerAsync(MixTenantViewModel data)
        {
            data.InitDomain();
            data.CloneCulture(Culture);
            var tenantId = await base.CreateHandlerAsync(data);
            await Uow.CompleteAsync();
            var user = await _userManager.FindByIdAsync(MixIdentityService.GetClaim(User, MixClaims.Id));
            await _userManager.AddToRoleAsync(user, MixRoleEnums.Owner.ToString(), tenantId);
            await _userManager.AddToTenant(user, tenantId);

            // Save setting and reload tenant
            await ReloadTenantConfiguration(data);

            return tenantId;
        }

        private async Task ReloadTenantConfiguration(MixTenantViewModel data)
        {
            var tenantConfigService = new TenantConfigService(data.SystemName);
            tenantConfigService.AppSettings.DefaultCulture = data.Culture.Specificulture;
            tenantConfigService.AppSettings.Domain = data.PrimaryDomain.TrimEnd('/');
            tenantConfigService.AppSettings.ApiEncryptKey = AesEncryptionHelper.GenerateCombinedKeys();
            tenantConfigService.SaveSettings();
            await _mixTenantService.Reload();
        }

        protected override async Task DeleteHandler(MixTenantViewModel data)
        {
            if (data.Id == 1)
            {
                throw new MixException(MixErrorStatus.Badrequest, "Cannot delete root tenant");
            }

            await base.DeleteHandler(data);

            // Complete and close CMS context transaction (SignalR cannot open parallel context).
            await Uow.CompleteAsync();

            // Reload tenants after deleting the tenant.
            await _mixTenantService.Reload();

            await DeleteTenantAccount(data.Id);
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

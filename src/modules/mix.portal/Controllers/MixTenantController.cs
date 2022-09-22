using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Account;
using Mix.Heart.Helpers;
using Mix.Identity.Enums;
using Mix.Lib.Repositories;
using Mix.Shared.Services;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-tenant")]
    [Route($"api/v2/rest/mix-portal/mix-tenant/{MixRequestQueryKeywords.Specificulture}")]
    [ApiController]
    [MixAuthorize]
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
            TenantUserManager userManager,
            MixCmsAccountContext accContext,
            UnitOfWorkInfo<MixCacheDbContext> cacheUOW,
            UnitOfWorkInfo<MixCmsContext> cmsUOW,
            IQueueService<MessageQueueModel> queueService)
            : base(httpContextAccessor, configuration, mixService, translator, cultureRepository, mixIdentityService, cacheUOW, cmsUOW, queueService)
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

            MixTenantRepository.Instance.AllTenants = await MixTenantSystemViewModel.GetRepository(_uow).GetAllAsync(m => true);
            await ReloadTenantConfiguration(data);
            await _uow.CompleteAsync();
            var user = await _userManager.FindByIdAsync(_mixIdentityService.GetClaim(User, MixClaims.Id));
            await _userManager.AddToRoleAsync(user, MixRoleEnums.Owner.ToString(), tenantId);
            await _userManager.AddToTenant(user, tenantId);

            
            return tenantId;
        }

        private async Task ReloadTenantConfiguration(MixTenantViewModel data)
        {
            var tenantConfigService = new TenantConfigService(data.SystemName);
            tenantConfigService.AppSettings.DefaultCulture = data.Culture.Specificulture;
            tenantConfigService.AppSettings.Domain = data.PrimaryDomain.TrimEnd('/');
            tenantConfigService.AppSettings.ApiEncryptKey = AesEncryptionHelper.GenerateCombinedKeys();
            tenantConfigService.SaveSettings();
            await MixTenantRepository.Instance.Reload(_uow);
        }

        protected override async Task DeleteHandler(MixTenantViewModel data)
        {
            if (data.Id == 1)
            {
                throw new MixException(MixErrorStatus.Badrequest, "Cannot delete root tenant");
            }

            await base.DeleteHandler(data);
            MixTenantRepository.Instance.AllTenants = await MixTenantSystemViewModel.GetRepository(_uow).GetAllAsync(m => true);
            // Complete and close cms context transaction (signalr cannot open parallel context)
            await _uow.CompleteAsync();

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

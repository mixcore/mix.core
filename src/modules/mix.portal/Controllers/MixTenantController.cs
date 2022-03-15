using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Mix.Database.Entities.Account;
using Mix.Identity.Enums;

namespace Mix.Portal.Controllers
{
    [Route("api/v2/rest/mix-portal/mix-tenant")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MixTenantController
        : MixRestApiControllerBase<MixTenantViewModel, MixCmsContext, MixTenant, int>
    {
        private readonly RoleManager<MixRole> _roleManager;
        public MixTenantController(
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration,
            MixService mixService,
            TranslatorService translator,
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepository,
            MixIdentityService mixIdentityService,
            MixCmsContext context,
            IQueueService<MessageQueueModel> queueService, 
            RoleManager<MixRole> roleManager)
            : base(httpContextAccessor, configuration, mixService, translator, cultureRepository, mixIdentityService, context, queueService)
        {
            _roleManager = roleManager;
        }

        #region Overrides
        protected override async Task<int> CreateHandlerAsync(MixTenantViewModel data)
        {
            var tenantId = await base.CreateHandlerAsync(data);
            await _uow.CompleteAsync();
            var roles = MixHelper.LoadEnumValues(typeof(MixRoles));
            foreach (var role in roles)
            {
                await _roleManager.CreateAsync(new MixRole()
                {
                    Id = Guid.NewGuid(),
                    Name = role.ToString(),
                    MixTenantId = tenantId
                }
                );
            }
            return tenantId;
        }
        #endregion


    }
}

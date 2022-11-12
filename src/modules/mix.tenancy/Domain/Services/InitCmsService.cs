using Microsoft.AspNetCore.Identity;
using Mix.Database.Entities.Account;
using Mix.Database.Services;
using Mix.Lib.Services;

namespace Mix.Tenancy.Domain.Services
{
    public partial class InitCmsService
    {
        private readonly MixTenantService _mixTenantService;
        private readonly MixIdentityService _identityService;
        private readonly TenantUserManager _userManager;
        private readonly RoleManager<MixRole> _roleManager;
        private readonly DatabaseService _databaseService;
        private readonly IConfiguration _configuration;
        private readonly MixCmsContext _context;
        private readonly MixDataService _mixDataService;
        private readonly UnitOfWorkInfo<MixCmsContext> _cmsUow;
        private readonly MixConfigurationService _configsService;
        public InitCmsService(
            IHttpContextAccessor httpContextAccessor,
            TenantUserManager userManager,
            MixIdentityService identityService,
            DatabaseService databaseService,
            RoleManager<MixRole> roleManager,
            IConfiguration configuration,
            UnitOfWorkInfo<MixCmsContext> cmsUow, MixDataService mixDataService, MixTenantService mixTenantService, MixConfigurationService configsService)
        {
            _configuration = configuration;
            _userManager = userManager;
            _identityService = identityService;
            _roleManager = roleManager;
            _mixDataService = mixDataService;
            _cmsUow = cmsUow;
            _context = cmsUow.DbContext;
            _databaseService = databaseService;
            _mixDataService.SetUnitOfWork(_cmsUow);
            _mixTenantService = mixTenantService;
            _configsService = configsService;
        }

    }
}
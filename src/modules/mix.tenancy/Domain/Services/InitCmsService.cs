using Microsoft.AspNetCore.Identity;
using Mix.Database.Entities.Account;
using Mix.Database.Services;
using Mix.Lib.Services;

namespace Mix.Tenancy.Domain.Services
{
    public partial class InitCmsService
    {
        private readonly MixIdentityService _identityService;
        private readonly TenantUserManager _userManager;
        private readonly RoleManager<MixRole> _roleManager;
        private readonly DatabaseService _databaseService;
        private readonly IConfiguration _configuration;
        private readonly MixCmsContext _context;
        private readonly MixDataService _mixDataService;
        private readonly UnitOfWorkInfo _cmsUow;
        public readonly int tenantId = 1;
        public InitCmsService(
            IHttpContextAccessor httpContextAccessor,
            TenantUserManager userManager,
            MixIdentityService identityService,
            DatabaseService databaseService,
            RoleManager<MixRole> roleManager,
            IConfiguration configuration,
            MixCmsContext context, MixDataService mixDataService)
        {
            _configuration = configuration;
            _userManager = userManager;
            _identityService = identityService;
            _roleManager = roleManager;
            _context = context;
            _mixDataService = mixDataService;
            _cmsUow = new(context);
            _databaseService = databaseService;
            _mixDataService.SetUnitOfWork(_cmsUow);
            if (httpContextAccessor.HttpContext != null && httpContextAccessor.HttpContext.Session.GetInt32(MixRequestQueryKeywords.TenantId).HasValue)
            {
                tenantId = httpContextAccessor.HttpContext.Session.GetInt32(MixRequestQueryKeywords.TenantId).Value;
            }
        }

    }
}
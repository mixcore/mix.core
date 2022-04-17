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
        private readonly MixDatabaseService _databaseService;
        private readonly IConfiguration _configuration;
        private readonly MixCmsContext _context;
        private readonly UnitOfWorkInfo _cmsUow;
        public readonly int tenantId = 1;
        public InitCmsService(
            IHttpContextAccessor httpContextAccessor,
            TenantUserManager userManager,
            MixIdentityService identityService,
            MixDatabaseService databaseService,
            RoleManager<MixRole> roleManager,
            IConfiguration configuration,
            MixCmsContext context)
        {
            _configuration = configuration;
            _userManager = userManager;
            _identityService = identityService;
            _roleManager = roleManager;
            _context = context;
            _cmsUow = new(context);
            _databaseService = databaseService;
            if (httpContextAccessor.HttpContext != null && httpContextAccessor.HttpContext.Session.GetInt32(MixRequestQueryKeywords.MixTenantId).HasValue)
            {
                tenantId = httpContextAccessor.HttpContext.Session.GetInt32(MixRequestQueryKeywords.MixTenantId).Value;
            }
        }

    }
}
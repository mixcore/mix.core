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
        public readonly int tenantId;
        public InitCmsService(
             IHttpContextAccessor httpContext,
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
            _databaseService = databaseService;
            if (httpContext.HttpContext.Session.GetInt32(MixRequestQueryKeywords.MixTenantId).HasValue)
            {
                tenantId = httpContext.HttpContext.Session.GetInt32(MixRequestQueryKeywords.MixTenantId).Value;
            }
        }

    }
}
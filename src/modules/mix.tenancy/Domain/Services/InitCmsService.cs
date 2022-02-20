using Microsoft.AspNetCore.Identity;
using Mix.Database.Entities.Account;
using Mix.Database.Services;
using Mix.Lib.Services;

namespace Mix.Tenancy.Domain.Services
{
    public partial class InitCmsService
    {
        private readonly MixIdentityService _identityService;
        private readonly UserManager<MixUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly MixDatabaseService _databaseService;
        private readonly IConfiguration _configuration;
        private readonly MixCmsContext _context;
        public InitCmsService(

            UserManager<MixUser> userManager,
            MixIdentityService identityService,
            MixDatabaseService databaseService,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            MixCmsContext context)
        {
            _configuration = configuration;
            _userManager = userManager;
            _identityService = identityService;
            _databaseService = databaseService;
            _roleManager = roleManager;
            _context = context;
        }

    }
}
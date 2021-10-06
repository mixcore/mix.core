using Mix.Database.Entities.Cms;
using Mix.Heart.Repository;
using Microsoft.AspNetCore.Identity;
using Mix.Database.Entities.Account;
using Mix.Shared.Services;
using Mix.Database.Services;
using Mix.Lib.Services;

namespace Mix.Theme.Domain.Services
{
    public partial class InitCmsService
    {
        private readonly MixIdentityService _identityService;
        private readonly UserManager<MixUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly EntityRepository<MixCmsContext, MixCulture, int> _cultureRepo;
        private readonly EntityRepository<MixCmsContext, MixTenant, int> _siteRepo;
        private readonly GlobalConfigService _globalConfigService;
        private readonly MixDatabaseService _databaseService;
        public InitCmsService(
            EntityRepository<MixCmsContext, MixCulture, int> cultureRepo,
            UserManager<MixUser> userManager,
            GlobalConfigService globalConfigService,
            MixIdentityService identityService,
            MixDatabaseService databaseService,
            RoleManager<IdentityRole> roleManager,
            EntityRepository<MixCmsContext, MixTenant, int> siteRepo)
        {
            _cultureRepo = cultureRepo;
            _userManager = userManager;
            _globalConfigService = globalConfigService;
            _identityService = identityService;
            _databaseService = databaseService;
            _roleManager = roleManager;
            _siteRepo = siteRepo;
        }

    }
}
using Mix.Database.Entities.Cms.v2;
using Mix.Heart.Repository;
using Microsoft.AspNetCore.Identity;
using Mix.Identity.Services;
using Mix.Database.Entities.Account;
using Mix.Shared.Services;
using Mix.Database.Services;

namespace Mix.Theme.Domain.Services
{
    public partial class InitCmsService
    {
        private readonly MixIdentityService _identityService;
        private readonly UserManager<MixUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly Repository<MixCmsContext, MixCulture, int> _cultureRepo;
        private readonly Repository<MixCmsContext, MixSite, int> _siteRepo;
        private readonly MixAppSettingService _appSettingService;
        private readonly MixDatabaseService _databaseService;
        public InitCmsService(
            Repository<MixCmsContext, MixCulture, int> cultureRepo,
            UserManager<MixUser> userManager,
            MixAppSettingService appSettingService,
            MixIdentityService identityService,
            MixDatabaseService databaseService,
            RoleManager<IdentityRole> roleManager, 
            Repository<MixCmsContext, MixSite, int> siteRepo)
        {
            _cultureRepo = cultureRepo;
            _userManager = userManager;
            _appSettingService = appSettingService;
            _identityService = identityService;
            _databaseService = databaseService;
            _roleManager = roleManager;
            _siteRepo = siteRepo;
        }

    }
}
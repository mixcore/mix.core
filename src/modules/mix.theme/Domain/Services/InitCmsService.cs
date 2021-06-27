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
        private readonly MixIdentityService _idHelper;
        private readonly UserManager<MixUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly CommandRepository<MixCmsContext, MixCulture, int> _cultureRepo;
        private readonly MixAppSettingService _appSettingService;
        private readonly MixDatabaseService _databaseService;
        public InitCmsService(
            CommandRepository<MixCmsContext, MixCulture, int> cultureRepo,
            UserManager<MixUser> userManager,
            MixAppSettingService appSettingService,
            MixIdentityService idHelper,
            MixDatabaseService databaseService, 
            RoleManager<IdentityRole> roleManager)
        {
            _cultureRepo = cultureRepo;
            _userManager = userManager;
            _appSettingService = appSettingService;
            _idHelper = idHelper;
            _databaseService = databaseService;
            _roleManager = roleManager;
        }

    }
}
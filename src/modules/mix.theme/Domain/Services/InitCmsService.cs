using Mix.Database.Entities.Cms.v2;
using Mix.Heart.Repository;
using Microsoft.AspNetCore.Identity;
using Mix.Identity.Services;
using Mix.Database.Entities.Account;

namespace Mix.Theme.Domain.Services
{
    public partial class InitCmsService
    {
        private readonly MixIdentityService _idHelper;
        private readonly UserManager<MixUser> _userManager;
        private readonly CommandRepository<MixCmsContext, MixSite, int> siteRepo;
        private readonly CommandRepository<MixCmsContext, MixCulture, int> _cultureRepo;

        public InitCmsService(
            CommandRepository<MixCmsContext, MixSite, int> siteRepo,
            CommandRepository<MixCmsContext, MixCulture, int> cultureRepo, 
            UserManager<MixUser> userManager)
        {
            this.siteRepo = siteRepo;
            this._cultureRepo = cultureRepo;
            _userManager = userManager;
        }
        
    }
}
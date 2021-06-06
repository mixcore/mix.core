using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Mix.Database.Services;
using Mix.Database.Entities.Cms.v2;
using Mix.Theme.Domain.Dtos;
using Mix.Theme.Domain.ViewModels.Init;
using Mix.Heart.Repository;
using Mix.Heart.Extensions;

namespace Mix.Lib.Services
{
    public class InitCmsService
    {
        private readonly CommandRepository<MixCmsContext, MixSite, int> siteRepo;
        private readonly CommandRepository<MixCmsContext, MixCulture, int> _cultureRepo;

        public InitCmsService(
            CommandRepository<MixCmsContext, MixSite, int> siteRepo,
            CommandRepository<MixCmsContext, MixCulture, int> cultureRepo)
        {
            this.siteRepo = siteRepo;
            this._cultureRepo = cultureRepo;
        }

        public async Task InitSiteAsync(InitCmsDto model)
        {
            MixDatabaseService.Instance.InitMixCmsContext(
                model.ConnectionString,
                model.DatabaseProvider,
                model.Culture.Specificulture);

            var dbContext = MixDatabaseService.Instance.GetDbContext();
            dbContext.Database.Migrate();

            InitSiteViewModel vm = new(dbContext, siteRepo, _cultureRepo, model)
            {
                Id = 1,
                DisplayName = model.SiteName,
                SystemName = model.SiteName.ToSEOString('_'),
                Description = model.SiteName,
                CreatedDateTime = DateTime.UtcNow
            };
            await vm.SaveAsync();
        }
    }
}
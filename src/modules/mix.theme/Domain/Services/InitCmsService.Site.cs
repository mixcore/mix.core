using Microsoft.EntityFrameworkCore;
using Mix.Database.Services;
using Mix.Theme.Domain.Dtos;
using Mix.Theme.Domain.ViewModels.Init;
using System;
using System.Threading.Tasks;
using Mix.Heart.Extensions;

namespace Mix.Theme.Domain.Services
{
    public partial class InitCmsService
    {
        public async Task InitSiteAsync(InitCmsDto model)
        {
            _databaseService.InitMixCmsContext(
                model.ConnectionString,
                model.DatabaseProvider,
                model.Culture.Specificulture);

            var dbContext = _databaseService.GetDbContext();
            dbContext.Database.Migrate();

            InitSiteViewModel vm = new(model, _siteRepo)
            {
                Id = 1,
                DisplayName = model.SiteName,
                SystemName = model.SiteName.ToSEOString('_'),
                Description = model.SiteName,
                CreatedDateTime = DateTime.UtcNow,

                Culture = new InitCultureViewModel(_cultureRepo)
                {
                    Id = 1,
                    Specificulture = model.Culture.Specificulture,
                    Alias = model.Culture.Alias,
                    Icon = model.Culture.Icon,
                    DisplayName = model.Culture.FullName,
                    SystemName = model.Culture.Specificulture,
                    Description = model.SiteName,
                    CreatedDateTime = DateTime.UtcNow
                }
            };

            await vm.SaveAsync();
        }
    }
}

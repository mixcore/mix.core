using Microsoft.EntityFrameworkCore;
using Mix.Theme.Domain.Dtos;
using Mix.Theme.Domain.ViewModels.Init;
using System;
using System.Threading.Tasks;
using Mix.Heart.Extensions;
using Mix.Heart.Enums;

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

            InitSiteViewModel vm = new(model);
            await vm.SaveAsync();
        }
    }
}

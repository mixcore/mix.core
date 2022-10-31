
using Mix.Lib.Services;
using Mix.Tenancy.Domain.Dtos;
using Mix.Tenancy.Domain.ViewModels.Init;

namespace Mix.Tenancy.Domain.Services
{
    public partial class InitCmsService
    {
        public async Task InitDbContext(InitCmsDto model)
        {
            _databaseService.InitConnectionStrings(
                model.ConnectionString,
                model.DatabaseProvider,
                model.Culture.Specificulture);

            await _databaseService.UpdateMixCmsContextAsync();
        }

        public async Task InitTenantAsync(InitCmsDto model)
        {
            InitTenantViewModel vm = new(_context, model);
            await vm.SaveAsync();
            await _mixTenantService.Reload(_cmsUow);
            GlobalConfigService.Instance.AppSettings.InitStatus = InitStep.InitTenant;
            GlobalConfigService.Instance.SaveSettings();
           
        }
    }
}

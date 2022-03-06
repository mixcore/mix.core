using Mix.Shared.Enums;
using Mix.Tenancy.Domain.Dtos;
using Mix.Tenancy.Domain.ViewModels.Init;

namespace Mix.Tenancy.Domain.Services
{
    public partial class InitCmsService
    {
        public async Task InitTenantAsync(InitCmsDto model)
        {
            _databaseService.InitMixCmsContext(
                model.ConnectionString,
                model.DatabaseProvider,
                model.Culture.Specificulture);

            _databaseService.InitMixCmsContext();

            InitTenantViewModel vm = new(_context);
            vm.InitSiteData(model);
            await vm.SaveAsync();
            GlobalConfigService.Instance.AppSettings.ApiEncryptKey = AesEncryptionHelper.GenerateCombinedKeys(128);
            GlobalConfigService.Instance.AppSettings.DefaultCulture = model.Culture.Specificulture;
            GlobalConfigService.Instance.AppSettings.InitStatus = InitStep.InitTenant;
            GlobalConfigService.Instance.SaveSettings();
        }
    }
}

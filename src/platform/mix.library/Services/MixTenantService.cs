using Mix.Database.Services;

namespace Mix.Lib.Services
{
    public sealed class MixTenantService
    {

        public List<MixTenantSystemViewModel> AllTenants { get; set; }
        private readonly DatabaseService _databaseService;

        public MixTenantService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task Reload(UnitOfWorkInfo uow = null)
        {
            if (GlobalConfigService.Instance.InitStatus != InitStep.Blank)
            {
                if (uow != null)
                {
                    AllTenants = await MixTenantSystemViewModel.GetRepository(uow).GetAllAsync(m => true);
                }
                else
                {
                    uow = new(new MixCmsContext(_databaseService));
                    AllTenants = await MixTenantSystemViewModel.GetRepository(uow).GetAllAsync(m => true);
                    await uow.DisposeAsync();
                }
            }
        }
        public MixTenantSystemViewModel GetCurrentTenant(string host)
        {
            return AllTenants.FirstOrDefault(m => m.Domains.Any(d => d.Host == host)) ?? AllTenants.First();
        }
    }
}

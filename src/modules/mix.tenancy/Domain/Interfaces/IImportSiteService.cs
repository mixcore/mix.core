using Mix.Tenancy.Domain.ViewModels;

namespace Mix.Tenancy.Domain.Interfaces
{
    public interface IImportSiteService
    {
        public Task ImportAsync(TenantDataViewModel data, string destCulture);
    }
}

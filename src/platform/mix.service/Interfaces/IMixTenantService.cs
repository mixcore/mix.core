using Mix.Database.Entities.Cms;
using Mix.Service.Models;

namespace Mix.Lib.Interfaces
{
    public interface IMixTenantService
    {
        public List<MixTenantSystemModel> AllTenants { get; set; }

        public List<MixCulture> AllCultures { get; set; }

        public Task Reload(CancellationToken cancellationToken = default);

        public MixTenantSystemModel GetTenant(string host);
    }
}

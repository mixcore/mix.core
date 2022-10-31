using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Lib.Services
{
    public sealed class MixTenantService
    {

        public List<MixTenantSystemViewModel> AllTenants { get; set; }
        private IHttpContextAccessor _httpContextAccessor;

        public MixTenantService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
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
                    uow = new(new MixCmsContext(_httpContextAccessor));
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

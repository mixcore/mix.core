using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mix.Lib.Repositories;
using System.Text;

namespace Mix.Lib.Middlewares
{
    public class TenantSecurityMiddleware
    {
        private readonly RequestDelegate next;

        public TenantSecurityMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(
            HttpContext context,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            EntityRepository<MixCmsContext, MixTenant, int> repository)
        {
            if (GlobalConfigService.Instance.IsInit)
            {
                await next.Invoke(context);
            }
            else
            {
                if (MixTenantRepository.Instance.AllTenants == null)
                {
                    MixTenantRepository.Instance.AllTenants = await repository.GetAllQuery().ToListAsync();
                }
                MixTenantRepository.Instance.LoadCurrentTenant(context.Request.Headers.Host);
                await next.Invoke(context);
            }
        }
    }
}

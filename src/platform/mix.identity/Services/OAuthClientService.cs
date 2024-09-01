using Microsoft.Extensions.DependencyInjection;
using Mix.Database.Entities.Account;
using Mix.Identity.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Identity.Services
{
    public sealed class OAuthClientService : IOAuthClientService
    {
        private readonly IServiceProvider _serviceProvider;

        public List<OAuthClient> Clients { get; set; }
        public OAuthClientService(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;

            using (var scope = _serviceProvider.CreateScope())
            {
                var accContext = scope.ServiceProvider.GetService<MixCmsAccountContext>();
                LoadClients(accContext);
            }
        }

        public List<OAuthClient> LoadClients(MixCmsAccountContext accContext, bool isReload = false)
        {
            if (isReload || Clients == null)
            {
                Clients = accContext.OAuthClient.ToList();
                accContext.Dispose();
            }
            return Clients;
        }
    }
}

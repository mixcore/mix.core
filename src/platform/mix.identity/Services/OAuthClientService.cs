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
            LoadClients();
        }

        public List<OAuthClient> LoadClients(bool isReload = false)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _accContext = scope.ServiceProvider.GetService<MixCmsAccountContext>();
                if (isReload || Clients == null)
                {
                    Clients = _accContext.OAuthClient.ToList();
                }
                _accContext.Dispose();
                return Clients;
            }
        }
    }
}

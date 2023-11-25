using Mix.Database.Entities.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Identity.Services
{
    public sealed class OAuthClientService
    {
        private readonly MixCmsAccountContext _accContext;

        public List<OAuthClient> Clients { get; set; }
        public OAuthClientService(MixCmsAccountContext accContext)
        {
            _accContext = accContext;
        }

        public List<OAuthClient> GetClients(bool isReload = false)
        {
            if (isReload || Clients == null)
            {
                Clients = _accContext.OAuthClient.ToList();
            }
            return Clients;
        }
    }
}

using Mix.Database.Entities.Account;
using System.Collections.Generic;

namespace Mix.Identity.Interfaces
{
    public interface IOAuthClientService
    {
        List<OAuthClient> Clients { get; set; }

        List<OAuthClient> GetClients(bool isReload = false);
    }
}
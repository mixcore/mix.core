using Mix.Database.Entities.Account;
using Mix.Identity.ViewModels;
using System.Collections.Generic;

namespace Mix.Identity.Interfaces
{
    public interface IOAuthClientService
    {
        List<OAuthClient> Clients { get; set; }

        List<OAuthClient> LoadClients(bool isReload = false);

        List<OAuthClient> AddClients(OAuthClientViewModel data);
    }
}
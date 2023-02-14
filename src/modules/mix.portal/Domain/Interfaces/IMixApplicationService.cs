using Microsoft.AspNetCore.SignalR;

namespace Mix.Portal.Domain.Interfaces
{
    public interface IMixApplicationService
    {
        public Task<MixApplicationViewModel> Install(MixApplicationViewModel app);

        public Task AlertAsync<T>(IClientProxy clients, string action, int status, T message);
    }
}

using Microsoft.AspNetCore.SignalR;

namespace Mix.Portal.Domain.Interfaces
{
    public interface IMixApplicationService
    {
        public Task<MixApplicationViewModel> Install(MixApplicationViewModel app, CancellationToken cancellationToken = default);
        public Task<MixApplicationViewModel> UpdatePackage(MixApplicationViewModel app, string pakageFilePath, CancellationToken cancellationToken = default);

        public Task AlertAsync<T>(IClientProxy clients, string action, int status, T message);
    }
}

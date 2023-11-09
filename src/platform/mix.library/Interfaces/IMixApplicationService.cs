using Microsoft.AspNetCore.SignalR;
using Mix.Lib.Dtos;

namespace Mix.Lib.Interfaces
{
    public interface IMixApplicationService
    {
        public Task<MixApplicationViewModel> Install(MixApplicationViewModel app, CancellationToken cancellationToken = default);
        public Task<MixApplicationViewModel> UpdatePackage(MixApplicationViewModel app, string pakageFilePath, CancellationToken cancellationToken = default);

        public Task AlertAsync<T>(IClientProxy clients, string action, int status, T message);
        Task<MixApplicationViewModel> RestorePackage(RestoreMixApplicationPackageDto dto, CancellationToken cancellationToken);
    }
}

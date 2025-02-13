using Mix.Database.Entities.Account;
using Mix.Mixdb.ViewModels;
using Mix.Services.Databases.Lib.Dtos;

namespace Mix.Services.Databases.Lib.Interfaces
{
    public interface IMixUserDataService
    {
        public Task<MixUserDataViewModel> GetUserDataAsync(Guid userId, CancellationToken cancellationToken = default);

        public Task<MixUserDataViewModel> CreateDefaultUserData(Guid userId, CancellationToken cancellationToken = default);

        public Task UpdateProfile(MixUserDataViewModel profile, CancellationToken cancellationToken = default);
    }
}

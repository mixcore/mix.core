using Mix.Database.Entities.Account;
using Mix.Mixdb.ViewModels;
using Mix.Services.Databases.Lib.Dtos;

namespace Mix.Services.Databases.Lib.Interfaces
{
    public interface IMixUserDataService
    {
        public Task<MixUserDataViewModel> GetUserDataAsync(Guid userId, CancellationToken cancellationToken = default);

        public Task<MixUserDataViewModel> CreateDefaultUserData(Guid userId, CancellationToken cancellationToken = default);

        public Task DeleteUserAddress(int addressId, MixUser user, CancellationToken cancellationToken = default);

        public Task<MixUserDataViewModel> CreateUserAddress(CreateUserAddressDto dto, MixUser user, CancellationToken cancellationToken = default);

        public Task UpdateUserAddress(MixContactAddressViewModel address, MixUser user, CancellationToken cancellationToken);

        public Task UpdateProfile(MixUserDataViewModel profile, CancellationToken cancellationToken = default);
    }
}

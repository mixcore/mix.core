using Mix.Mixdb.ViewModels;
using Mix.Services.Databases.Lib.Dtos;

namespace Mix.Services.Databases.Lib.Interfaces
{
    public interface IMixPermissionService
    {
        public Task<List<MixPermissionViewModel>> GetPermissionAsync(Guid userId);

        public Task AddUserPermission(CreateUserPermissionDto dto);
    }
}

using Mix.Database.Entities.Account;
using Mix.Identity.Dtos;
using Mix.Mixdb.ViewModels;
using Mix.Services.Databases.Lib.Dtos;

namespace Mix.Services.Databases.Lib.Interfaces
{
    public interface IMixPermissionService
    {
        public Task<List<MixPermissionViewModel>> GetPermissionAsync(Guid userId);

        public Task AddUserPermission(CreateUserPermissionDto dto);
        Task<List<Database.Entities.Account.MixPermission>> GetPermissionByRoleId(Guid roleId);
        Task GrantPermissions(GrantPermissionsDto dto);
    }
}

using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Account;
using Mix.Identity.Models.AccountViewModels;
using Mix.Identity.Models.ManageViewModels;
using System.Text.Json.Serialization;

namespace Mix.Lib.ViewModels
{
    public class MixUserViewModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? LockoutEnd { get; set; }
        public bool IsActived { get; set; }
        public System.DateTime? LastModified { get; set; }
        public string ModifiedBy { get; set; }

        public string PhoneNumber { get; set; }

        public FileModel MediaFile { get; set; } = new();

        public AdditionalDataContentViewModel UserData { get; set; }

        public List<AspNetUserRoles> Roles { get; set; }

        #region Change Password

        public ResetPasswordViewModel ResetPassword { get; set; }

        public bool IsChangePassword { get; set; }

        public ChangePasswordViewModel ChangePassword { get; set; }
        [JsonIgnore]
        private UnitOfWorkInfo _cmsUow { get; }

        #endregion Change Password


        public MixUserViewModel(MixUser user, UnitOfWorkInfo uow)
        {
            ReflectionHelper.Mapping(user, this);
            _cmsUow = uow;
        }

        public async Task LoadUserDataAsync(int tenantId)
        {
            UserData ??= await MixDataHelper.GetAdditionalDataAsync(
                _cmsUow,
                MixDatabaseParentType.User,
                MixDatabaseNames.SYSTEM_USER_DATA,
                Id);
            using var context = new MixCmsAccountContext();
            var roles = from ur in context.AspNetUserRoles
                        join r in context.MixRoles
                        on ur.RoleId equals r.Id
                        where ur.UserId == Id && ur.MixTenantId == tenantId
                        select ur;
            Roles = await roles.ToListAsync();

        }
    }
}

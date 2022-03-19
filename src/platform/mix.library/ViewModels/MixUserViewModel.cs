using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Account;
using Mix.Identity.Models.AccountViewModels;
using Mix.Identity.Models.ManageViewModels;
namespace Mix.Lib.ViewModels
{
    public class MixUserViewModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime? LockoutEnd { get; set; }
        public bool IsActived { get; set; }
        public System.DateTime? LastModified { get; set; }
        public string ModifiedBy { get; set; }

        //public MixUser User { get; set; }

        public FileModel MediaFile { get; set; } = new();

        public AdditionalDataContentViewModel UserData { get; set; }

        public List<AspNetUserRoles> Roles { get; set; }

        #region Change Password

        public ResetPasswordViewModel ResetPassword { get; set; }

        public bool IsChangePassword { get; set; }

        public ChangePasswordViewModel ChangePassword { get; set; }

        private readonly MixCmsContext _cmsContext;

        #endregion Change Password

        public MixUserViewModel(MixUser user, MixCmsContext cmsContext)
        {
            _cmsContext = cmsContext;
            ReflectionHelper.Mapping(user, this);
        }

        public async Task LoadUserDataAsync(int tenantId)
        {
            UserData ??= await MixDataHelper.GetAdditionalDataAsync(
                _cmsContext,
                MixDatabaseParentType.User,
                MixDatabaseNames.SYSTEM_USER_DATA,
                Id);
            using var context = new MixCmsAccountContext();
            var roles = from ur in context.AspNetUserRoles
                        join r in context.MixRoles.Where(m=>m.MixTenantId == tenantId)
                        on ur.RoleId equals r.Id
                        where ur.UserId == Id
                        select ur;
            Roles = await roles.ToListAsync();

        }
    }
}

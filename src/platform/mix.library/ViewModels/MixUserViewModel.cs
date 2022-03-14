using Mix.Database.Entities.Account;
using Mix.Identity.Models.AccountViewModels;
using Mix.Identity.Models.ManageViewModels;

namespace Mix.Lib.ViewModels
{
    public class MixUserViewModel
    {
        public MixUser User { get; set; }

        public FileModel MediaFile { get; set; } = new();

        public AdditionalDataContentViewModel UserData { get; set; }

        public List<AspNetUserRoles> Roles { get; set; }

        #region Change Password

        public ResetPasswordViewModel ResetPassword { get; set; }

        public bool IsChangePassword { get; set; }

        public ChangePasswordViewModel ChangePassword { get; set; }

        private readonly MixCmsContext _context;

        #endregion Change Password

        public MixUserViewModel(MixCmsContext context, MixUser user)
        {
            _context = context;
            User = user;
        }

        public async Task LoadUserDataAsync()
        {
            if (User != null)
            {
                UserData ??= await MixDataHelper.GetAdditionalDataAsync(
                    _context,
                    MixDatabaseParentType.User,
                    MixDatabaseNames.SYSTEM_USER_DATA,
                    User.Id);
                using var context = new MixCmsAccountContext();
                Roles = context.AspNetUserRoles.Where(m => m.UserId == User.Id).ToList();
            }
        }
    }
}

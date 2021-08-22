using Mix.Database.Entities.Account;
using Mix.Identity.Models.AccountViewModels;
using Mix.Identity.Models.ManageViewModels;
using Mix.Lib.Helpers;
using Mix.Lib.ViewModels;
using Mix.Shared.Constants;
using Mix.Shared.Enums;
using Mix.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Account.Domain.ViewModels
{
    public class MixUserViewModel
    {
        public MixUser User { get; set; }

        public FileViewModel MediaFile { get; set; } = new FileViewModel();

        public AdditionalDataContentViewModel UserData { get; set; }

        public List<AspNetUserRoles> UserRoles { get; set; }

        #region Change Password

        public ResetPasswordViewModel ResetPassword { get; set; }

        public bool IsChangePassword { get; set; }

        public ChangePasswordViewModel ChangePassword { get; set; }

        #endregion Change Password

        public MixUserViewModel(MixUser user)
        {
            User = user;
        }

        public async Task LoadUserDataAsync()
        {
            if (User != null)
            {
                UserData ??= await MixDataHelper.GetAdditionalDataAsync(
                    MixDatabaseParentType.User,
                    MixDatabaseNames.SYSTEM_USER_DATA,
                    Guid.Parse(User.Id));
                using var context = new MixCmsAccountContext();
                UserRoles = context.AspNetUserRoles.Where(m => m.UserId == User.Id).ToList();
            }
        }
    }
}

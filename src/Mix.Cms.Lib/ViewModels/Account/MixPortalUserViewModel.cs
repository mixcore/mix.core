using Mix.Heart.Models;
using Mix.Identity.Models;
using Mix.Identity.Models.AccountViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.Account
{
    public class MixPortalUserViewModel
    {
        [JsonProperty("user")]
        public ApplicationUser User { get; set; }

        [JsonProperty("mediaFile")]
        public FileViewModel MediaFile { get; set; } = new FileViewModel();

        [JsonProperty("userData")]
        public MixDatabaseDatas.AdditionalViewModel UserData { get; set; }

        [JsonProperty("userRoles")]
        public List<NavUserRoleViewModel> UserRoles { get; set; }

        #region Change Password

        [JsonProperty("resetPassword")]
        public ResetPasswordViewModel ResetPassword { get; set; }

        [JsonProperty("isChangePassword")]
        public bool IsChangePassword { get; set; }

        [JsonProperty("changePassword")]
        public ChangePasswordViewModel ChangePassword { get; set; }

        #endregion Change Password

        public MixPortalUserViewModel(ApplicationUser user)
        {
            User = user;
        }

        public async Task LoadUserDataAsync()
        {
            if (User != null)
            {
                UserData ??= await MixAccountHelper.LoadUserInfoAsync(User.UserName);
                UserRoles = MixAccountHelper.GetRoleNavs(User.Id);
            }
        }
    }
}
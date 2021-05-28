using Mix.Heart.Models;
using Mix.Identity.Models;
using Mix.Identity.Models.AccountViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mix.Lib.ViewModels.Account
{
    public class MixUserViewModel
    {
        public ApplicationUser User { get; set; }

        public FileViewModel MediaFile { get; set; } = new FileViewModel();

        // TODO: Update later
        //[JsonProperty("userData")]
        //public MixDatabaseDatas.AdditionalViewModel UserData { get; set; }

        public List<NavUserRoleViewModel> UserRoles { get; set; }

        #region Change Password

        [JsonProperty("resetPassword")]
        public ResetPasswordViewModel ResetPassword { get; set; }

        [JsonProperty("isChangePassword")]
        public bool IsChangePassword { get; set; }

        [JsonProperty("changePassword")]
        public ChangePasswordViewModel ChangePassword { get; set; }

        #endregion Change Password

        public MixUserViewModel(ApplicationUser user)
        {
            User = user;
        }

        public async Task LoadUserDataAsync()
        {
            // TODO: Update later
            //if (User != null)
            //{
            //    UserData ??= await MixAccountHelper.LoadUserInfoAsync(User.UserName);
            //    UserRoles = MixAccountHelper.GetRoleNavs(User.Id);
            //}
        }
    }
}
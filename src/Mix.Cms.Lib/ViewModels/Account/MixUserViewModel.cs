using Mix.Heart.Models;
using Mix.Identity.Models;
using Mix.Identity.Models.AccountViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.Account
{
    public class MixUserViewModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("userData")]
        public JObject UserData { get; set; }

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

        public MixUserViewModel(ApplicationUser user)
        {
            if (user != null)
            {
                Id = user.Id;
                Username = user.UserName;
                Email = user.Email;
                FirstName = user.FirstName;
                LastName = user.LastName;
            }
        }

        public async Task LoadUserDataAsync()
        {
            if (!string.IsNullOrEmpty(Username))
            {
                if(UserData == null)
                {
                    var data = await MixAccountHelper.LoadUserInfoAsync(Username);
                    UserData = data?.Obj;
                }
                UserRoles = MixAccountHelper.GetRoleNavs(Id);
            }
        }
    }
}
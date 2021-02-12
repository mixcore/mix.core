using Mix.Identity.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.Account
{
    public class MixUserViewModel
    {
        [JsonProperty("user")]
        public ApplicationUser User { get; set; }

        [JsonProperty("userData")]
        public MixAttributeSetDatas.AdditionalViewModel UserData { get; set; }

        public MixUserViewModel(ApplicationUser user)
        {
            User = user;
        }

        public async Task LoadUserDataAsync()
        {
            UserData ??= await Helper.LoadUserInfoAsync(User.Id);
        }
    }
}

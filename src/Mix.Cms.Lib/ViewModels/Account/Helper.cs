using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.Account
{
    public class Helper
    {
        public static async Task<MixAttributeSetDatas.AdditionalViewModel> LoadUserInfoAsync(string userId)
        {
            var getInfo = await MixAttributeSetDatas.Helper.LoadAdditionalData(MixDatabaseParentType.User, userId, MixDatabaseNames.SYSTEM_USER_INFO);
            return getInfo.Data;

            //var info = await UserInfoViewModel.Repository.GetSingleModelAsync(u => u.Username == user.UserName);
            //if (!info.IsSucceed)
            //{
            //    info.Data = new UserInfoViewModel();
            //}
            //else
            //{
            //    info.Data.UserRoles = UserRoleViewModel.Repository.GetModelListBy(ur => ur.UserId == info.Data.Id).Data;
            //}

        }
    }
}

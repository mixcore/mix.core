using Mix.Cms;
using Mix.Identity.Models;
using Mix.Identity.Models.AccountViewModels;
using Mix.Identity.Models.ManageViewModels;
using Microsoft.AspNetCore.Identity;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Localization;
using Mix.Domain.Core.ViewModels;
using Mix.Cms.Lib.ViewModels;
using Mix.Cms.Lib;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.Enums;
using Mix.Heart.Enums;
using Mix.Cms.Lib.Constants;
using Newtonsoft.Json.Linq;

using MixModules = Mix.Cms.Lib.ViewModels.MixModules;
using MixPosts = Mix.Cms.Lib.ViewModels.MixPosts;
using MixPages = Mix.Cms.Lib.ViewModels.MixPages;
using MixDatas = Mix.Cms.Lib.ViewModels.MixDatabaseDatas;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Mix.Theme.Blog
{
    public class Posts
    {
        public async Task<dynamic> GetPosts()
        {
            return await MixPosts.ReadMvcViewModel.Repository.GetModelListByAsync(
                m => m.Status == MixContentStatus.Published,
                "CreatedDateTime",
                MixHeartEnums.DisplayDirection.Desc, 15, 0);

        }
    }
}

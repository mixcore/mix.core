using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Core.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels
{
    public class SiteStructureViewModel
    {
        public List<MixPages.ImportViewModel> Pages { get; set; }
        
        public SiteStructureViewModel()
        {

        }
        public async Task InitAsync(string culture)
        {
            Pages = (await MixPages.ImportViewModel.Repository.GetModelListByAsync(p => p.Specificulture == culture)).Data;
        }

        public async Task<RepositoryResponse<bool>> Import(List<MixModule> arrModule, string destCulture)
        {
            return await MixPages.Helper.ImportAsync(Pages, destCulture);
        }
    }
}

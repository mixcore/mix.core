using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Core.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.MixRelatedAttributeDatas
{
    public class Helper
    {
        public static async Task<RepositoryResponse<List<MixRelatedAttributeData>>> RemoveRelatedDataAsync(
            string parentId, MixEnums.MixAttributeSetDataType parentType, string specificulture
            , MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = await MixRelatedAttributeDatas.DeleteViewModel.Repository.RemoveListModelAsync(
                true
                , a => a.ParentId == parentId && a.ParentType == parentType.ToString()
                    && a.Specificulture == specificulture
                , context, transaction);

            return result;
        }
    }
}

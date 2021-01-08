using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Core.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
namespace Mix.Cms.Lib.ViewModels.MixRelatedAttributeDatas
{
    public class Helper
    {
        public static async Task<RepositoryResponse<List<MixDatabaseContentAssociation>>> RemoveRelatedDataAsync(
            string parentId, MixDatabaseContentAssociationType parentType, string specificulture
            , MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = await MixRelatedAttributeDatas.DeleteViewModel.Repository.RemoveListModelAsync(
                true
                , a => a.ParentId == parentId && a.ParentType == parentType
                    && a.Specificulture == specificulture
                , context, transaction);

            return result;
        }
    }
}

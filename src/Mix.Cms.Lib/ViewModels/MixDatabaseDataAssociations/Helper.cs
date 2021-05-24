using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Heart.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.MixDatabaseDataAssociations
{
    public class Helper
    {
        public static async Task<RepositoryResponse<List<MixDatabaseDataAssociation>>> RemoveRelatedDataAsync(
            string parentId, MixDatabaseParentType parentType, string specificulture
            , MixCmsContext context, IDbContextTransaction transaction)
        {
            var result = await MixDatabaseDataAssociations.DeleteViewModel.Repository.RemoveListModelAsync(
                true
                , a => a.ParentId == parentId && a.ParentType == parentType
                    && a.Specificulture == specificulture
                , context, transaction);

            return result;
        }
    }
}
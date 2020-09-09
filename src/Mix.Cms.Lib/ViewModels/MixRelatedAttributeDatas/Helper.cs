using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Domain.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.MixRelatedAttributeDatas
{
    public class Helper
    {
        public static async Task<RepositoryResponse<MixRelatedAttributeData>> RemoveRelatedDataAsync(
            string attributeSetName
            , string parentId, MixEnums.MixAttributeSetDataType parentType, string specificulture
            , MixCmsContext context, IDbContextTransaction transaction)
        {
            var getAttrs = await MixAttributeSets.ReadViewModel.Repository.GetSingleModelAsync(
            m => m.Name == attributeSetName, context, transaction);
            var getRelatedData = await MixRelatedAttributeDatas.DeleteViewModel.Repository.GetSingleModelAsync(
                a => a.ParentId == parentId && a.ParentType == parentType.ToString() 
                    && a.Specificulture == specificulture && a.AttributeSetId == getAttrs.Data.Id
                    , context, transaction);
            if (getRelatedData.IsSucceed)
            {
                var removeRelated = await getRelatedData.Data.RemoveModelAsync(true, context, transaction);
                return removeRelated;
            }
            return new RepositoryResponse<MixRelatedAttributeData>();
        }
    }
}

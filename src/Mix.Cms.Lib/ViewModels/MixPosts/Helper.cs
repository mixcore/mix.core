using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Common.Helper;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.Repository;
using Mix.Domain.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.MixPosts
{
    public class Helper
    {
        public static async Task<RepositoryResponse<PaginationModel<TView>>> GetModelistByMeta<TView>(
            string culture, string metaName, string metaValue
            , string orderByPropertyName, int direction, int? pageSize, int? pageIndex
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            where TView : ViewModelBase<MixCmsContext, MixPost, TView>
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                var result = new RepositoryResponse<PaginationModel<TView>>()
                {
                    IsSucceed = true,
                    Data = new PaginationModel<TView>()
                    {
                        PageIndex = pageIndex.HasValue ? pageIndex.Value : 0,
                        PageSize = pageSize
                    }
                };
                var tasks = new List<Task<RepositoryResponse<TView>>>();
                // Get Tag
                var getVal = await MixAttributeSetValues.ReadViewModel.Repository.GetSingleModelAsync(m => m.AttributeSetName == metaName && m.StringValue == metaValue
                , context, transaction);
                if (getVal.IsSucceed)
                {
                    var getRelatedData = await MixRelatedAttributeDatas.ReadViewModel.Repository.GetModelListByAsync(
                        m => m.Specificulture == culture && m.Id == getVal.Data.DataId
                        && m.ParentType == (int)MixEnums.MixAttributeSetDataType.Post
                        , orderByPropertyName, direction, pageIndex, pageSize
                        , _context: context, _transaction: transaction
                        );
                    if (getRelatedData.IsSucceed)
                    {
                        foreach (var item in getRelatedData.Data.Items)
                        {
                            if (int.TryParse(item.ParentId, out int postId))
                            {
                                var getData = await DefaultRepository<MixCmsContext, MixPost, TView>.Instance.GetSingleModelAsync(
                                m => m.Specificulture == item.Specificulture && m.Id == postId
                                    , context, transaction);
                                if (getData.IsSucceed)
                                {
                                    result.Data.Items.Add(getData.Data);
                                }
                            }
                        }
                        result.Data.TotalItems = getRelatedData.Data.TotalItems;
                        result.Data.TotalPage = getRelatedData.Data.TotalPage;
                    }
                    //var query = context.MixRelatedAttributeData.Where(m=> m.Specificulture == culture
                    //    && m.Id == getVal.Data.DataId && m.ParentId == parentId && m.ParentType == (int) MixEnums.MixAttributeSetDataType.Post)
                    //    .Select(m => m.ParentId).Distinct().ToList();
                }
                Expression<Func<MixAttributeSetValue, bool>> valPredicate = m => m.Specificulture == culture && m.AttributeSetName == metaName && m.StringValue == metaValue;

                return result;
            }
            catch (Exception ex)
            {
                return UnitOfWorkHelper<MixCmsContext>.HandleException<PaginationModel<TView>>(ex, isRoot, transaction);
            }
            finally
            {
                if (isRoot)
                {
                    //if current Context is Root
                    context.Dispose();
                }
            }
        }
    }
}
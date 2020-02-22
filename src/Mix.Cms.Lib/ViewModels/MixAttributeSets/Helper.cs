using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Domain.Core.ViewModels;
using System;
using System.Linq.Expressions;

namespace Mix.Cms.Lib.ViewModels.MixAttributeSets
{
    public class Helper
    {
        public static RepositoryResponse<PaginationModel<MixPostAttributeDatas.UpdateViewModel>> LoadPostData(int postId, string specificulture, int? pageSize = null, int? pageIndex = 0
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Expression<Func<MixPostAttributeData, bool>> dataExp = null;
            dataExp = m => m.PostId == postId && m.Specificulture == specificulture && (m.PostId == postId);
            var getDataResult = MixPostAttributeDatas.UpdateViewModel.Repository
            .GetModelListBy(
                dataExp
                , MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.OrderBy), 0
                , pageSize, pageIndex
                , _context: _context, _transaction: _transaction);
            if (getDataResult.Data.TotalItems == 0)
            {
                getDataResult.Data.Items.Add(new MixPostAttributeDatas.UpdateViewModel());
            }
            return getDataResult;
        }

        public static async System.Threading.Tasks.Task<RepositoryResponse<PaginationModel<MixPostAttributeDatas.UpdateViewModel>>> LoadPostDataAsync(int postId, string specificulture, int? pageSize = null, int? pageIndex = 0
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Expression<Func<MixPostAttributeData, bool>> dataExp = null;
            dataExp = m => m.PostId == postId && m.Specificulture == specificulture && (m.PostId == postId);
            var getDataResult = await MixPostAttributeDatas.UpdateViewModel.Repository
            .GetModelListByAsync(
                dataExp
                , MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.OrderBy), 0
                , pageSize, pageIndex, null, null
                , _context: _context, _transaction: _transaction);
            if (getDataResult.Data.TotalItems == 0)
            {
                getDataResult.Data.Items.Add(new MixPostAttributeDatas.UpdateViewModel());
            }
            return getDataResult;
        }
    }
}
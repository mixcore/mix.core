using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Common.Helper;
using Mix.Domain.Core.ViewModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using static Mix.Cms.Lib.MixEnums;

namespace Mix.Cms.Lib.ViewModels.MixAttributeSets
{
    public class Helper
    {
        public static RepositoryResponse<PaginationModel<MixPostAttributeDatas.UpdateViewModel>> LoadPostData(int articleId, string specificulture, int? pageSize = null, int? pageIndex = 0
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Expression<Func<MixPostAttributeData, bool>> dataExp = null;
            dataExp = m => m.PostId == articleId && m.Specificulture == specificulture && (m.PostId == articleId);
                var getDataResult = MixPostAttributeDatas.UpdateViewModel.Repository
                .GetModelListBy(
                    dataExp
                    , MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.OrderBy), 0
                    , pageSize, pageIndex
                    , _context: _context, _transaction: _transaction);
            if (getDataResult.Data.TotalItems==0)
            {
                getDataResult.Data.Items.Add(new MixPostAttributeDatas.UpdateViewModel());
            }
            return getDataResult;
        }
        public static async System.Threading.Tasks.Task<RepositoryResponse<PaginationModel<MixPostAttributeDatas.UpdateViewModel>>> LoadPostDataAsync(int articleId, string specificulture, int? pageSize = null, int? pageIndex = 0
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Expression<Func<MixPostAttributeData, bool>> dataExp = null;
            dataExp = m => m.PostId == articleId && m.Specificulture == specificulture && (m.PostId == articleId);
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

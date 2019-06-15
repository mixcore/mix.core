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
        public static RepositoryResponse<PaginationModel<MixArticleAttributeValues.UpdateViewModel>> LoadArticleData(int articleId, string specificulture, int? pageSize = null, int? pageIndex = 0
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Expression<Func<MixArticleAttributeValue, bool>> dataExp = null;
            dataExp = m => m.ArticleId == articleId && m.Specificulture == specificulture && (m.ArticleId == articleId);
                var getDataResult = MixArticleAttributeValues.UpdateViewModel.Repository
                .GetModelListBy(
                    dataExp
                    , MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.OrderBy), 0
                    , pageSize, pageIndex
                    , _context: _context, _transaction: _transaction);
                
            return getDataResult;
        }
        public static async System.Threading.Tasks.Task<RepositoryResponse<PaginationModel<MixArticleAttributeValues.UpdateViewModel>>> LoadArticleDataAsync(int articleId, string specificulture, int? pageSize = null, int? pageIndex = 0
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            Expression<Func<MixArticleAttributeValue, bool>> dataExp = null;
            dataExp = m => m.ArticleId == articleId && m.Specificulture == specificulture && (m.ArticleId == articleId);
                var getDataResult = await MixArticleAttributeValues.UpdateViewModel.Repository
                .GetModelListByAsync(
                    dataExp
                    , MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.OrderBy), 0
                    , pageSize, pageIndex
                    , _context: _context, _transaction: _transaction);
                
            return getDataResult;
        }

    }
}

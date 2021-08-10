using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Cms.Lib.ViewModels.MixDatabaseDatas;
using Mix.Common.Helper;
using Mix.Heart.Enums;
using Mix.Heart.Extensions;
using Mix.Heart.Infrastructure.Repositories;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.Helpers
{
    public static class MixDataHelper
    {
        public static async Task<PaginationModel<ReadMvcViewModel>> GetMixData(
              string mixDatabaseName,
              string culture = null,
              string keyword = null,
              string fieldName = null,
              string filterType = "contain", // or "equal"
              int? pageIndex = 0,
              int pageSize = 100,
              string orderBy = "Priority",
              DisplayDirection direction = DisplayDirection.Asc,
              MixCmsContext _context = null,
              IDbContextTransaction _transaction = null)
        {
            return await GetMixData<ReadMvcViewModel>(mixDatabaseName, culture, keyword, fieldName, filterType, pageIndex, pageSize
                , orderBy, direction);
        }

        public static async Task<RepositoryResponse<PaginationModel<ReadMvcViewModel>>> GetMixDataByParent(
            string mixDatabaseName,
            string parentId,
            MixDatabaseParentType parentType,
            string culture = null,
            string orderBy = "CreatedDateTime",
            DisplayDirection direction = DisplayDirection.Desc,
            int? pageSize = null,
            int? pageIndex = 0,
            MixCmsContext _context = null,
            IDbContextTransaction _transaction = null)
        {
            culture ??= MixService.GetAppSetting<string>(MixAppSettingKeywords.DefaultCulture);
            return await Helper.GetMixDataByParent<ReadMvcViewModel>(
                culture, mixDatabaseName,
                parentId, parentType, orderBy, direction, pageSize, pageIndex, _context, _transaction);
        }

        #region Generic

        public static async Task<PaginationModel<TView>> GetMixData<TView>(
        string mixDatabaseName,
        string culture = null,
        string keyword = null,
        string fieldName = null,
        string filterType = "contain", // or "equal"
        int? pageIndex = 0,
        int pageSize = 100,
        string orderBy = "Priority",
        DisplayDirection direction = DisplayDirection.Asc,
        MixCmsContext _context = null,
        IDbContextTransaction _transaction = null)
        where TView : ViewModelBase<MixCmsContext, MixDatabaseData, TView>
        {
            culture ??= MixService.GetAppSetting<string>(MixAppSettingKeywords.DefaultCulture);
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                var getfields = await ViewModels.MixDatabaseColumns.ReadViewModel.Repository.GetModelListByAsync(
                    m => m.MixDatabaseName == mixDatabaseName, context, transaction);
                var fields = getfields.IsSucceed ? getfields.Data : new List<ViewModels.MixDatabaseColumns.ReadViewModel>();
                // Data predicate
                Expression<Func<MixDatabaseData, bool>> predicate = m => m.Specificulture == culture
                   && (m.MixDatabaseName == mixDatabaseName);

                // val predicate
                Expression<Func<MixDatabaseDataValue, bool>> attrPredicate = m => m.Specificulture == culture
                   && (m.MixDatabaseName == mixDatabaseName);

                // if filter by field name or keyword => filter by attr value
                if (!string.IsNullOrEmpty(keyword))
                {
                    // filter by all fields if have keyword
                    Expression<Func<MixDatabaseDataValue, bool>> pre = null;

                    if (!string.IsNullOrEmpty(fieldName)) // filter by specific field name
                    {
                        pre = m => m.MixDatabaseColumnName == fieldName;
                        pre = pre.AndAlsoIf(filterType == "equal", m => m.StringValue == keyword);
                        pre = pre.AndAlsoIf(filterType == "contain", m => EF.Functions.Like(m.StringValue, $"%{keyword}%"));
                        attrPredicate = attrPredicate.AndAlsoIf(pre != null, pre);
                    }
                    else
                    {
                        foreach (var field in fields)
                        {
                            Expression<Func<MixDatabaseDataValue, bool>> keywordPredicate = m => m.MixDatabaseColumnName == field.Name;
                            keywordPredicate = keywordPredicate.AndAlsoIf(filterType == "equal", m => m.StringValue == keyword);
                            keywordPredicate = keywordPredicate.AndAlsoIf(filterType == "contain", m => EF.Functions.Like(m.StringValue, $"%{keyword}%"));

                            pre = pre == null
                                ? keywordPredicate
                                : pre.Or(keywordPredicate);
                        }
                        attrPredicate = attrPredicate.AndAlsoIf(pre != null, pre);
                    }

                    var valDataIds = context.MixDatabaseDataValue.Where(attrPredicate).Select(m => m.DataId).Distinct();
                    predicate = predicate.AndAlsoIf(valDataIds != null, m => valDataIds.Any(id => m.Id == id));
                }
                else
                {
                    predicate = m => m.Specificulture == culture && (m.MixDatabaseName == mixDatabaseName);
                    predicate = predicate.AndAlso(m => m.Status == MixContentStatus.Published);
                }
                var getData = await DefaultRepository<MixCmsContext, MixDatabaseData, TView>.Instance.GetModelListByAsync(
                        predicate,
                        orderBy,
                        direction,
                        pageSize,
                        pageIndex,
                        null, null,
                        context,
                        transaction);

                return getData.Data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
            finally
            {
                if (isRoot)
                {
                    //if current Context is Root
                    UnitOfWorkHelper<MixCmsContext>.CloseDbContext(ref context, ref transaction);
                }
            }
        }


        #endregion
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Common.Helper;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.Repository;
using Mix.Domain.Data.ViewModels;
using Mix.Heart.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.Helpers
{
    class MixAttributeSetValueHelper
    {
        public static async Task<RepositoryResponse<List<TView>>> FilterByOtherValueAsync<TView>(
           string culture, string attributeSetName
           , string filterType, Dictionary<string, string> queries
           , string responseName
           , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
           where TView : ViewModelBase<MixCmsContext, MixAttributeSetValue, TView>
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                Expression<Func<MixAttributeSetValue, bool>> valPredicate = m => m.AttributeSetName == attributeSetName;
                RepositoryResponse<List<TView>> result = new RepositoryResponse<List<TView>>()
                {
                    IsSucceed = true,
                    Data = new List<TView>()
                };
                foreach (var fieldQuery in queries)
                {
                    Expression<Func<MixAttributeSetValue, bool>> pre = GetValueFilter(filterType, fieldQuery.Key, fieldQuery.Value);
                    valPredicate = ReflectionHelper.CombineExpression(valPredicate, pre, Heart.Enums.MixHeartEnums.ExpressionMethod.And);
                }
                var query = context.MixAttributeSetValue.Where(valPredicate).Select(m => m.DataId).Distinct();
                var dataIds = query.ToList();
                if (query != null)
                {
                    Expression<Func<MixAttributeSetValue, bool>> predicate =
                        m => dataIds.Any(id => m.DataId == id) &&
                            m.AttributeFieldName == responseName;
                    result = await DefaultRepository<MixCmsContext, MixAttributeSetValue, TView>.Instance.GetModelListByAsync(
                        predicate, context, transaction);
                }
                return result;
            }
            catch (Exception ex)
            {
                return UnitOfWorkHelper<MixCmsContext>.HandleException<List<TView>>(ex, isRoot, transaction);
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

        private static Expression<Func<MixAttributeSetValue, bool>> GetValueFilter(string filterType, string key, string value)
        {
            switch (filterType)
            {
                case "equal":
                    return m => m.AttributeFieldName == key
                        && (EF.Functions.Like(m.StringValue, $"{value}"));
                case "contain":
                    return m => m.AttributeFieldName == key &&
                                            (EF.Functions.Like(m.StringValue, $"%{value}%"));

            }
            return null;
        }
    }
}

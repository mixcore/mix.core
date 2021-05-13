using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Common.Helper;
using Mix.Heart.Infrastructure.Repositories;
using Mix.Heart.Models;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.Helpers
{
    internal class MixDatabaseDataValueHelper
    {
        public static async Task<RepositoryResponse<List<TView>>> FilterByOtherValueAsync<TView>(
           string culture, string mixDatabaseName
           , string filterType, Dictionary<string, string> queries
           , string responseName
           , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
           where TView : ViewModelBase<MixCmsContext, MixDatabaseDataValue, TView>
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                Expression<Func<MixDatabaseDataValue, bool>> valPredicate = m => m.MixDatabaseName == mixDatabaseName;
                RepositoryResponse<List<TView>> result = new RepositoryResponse<List<TView>>()
                {
                    IsSucceed = true,
                    Data = new List<TView>()
                };
                foreach (var fieldQuery in queries)
                {
                    Expression<Func<MixDatabaseDataValue, bool>> pre = GetValueFilter(filterType, fieldQuery.Key, fieldQuery.Value);
                    valPredicate = valPredicate.AndAlso(pre);
                }
                var query = context.MixDatabaseDataValue.Where(valPredicate).Select(m => m.DataId).Distinct();
                var dataIds = query.ToList();
                if (query != null)
                {
                    Expression<Func<MixDatabaseDataValue, bool>> predicate =
                        m => dataIds.Any(id => m.DataId == id) &&
                            m.MixDatabaseColumnName == responseName;
                    result = await DefaultRepository<MixCmsContext, MixDatabaseDataValue, TView>.Instance.GetModelListByAsync(
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

        private static Expression<Func<MixDatabaseDataValue, bool>> GetValueFilter(string filterType, string key, string value)
        {
            switch (filterType)
            {
                case "equal":
                    return m => m.MixDatabaseColumnName == key
                        && (EF.Functions.Like(m.StringValue, $"{value}"));

                case "contain":
                    return m => m.MixDatabaseColumnName == key &&
                                            (EF.Functions.Like(m.StringValue, $"%{value}%"));
            }
            return null;
        }
    }
}
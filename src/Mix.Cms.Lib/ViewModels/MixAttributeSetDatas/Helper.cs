using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Helpers;
using Mix.Cms.Lib.Models.Cms;
using Mix.Common.Helper;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.Repository;
using Mix.Domain.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Cms.Lib.ViewModels.MixAttributeSetDatas
{
    public class Helper
    {
        public static Task<RepositoryResponse<List<TView>>> FilterByValueAsync<TView>(string culture, string attributeSetName
            , Dictionary<string, Microsoft.Extensions.Primitives.StringValues> queryDictionary
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            where TView : ODataViewModelBase<MixCmsContext, MixAttributeSetData, TView>
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                Expression<Func<MixAttributeSetValue, bool>> valPredicate = m => m.Specificulture == culture && m.AttributeSetName == attributeSetName;
                RepositoryResponse<List<TView>> result = new RepositoryResponse<List<TView>>() {
                    IsSucceed = true,
                    Data = new List<TView>()
                };
                var tasks = new List<Task<RepositoryResponse<TView>>>();

                // Loop queries string => predicate
                foreach (var q in queryDictionary)
                {
                    Expression<Func<MixAttributeSetValue, bool>> pre = m => m.AttributeFieldName == q.Key && m.StringValue == q.Value;
                    valPredicate = ODataHelper<MixAttributeSetValue>.CombineExpression(valPredicate, pre, Microsoft.OData.UriParser.BinaryOperatorKind.And);
                }
                var query = context.MixAttributeSetValue.Where(valPredicate).Select(m => m.DataId).Distinct().ToList();
                if (query!=null)
                {
                    foreach (var item in query)
                    {
                        tasks.Add(Task.Run(async () => {
                            var resp = await ODataDefaultRepository<MixCmsContext, MixAttributeSetData, TView>.Instance.GetCachedSingleAsync(
                            $"{culture}_{item}", m => m.Id == item && m.Specificulture == culture);
                            return resp;
                        }));

                    }
                    var continuation = Task.WhenAll(tasks);
                    continuation.Wait();
                    if (continuation.Status == TaskStatus.RanToCompletion)
                    {
                        foreach (var data in continuation.Result)
                        {
                            if (data.IsSucceed)
                            {
                                result.Data.Add(data.Data);
                            }
                            else
                            {
                                result.Errors.AddRange(data.Errors);
                            }
                        }
                    }
                    // Display information on faulted tasks.
                    else
                    {
                        foreach (var t in tasks)
                        {
                            result.Errors.Add($"Task {t.Id}: {t.Status}");
                        }
                    }

                }
                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                return Task.FromResult(UnitOfWorkHelper<MixCmsContext>.HandleException<List<TView>>(ex, isRoot, transaction));
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

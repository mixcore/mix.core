using Microsoft.EntityFrameworkCore;
using Mix.Lib.Dtos;
using Mix.Lib.Models.Common;
using System.Linq.Expressions;

namespace Mix.Lib.Services
{
    public class MixDataService : IDisposable
    {
        private readonly MixCmsContext _dbContext;
        private UnitOfWorkInfo _uow;
        public MixDataService(MixCmsContext dbContext)
        {
            _dbContext = dbContext;
            _uow = new(_dbContext);
        }

        public void SetUnitOfWork(UnitOfWorkInfo uow)
        {
            if (uow != null)
            {
                _uow = uow;
            }
        }

        public async Task<PagingResponseModel<TView>> FilterByKeywordAsync<TView>(
            SearchMixDataDto request,
            string culture = null)
           where TView : ViewModelBase<MixCmsContext, MixDataContent, Guid, TView>
        {
            try
            {
                var _colRepo = MixDatabaseColumnViewModel.GetRepository(_uow);
                var _contentRepo = new Repository<MixCmsContext, MixDataContent, Guid, TView>(_uow);

                var tasks = new List<Task<TView>>();
                culture ??= GlobalConfigService.Instance.AppSettings.DefaultCulture;

                var fields = await _colRepo.GetListQuery(
                    m => m.MixDatabaseId == request.MixDatabaseId).ToListAsync();

                // Data predicate
                Expression<Func<MixDataContent, bool>> andPredicate = m => m.Specificulture == culture
                   && (m.MixDatabaseId == request.MixDatabaseId);

                var searchRequest = new SearchQueryModel<MixDataContent, Guid>(request, andPredicate);
                // val predicate
                Expression<Func<MixDataContentValue, bool>> attrPredicate = m => (m.MixDatabaseId == request.MixDatabaseId);

                PagingResponseModel<TView> result = new()
                {
                    Items = new List<TView>()
                };

                // if filter by field name or keyword => filter by attr value
                if (fields.Count > 0 || !string.IsNullOrEmpty(searchRequest.Keyword))
                {
                    // filter by all fields if have keyword
                    if (!string.IsNullOrEmpty(searchRequest.Keyword))
                    {
                        Expression<Func<MixDataContentValue, bool>> pre = null;
                        foreach (var field in fields)
                        {
                            Expression<Func<MixDataContentValue, bool>> keywordPredicate =
                                m => m.MixDatabaseColumnName == field.SystemName;
                            keywordPredicate = keywordPredicate
                                                .AndAlsoIf(request.CompareKind == MixCompareOperatorKind.Equal,
                                                            m => m.StringValue == searchRequest.Keyword);
                            keywordPredicate = keywordPredicate
                                                .AndAlsoIf(request.CompareKind == MixCompareOperatorKind.Contain,
                                                            m => EF.Functions.Like(m.StringValue, $"%{searchRequest.Keyword}%"));

                            pre = pre == null
                                ? keywordPredicate
                                : pre.Or(keywordPredicate);
                        }
                        attrPredicate = attrPredicate.AndAlsoIf(pre != null, pre);
                    }

                    if (request.Fields != null && request.Fields.Properties().Any()) // filter by specific field name
                    {
                        var valPredicate = GetFilterValueByFields(fields, request.Fields, request.CompareKind);
                        attrPredicate = attrPredicate.AndAlsoIf(valPredicate != null, valPredicate);
                    }

                    var valDataIds = _dbContext.MixDataContentValue.Where(attrPredicate).Select(m => m.ParentId).Distinct();
                    searchRequest.Predicate = searchRequest.Predicate.AndAlsoIf(valDataIds != null, m => valDataIds.Any(id => m.Id == id));
                }

                if (request.IsGroup)
                {
                    var excludeIds = _dbContext.MixDataContentAssociation.Where(
                        m => (m.MixDatabaseId == request.MixDatabaseId)
                        && m.Specificulture == culture
                        && m.ParentType == MixDatabaseParentType.Set
                        && m.ParentId != Guid.Empty)
                        .Select(m => m.DataContentId);
                    searchRequest.Predicate = searchRequest.Predicate.AndAlso(m => !excludeIds.Any(n => n == m.Id));
                }

                result = await _contentRepo.GetPagingAsync(searchRequest.Predicate, searchRequest.PagingData);
                return result;
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.Badrequest, ex);
            }
        }

        public async Task LoadAllReferenceDataAsync<TView>(
            JObject obj,
            Guid dataContentId,
            string mixDatabaseName,
            List<MixDatabaseColumn> refColumns = null)
             where TView : ViewModelBase<MixCmsContext, MixDataContentAssociation, Guid, TView>
        {
            var context = (MixCmsContext)_uow.ActiveDbContext;
            refColumns ??= context.MixDatabaseColumn.Where(
                   m => m.MixDatabaseName == mixDatabaseName
                    && m.DataType == MixDataType.Reference).ToList();

            foreach (var item in refColumns.Where(p => p.DataType == MixDataType.Reference))
            {
                var arr = await GetRelatedDataContentAsync<TView>(item.ReferenceId.Value, dataContentId);

                if (obj.ContainsKey(item.SystemName))
                {
                    obj[item.SystemName] = arr;
                }
                else
                {
                    obj.Add(new JProperty(item.SystemName, arr));
                }
            }
        }

        public async Task<JArray> GetRelatedDataContentAsync<TView>(
            int referenceId,
            Guid dataContentId)
            where TView : ViewModelBase<MixCmsContext, MixDataContentAssociation, Guid, TView>
        {
            var _assoRepo = MixDataContentAssociationViewModel.GetRepository(_uow);

            Expression<Func<MixDataContentAssociation, bool>> predicate =
                    model => (model.MixDatabaseId == referenceId)
                    && (model.GuidParentId == dataContentId && model.ParentType == MixDatabaseParentType.Set);
            var relatedContents = await _assoRepo.GetListAsync(predicate);

            JArray arr = new();
            foreach (var nav in relatedContents.OrderBy(v => v.Priority))
            {
                arr.Add(nav);
            }
            return arr;
        }

        private static Expression<Func<MixDataContentValue, bool>> GetFilterValueByFields(
                List<MixDatabaseColumn> fields, JObject fieldQueries, MixCompareOperatorKind compareKind)
        {
            Expression<Func<MixDataContentValue, bool>> valPredicate = null;
            foreach (var q in fieldQueries)
            {
                if (fields.Any(f => f.SystemName == q.Key))
                {
                    string value = q.Value.ToString();
                    if (!string.IsNullOrEmpty(value))
                    {
                        Expression<Func<MixDataContentValue, bool>> pre = m => m.MixDatabaseColumnName == q.Key;
                        pre = pre.AndAlsoIf(compareKind == MixCompareOperatorKind.Equal, m => m.StringValue == (q.Value.ToString()));
                        pre = pre.AndAlsoIf(compareKind == MixCompareOperatorKind.Contain, m => EF.Functions.Like(m.StringValue, $"%{q.Value}%"));

                        valPredicate = valPredicate == null
                            ? pre
                            : valPredicate.Or(pre);
                    }
                }
            }
            return valPredicate;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}

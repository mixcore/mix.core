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

        public async Task<List<TView>> GetByAllParent<TView>(
            SearchDataContentModel request,
            string culture = null)
           where TView : ViewModelBase<MixCmsContext, MixDataContent, Guid, TView>
        {
            try
            {
                var _associationRepo = MixDataContentAssociationViewModel.GetRepository(_uow);
                var _contentRepo = new Repository<MixCmsContext, MixDataContent, Guid, TView>(_uow);
                Expression<Func<MixDataContentAssociation, bool>> predicate = mbox => false;
                predicate = predicate.OrIf(request.IntParentId.HasValue, m => m.IntParentId == request.IntParentId.Value);
                predicate = predicate.OrIf(request.GuidParentId.HasValue, m => m.GuidParentId == request.GuidParentId.Value);
                var associations = _associationRepo.GetListQuery(predicate);
                return await _contentRepo.GetAllAsync(m => associations.Any(n => n.DataContentId == m.Id));
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.Badrequest, ex);
            }
        }

        public async Task<PagingResponseModel<TView>> Search<TView>(
            SearchDataContentModel searchRequest,
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
                    m => m.MixDatabaseId == searchRequest.MixDatabaseId
                            || m.MixDatabaseName == searchRequest.MixDatabaseName).ToListAsync();

                // Data predicate
                Expression<Func<MixDataContent, bool>> andPredicate = m => m.Specificulture == culture
                   && (m.MixDatabaseId == searchRequest.MixDatabaseId
                        || m.MixDatabaseName == searchRequest.MixDatabaseName);

                // val predicate
                Expression<Func<MixDataContentValue, bool>> attrPredicate = m => (m.MixDatabaseId == searchRequest.MixDatabaseId
                || m.MixDatabaseName == searchRequest.MixDatabaseName);

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
                                                .AndAlsoIf(searchRequest.SearchMethod == ExpressionMethod.Eq ,
                                                            m => m.StringValue == searchRequest.Keyword);
                            keywordPredicate = keywordPredicate
                                                .AndAlsoIf(searchRequest.SearchMethod == ExpressionMethod.Ct,
                                                            m => EF.Functions.Like(m.StringValue, $"%{searchRequest.Keyword}%"));

                            pre = pre == null
                                ? keywordPredicate
                                : pre.Or(keywordPredicate);
                        }
                        attrPredicate = attrPredicate.AndAlsoIf(pre != null, pre);
                    }

                    if (searchRequest.Fields != null && searchRequest.Fields.Properties().Any()) // filter by specific field name
                    {
                        var valPredicate = GetFilterValueByFields(fields, searchRequest.Fields, searchRequest.SearchMethod);
                        attrPredicate = attrPredicate.AndAlsoIf(valPredicate != null, valPredicate);
                    }

                    var valDataIds = _dbContext.MixDataContentValue.Where(attrPredicate).Select(m => m.ParentId).Distinct();
                    searchRequest.Predicate = searchRequest.Predicate.AndAlsoIf(valDataIds != null, m => valDataIds.Any(id => m.Id == id));
                }

                if (searchRequest.IsGroup)
                {
                    var excludeIds = _dbContext.MixDataContentAssociation.Where(
                        m => (m.MixDatabaseId == searchRequest.MixDatabaseId || m.MixDatabaseName == searchRequest.MixDatabaseName)
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
            refColumns ??= _dbContext.MixDatabaseColumn.Where(
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
                List<MixDatabaseColumn> fields, JObject fieldQueries, ExpressionMethod? compareKind)
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
                        pre = pre.AndAlsoIf(compareKind == ExpressionMethod.Eq, m => m.StringValue == (q.Value.ToString()));
                        pre = pre.AndAlsoIf(compareKind == ExpressionMethod.Ct, m => EF.Functions.Like(m.StringValue, $"%{q.Value}%"));

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

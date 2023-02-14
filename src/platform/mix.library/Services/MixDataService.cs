using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Mix.Lib.Interfaces;
using Mix.Lib.Models.Common;
using System.Linq.Expressions;

namespace Mix.Lib.Services
{
    public class MixDataService : IDisposable, IMixDataService
    {
        private readonly ISession _session;
        private MixTenantSystemModel _currentTenant;
        public MixTenantSystemModel CurrentTenant
        {
            get
            {
                if (_currentTenant == null)
                {
                    _currentTenant = _session.Get<MixTenantSystemModel>(MixRequestQueryKeywords.Tenant);
                }
                return _currentTenant;
            }
        }
        private readonly MixCmsContext _dbContext;
        private UnitOfWorkInfo _uow;
        public MixDataService(IHttpContextAccessor httpContext, UnitOfWorkInfo<MixCmsContext> uow)
        {
            _uow = uow;
            _dbContext = uow.DbContext;
            _session = httpContext.HttpContext?.Session;
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
                var associationRepo = MixDataContentAssociationViewModel.GetRepository(_uow);
                var contentRepo = new Repository<MixCmsContext, MixDataContent, Guid, TView>(_uow);
                Expression<Func<MixDataContentAssociation, bool>> predicate = mbox => false;
                predicate = predicate.OrIf(request.IntParentId.HasValue, m => m.IntParentId == request.IntParentId.Value);
                predicate = predicate.OrIf(request.GuidParentId.HasValue, m => m.GuidParentId == request.GuidParentId.Value);
                var associations = associationRepo.GetListQuery(predicate);
                return await contentRepo.GetAllAsync(m => associations.Any(n => n.DataContentId == m.Id));
            }
            catch (Exception ex)
            {
                throw new MixException(MixErrorStatus.Badrequest, ex);
            }
        }

        public async Task<PagingResponseModel<TView>> Search<TView>(
            SearchDataContentModel searchRequest,
            string culture = null,
            CancellationToken cancellationToken = default)
           where TView : ViewModelBase<MixCmsContext, MixDataContent, Guid, TView>
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var colRepo = MixDatabaseColumnViewModel.GetRepository(_uow);
                var contentRepo = new Repository<MixCmsContext, MixDataContent, Guid, TView>(_uow);

                var tasks = new List<Task<TView>>();
                culture ??= CurrentTenant.Configurations.DefaultCulture;
                var fields = await colRepo.GetListQuery(
                    m => m.MixDatabaseId == searchRequest.MixDatabaseId
                            || m.MixDatabaseName == searchRequest.MixDatabaseName, cancellationToken).ToListAsync(cancellationToken: cancellationToken);

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
                                                .AndAlsoIf(searchRequest.SearchMethod == ExpressionMethod.Equal,
                                                            m => m.StringValue == searchRequest.Keyword);
                            keywordPredicate = keywordPredicate
                                                .AndAlsoIf(searchRequest.SearchMethod == ExpressionMethod.Like,
                                                            m => EF.Functions.Like(m.StringValue, $"%{searchRequest.Keyword}%"));

                            pre = pre == null
                                ? keywordPredicate
                                : pre.Or(keywordPredicate);
                        }
                        attrPredicate = attrPredicate.AndAlsoIf(pre != null, pre);
                    }

                    if (searchRequest.Fields != null && searchRequest.Fields.Any()) // filter by specific field name
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
                        && m.ParentType == MixDatabaseParentType.MixDatabse
                        && m.ParentId != Guid.Empty)
                        .Select(m => m.DataContentId);
                    searchRequest.Predicate = searchRequest.Predicate.AndAlso(m => !excludeIds.Any(n => n == m.Id));
                }

                result = await contentRepo.GetPagingAsync(searchRequest.Predicate, searchRequest.PagingData, cancellationToken);
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
            var assoRepo = MixDataContentAssociationViewModel.GetRepository(_uow);

            Expression<Func<MixDataContentAssociation, bool>> predicate =
                    model => (model.MixDatabaseId == referenceId)
                    && (model.GuidParentId == dataContentId && model.ParentType == MixDatabaseParentType.MixDatabse);
            var relatedContents = await assoRepo.GetListAsync(predicate);

            JArray arr = new();
            foreach (var nav in relatedContents.OrderBy(v => v.Priority))
            {
                arr.Add(nav);
            }
            return arr;
        }

        private static Expression<Func<MixDataContentValue, bool>> GetFilterValueByFields(
            List<MixDatabaseColumn> columns, 
            List<SearchContentValueModel> fieldQueries, 
            ExpressionMethod? compareKind)
        {
            Expression<Func<MixDataContentValue, bool>> valPredicate = null;
            foreach (var q in fieldQueries)
            {
                var column = columns.SingleOrDefault(f => f.SystemName == q.ColumnName);
                if (column != null)
                {
                    if (q.Value != null)
                    {
                        Expression<Func<MixDataContentValue, bool>> pre =
                            m => m.MixDatabaseColumnName == q.ColumnName;
                        Expression<Func<MixDataContentValue, bool>> andPredicate = null;
                        switch (column.DataType)
                        {
                            case MixDataType.DateTime:
                            case MixDataType.Date:
                            case MixDataType.Time:
                                switch (q.SearchMethod)
                                {
                                    case ExpressionMethod.Equal:
                                        andPredicate = m => m.DateTimeValue == (DateTime)q.Value;
                                        break;
                                    case ExpressionMethod.LessThan:
                                        andPredicate = m => (DateTime)q.Value < m.DateTimeValue;
                                        break;
                                    case ExpressionMethod.GreaterThan:
                                        andPredicate = m => (DateTime)q.Value > m.DateTimeValue;
                                        break;
                                    case ExpressionMethod.LessThanOrEqual:
                                        andPredicate = m => (DateTime)q.Value >= m.DateTimeValue;
                                        break;
                                    case ExpressionMethod.GreaterThanOrEqual:
                                        andPredicate = m => (DateTime)q.Value >= m.DateTimeValue;
                                        break;
                                    default:
                                        break;
                                }

                                break;
                            case MixDataType.Integer:
                                switch (q.SearchMethod)
                                {
                                    case ExpressionMethod.Equal:
                                        andPredicate = m => m.IntegerValue == (int)q.Value;
                                        break;
                                    case ExpressionMethod.LessThan:
                                        andPredicate = m => (int)q.Value < m.IntegerValue;
                                        break;
                                    case ExpressionMethod.GreaterThan:
                                        andPredicate = m => (int)q.Value > m.IntegerValue;
                                        break;
                                    case ExpressionMethod.LessThanOrEqual:
                                        andPredicate = m => (int)q.Value >= m.IntegerValue;
                                        break;
                                    case ExpressionMethod.GreaterThanOrEqual:
                                        andPredicate = m => (int)q.Value >= m.IntegerValue;
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case MixDataType.Long:
                                switch (q.SearchMethod)
                                {
                                    case ExpressionMethod.Equal:
                                        andPredicate = m => m.LongValue == (int)q.Value;
                                        break;
                                    case ExpressionMethod.LessThan:
                                        andPredicate = m => (int)q.Value < m.LongValue;
                                        break;
                                    case ExpressionMethod.GreaterThan:
                                        andPredicate = m => (int)q.Value > m.LongValue;
                                        break;
                                    case ExpressionMethod.LessThanOrEqual:
                                        andPredicate = m => (int)q.Value >= m.LongValue;
                                        break;
                                    case ExpressionMethod.GreaterThanOrEqual:
                                        andPredicate = m => (int)q.Value >= m.LongValue;
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case MixDataType.Double:
                                switch (q.SearchMethod)
                                {
                                    case ExpressionMethod.Equal:
                                        andPredicate = m => m.DoubleValue == (double)q.Value;
                                        break;
                                    case ExpressionMethod.LessThan:
                                        andPredicate = m => (double)q.Value < m.DoubleValue;
                                        break;
                                    case ExpressionMethod.GreaterThan:
                                        andPredicate = m => (double)q.Value > m.DoubleValue;
                                        break;
                                    case ExpressionMethod.LessThanOrEqual:
                                        andPredicate = m => (double)q.Value >= m.DoubleValue;
                                        break;
                                    case ExpressionMethod.GreaterThanOrEqual:
                                        andPredicate = m => (double)q.Value >= m.DoubleValue;
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case MixDataType.Boolean:
                                andPredicate = m => m.BooleanValue == (bool)q.Value;
                                break;
                            case MixDataType.Reference:
                                break;
                            default:
                                switch (q.SearchMethod)
                                {
                                    case ExpressionMethod.Equal:
                                        andPredicate = m => m.StringValue == (string)q.Value;
                                        break;
                                    case ExpressionMethod.Like:
                                        andPredicate = m => EF.Functions.Like(m.StringValue, $"%{q.Value}%");
                                        break;

                                }
                                break;
                        }

                        pre = pre.AndAlso(andPredicate);

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

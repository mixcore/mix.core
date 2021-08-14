using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Mix.Database.Entities.Cms;
using Mix.Heart.Model;
using Mix.Heart.Repository;
using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Mix.Lib.Models.Common;
using Mix.Shared.Constants;
using Mix.Shared.Enums;
using Mix.Shared.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mix.Heart.Extensions;
using Mix.Heart.Exceptions;
using Mix.Heart.Enums;
using Mix.Lib.Dtos;

namespace Mix.Lib.Services
{
    public class MixDataService: IDisposable
    {
        private readonly MixAppSettingService _appSettingService;
        private readonly MixCmsContext _dbContext;
        private QueryRepository<MixCmsContext, MixDatabaseColumn, int> _colRepo;
        private QueryRepository<MixCmsContext, MixDataContent, Guid> _contentRepo;
        private QueryRepository<MixCmsContext, MixDataContentAssociation, Guid> _assoRepo;

        public MixDataService(
            MixAppSettingService appSettingService,
            MixCmsContext dbContext,
            QueryRepository<MixCmsContext, MixDatabaseColumn, int> colRepo,
            QueryRepository<MixCmsContext, MixDataContent, Guid> contentRepo, 
            QueryRepository<MixCmsContext, MixDataContentAssociation, Guid> assoRepo)
        {
            _appSettingService = appSettingService;
            _dbContext = dbContext;
            _colRepo = colRepo;
            _contentRepo = contentRepo;
            _assoRepo = assoRepo;
        }

        public async Task<PagingResponseModel<TView>> FilterByKeywordAsync<TView>(
            SearchMixDataDto request,
            string culture = null,
            string mixDatabaseName = null,
            UnitOfWorkInfo uowInfo = null)
           where TView : ViewModelBase<MixCmsContext, MixDataContent, Guid>
        {
            try
            {
                if (uowInfo != null)
                {
                    _colRepo.SetUowInfo(uowInfo);
                }
                var tasks = new List<Task<TView>>();
                culture = culture ?? _appSettingService.GetConfig<string>
                                        (MixAppSettingsSection.GlobalSettings,
                                        MixAppSettingKeywords.DefaultCulture);

                var fields = await _colRepo.GetListQuery(
                    m => m.MixDatabaseId == request.MixDatabaseId || m.MixDatabaseName == mixDatabaseName).ToListAsync();
                
                // Data predicate
                Expression<Func<MixDataContent, bool>> andPredicate = m => m.Specificulture == culture
                   && (m.MixDatabaseName == mixDatabaseName);

                var searchRequest = new SearchQueryModel<MixDataContent, Guid>(request, andPredicate);
                // val predicate
                Expression<Func<MixDataContentValue, bool>> attrPredicate = m => (m.MixDatabaseName == mixDatabaseName);

                PagingResponseModel<TView> result = new PagingResponseModel<TView>()
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

                    if (request.Fields != null && request.Fields.Properties().Count() > 0) // filter by specific field name
                    {
                        var valPredicate = GetFilterValueByFields(fields, request.Fields, request.CompareKind);
                        attrPredicate = attrPredicate.AndAlsoIf(valPredicate != null, valPredicate);
                    }

                    var valDataIds = _dbContext.MixDataContentValue.Where(attrPredicate).Select(m => m.MixDataContentId).Distinct();
                    searchRequest.Predicate = searchRequest.Predicate.AndAlsoIf(valDataIds != null, m => valDataIds.Any(id => m.Id == id));
                }

                if (request.IsGroup)
                {
                    var excludeIds = _dbContext.MixDataContentAssociation.Where(
                        m => (m.MixDatabaseId == request.MixDatabaseId || m.MixDatabaseName == mixDatabaseName)
                        && m.Specificulture == culture
                        && m.ParentType == MixDatabaseParentType.Set
                        && m.ParentId != Guid.Empty)
                        .Select(m => m.DataContentId);
                    searchRequest.Predicate = searchRequest.Predicate.AndAlso(m => !excludeIds.Any(n => n == m.Id));
                }

                result = await _contentRepo.GetPagingViewAsync<TView>(searchRequest.Predicate, searchRequest.PagingData);
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
            UnitOfWorkInfo uowInfo,
            List<MixDatabaseColumn> refColumns = null)
             where TView : ViewModelBase<MixCmsContext, MixDataContentAssociation, Guid>
        {
            var context = (MixCmsContext)uowInfo.ActiveDbContext;
            refColumns ??= context.MixDatabaseColumn.Where(
                   m => m.MixDatabaseName == mixDatabaseName
                    && m.DataType == MixDataType.Reference).ToList();

            foreach (var item in refColumns.Where(p => p.DataType == MixDataType.Reference))
            {
                var arr = await GetRelatedDataContentAsync<TView>(item.ReferenceId.Value, dataContentId, uowInfo);

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
            Guid dataContentId, 
            UnitOfWorkInfo uowInfo = null)
            where TView : ViewModelBase<MixCmsContext, MixDataContentAssociation, Guid>
        {
            if (uowInfo != null) _assoRepo.SetUowInfo(uowInfo);

            Expression<Func<MixDataContentAssociation, bool>> predicate = 
                    model => (model.MixDatabaseId == referenceId)
                    && (model.GuidParentId == dataContentId && model.ParentType == MixDatabaseParentType.Set);
            var relatedContents = await _assoRepo.GetListViewAsync<TView>(predicate);

            JArray arr = new JArray();
            foreach (var nav in relatedContents.OrderBy(v => v.Priority))
            {
                arr.Add(nav);
            }
            return arr;
        }

        private Expression<Func<MixDataContentValue, bool>> GetFilterValueByFields(
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

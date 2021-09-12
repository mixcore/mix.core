using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Extensions;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Common.Helper;
using Mix.Heart.Infrastructure.Repositories;
using Mix.Heart.Models;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Extensions;
using Mix.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Mix.Cms.Lib.ViewModels.MixDatabaseDatas
{
    public static class Helper
    {
        public static async Task<RepositoryResponse<bool>> ImportData(
            string culture, Lib.ViewModels.MixDatabases.ReadViewModel mixDatabase, IFormFile file)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(null, null, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                List<ImportViewModel> data = LoadFileData(culture, mixDatabase, file);

                var fields = MixDatabaseColumns.UpdateViewModel.Repository.GetModelListBy(f => f.MixDatabaseId == mixDatabase.Id, context, transaction).Data;
                foreach (var item in data)
                {
                    if (result.IsSucceed)
                    {
                        var isCreateNew = string.IsNullOrEmpty(item.Id);
                        item.Columns = fields;
                        item.MixDatabaseName = mixDatabase.Name;
                        item.Status = MixService.GetEnumConfig<MixContentStatus>(MixAppSettingKeywords.DefaultContentStatus);
                        var saveResult = await item.SaveModelAsync(true, context, transaction);
                        ViewModelHelper.HandleResult(saveResult, ref result);
                    }
                }
                UnitOfWorkHelper<MixCmsContext>.HandleTransaction(result.IsSucceed, isRoot, transaction);
                return result;
            }
            catch (Exception ex)
            {
                return UnitOfWorkHelper<MixCmsContext>.HandleException<bool>(ex, isRoot, transaction);
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

        public static async Task SendMail(string mixDatabase, string culture, JObject data)
        {
            var getEdmInfo = await GetSingleDataAsync<FormViewModel>(mixDatabase, "databaseName", MixDatabaseNames.EDM, culture);
            if (getEdmInfo.IsSucceed)
            {
                var edm = getEdmInfo.Data;
                bool sendToSender = edm.Property<bool>("sendToSender");
                string senderColumnName = edm.Property<string>("senderColumnName");
                string recipients = edm.Property<string>("recipients");
                string senderEmail = GetJToken(senderColumnName, data)?.Value<string>();

                if (sendToSender && !string.IsNullOrEmpty(senderEmail))
                {
                    string senderBody = GetEdmBody(edm.Property<string>("senderTemplate"), data);
                    MixService.SendMail(
                        edm.Property<string>("title"),
                        senderBody,
                        senderEmail,
                        edm.Property<string>("from")
                        );
                }

                if (!string.IsNullOrEmpty(recipients))
                {
                    string senderBody = GetEdmBody(edm.Property<string>("adminTemplate"), data);
                    MixService.SendMail(
                        edm.Property<string>("title"),
                        senderBody,
                        senderEmail,
                        edm.Property<string>("from")
                        );
                }
            }
        }

        private static string GetEdmBody(string template, JObject data)
        {
            string regex = @"(\[\[)(\w+)(\]\])";
            Regex rgx = new Regex(regex, RegexOptions.IgnoreCase);
            var matches = rgx.Matches(template);
            if (matches.Count > 0)
            {
                foreach (Match m in matches)
                {
                    var colName = m.Groups[2].Value;
                    JToken val = GetJToken(m.Groups[2].Value, data);
                    template = template.Replace($"[[{colName}]]", val?.Value<string>() ?? m.Groups[0].Value);
                }
            }
            template = template.Replace($"[[createdDateTime]]", DateTime.Now.ToLongDateString());
            return template;
        }

        private static JToken GetJToken(string path, JObject data)
        {
            JToken result = data;
            string[] names = path.Split('.');
            foreach (var name in names)
            {
                result = result[name];
                if (result is null)
                {
                    break;
                }
            }
            return result;
        }

        public static async Task<RepositoryResponse<AdditionalViewModel>> GetAdditionalData(
            MixDatabaseParentType parentType, string parentId,
            HttpRequest request, string culture = null,
            MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                // Additional Data is sub data of page / post / module only
                culture = culture ?? MixService.GetAppSetting<string>(MixAppSettingKeywords.DefaultCulture);
                var databaseName = request.Query["databaseName"].ToString();

                var result = await LoadAdditionalDataAsync(parentType, parentId, databaseName, culture, context, transaction);
                UnitOfWorkHelper<MixCmsContext>.HandleTransaction(true, isRoot, transaction);
                return result;
            }
            catch (Exception ex)
            {
                return UnitOfWorkHelper<MixCmsContext>.HandleException<AdditionalViewModel>(ex, isRoot, transaction);
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

        public static async Task<RepositoryResponse<AdditionalViewModel>> LoadAdditionalDataAsync(
            MixDatabaseParentType parentType,
            string parentId,
            string databaseName,
            string culture = null,
            MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            culture = culture ?? MixService.GetAppSetting<string>("DefaultCulture");
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                var dataId = (await context.MixDatabaseDataAssociation.FirstOrDefaultAsync(
                    m => m.MixDatabaseName == databaseName && m.ParentType == parentType && m.ParentId == parentId && m.Specificulture == culture))?.DataId;
                if (!string.IsNullOrEmpty(dataId))
                {
                    var result = await AdditionalViewModel.Repository.GetFirstModelAsync(
                        m => m.Id == dataId && m.Specificulture == culture
                        , context, transaction);
                    if (result.IsSucceed)
                    {
                        result.Data.ParentId = parentId;
                        result.Data.ParentType = parentType;
                    }
                    return result;
                }
                else
                {
                    // Init default data
                    var getAttrSet = await Lib.ViewModels.MixDatabases.UpdateViewModel.Repository.GetSingleModelAsync(
                    m => m.Name == databaseName
                    , context, transaction);
                    if (getAttrSet.IsSucceed)
                    {
                        AdditionalViewModel result = new AdditionalViewModel()
                        {
                            Id = Guid.NewGuid().ToString(),
                            Specificulture = culture,
                            MixDatabaseId = getAttrSet.Data.Id,
                            MixDatabaseName = getAttrSet.Data.Name,
                            Status = MixContentStatus.Published,
                            Columns = getAttrSet.Data.Columns,
                            ParentType = parentType,
                            ParentId = parentId,
                            CreatedDateTime = DateTime.UtcNow
                        };
                        await result.SaveModelAsync(false, context, transaction);
                        result.ExpandView(context, transaction);
                        return new RepositoryResponse<AdditionalViewModel>()
                        {
                            IsSucceed = true,
                            Data = result
                        };
                    }
                    return new RepositoryResponse<AdditionalViewModel>();
                }
            }
            catch (Exception ex)
            {
                return UnitOfWorkHelper<MixCmsContext>.HandleException<AdditionalViewModel>(ex, isRoot, transaction);
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

        public static async Task<RepositoryResponse<FormViewModel>> SaveObjAsync(string databaseName, JObject obj, string parentId = null, MixDatabaseParentType? parentType = null, string culture = null)
        {
            culture ??= MixService.GetAppSetting<string>("DefaultCulture");
            string id = obj.Value<string>("id");
            FormViewModel formData;

            if (id == null)
            {
                formData = await GetFormDataAsync(databaseName, culture);
            }
            else
            {
                var getFormData = await FormViewModel.Repository.GetSingleModelAsync(m => m.Id == id && m.Specificulture == culture);
                formData = getFormData.Data;
            }

            if (formData != null)
            {
                formData.ParentId = parentId;
                if (parentType.HasValue)
                {
                    formData.ParentType = parentType.Value;
                }
                formData.Obj = obj;
                return await formData.SaveModelAsync(true);
            }
            return new();
        }

        public static async Task<FormViewModel> GetFormDataAsync(string mixDatabase, string culture)
        {
            _ = int.TryParse(mixDatabase, out int mixDatabaseId);
            var getDatabase = await MixDatabases.UpdateViewModel.Repository.GetSingleModelAsync(
                m => m.Name == mixDatabase || m.Id == mixDatabaseId);
            if (getDatabase.IsSucceed)
            {
                FormViewModel result = new FormViewModel()
                {
                    Specificulture = culture,
                    MixDatabaseId = getDatabase.Data.Id,
                    MixDatabaseName = getDatabase.Data.Name,
                    Status = MixContentStatus.Published,
                    Columns = getDatabase.Data.Columns,
                    Obj = new()
                };
                foreach (var item in result.Columns)
                {
                    result.Obj.Add(new JProperty(item.Name, item.DefaultValue ?? string.Empty));
                }
                return result;
            }
            return null;
        }

        public static RepositoryResponse<AdditionalViewModel> LoadAdditionalData(
            MixDatabaseParentType parentType,
            string parentId,
            string databaseName,
            string culture = null,
            MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            culture = culture ?? MixService.GetAppSetting<string>("DefaultCulture");
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                var dataId = context.MixDatabaseDataAssociation.FirstOrDefault(
                    m => m.MixDatabaseName == databaseName && m.ParentType == parentType && m.ParentId == parentId && m.Specificulture == culture)?.DataId;
                if (!string.IsNullOrEmpty(dataId))
                {
                    return AdditionalViewModel.Repository.GetSingleModel(
                        m => m.Id == dataId && m.Specificulture == culture
                        , context, transaction);
                }
                else
                {
                    // Init default data
                    var getAttrSet = MixDatabases.UpdateViewModel.Repository.GetSingleModel(
                    m => m.Name == databaseName
                    , context, transaction);
                    if (getAttrSet.IsSucceed)
                    {
                        AdditionalViewModel result = new AdditionalViewModel()
                        {
                            Specificulture = culture,
                            MixDatabaseId = getAttrSet.Data.Id,
                            MixDatabaseName = getAttrSet.Data.Name,
                            Status = MixContentStatus.Published,
                            Columns = getAttrSet.Data.Columns,
                            ParentType = parentType,
                            ParentId = parentId
                        };
                        result.ExpandView(context, transaction);
                        return new RepositoryResponse<AdditionalViewModel>()
                        {
                            IsSucceed = true,
                            Data = result
                        };
                    }
                    return new RepositoryResponse<AdditionalViewModel>();
                }
            }
            catch (Exception ex)
            {
                return UnitOfWorkHelper<MixCmsContext>.HandleException<AdditionalViewModel>(ex, isRoot, transaction);
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

        private static List<ImportViewModel> LoadFileData(
           string culture, Lib.ViewModels.MixDatabases.ReadViewModel mixDatabase, IFormFile file)
        {
            //create a list to hold all the values
            List<ImportViewModel> excelData = new List<ImportViewModel>();

            //create a new Excel package in a memorystream
            using (var stream = file.OpenReadStream())
            using (ExcelPackage excelPackage = new ExcelPackage(stream))
            {
                //loop all worksheets
                foreach (ExcelWorksheet worksheet in excelPackage.Workbook.Worksheets)
                {
                    // First row is supose to be headers (list field name) => start from row 2
                    int startRow = 2;//worksheet.Dimension.Start.Row
                    //loop all rows
                    for (int i = startRow; i <= worksheet.Dimension.End.Row; i++)
                    {
                        JObject obj = new JObject();
                        //loop all columns in a row
                        for (int j = worksheet.Dimension.Start.Column; j <= worksheet.Dimension.End.Column; j++)
                        {
                            obj.Add(new JProperty(worksheet.Cells[1, j].Value.ToString(), worksheet.Cells[i, j].Value));
                        }
                        ImportViewModel data = new ImportViewModel()
                        {
                            Id = obj["id"]?.ToString(),
                            MixDatabaseId = mixDatabase.Id,
                            MixDatabaseName = mixDatabase.Name,
                            Specificulture = culture,
                            Obj = obj
                        };
                        excelData.Add(data);
                    }
                }
                return excelData;
            }
        }

        public static async Task<RepositoryResponse<PaginationModel<TView>>> FilterByKeywordAsync<TView>(string culture, string mixDatabaseName
            , RequestPaging request, string keyword
            , Dictionary<string, Microsoft.Extensions.Primitives.StringValues> queryDictionary = null
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            where TView : ViewModelBase<MixCmsContext, MixDatabaseData, TView>
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                Expression<Func<MixDatabaseDataValue, bool>> attrPredicate =
                    m => m.Specificulture == culture && m.MixDatabaseName == mixDatabaseName
                     && (!request.FromDate.HasValue
                                        || (m.CreatedDateTime >= request.FromDate.Value)
                                    )
                                    && (!request.ToDate.HasValue
                                        || (m.CreatedDateTime <= request.ToDate.Value)
                                        )
                    ;
                Expression<Func<MixDatabaseDataValue, bool>> valPredicate = null;
                RepositoryResponse<PaginationModel<TView>> result = new RepositoryResponse<PaginationModel<TView>>()
                {
                    IsSucceed = true,
                    Data = new PaginationModel<TView>()
                };
                var filterType = queryDictionary.FirstOrDefault(q => q.Key == "filterType");
                var tasks = new List<Task<RepositoryResponse<TView>>>();
                if (queryDictionary != null)
                {
                    foreach (var q in queryDictionary)
                    {
                        if (!string.IsNullOrEmpty(q.Key) && q.Key != "mixDatabaseId" && q.Key != "mixDatabaseName" && q.Key != "filterType" && !string.IsNullOrEmpty(q.Value))
                        {
                            if (!string.IsNullOrEmpty(filterType.Value) && filterType.Value == "equal")
                            {
                                Expression<Func<MixDatabaseDataValue, bool>> pre = m =>
                                    m.MixDatabaseColumnName == q.Key && m.StringValue == (q.Value.ToString());
                                valPredicate = valPredicate == null
                                    ? pre
                                    : valPredicate = valPredicate.AndAlso(pre);
                            }
                            else
                            {
                                Expression<Func<MixDatabaseDataValue, bool>> pre =
                                    m => m.MixDatabaseColumnName == q.Key &&
                                    (EF.Functions.Like(m.StringValue, $"%{q.Value}%"));
                                valPredicate = valPredicate == null
                                    ? pre
                                    : valPredicate = valPredicate.AndAlso(pre);
                            }
                        }
                    }
                    if (valPredicate != null)
                    {
                        attrPredicate = valPredicate.AndAlso(attrPredicate);
                    }
                }
                // Loop queries string => predicate
                if (!string.IsNullOrEmpty(keyword))
                {
                    Expression<Func<MixDatabaseDataValue, bool>> pre = m => m.MixDatabaseName == mixDatabaseName && m.Specificulture == culture && m.StringValue.Contains(keyword);
                    attrPredicate = attrPredicate.AndAlso(pre);
                }

                var query = context.MixDatabaseDataValue.Where(attrPredicate).Select(m => m.DataId).Distinct();
                var dataIds = query.ToList();
                if (query != null)
                {
                    Expression<Func<MixDatabaseData, bool>> predicate = m => dataIds.Any(id => m.Id == id);
                    result = await DefaultRepository<MixCmsContext, MixDatabaseData, TView>.Instance.GetModelListByAsync(
                        predicate, request.OrderBy, request.Direction, request.PageSize, request.PageIndex, null, null, context, transaction);
                }
                return result;
            }
            catch (Exception ex)
            {
                return UnitOfWorkHelper<MixCmsContext>.HandleException<PaginationModel<TView>>(ex, isRoot, transaction);
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

        public static async Task<RepositoryResponse<PaginationModel<TView>>> FilterByKeywordAsync<TView>(HttpRequest request, string culture = null, string mixDatabaseName = null, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            where TView : ViewModelBase<MixCmsContext, MixDatabaseData, TView>
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                culture = culture ?? MixService.GetAppSetting<string>("DefaultCulture");
                var queryDictionary = request.Query.ToList();
                mixDatabaseName = mixDatabaseName ?? request.Query["mixDatabaseName"].ToString().Trim();
                var keyword = request.Query["keyword"].ToString();
                var filterType = request.Query["filterType"].ToString();
                var orderBy = request.Query["orderBy"].ToString();
                int.TryParse(request.Query["mixDatabaseId"], out int mixDatabaseId);
                bool isDirection = Enum.TryParse(request.Query["direction"], out Heart.Enums.DisplayDirection direction);
                bool.TryParse(request.Query["isGroup"], out bool isGroup);
                int.TryParse(request.Query["pageIndex"], out int pageIndex);
                var isPageSize = int.TryParse(request.Query["pageSize"], out int pageSize);
                bool isFromDate = DateTime.TryParse(request.Query["fromDate"], out DateTime fromDate);
                bool isToDate = DateTime.TryParse(request.Query["toDate"], out DateTime toDate);
                bool isStatus = Enum.TryParse(request.Query["status"], out MixContentStatus status);
                var tasks = new List<Task<RepositoryResponse<TView>>>();
                var getfields = await MixDatabaseColumns.ReadViewModel.Repository.GetModelListByAsync(
                    m => m.MixDatabaseId == mixDatabaseId || m.MixDatabaseName == mixDatabaseName, context, transaction);
                var fields = getfields.IsSucceed ? getfields.Data : new List<MixDatabaseColumns.ReadViewModel>();
                var fieldQueries = !string.IsNullOrEmpty(request.Query["query"]) ? JObject.Parse(request.Query["query"]) : new JObject();

                // Data predicate
                Expression<Func<MixDatabaseData, bool>> predicate = m => m.Specificulture == culture
                   && (m.MixDatabaseName == mixDatabaseName);

                // val predicate
                Expression<Func<MixDatabaseDataValue, bool>> attrPredicate = m => m.Specificulture == culture
                   && (m.MixDatabaseName == mixDatabaseName);

                RepositoryResponse<PaginationModel<TView>> result = new RepositoryResponse<PaginationModel<TView>>()
                {
                    IsSucceed = true,
                    Data = new PaginationModel<TView>()
                };

                // if filter by field name or keyword => filter by attr value
                if (fieldQueries.Count > 0 || !string.IsNullOrEmpty(keyword))
                {
                    // filter by all fields if have keyword
                    if (!string.IsNullOrEmpty(keyword))
                    {
                        Expression<Func<MixDatabaseDataValue, bool>> pre = null;
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

                    if (fieldQueries != null && fieldQueries.Properties().Count() > 0) // filter by specific field name
                    {
                        var valPredicate = GetFilterValueByFields(fields, fieldQueries, filterType);
                        attrPredicate = attrPredicate.AndAlsoIf(valPredicate != null, valPredicate);
                    }

                    var valDataIds = context.MixDatabaseDataValue.Where(attrPredicate).Select(m => m.DataId).Distinct();
                    predicate = predicate.AndAlsoIf(valDataIds != null, m => valDataIds.Any(id => m.Id == id));
                }
                else
                {
                    predicate = m => m.Specificulture == culture && (m.MixDatabaseId == mixDatabaseId || m.MixDatabaseName == mixDatabaseName);
                    predicate = predicate.AndAlsoIf(isStatus, m => m.Status == status);
                    predicate = predicate.AndAlsoIf(isFromDate, m => m.CreatedDateTime >= fromDate);
                    predicate = predicate.AndAlsoIf(isToDate, m => m.CreatedDateTime <= toDate);
                }

                if (isGroup)
                {
                    var excludeIds = context.MixDatabaseDataAssociation.Where(
                        m => (m.MixDatabaseId == mixDatabaseId || m.MixDatabaseName == mixDatabaseName)
                        && m.Specificulture == culture
                        && m.ParentType == MixDatabaseParentType.Set
                        && !string.IsNullOrEmpty(m.ParentId))
                        .Select(m => m.DataId);
                    predicate = predicate.AndAlso(m => !excludeIds.Any(n => n == m.Id));
                }

                result = await DefaultRepository<MixCmsContext, MixDatabaseData, TView>.Instance.GetModelListByAsync(
                            predicate,
                            orderBy, direction, isPageSize ? pageSize : default, isPageSize ? pageIndex : 0, null, null, context, transaction);
                return result;
            }
            catch (Exception ex)
            {
                return UnitOfWorkHelper<MixCmsContext>.HandleException<PaginationModel<TView>>(ex, isRoot, transaction);
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

        public static async Task<RepositoryResponse<List<TView>>> GetAllRootDataAsync<TView>(
            string mixDatabaseName,
            string culture,
            MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            where TView : ViewModelBase<MixCmsContext, MixDatabaseData, TView>
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                Expression<Func<MixDatabaseData, bool>> predicate = m => m.MixDatabaseName == mixDatabaseName && m.Specificulture == culture;
                var excludeIds = context.MixDatabaseDataAssociation.Where(
                    m => m.MixDatabaseName == mixDatabaseName
                    && m.Specificulture == culture
                    && m.ParentType == MixDatabaseParentType.Set
                    && !string.IsNullOrEmpty(m.ParentId))
                    .Select(m => m.DataId);

                predicate = predicate.AndAlso(m => !excludeIds.Any(n => n == m.Id));

                return await DefaultRepository<MixCmsContext, MixDatabaseData, TView>.Instance.GetModelListByAsync(
                            predicate,
                            context, transaction);
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

        private static Expression<Func<MixDatabaseDataValue, bool>> GetFilterValueByFields(List<MixDatabaseColumns.ReadViewModel> fields, JObject fieldQueries, string filterType)
        {
            Expression<Func<MixDatabaseDataValue, bool>> valPredicate = null;
            foreach (var q in fieldQueries)
            {
                if (fields.Any(f => f.Name == q.Key))
                {
                    string value = q.Value.ToString();
                    if (!string.IsNullOrEmpty(value))
                    {
                        Expression<Func<MixDatabaseDataValue, bool>> pre = m => m.MixDatabaseColumnName == q.Key;
                        pre = pre.AndAlsoIf(filterType == "equal", m => m.StringValue == (q.Value.ToString()));
                        pre = pre.AndAlsoIf(filterType == "contain", m => EF.Functions.Like(m.StringValue, $"%{q.Value}%"));

                        valPredicate = valPredicate == null
                            ? pre
                            : valPredicate.Or(pre);
                    }
                }
            }
            return valPredicate;
        }

        public static async Task<RepositoryResponse<TView>> GetSingleDataAsync<TView>(
            string keyword
            , string columnName
            , string mixDatabaseName
            , string culture = null
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            where TView : ViewModelBase<MixCmsContext, MixDatabaseData, TView>
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                culture ??= MixService.GetAppSetting<string>(MixAppSettingKeywords.DefaultCulture);
                Expression<Func<MixDatabaseDataValue, bool>> attrPredicate = m => m.Specificulture == culture && m.MixDatabaseName == mixDatabaseName;
                Expression<Func<MixDatabaseDataValue, bool>> valPredicate = null;
                RepositoryResponse<TView> result = new RepositoryResponse<TView>()
                {
                    IsSucceed = false,
                };
                Expression<Func<MixDatabaseDataValue, bool>> pre =
                    m => m.MixDatabaseColumnName == columnName && EF.Functions.Like(m.StringValue, keyword);

                valPredicate = valPredicate == null
                               ? pre
                               : valPredicate = valPredicate.AndAlso(pre);
                if (valPredicate != null)
                {
                    attrPredicate = valPredicate.AndAlso(attrPredicate);
                }

                var query = context.MixDatabaseDataValue.Where(attrPredicate).Select(m => m.DataId).Distinct();
                if (query != null)
                {
                    var dataId = query.FirstOrDefault();
                    Expression<Func<MixDatabaseData, bool>> predicate =
                        m => m.Specificulture == culture && m.Id == dataId;
                    result = await DefaultRepository<MixCmsContext, MixDatabaseData, TView>.Instance.GetFirstModelAsync(
                        predicate, context, transaction);
                }
                return result;
            }
            catch (Exception ex)
            {
                return UnitOfWorkHelper<MixCmsContext>.HandleException<TView>(ex, isRoot, transaction);
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

        public static RepositoryResponse<TView> GetSingleDataByParentId<TView>(
            string parentId
            , MixDatabaseParentType parentType
            , string culture = null
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            where TView : ViewModelBase<MixCmsContext, MixDatabaseData, TView>
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                culture ??= MixService.GetAppSetting<string>(MixAppSettingKeywords.DefaultCulture);
                var dataId = _context.MixDatabaseDataAssociation.FirstOrDefault(
                        m => m.ParentId == parentId && m.ParentType == parentType)?.DataId;
                if (dataId is not null)
                {
                    return DefaultRepository<MixCmsContext, MixDatabaseData, TView>.Instance.GetSingleModel(
                        m => m.Id == dataId);
                }
                return default;
            }
            catch (Exception ex)
            {
                return UnitOfWorkHelper<MixCmsContext>.HandleException<TView>(ex, isRoot, transaction);
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

        public static async Task<RepositoryResponse<TView>> GetSingleDataByParentIdAsync<TView>(
            string parentId
            , MixDatabaseParentType parentType
            , string culture = null
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            where TView : ViewModelBase<MixCmsContext, MixDatabaseData, TView>
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                culture ??= MixService.GetAppSetting<string>(MixAppSettingKeywords.DefaultCulture);
                var dataId = context.MixDatabaseDataAssociation.FirstOrDefault(
                        m => m.ParentId == parentId && m.ParentType == parentType)?.DataId;
                if (dataId is not null)
                {
                    return await DefaultRepository<MixCmsContext, MixDatabaseData, TView>.Instance.GetSingleModelAsync(
                        m => m.Id == dataId, context, transaction);
                }
                return default;
            }
            catch (Exception ex)
            {
                return UnitOfWorkHelper<MixCmsContext>.HandleException<TView>(ex, isRoot, transaction);
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

        public static RepositoryResponse<TView> GetSingleData<TView>(
            string keyword
            , string columnName
            , string mixDatabaseName
            , string culture = null
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            where TView : ViewModelBase<MixCmsContext, MixDatabaseData, TView>
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                culture ??= MixService.GetAppSetting<string>(MixAppSettingKeywords.DefaultCulture);
                Expression<Func<MixDatabaseDataValue, bool>> attrPredicate = m => m.Specificulture == culture && m.MixDatabaseName == mixDatabaseName;
                Expression<Func<MixDatabaseDataValue, bool>> valPredicate = null;
                RepositoryResponse<TView> result = new RepositoryResponse<TView>()
                {
                    IsSucceed = false,
                };
                Expression<Func<MixDatabaseDataValue, bool>> pre =
                    m => m.MixDatabaseColumnName == columnName && EF.Functions.Like(m.StringValue, keyword);

                valPredicate = valPredicate == null
                               ? pre
                               : valPredicate = valPredicate.AndAlso(pre);
                if (valPredicate != null)
                {
                    attrPredicate = valPredicate.AndAlso(attrPredicate);
                }

                var query = context.MixDatabaseDataValue.Where(attrPredicate).Select(m => m.DataId).Distinct();
                if (query != null)
                {
                    var dataId = query.FirstOrDefault();
                    Expression<Func<MixDatabaseData, bool>> predicate =
                        m => m.Specificulture == culture && m.Id == dataId;
                    result = DefaultRepository<MixCmsContext, MixDatabaseData, TView>.Instance.GetFirstModel(
                        predicate, context, transaction);
                }
                return result;
            }
            catch (Exception ex)
            {
                return UnitOfWorkHelper<MixCmsContext>.HandleException<TView>(ex, isRoot, transaction);
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

        public static async Task<RepositoryResponse<List<TView>>> FilterByKeywordAsync<TView>(string culture, string mixDatabaseName
            , string filterType, string fieldName, string keyword
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            where TView : ViewModelBase<MixCmsContext, MixDatabaseData, TView>
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                Expression<Func<MixDatabaseDataValue, bool>> attrPredicate = m => m.Specificulture == culture && m.MixDatabaseName == mixDatabaseName;
                Expression<Func<MixDatabaseDataValue, bool>> valPredicate = null;
                RepositoryResponse<List<TView>> result = new RepositoryResponse<List<TView>>()
                {
                    IsSucceed = true,
                    Data = new List<TView>()
                };
                if (filterType == "equal")
                {
                    Expression<Func<MixDatabaseDataValue, bool>> pre = m => m.MixDatabaseColumnName == fieldName && EF.Functions.Like(m.StringValue, keyword);

                    valPredicate = valPredicate == null
                                   ? pre
                                   : valPredicate = valPredicate.AndAlso(pre);
                }
                else
                {
                    Expression<Func<MixDatabaseDataValue, bool>> pre = m => m.MixDatabaseColumnName == fieldName && m.StringValue.Contains(keyword);
                    valPredicate = valPredicate == null
                                   ? pre
                                   : valPredicate = valPredicate.AndAlso(pre);
                }
                if (valPredicate != null)
                {
                    attrPredicate = valPredicate.AndAlso(attrPredicate);
                }

                var query = context.MixDatabaseDataValue.Where(attrPredicate).Select(m => m.DataId).Distinct();
                var dataIds = query.ToList();
                if (query != null)
                {
                    Expression<Func<MixDatabaseData, bool>> predicate = m => m.Specificulture == culture && dataIds.Any(id => m.Id == id);
                    result = await DefaultRepository<MixCmsContext, MixDatabaseData, TView>.Instance.GetModelListByAsync(
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

        public static RepositoryResponse<List<TView>> FilterByKeyword<TView>(string culture, string mixDatabaseName
            , string filterType, string fieldName, string keyword
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            where TView : ViewModelBase<MixCmsContext, MixDatabaseData, TView>
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                Expression<Func<MixDatabaseDataValue, bool>> attrPredicate = m => m.Specificulture == culture && m.MixDatabaseName == mixDatabaseName;
                Expression<Func<MixDatabaseDataValue, bool>> valPredicate = null;
                RepositoryResponse<List<TView>> result = new RepositoryResponse<List<TView>>()
                {
                    IsSucceed = true,
                    Data = new List<TView>()
                };
                if (filterType == "equal")
                {
                    Expression<Func<MixDatabaseDataValue, bool>> pre = m => m.MixDatabaseColumnName == fieldName && EF.Functions.Like(m.StringValue, keyword);

                    valPredicate = valPredicate == null
                                   ? pre
                                   : valPredicate = valPredicate.AndAlso(pre);
                }
                else
                {
                    Expression<Func<MixDatabaseDataValue, bool>> pre = m => m.MixDatabaseColumnName == fieldName && m.StringValue.Contains(keyword);
                    valPredicate = valPredicate == null
                                   ? pre
                                   : valPredicate = valPredicate.AndAlso(pre);
                }
                if (valPredicate != null)
                {
                    attrPredicate = valPredicate.AndAlso(attrPredicate);
                }

                var query = context.MixDatabaseDataValue.Where(attrPredicate).Select(m => m.DataId).Distinct();
                var dataIds = query.ToList();
                if (query != null)
                {
                    Expression<Func<MixDatabaseData, bool>> predicate = m => m.Specificulture == culture && dataIds.Any(id => m.Id == id);
                    result = DefaultRepository<MixCmsContext, MixDatabaseData, TView>.Instance.GetModelListBy(
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

        public static async Task<RepositoryResponse<PaginationModel<TView>>> GetMixDataByParent<TView>(
            string culture, string mixDatabaseName,
            string parentId, MixDatabaseParentType parentType,
            string orderBy, Heart.Enums.DisplayDirection direction,
            int? pageSize, int? pageIndex,
            MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            where TView : ViewModelBase<MixCmsContext, MixDatabaseData, TView>
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                var tasks = new List<Task<RepositoryResponse<TView>>>();

                Expression<Func<MixDatabaseDataAssociation, bool>> predicate = m => m.Specificulture == culture
                    && (m.MixDatabaseName == mixDatabaseName)
                    && (m.Status == MixContentStatus.Published)
                    && (string.IsNullOrEmpty(parentId)
                         || (m.ParentId == parentId && m.ParentType == parentType)
                         );
                ;
                var query = context.MixDatabaseDataAssociation.Where(predicate).Select(m => m.DataId).Distinct();
                var dataIds = query.ToList();
                Expression<Func<MixDatabaseData, bool>> pre = m => dataIds.Any(id => m.Id == id);
                return await DefaultRepository<MixCmsContext, MixDatabaseData, TView>.Instance.GetModelListByAsync(
                            pre, orderBy, direction, pageSize, pageIndex, null, null, context, transaction);
            }
            catch (Exception ex)
            {
                return UnitOfWorkHelper<MixCmsContext>.HandleException<PaginationModel<TView>>(ex, isRoot, transaction);
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

        public static RepositoryResponse<FileViewModel> ExportAttributeToExcel(List<JObject> lstData, string sheetName
          , string folderPath, string fileName
          , List<string> headers = null)
        {
            var result = new RepositoryResponse<FileViewModel>()
            {
                Data = new FileViewModel()
                {
                    FileFolder = folderPath,
                    Filename = fileName + "-" + DateTime.Now.ToString("yyyyMMdd"),
                    Extension = ".xlsx"
                }
            };
            try
            {
                if (lstData.Count > 0)
                {
                    var filenameE = $"{ result.Data.Filename}{result.Data.Extension}";

                    // create new data table
                    var dtable = new DataTable();

                    if (headers == null)
                    {
                        // get first item
                        var listColumn = lstData[0].Properties();

                        // add column name to table
                        foreach (var item in listColumn)
                        {
                            dtable.Columns.Add(item.Name, typeof(string));
                        }
                    }
                    else
                    {
                        foreach (var item in headers)
                        {
                            dtable.Columns.Add(item, typeof(string));
                        }
                    }

                    // Row value
                    foreach (var a in lstData)
                    {
                        var r = dtable.NewRow();
                        foreach (var prop in a.Properties())
                        {
                            bool isHaveValue = a.TryGetValue(prop.Name, out JToken val);
                            if (isHaveValue)
                            {
                                r[prop.Name] = val.ToString();
                            }
                        }
                        dtable.Rows.Add(r);
                    }

                    // Save Excel file
                    using (var pck = new ExcelPackage())
                    {
                        string SheetName = sheetName != string.Empty ? sheetName : "Report";
                        var wsDt = pck.Workbook.Worksheets.Add(SheetName);
                        wsDt.Cells["A1"].LoadFromDataTable(dtable, true, TableStyles.None);
                        wsDt.Cells[wsDt.Dimension.Address].AutoFitColumns();

                        MixCommonHelper.SaveFileBytes(folderPath, filenameE, pck.GetAsByteArray());
                        result.IsSucceed = true;

                        return result;
                    }
                }
                else
                {
                    result.Errors.Add("Can not export data of empty list");
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Errors.Add(ex.Message);
                return result;
            }
        }

        public static JObject ParseData(
            string dataId,
            string culture,
            MixCmsContext _context = null,
            IDbContextTransaction _transaction = null)
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(
                    _context, _transaction,
                    out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            var values = context.MixDatabaseDataValue.Where(
                m => m.DataId == dataId && m.Specificulture == culture
                    && !string.IsNullOrEmpty(m.MixDatabaseColumnName));
            var properties = values.Select(m => m.ToJProperty(_context, _transaction));
            var obj = new JObject(
                new JProperty("id", dataId),
                properties
            );

            if (isRoot)
            {
                transaction.Dispose();
                context.Dispose();
            }

            return obj;
        }

        public static void CleanCache(this MixDatabaseData data, MixCmsContext context)
        {
            var tasks = new List<Task>();
            // Get Parent Ids
            var relatedModels = context.MixDatabaseDataAssociation.Where(
                p => p.DataId == data.Id && p.Specificulture == data.Specificulture)
                    .Select(m => new { navId = m.Id, parentId = m.ParentId });
            foreach (var model in relatedModels)
            {
                var parentKey = $"_{model.parentId}_{data.Specificulture}";
                var navKey = $"_{model.navId}_{data.Specificulture}";
                tasks.Add(MixCacheService.RemoveCacheAsync(typeof(MixDatabaseData), parentKey));
                tasks.Add(MixCacheService.RemoveCacheAsync(typeof(MixDatabaseDataAssociation), navKey));
            }
            Task.WhenAll(tasks);
        }

        public static async Task<bool> MigrateData(int databaseId, string culture)
        {
            using (var ctx = new MixCmsContext())
            {
                var getDatabase = await MixDatabases.UpdateViewModel.Repository.GetSingleModelAsync(m => m.Id == databaseId, ctx, null);
                if (getDatabase.IsSucceed)
                {
                    string databaseName = $"{MixConstants.CONST_MIXDB_PREFIX}{getDatabase.Data.Name}_{culture.Replace("-", "_")}";


                    int page = 0;
                    int pageSize = 10;
                    string truncateSql = $"DELETE FROM {databaseName};";
                    await ctx.Database.ExecuteSqlRawAsync(truncateSql);
                    var getData = await FormViewModel.Repository.GetModelListByAsync(
                        m => m.MixDatabaseName == getDatabase.Data.Name && m.Specificulture == culture
                        , "Id", Heart.Enums.DisplayDirection.Asc
                        , pageSize, page, null, null
                        , ctx, null);
                    while (page < getData.Data.TotalPage)
                    {
                        List<string> datas = new List<string>();
                        page++;
                        foreach (var item in getData.Data.Items)
                        {
                            datas.Add(GenerateInsertValuesSql(item, getDatabase.Data.Columns, ctx));
                        }
                        if (datas.Count > 0)
                        {
                            string columns = string.Join(", ", getDatabase.Data.Columns
                                .Select(c => c.Name).ToList());
                            string values = string.Join(", ", datas);

                            string commandText = $@"INSERT INTO {databaseName} (id, specificulture, mix_status, createdDateTime, {columns}) VALUES {values}";

                            await ctx.Database.ExecuteSqlRawAsync(commandText);

                            await ctx.SaveChangesAsync();
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        private static string GenerateInsertValuesSql(FormViewModel data, List<MixDatabaseColumns.UpdateViewModel> columns, MixCmsContext ctx)
        {
            List<string> values = new List<string>();
            values.Add(data.Id);
            values.Add(data.Specificulture);
            values.Add(data.Status.ToString());
            values.Add(data.CreatedDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            foreach (var col in columns)
            {
                if (col.DataType == MixDataType.Reference)
                {
                    var dbName = ctx.MixDatabase.FirstOrDefault(m => m.Id == col.ReferenceId).Name;
                    var arr = JArray.FromObject(ctx.MixDatabaseDataAssociation.Where(
                            m => m.ParentId == data.Id && m.MixDatabaseName == dbName).ToList());
                    JObject refData = new JObject(
                        new JProperty("ref_table_name", dbName),
                        new JProperty("children", arr));
                    values.Add(EncodeRefData(refData));
                }
                else
                {
                    values.Add(data.Obj.Value<string>(col.Name));
                }
            }
            return $"({string.Join(", ", values.Select(m => $"'{m}'"))})";
        }

        public static string EncodeRefData(JObject refData)
        {
            return Convert.ToBase64String(
                Encoding.Unicode
                .GetBytes(refData
                .ToString(Formatting.None)));
        }

        public static JObject DecodeRefData(string encoded)
        {
            var data = !string.IsNullOrEmpty(encoded) ? Convert.FromBase64String(encoded) : null;
            if (data != null)
            {
                return JObject.Parse(Encoding.Unicode.GetString(data));
            }
            return null;
        }
    }
}
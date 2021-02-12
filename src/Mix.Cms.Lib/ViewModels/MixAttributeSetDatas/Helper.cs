using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Extensions;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using Mix.Common.Helper;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.Repository;
using Mix.Domain.Data.ViewModels;
using Mix.Heart.Helpers;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Mix.Heart.Extensions;
using Mix.Services;

namespace Mix.Cms.Lib.ViewModels.MixAttributeSetDatas
{
    public static class Helper
    {
        public static async Task<RepositoryResponse<bool>> ImportData(
            string culture, Lib.ViewModels.MixAttributeSets.ReadViewModel attributeSet, IFormFile file)
        {

            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(null, null, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                List<ImportViewModel> data = LoadFileData(culture, attributeSet, file);

                var fields = MixAttributeFields.UpdateViewModel.Repository.GetModelListBy(f => f.AttributeSetId == attributeSet.Id, context, transaction).Data;
                foreach (var item in data)
                {
                    if (result.IsSucceed)
                    {
                        var isCreateNew = string.IsNullOrEmpty(item.Id);
                        item.Fields = fields;
                        item.AttributeSetName = attributeSet.Name;
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

        public static async Task<RepositoryResponse<AdditionalViewModel>> GetAdditionalData(
            MixDatabaseParentType parentType, string parentId,
            HttpRequest request, string culture = null,
            MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                // Additional Data is sub data of page / post / module only
                culture = culture ?? MixService.GetConfig<string>(MixAppSettingKeywords.DefaultCulture);
                var databaseName = request.Query["databaseName"].ToString();

                return await LoadAdditionalData(parentType, parentId, databaseName, culture, context, transaction);
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

        public static async Task<RepositoryResponse<AdditionalViewModel>> LoadAdditionalData(
            MixDatabaseParentType parentType,
            string parentId,
            string databaseName,
            string culture = null,
            MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            culture = culture ?? MixService.GetConfig<string>("DefaultCulture");
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                var dataId = (await context.MixRelatedAttributeData.FirstOrDefaultAsync(
                    m => m.AttributeSetName == databaseName && m.ParentType == parentType && m.ParentId == parentId && m.Specificulture == culture))?.DataId;
                if (!string.IsNullOrEmpty(dataId))
                {
                    return await AdditionalViewModel.Repository.GetSingleModelAsync(
                        m => m.Id == dataId && m.Specificulture == culture
                        , context, transaction);
                }
                else
                {
                    // Init default data
                    var getAttrSet = await Lib.ViewModels.MixAttributeSets.UpdateViewModel.Repository.GetSingleModelAsync(
                    m => m.Name == databaseName
                    , context, transaction);
                    if (getAttrSet.IsSucceed)
                    {
                        AdditionalViewModel result = new AdditionalViewModel()
                        {
                            Specificulture = culture,
                            AttributeSetId = getAttrSet.Data.Id,
                            AttributeSetName = getAttrSet.Data.Name,
                            Status = MixContentStatus.Published,
                            Fields = getAttrSet.Data.Fields,
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
           string culture, Lib.ViewModels.MixAttributeSets.ReadViewModel attributeSet, IFormFile file)
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
                            AttributeSetId = attributeSet.Id,
                            AttributeSetName = attributeSet.Name,
                            Specificulture = culture,
                            Obj = obj
                        };
                        excelData.Add(data);
                    }
                }
                return excelData;
            }
        }

        public static async Task<RepositoryResponse<PaginationModel<TView>>> FilterByKeywordAsync<TView>(string culture, string attributeSetName
            , RequestPaging request, string keyword
            , Dictionary<string, Microsoft.Extensions.Primitives.StringValues> queryDictionary = null
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            where TView : ViewModelBase<MixCmsContext, MixAttributeSetData, TView>
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                Expression<Func<MixAttributeSetValue, bool>> attrPredicate =
                    m => m.Specificulture == culture && m.AttributeSetName == attributeSetName
                     && (!request.FromDate.HasValue
                                        || (m.CreatedDateTime >= request.FromDate.Value)
                                    )
                                    && (!request.ToDate.HasValue
                                        || (m.CreatedDateTime <= request.ToDate.Value)
                                        )
                    ;
                Expression<Func<MixAttributeSetValue, bool>> valPredicate = null;
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
                        if (!string.IsNullOrEmpty(q.Key) && q.Key != "attributeSetId" && q.Key != "attributeSetName" && q.Key != "filterType" && !string.IsNullOrEmpty(q.Value))
                        {
                            if (!string.IsNullOrEmpty(filterType.Value) && filterType.Value == "equal")
                            {
                                Expression<Func<MixAttributeSetValue, bool>> pre = m =>
                                    m.AttributeFieldName == q.Key && m.StringValue == (q.Value.ToString());
                                valPredicate = valPredicate == null
                                    ? pre
                                    : valPredicate = valPredicate.AndAlso(pre);
                            }
                            else
                            {
                                Expression<Func<MixAttributeSetValue, bool>> pre =
                                    m => m.AttributeFieldName == q.Key &&
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
                    Expression<Func<MixAttributeSetValue, bool>> pre = m => m.AttributeSetName == attributeSetName && m.Specificulture == culture && m.StringValue.Contains(keyword);
                    attrPredicate = attrPredicate.AndAlso(pre);
                }

                var query = context.MixAttributeSetValue.Where(attrPredicate).Select(m => m.DataId).Distinct();
                var dataIds = query.ToList();
                if (query != null)
                {
                    Expression<Func<MixAttributeSetData, bool>> predicate = m => dataIds.Any(id => m.Id == id);
                    result = await DefaultRepository<MixCmsContext, MixAttributeSetData, TView>.Instance.GetModelListByAsync(
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

        public static async Task<RepositoryResponse<PaginationModel<TView>>> FilterByKeywordAsync<TView>(HttpRequest request, string culture = null, string attributeSetName = null, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            where TView : ViewModelBase<MixCmsContext, MixAttributeSetData, TView>
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                culture = culture ?? MixService.GetConfig<string>("DefaultCulture");
                var queryDictionary = request.Query.ToList();
                attributeSetName = attributeSetName ?? request.Query["attributeSetName"].ToString();
                var keyword = request.Query["keyword"].ToString();
                var filterType = request.Query["filterType"].ToString();
                var orderBy = request.Query["orderBy"].ToString();
                int.TryParse(request.Query["attributeSetId"], out int attributeSetId);
                bool isDirection = Enum.TryParse(request.Query["direction"], out Heart.Enums.MixHeartEnums.DisplayDirection direction);
                int.TryParse(request.Query["pageIndex"], out int pageIndex);
                var isPageSize = int.TryParse(request.Query["pageSize"], out int pageSize);
                bool isFromDate = DateTime.TryParse(request.Query["fromDate"], out DateTime fromDate);
                bool isToDate = DateTime.TryParse(request.Query["toDate"], out DateTime toDate);
                bool isStatus = Enum.TryParse(request.Query["status"], out MixContentStatus status);
                var tasks = new List<Task<RepositoryResponse<TView>>>();
                var getfields = await MixAttributeFields.ReadViewModel.Repository.GetModelListByAsync(
                    m => m.AttributeSetId == attributeSetId || m.AttributeSetName == attributeSetName, context, transaction);
                var fields = getfields.IsSucceed ? getfields.Data : new List<MixAttributeFields.ReadViewModel>();
                var fieldQueries = !string.IsNullOrEmpty(request.Query["query"]) ? JObject.Parse(request.Query["query"]) : new JObject();


                // Data predicate
                Expression<Func<MixAttributeSetData, bool>> predicate = m => m.Specificulture == culture
                   && (m.AttributeSetName == attributeSetName);

                // val predicate
                Expression<Func<MixAttributeSetValue, bool>> attrPredicate = m => m.Specificulture == culture
                   && (m.AttributeSetName == attributeSetName);

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
                        Expression<Func<MixAttributeSetValue, bool>> pre = null;
                        foreach (var field in fields)
                        {
                            Expression<Func<MixAttributeSetValue, bool>> keywordPredicate = m => m.AttributeFieldName == field.Name;
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
                        attrPredicate.AndAlsoIf(valPredicate != null, valPredicate);
                    }

                    var query = context.MixAttributeSetValue.Where(attrPredicate).Select(m => m.DataId).Distinct();
                    var dataIds = query.ToList();

                    predicate = predicate.AndAlsoIf(query != null, m => dataIds.Any(id => m.Id == id));
                }
                else
                {
                    predicate = m => m.Specificulture == culture
                    && (m.AttributeSetId == attributeSetId || m.AttributeSetName == attributeSetName)
                    && (!isStatus || (m.Status == status))
                    && (!isFromDate || (m.CreatedDateTime >= fromDate))
                    && (!isToDate || (m.CreatedDateTime <= toDate));
                }
                result = await DefaultRepository<MixCmsContext, MixAttributeSetData, TView>.Instance.GetModelListByAsync(
                            predicate, orderBy, direction, isPageSize ? pageSize : default, isPageSize ? pageIndex : 0, null, null, context, transaction);
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

        private static Expression<Func<MixAttributeSetValue, bool>> GetFilterValueByFields(List<MixAttributeFields.ReadViewModel> fields, JObject fieldQueries, string filterType)
        {
            Expression<Func<MixAttributeSetValue, bool>> valPredicate = null;
            foreach (var q in fieldQueries)
            {
                if (fields.Any(f => f.Name == q.Key))
                {
                    string value = q.Value.ToString();
                    if (!string.IsNullOrEmpty(value))
                    {
                        Expression<Func<MixAttributeSetValue, bool>> pre = m => m.AttributeFieldName == q.Key;
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

        public static async Task<RepositoryResponse<List<TView>>> FilterByKeywordAsync<TView>(string culture, string attributeSetName
            , string filterType, string fieldName, string keyword
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            where TView : ViewModelBase<MixCmsContext, MixAttributeSetData, TView>
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                Expression<Func<MixAttributeSetValue, bool>> attrPredicate = m => m.Specificulture == culture && m.AttributeSetName == attributeSetName;
                Expression<Func<MixAttributeSetValue, bool>> valPredicate = null;
                RepositoryResponse<List<TView>> result = new RepositoryResponse<List<TView>>()
                {
                    IsSucceed = true,
                    Data = new List<TView>()
                };
                if (filterType == "equal")
                {
                    Expression<Func<MixAttributeSetValue, bool>> pre = m => m.AttributeFieldName == fieldName && m.StringValue == keyword;

                    valPredicate = valPredicate == null
                                   ? pre
                                   : valPredicate = valPredicate.AndAlso(pre);
                }
                else
                {
                    Expression<Func<MixAttributeSetValue, bool>> pre = m => m.AttributeFieldName == fieldName && m.StringValue.Contains(keyword);
                    valPredicate = valPredicate == null
                                   ? pre
                                   : valPredicate = valPredicate.AndAlso(pre);
                }
                if (valPredicate != null)
                {
                    attrPredicate = valPredicate.AndAlso(attrPredicate);
                }

                var query = context.MixAttributeSetValue.Where(attrPredicate).Select(m => m.DataId).Distinct();
                var dataIds = query.ToList();
                if (query != null)
                {
                    Expression<Func<MixAttributeSetData, bool>> predicate = m => m.Specificulture == culture && dataIds.Any(id => m.Id == id);
                    result = await DefaultRepository<MixCmsContext, MixAttributeSetData, TView>.Instance.GetModelListByAsync(
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

        public static async Task<RepositoryResponse<PaginationModel<TView>>> GetAttributeDataByParent<TView>(
            string culture, string attributeSetName,
            string parentId, MixDatabaseParentType parentType,
            string orderBy, Heart.Enums.MixHeartEnums.DisplayDirection direction,
            int? pageSize, int? pageIndex,
            MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            where TView : ViewModelBase<MixCmsContext, MixAttributeSetData, TView>
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                var tasks = new List<Task<RepositoryResponse<TView>>>();

                Expression<Func<MixRelatedAttributeData, bool>> predicate = m => m.Specificulture == culture
                    && (m.AttributeSetName == attributeSetName)
                    && (m.Status == MixContentStatus.Published)
                    && (string.IsNullOrEmpty(parentId)
                         || (m.ParentId == parentId && m.ParentType == parentType)
                         );
                ;
                var query = context.MixRelatedAttributeData.Where(predicate).Select(m => m.DataId).Distinct();
                var dataIds = query.ToList();
                Expression<Func<MixAttributeSetData, bool>> pre = m => dataIds.Any(id => m.Id == id);
                return await DefaultRepository<MixCmsContext, MixAttributeSetData, TView>.Instance.GetModelListByAsync(
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

                        CommonHelper.SaveFileBytes(folderPath, filenameE, pck.GetAsByteArray());
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

        public static JObject ParseData(string dataId, string culture, MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(
                    _context, _transaction,
                    out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            var values = context.MixAttributeSetValue.Where(
                m => m.DataId == dataId && m.Specificulture == culture
                    && !string.IsNullOrEmpty(m.AttributeFieldName));
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

        public static void CleanCache(this MixAttributeSetData data, MixCmsContext context)
        {
            var tasks = new List<Task>();
            // Get Parent Ids
            var relatedModels = context.MixRelatedAttributeData.Where(
                p => p.DataId == data.Id && p.Specificulture == data.Specificulture)
                    .Select(m => new { navId = m.Id, parentId = m.ParentId });
            foreach (var model in relatedModels)
            {
                var parentKey = $"_{model.parentId}_{data.Specificulture}";
                var navKey = $"_{model.navId}_{data.Specificulture}";
                tasks.Add(MixService.RemoveCacheAsync(typeof(MixAttributeSetData), parentKey));
                tasks.Add(MixService.RemoveCacheAsync(typeof(MixRelatedAttributeData), navKey));
            }
            Task.WhenAll(tasks);
        }

    }
}
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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

namespace Mix.Cms.Lib.ViewModels.MixAttributeSetDatas
{
    public static class Helper
    {
        public static async Task<RepositoryResponse<bool>> ImportData(
            string culture, Lib.ViewModels.MixAttributeSets.ReadViewModel attributeSet, IFormFile file)
        {
            using (var context = new MixCmsContext())
            {
                var result = new RepositoryResponse<bool>() { IsSucceed = true };
                try
                {
                    List<ImportViewModel> data = LoadFileData(culture, attributeSet, file);

                    var fields = MixAttributeFields.UpdateViewModel.Repository.GetModelListBy(f => f.AttributeSetId == attributeSet.Id).Data;
                    var priority = ImportViewModel.Repository.Count(m => m.AttributeSetName == attributeSet.Name && m.Specificulture == culture).Data;
                    foreach (var item in data)
                    {
                        priority += 1;

                        item.Priority = priority;
                        item.Fields = fields;
                        item.AttributeSetName = attributeSet.Name;
                        item.Status = Enum.Parse<MixEnums.MixContentStatus>(MixService.GetConfig<string>(MixConstants.ConfigurationKeyword.DefaultContentStatus));
                        item.ParseModel();
                        context.Entry(item.Model).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                        foreach (var val in item.Values)
                        {
                            val.DataId = item.Id;
                            val.Specificulture = culture;
                            val.ParseModel();
                            context.Entry(val.Model).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                        }
                    }
                    int tmp = await context.SaveChangesAsync();
                    return result;
                }
                catch (Exception ex)
                {
                    result.IsSucceed = false;
                    result.Exception = ex;
                    result.Errors.Add(ex.Message);
                    return result;
                }
            }
        }

        public static async Task<RepositoryResponse<AddictionalViewModel>> GetAddictionalData(
            MixEnums.MixAttributeSetDataType parentType, int parentId,
            HttpRequest request, string culture = null,
            MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                // Addictional Data is sub data of page / post / module only
                culture = culture ?? MixService.GetConfig<string>("DefaultCulture");
                var databaseName = request.Query["databaseName"].ToString();
                var dataId = (await context.MixRelatedAttributeData.FirstOrDefaultAsync(
                    m => m.AttributeSetName == databaseName && m.ParentType == parentType.ToString() && m.ParentId == parentId.ToString() && m.Specificulture == culture))?.DataId;
                if (!string.IsNullOrEmpty(dataId))
                {
                    return await AddictionalViewModel.Repository.GetSingleModelAsync(
                        m => m.Id == dataId && m.Specificulture == culture);
                }
                else
                {
                    // Init default data
                    var getAttrSet = await Lib.ViewModels.MixAttributeSets.UpdateViewModel.Repository.GetSingleModelAsync(
                    m => m.Name == request.Query["databaseName"].ToString());
                    if (getAttrSet.IsSucceed)
                    {
                        AddictionalViewModel result = new AddictionalViewModel()
                        {
                            Specificulture = culture,
                            AttributeSetId = getAttrSet.Data.Id,
                            AttributeSetName = getAttrSet.Data.Name,
                            Status = MixEnums.MixContentStatus.Published,
                            Fields = getAttrSet.Data.Fields,
                            ParentType = parentType
                        };
                        result.ExpandView();
                        return new RepositoryResponse<AddictionalViewModel>()
                        {
                            IsSucceed = true,
                            Data = result
                        };
                    }
                }
                return new RepositoryResponse<AddictionalViewModel>();
            }
            catch (Exception ex)
            {
                return UnitOfWorkHelper<MixCmsContext>.HandleException<AddictionalViewModel>(ex, isRoot, transaction);
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
                            AttributeSetId = attributeSet.Id,
                            AttributeSetName = attributeSet.Name,
                            Specificulture = culture,
                            Data = obj
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
                                if (valPredicate != null)
                                {
                                    valPredicate = ReflectionHelper.CombineExpression(valPredicate, pre, Heart.Enums.MixHeartEnums.ExpressionMethod.And);
                                }
                                else
                                {
                                    valPredicate = pre;
                                }
                            }
                            else
                            {
                                Expression<Func<MixAttributeSetValue, bool>> pre =
                                    m => m.AttributeFieldName == q.Key &&
                                    (EF.Functions.Like(m.StringValue, $"%{q.Value.ToString()}%"));
                                if (valPredicate != null)
                                {
                                    valPredicate = ReflectionHelper.CombineExpression(valPredicate, pre, Heart.Enums.MixHeartEnums.ExpressionMethod.And);
                                }
                                else
                                {
                                    valPredicate = pre;
                                }
                            }
                        }
                    }
                    if (valPredicate != null)
                    {
                        attrPredicate = ReflectionHelper.CombineExpression(valPredicate, attrPredicate, Heart.Enums.MixHeartEnums.ExpressionMethod.And);
                    }
                }
                // Loop queries string => predicate
                if (!string.IsNullOrEmpty(keyword))
                {
                    Expression<Func<MixAttributeSetValue, bool>> pre = m => m.AttributeSetName == attributeSetName && m.Specificulture == culture && m.StringValue.Contains(keyword);
                    attrPredicate = ReflectionHelper.CombineExpression(attrPredicate, pre, Heart.Enums.MixHeartEnums.ExpressionMethod.And);
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
                bool isStatus = Enum.TryParse(request.Query["status"], out MixEnums.MixContentStatus status);
                var tasks = new List<Task<RepositoryResponse<TView>>>();
                var getfields = await MixAttributeFields.ReadViewModel.Repository.GetModelListByAsync(
                    m => m.AttributeSetId == attributeSetId || m.AttributeSetName == attributeSetName, context, transaction);
                var fields = getfields.IsSucceed ? getfields.Data : new List<MixAttributeFields.ReadViewModel>();
                var fieldQueries = !string.IsNullOrEmpty(request.Query["query"]) ? JObject.Parse(request.Query["query"]) : new JObject();
                Expression<Func<MixAttributeSetValue, bool>> attrPredicate = m => m.Specificulture == culture
                   && (m.AttributeSetName == attributeSetName);
                // val predicate
                Expression<Func<MixAttributeSetValue, bool>> valPredicate = null;
                // Data predicate
                Expression<Func<MixAttributeSetData, bool>> predicate = null;
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
                        foreach (var field in fields)
                        {
                            Expression<Func<MixAttributeSetValue, bool>> pre =
                                       m => m.AttributeFieldName == field.Name &&
                                            (filterType == "equal" && m.StringValue == keyword) ||
                                            (filterType == "contain" && (EF.Functions.Like(m.StringValue, $"%{keyword}%")));
                            if (valPredicate != null)
                            {
                                valPredicate = ReflectionHelper.CombineExpression(valPredicate, pre, Heart.Enums.MixHeartEnums.ExpressionMethod.And);
                            }
                            else
                            {
                                valPredicate = pre;
                            }
                        }
                    }
                    if (fieldQueries != null && fieldQueries.Properties().Count() > 0) // filter by specific field name
                    {
                        foreach (var q in fieldQueries)
                        {
                            if (fields.Any(f => f.Name == q.Key))
                            {
                                string value = q.Value.ToString();
                                if (!string.IsNullOrEmpty(value))
                                {
                                    Expression<Func<MixAttributeSetValue, bool>> pre =
                                       m => m.AttributeFieldName == q.Key &&
                                            (filterType == "equal" && m.StringValue == (q.Value.ToString())) ||
                                            (filterType == "contain" && (EF.Functions.Like(m.StringValue, $"%{q.Value.ToString()}%")));
                                    if (valPredicate != null)
                                    {
                                        valPredicate = ReflectionHelper.CombineExpression(valPredicate, pre, Heart.Enums.MixHeartEnums.ExpressionMethod.Or);
                                    }
                                    else
                                    {
                                        valPredicate = pre;
                                    }
                                }
                            }
                        }
                    }
                    if (valPredicate != null)
                    {
                        attrPredicate = attrPredicate == null ? valPredicate
                                : ReflectionHelper.CombineExpression(valPredicate, attrPredicate, Heart.Enums.MixHeartEnums.ExpressionMethod.And);
                    }

                    if (attrPredicate != null)
                    {
                        var query = context.MixAttributeSetValue.Where(attrPredicate).Select(m => m.DataId).Distinct();
                        var dataIds = query.ToList();
                        if (query != null)
                        {
                            Expression<Func<MixAttributeSetData, bool>> pre = m => dataIds.Any(id => m.Id == id);
                            predicate = pre; // ReflectionHelper.CombineExpression(pre, predicate, Heart.Enums.MixHeartEnums.ExpressionMethod.And);

                        }
                    }
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
                    if (valPredicate != null)
                    {
                        valPredicate = ReflectionHelper.CombineExpression(valPredicate, pre, Heart.Enums.MixHeartEnums.ExpressionMethod.And);
                    }
                    else
                    {
                        valPredicate = pre;
                    }
                }
                else
                {
                    Expression<Func<MixAttributeSetValue, bool>> pre = m => m.AttributeFieldName == fieldName && m.StringValue.Contains(keyword);
                    if (valPredicate != null)
                    {
                        valPredicate = ReflectionHelper.CombineExpression(valPredicate, pre, Heart.Enums.MixHeartEnums.ExpressionMethod.And);
                    }
                    else
                    {
                        valPredicate = pre;
                    }
                }
                if (valPredicate != null)
                {
                    attrPredicate = ReflectionHelper.CombineExpression(valPredicate, attrPredicate, Heart.Enums.MixHeartEnums.ExpressionMethod.And);
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
            string parentId, MixEnums.MixAttributeSetDataType parentType,
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
                    && (m.Status == MixEnums.MixContentStatus.Published)
                    && (string.IsNullOrEmpty(parentId)
                         || (m.ParentId == parentId && m.ParentType == parentType.ToString())
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
            var properties = values.Select(m => m.ToJProperty());

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

        public static void LoadReferenceData(this JObject obj
            , string dataId, int attributeSetId, string culture
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(
                    _context, _transaction,
                    out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            var refFields = context.MixAttributeField.Where(
                   m => m.AttributeSetId == attributeSetId
                    && m.DataType == MixEnums.MixDataType.Reference).ToList();

            foreach (var item in refFields)
            {
                Expression<Func<MixRelatedAttributeData, bool>> predicate = model =>
                    (model.AttributeSetId == item.ReferenceId)
                    && (model.ParentId == dataId && model.ParentType == MixEnums.MixAttributeSetDataType.Set.ToString())
                    && model.Specificulture == culture
                    ;
                var getData = MixRelatedAttributeDatas.ReadMvcViewModel.Repository.GetModelListBy(predicate, context, transaction);

                JArray arr = new JArray();
                foreach (var nav in getData.Data.OrderBy(v => v.Priority))
                {
                    arr.Add(nav.Data.Obj);
                }
                if (obj.ContainsKey(item.Name))
                {
                    obj[item.Name] = arr;
                }
                else
                {
                    obj.Add(new JProperty(item.Name, arr));
                }
            }
            if (isRoot)
            {
                transaction.Dispose();
                context.Dispose();
            }
        }

    }
}
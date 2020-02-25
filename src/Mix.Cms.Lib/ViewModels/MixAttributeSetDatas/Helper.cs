using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Helpers;
using Mix.Cms.Lib.Models.Cms;
using Mix.Common.Helper;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.Repository;
using Mix.Domain.Data.ViewModels;
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
    public class Helper
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

                        //item.Model.Id = Guid.NewGuid().ToString();
                        //item.Model.Specificulture = culture;
                        //item.Model.CreatedDateTime = DateTime.UtcNow;
                        //item.Model.Priority = context.MixAttributeSetData.Count() + 1;
                        item.Priority = priority;
                        item.Fields = fields;
                        item.AttributeSetName = attributeSet.Name;
                        item.ParseModel();
                        context.Entry(item.Model).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                        foreach (var val in item.Values)
                        {
                            val.DataId = item.Id;
                            val.Specificulture = culture;
                            val.ParseModel();
                            //val.Model.DataId = item.Id;
                            //val.Model.CreatedDateTime = DateTime.UtcNow;
                            //val.Model.Specificulture = culture;
                            //val.Model.Id = Guid.NewGuid().ToString();
                            context.Entry(val.Model).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                        }
                    }
                    int tmp = await context.SaveChangesAsync();
                    //if (result.IsSucceed)
                    //{
                    //    foreach (var item in data)
                    //    {
                    //       item.GenerateCache(item.Model, item);
                    //    }
                    //}
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

        public static Task<RepositoryResponse<List<TView>>> FilterByValueAsync<TView>(string culture, string attributeSetName
            , Dictionary<string, Microsoft.Extensions.Primitives.StringValues> queryDictionary
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            where TView : ODataViewModelBase<MixCmsContext, MixAttributeSetData, TView>
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                Expression<Func<MixAttributeSetValue, bool>> valPredicate = m => m.Specificulture == culture && m.AttributeSetName == attributeSetName;
                RepositoryResponse<List<TView>> result = new RepositoryResponse<List<TView>>()
                {
                    IsSucceed = true,
                    Data = new List<TView>()
                };
                var tasks = new List<Task<RepositoryResponse<TView>>>();

                // Loop queries string => predicate
                foreach (var q in queryDictionary)
                {
                    if (!string.IsNullOrEmpty(q.Key) && !string.IsNullOrEmpty(q.Value))
                    {
                        Expression<Func<MixAttributeSetValue, bool>> pre = m => m.AttributeFieldName == q.Key && m.StringValue.Contains(q.Value);
                        valPredicate = ODataHelper<MixAttributeSetValue>.CombineExpression(valPredicate, pre, Microsoft.OData.UriParser.BinaryOperatorKind.And);
                    }
                }
                var query = context.MixAttributeSetValue.Where(valPredicate).Select(m => m.DataId).Distinct().ToList();
                if (query != null)
                {
                    foreach (var item in query)
                    {
                        tasks.Add(Task.Run(async () =>
                        {
                            var resp = await ODataDefaultRepository<MixCmsContext, MixAttributeSetData, TView>.Instance.GetSingleModelAsync(
                                m => m.Id == item && m.Specificulture == culture);
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

        public static async Task<RepositoryResponse<PaginationModel<TView>>> FilterByKeywordAsync<TView>(string culture, string attributeSetName
            , RequestPaging request, string keyword
            , Dictionary<string, Microsoft.Extensions.Primitives.StringValues> queryDictionary = null
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            where TView : ViewModelBase<MixCmsContext, MixAttributeSetData, TView>
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                Expression<Func<MixAttributeSetValue, bool>> attrPredicate = m => m.Specificulture == culture && m.AttributeSetName == attributeSetName;
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
                                Expression<Func<MixAttributeSetValue, bool>> pre = m => m.AttributeFieldName == q.Key && m.StringValue == (q.Value.ToString());
                                if (valPredicate != null)
                                {
                                    valPredicate = ODataHelper<MixAttributeSetValue>.CombineExpression(valPredicate, pre, Microsoft.OData.UriParser.BinaryOperatorKind.And);
                                }
                                else
                                {
                                    valPredicate = pre;
                                }
                            }
                            else
                            {
                                Expression<Func<MixAttributeSetValue, bool>> pre = m => m.AttributeFieldName == q.Key && m.StringValue.Contains(q.Value.ToString());
                                if (valPredicate != null)
                                {
                                    valPredicate = ODataHelper<MixAttributeSetValue>.CombineExpression(valPredicate, pre, Microsoft.OData.UriParser.BinaryOperatorKind.And);
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
                        attrPredicate = ODataHelper<MixAttributeSetValue>.CombineExpression(valPredicate, attrPredicate, Microsoft.OData.UriParser.BinaryOperatorKind.And);
                    }
                }
                // Loop queries string => predicate
                if (!string.IsNullOrEmpty(keyword))
                {
                    Expression<Func<MixAttributeSetValue, bool>> pre = m => m.AttributeSetName == attributeSetName && m.Specificulture == culture && m.StringValue.Contains(keyword);
                    attrPredicate = ODataHelper<MixAttributeSetValue>.CombineExpression(attrPredicate, pre, Microsoft.OData.UriParser.BinaryOperatorKind.And);
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
                    context.Dispose();
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
                        valPredicate = ODataHelper<MixAttributeSetValue>.CombineExpression(valPredicate, pre, Microsoft.OData.UriParser.BinaryOperatorKind.And);
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
                        valPredicate = ODataHelper<MixAttributeSetValue>.CombineExpression(valPredicate, pre, Microsoft.OData.UriParser.BinaryOperatorKind.And);
                    }
                    else
                    {
                        valPredicate = pre;
                    }
                }
                if (valPredicate != null)
                {
                    attrPredicate = ODataHelper<MixAttributeSetValue>.CombineExpression(valPredicate, attrPredicate, Microsoft.OData.UriParser.BinaryOperatorKind.And);
                }

                var query = context.MixAttributeSetValue.Where(attrPredicate).Select(m => m.DataId).Distinct();
                var dataIds = query.ToList();
                if (query != null)
                {
                    Expression<Func<MixAttributeSetData, bool>> predicate = m => dataIds.Any(id => m.Id == id);
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
                    context.Dispose();
                }
            }
        }

        public static RepositoryResponse<string> ExportAttributeToExcel(List<JObject> lstData, string sheetName
          , string folderPath, string fileName
          , List<string> headers = null)
        {
            var result = new RepositoryResponse<string>();
            try
            {
                if (lstData.Count > 0)
                {
                    var filenameE = fileName + "-" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx";

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
                            r[prop.Name] = a[prop.Name].Value<string>();
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
                        result.Data = CommonHelper.GetFullPath(new string[]
                        {
                            folderPath,
                            filenameE
                        });

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
    }
}
﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Common.Helper;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Models;
using Mix.Shared.Constants;
using Mix.Shared.Enums;
using Mix.Theme.Domain.ViewModels.Import;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mix.Database.Entities.Cms.v2;
using Mix.Shared.Services;
using Mix.Heart.Infrastructure.Repositories;

namespace Mix.Theme.Domain.Helpers
{
    public class MixDataHelper
    {
        public static async Task<RepositoryResponse<bool>> ImportData(
            string culture, MixDatabase mixDatabase, IFormFile file,
            DefaultModelRepository<MixCmsContextV2, MixDatabase> mixDatabaseRepo,
            DefaultRepository<MixCmsContextV2, MixDatabaseColumn, ImportaMixDatabaseColumnViewModel> mixDatabaseColumnRepo)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            UnitOfWorkHelper<MixCmsContextV2>.InitTransaction(
                null, null, out MixCmsContextV2 context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                List<ImportMixDataViewModel> data = LoadFileData(culture, mixDatabase, file);

                var fields = mixDatabaseColumnRepo.GetModelListBy(
                    f => f.MixDatabaseId == mixDatabase.Id, context, transaction).Data;
                foreach (var item in data)
                {
                    if (result.IsSucceed)
                    {
                        var isCreateNew = string.IsNullOrEmpty(item.Id);
                        item.Columns = fields;
                        item.MixDatabaseName = mixDatabase.SystemName;
                        item.Status = MixAppSettingService.GetEnumConfig<MixContentStatus>(MixAppSettingKeywords.DefaultContentStatus);
                        var saveResult = await item.SaveModelAsync(true, context, transaction);
                        ViewModelHelper.HandleResult(saveResult, ref result);
                    }
                }
                UnitOfWorkHelper<MixCmsContextV2>.HandleTransaction(result.IsSucceed, isRoot, transaction);
                return result;
            }
            catch (Exception ex)
            {
                return UnitOfWorkHelper<MixCmsContextV2>.HandleException<bool>(ex, isRoot, transaction);
            }
            finally
            {
                if (isRoot)
                {
                    //if current Context is Root
                    UnitOfWorkHelper<MixCmsContextV2>.CloseDbContext(ref context, ref transaction);
                }
            }
        }

        private static List<ImportMixDataViewModel> LoadFileData(
           string culture, MixDatabase mixDatabase, IFormFile file)
        {
            //create a list to hold all the values
            List<ImportMixDataViewModel> excelData = new List<ImportMixDataViewModel>();

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
                        ImportMixDataViewModel data = new ImportMixDataViewModel()
                        {
                            Id = obj["id"]?.ToString(),
                            MixDatabaseId = mixDatabase.Id,
                            MixDatabaseName = mixDatabase.SystemName,
                            Specificulture = culture,
                            Obj = obj
                        };
                        excelData.Add(data);
                    }
                }
                return excelData;
            }
        }


    }
}
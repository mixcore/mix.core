using Mix.Cms.Lib;
using Mix.Cms.Lib.Services;
using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Mix.Cms.Lib.Enums;
using Newtonsoft.Json.Linq;
using Mix.Cms.Lib.Helpers;
using Mix.Cms.Lib.Constants;
using MixDatabases = Mix.Cms.Lib.ViewModels.MixDatabases;
using MixDatabaseDatas = Mix.Cms.Lib.ViewModels.MixDatabaseDatas;
using System.Collections.Generic;
using Mix.Cms.Lib.Models.Cms;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Heart.Models;

namespace Mix.Rest.Api.Client.Helpers
{
    public class MixDatabaseHelper
    {
        public static PaginationModel<JObject> GetListData<T>(HttpRequest request, string culture = null)
        {
            culture = culture ?? MixService.GetAppSetting<string>("DefaultCulture");
            string mixDatabaseName = request.Query["mixDatabaseName"].ToString().Trim();
            var orderBy = "Id";
            Enum.TryParse(request.Query["direction"], out Heart.Enums.DisplayDirection direction);
            Enum.TryParse(request.Query["filterType"], out MixCompareOperatorKind filterType);
            bool.TryParse(request.Query["isGroup"], out bool isGroup);
            int.TryParse(request.Query["pageIndex"], out int pageIndex);
            var isPageSize = int.TryParse(request.Query["pageSize"], out int pageSize);
            bool isFromDate = DateTime.TryParse(request.Query["fromDate"], out DateTime fromDate);
            bool isToDate = DateTime.TryParse(request.Query["toDate"], out DateTime toDate);
            bool isStatus = Enum.TryParse(request.Query["status"], out MixContentStatus status);
            var fieldQueries = !string.IsNullOrEmpty(request.Query["query"]) ? JObject.Parse(request.Query["query"]) : new JObject();

            string whereSql = GetQueryString(MixRequestQueryKeywords.Specificulture, filterType, culture);
            whereSql = whereSql.AndAlsoIf(
                isFromDate,
                GetQueryString(
                    MixQueryColumnName.CreatedDateTime,
                    MixCompareOperatorKind.GreaterThanOrEqual,
                    fromDate.ToString()));
            whereSql = whereSql.AndAlsoIf(
                isToDate,
                GetQueryString(
                    MixQueryColumnName.CreatedDateTime,
                    MixCompareOperatorKind.LessThanOrEqual,
                    toDate.ToString()));
            whereSql = whereSql.AndAlsoIf(
                isStatus,
                GetQueryString(
                    MixQueryColumnName.Status,
                    MixCompareOperatorKind.Equal,
                    status.ToString()));

            // if filter by field name or keyword => filter by attr value
            if (fieldQueries.Count > 0)
            {
                // filter by all fields if have keyword

                if (fieldQueries != null && fieldQueries.Properties().Count() > 0) // filter by specific field name
                {
                    foreach (JProperty property in fieldQueries.Properties())
                    {
                        whereSql = whereSql.AndAlso(GetQueryString(property.Name, filterType, fieldQueries.Value<string>(property.Name)));
                    }
                }
            }
            using (var context = new MixCmsContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    var database = MixDatabases.UpdateViewModel.Repository.GetSingleModel(m => m.Name == mixDatabaseName, context, transaction).Data;
                    string countSql = $"SELECT COUNT(*) as total FROM {MixConstants.CONST_MIXDB_PREFIX}{mixDatabaseName} WHERE {whereSql}";
                    string pagingSql = $" ORDER BY {orderBy} {direction} LIMIT {pageSize} OFFSET {pageIndex * pageSize}";
                    var count = EFCoreHelper.RawSqlQuery(countSql, context)[0].Value<int>("total");
                    var result = GetData(mixDatabaseName, whereSql, pagingSql, context, transaction);
                    return new PaginationModel<JObject>()
                    {
                        Items = result,
                        PageSize = pageSize,
                        PageIndex = pageIndex,
                        TotalItems = result.Count,
                        TotalPage = (count / pageSize) + (count % pageSize > 0 ? 1 : 0)
                    };
                }
            }
        }

        private static List<JObject> GetData(string mixDatabaseName, string whereSql, string pagingSql, MixCmsContext context, IDbContextTransaction transaction)
        {
            var database = MixDatabases.UpdateViewModel.Repository.GetSingleModel(m => m.Name == mixDatabaseName, context, transaction).Data;
            string commandText = $"SELECT * FROM {MixConstants.CONST_MIXDB_PREFIX}{mixDatabaseName} WHERE {whereSql} {pagingSql}";
            var result = EFCoreHelper.RawSqlQuery(commandText, context);
            foreach (var col in database.Columns.Where(c => c.DataType == MixDataType.Reference))
            {
                foreach (var item in result)
                {
                    var refData = MixDatabaseDatas.Helper.DecodeRefData(item.Value<string>(col.Name));
                    if (refData == null)
                    {
                        item[col.Name] = new JArray();
                        continue;
                    }
                    var dbName = refData.Value<string>("ref_table_name");
                    var children = refData.Value<JArray>("children").Select(m => m["DataId"]).ToList();
                    if (children.Count > 0)
                    {
                        string where = $"id IN ({string.Join(",", children.Select(m => $"'{m}'"))})";
                        item[col.Name] = JArray.FromObject(GetData(dbName, where, null, context, transaction));
                    }
                    else
                    {
                        item[col.Name] = new JArray();
                    }
                }
            }

            return result;
        }

        private static string GetQueryString(string columnName, MixCompareOperatorKind kind, params string[] value)
        {
            return kind switch
            {
                MixCompareOperatorKind.Equal => $"{columnName} = '{value[0]}'",
                MixCompareOperatorKind.NotEqual => $"{columnName} != '{value[0]}'",
                MixCompareOperatorKind.GreaterThanOrEqual => $"{columnName} >= '{value[0]}'",
                MixCompareOperatorKind.GreaterThan => $"{columnName} > '{value[0]}'",
                MixCompareOperatorKind.LessThanOrEqual => $"{columnName} <= '{value[0]}'",
                MixCompareOperatorKind.LessThan => $"{columnName} < '{value[0]}'",
                MixCompareOperatorKind.Contain => $"{columnName} LIKE '%{value[0]}%'",
                MixCompareOperatorKind.NotContain => $"{columnName} NOT LIKE '%{value[0]}%'",
                MixCompareOperatorKind.InRange => $"{columnName} IN ({string.Join(",", value)})",
                MixCompareOperatorKind.NotInRange => $"{columnName} NOT IN ({string.Join(",", value)})",
                _ => string.Empty,
            };
        }


    }
    public static class StringExtension
    {
        public static string AndAlsoIf(this string first, bool condition, string second)
        {
            return condition ? $"{first} AND {second}" : first;
        }

        public static string AndAlso(this string first, string second)
        {
            return $"{first} AND {second}";
        }
        public static T ToEnum<T>(this string str)
        {
            return (T)Enum.Parse(typeof(T), str);
        }
    }
}

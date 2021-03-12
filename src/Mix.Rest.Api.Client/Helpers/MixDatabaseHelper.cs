using Mix.Cms.Lib;
using Mix.Cms.Lib.Services;
using System;
using System.Linq;
using Mix.Domain.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Mix.Cms.Lib.Enums;
using Newtonsoft.Json.Linq;
using Mix.Cms.Lib.Helpers;
using Mix.Cms.Lib.Constants;

namespace Mix.Rest.Api.Client.Helpers
{
    public class MixDatabaseHelper
    {
        public static PaginationModel<JObject> GetListData<T>(HttpRequest request, string culture = null)
        {
            culture = culture ?? MixService.GetConfig<string>("DefaultCulture");
            string mixDatabaseName = request.Query["mixDatabaseName"].ToString().Trim();
            var orderBy = "Id";
            int.TryParse(request.Query["mixDatabaseId"], out int mixDatabaseId);
            Enum.TryParse(request.Query["direction"], out Heart.Enums.MixHeartEnums.DisplayDirection direction);
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
                    MixCompareOperatorKind.GreaterThanOrEqual , 
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

            string countSql = $"SELECT COUNT(*) FROM {MixConstants.CONST_MIXDB_PREFIX}{mixDatabaseName} WHERE {whereSql}";
            string commandText = $"SELECT * FROM {MixConstants.CONST_MIXDB_PREFIX}{mixDatabaseName}" +
                $" WHERE {whereSql}" +
                $" ORDER BY {orderBy} {direction} LIMIT {pageSize} OFFSET {pageIndex * pageSize}";
            var count = EFCoreHelper.RawSqlQuery(countSql);
            var result = EFCoreHelper.RawSqlQuery(commandText);

            return new PaginationModel<JObject>()
            {
                Items = result,
                PageSize = pageSize,
                PageIndex = pageIndex,
                TotalItems = result.Count,
                TotalPage = 1
            };

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

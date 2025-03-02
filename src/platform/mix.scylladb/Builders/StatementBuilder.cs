using Cassandra;
using Mix.Constant.Enums;
using Mix.Heart.Model;
using Mix.Shared.Dtos;
using Mix.Shared.Models;
using System.Linq;

namespace Mix.Scylladb.Builders
{
    public static class StatementBuilder
    {
        #region Statement Builder

        public static SimpleStatement CreateCount(string tableName, IEnumerable<MixQueryField>? where = null, MixConjunction conjunction = MixConjunction.And)
        {
            return new SimpleStatement($"SELECT COUNT(*) FROM {tableName} {ParseWhereQuery(where, conjunction)}");
        }

        #region Selects

        public static SimpleStatement CreateSelect(
            string tableName,
            IEnumerable<MixQueryField>? where = null,
            MixConjunction conjunction = MixConjunction.And,
            IEnumerable<string>? selectFieldNames = null,
            IEnumerable<MixSortByColumn>? sortByFields = null,
            PagingRequestModel? paging = null)
        {
            var query = $"""
                    SELECT {ParseSelectFieldNamesString(selectFieldNames)} 
                    FROM {tableName} 
                    {ParseWhereQuery(where, conjunction)} 
                    {ParseOrderByQuery(sortByFields)} 
                    {ParsePagingString(paging)} 
                    ALLOW FILTERING
                    """.Trim();
            return new SimpleStatement(query);
        }

        public static SimpleStatement CreateSelect(
            string tableName,
            MixQueryField where,
            IEnumerable<string>? selectFieldNames = null)
        {
            var query = $"""
                    SELECT {ParseSelectFieldNamesString(selectFieldNames)} 
                    FROM {tableName} 
                    WHERE {ParseQueryString(where)} 
                    ALLOW FILTERING
                    """;
            return new SimpleStatement(query);
        }
        #endregion
        #region Insert

        public static IStatement CreateInsert(string tableName, Dictionary<string, object?> dic)
        {
            return new SimpleStatement(ParseInsertQuery(tableName, dic.Keys.ToList()), dic.Values.ToArray());
        }

        public static IStatement CreateInsertMany(string tableName, List<Dictionary<string, object>> dics)
        {
            var staments = new BatchStatement();
            foreach (var item in dics)
            {
                staments.Add(CreateInsert(tableName, item) as SimpleStatement);
            }
            return staments;
        }

        private static string ParseInsertQuery(string tableName, List<string> cols)
        {
            return $"INSERT INTO {tableName} ({string.Join(',', cols)}) VALUES ({string.Join(',', cols.Select(m => "?"))});";
        }

        #endregion

        #region Delete
        public static SimpleStatement CreateDelete(
            string tableName,
            Guid id)
        {
            var query = $"""
                    DELETE 
                    FROM {tableName} 
                    WHERE id={id} 
                    """;
            return new SimpleStatement(query);
        }

        public static SimpleStatement CreateDelete(
            string tableName,
            IEnumerable<MixQueryField>? where = null,
            MixConjunction conjunction = MixConjunction.And)
        {
            var query = $"""
                    DELETE 
                    FROM {tableName} 
                    {ParseWhereQuery(where, conjunction)} 
                    """;
            return new SimpleStatement(query);
        }
        #endregion
        #endregion


        #region Helper

        public static string ParseWhereQuery(MixQueryField? where)
        {
            if (where == null)
            {
                return string.Empty;
            }

            return $"WHERE {ParseQueryString(where)}";
        }

        public static string ParseWhereQuery(IEnumerable<MixQueryField>? where, MixConjunction conjunction)
        {
            if (where == null || !where.Any())
            {
                return string.Empty;
            }

            return $"WHERE {string.Join($" {conjunction} ", where!.Select(m => ParseQueryString(m)).ToArray())}";
        }

        public static string ParseOrderByQuery(MixSortByColumn? sortByFields)
        {
            if (sortByFields == null)
            {
                return string.Empty;
            }

            return $"ORDER BY {ParseOrderByString(sortByFields)}";
        }

        public static string ParseOrderByQuery(IEnumerable<MixSortByColumn>? sortByFields)
        {
            if (sortByFields == null || !sortByFields.Any())
            {
                return string.Empty;
            }

            return $"ORDER BY {string.Join(',', sortByFields!.Select(m => ParseOrderByString(m)).ToArray())}";
        }

        public static string ParseSelectFieldNamesString(IEnumerable<string>? selectFieldNames)
        {
            if (selectFieldNames == null || !selectFieldNames.Any())
            {
                return "*";
            }
            return string.Join(",", selectFieldNames);
        }

        public static string ParsePagingString(PagingRequestModel? paging)
        {
            return paging == null
                ? string.Empty
                : $"LIMIT {paging.PageSize}";
        }

        public static string ParseOrderByString(MixSortByColumn queryField)
        {
            return $"{queryField.FieldName} {queryField.Direction}";
        }
        public static string ParseQueryString(MixQueryField queryField)
        {
            object val = queryField.Value.GetType() == typeof(string) ? $"'{queryField.Value}'" : queryField.Value;
            switch (queryField.CompareOperator)
            {
                case MixCompareOperator.Equal:
                    return $"{queryField.FieldName} = {val}";
                case MixCompareOperator.Like:
                case MixCompareOperator.ILike:
                    return $"{queryField.FieldName} like '%{val}%'";
                case MixCompareOperator.NotEqual:
                    return $"{queryField.FieldName} != {val}";
                case MixCompareOperator.Contain:
                    return queryField.Value != null
                        ? $"{queryField.FieldName} in {queryField.Value.ToString()?.Split(',', StringSplitOptions.TrimEntries)}"
                        : string.Empty;
                case MixCompareOperator.NotContain:
                    return queryField.Value != null
                        ? $"{queryField.FieldName} not in {queryField.Value.ToString()?.Split(',', StringSplitOptions.TrimEntries)}"
                        : string.Empty;
                case MixCompareOperator.InRange:
                    return queryField.Value != null
                        ? $"{queryField.FieldName} in {queryField.Value.ToString()?.Split(',', StringSplitOptions.TrimEntries)}"
                        : string.Empty;
                case MixCompareOperator.NotInRange:
                    return queryField.Value != null
                        ? $"{queryField.FieldName} not in {queryField.Value.ToString()?.Split(',', StringSplitOptions.TrimEntries)}"
                        : string.Empty;
                case MixCompareOperator.GreaterThanOrEqual:
                    return $"{queryField.FieldName} >= {val}";
                case MixCompareOperator.GreaterThan:
                    return $"{queryField.FieldName} > {val}";
                case MixCompareOperator.LessThanOrEqual:
                    return $"{queryField.FieldName} <= {val}";
                case MixCompareOperator.LessThan:
                    return $"{queryField.FieldName} < {val}";
                default:
                    return $"{queryField.FieldName} = {val}";
            }
        }

        #endregion
    }
}
